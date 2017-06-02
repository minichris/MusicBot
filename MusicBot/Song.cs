using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Audio;
using System.IO;
using NAudio.Wave;
using YoutubeExtractor;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

class Song
{
    public string FilePath;
    public VideoInfo Videoinfo;
    private MediaFoundationResampler resampler;
    private MediaFoundationReader mediaStream;
    private VideoDownloader downloader;
    private TaskCompletionSource<bool> DownloadFinishedEvent = new TaskCompletionSource<bool>();

    public static string GetMD5(string inputString)
    {
        Console.WriteLine($"Getting the MD5 of {inputString}");
        byte[] encodedPassword = new UTF8Encoding().GetBytes(inputString);
        byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
        string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();
        Console.Write($" which is {encoded}");
        return encoded;
    }

    private async Task DownloadVideo()
    {
        downloader = new VideoDownloader(Videoinfo, FilePath);
        downloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);
        try
        {
            downloader.Execute();
            DownloadFinishedEvent.SetResult(true);
        }
        catch (Exception DownloadException)
        {
            Console.WriteLine(DownloadException);
        }
        await DownloadFinishedEvent.Task;
    }

    public async Task<string> SongViaUrl(string RawURL)
    {
        Console.WriteLine($"Opening / creating output folder at: {Program.OutputFolder}");
        Directory.CreateDirectory(Program.OutputFolder); // Create video folder if not found
        IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(RawURL);
        Videoinfo = videoInfos.First();
        FilePath = Path.Combine(Program.OutputFolder, GetMD5(Videoinfo.Title) + Videoinfo.AudioExtension); // Grabs path to downloaded song
        Console.WriteLine($"Trying to find song at path: {FilePath}");
        if (!File.Exists(FilePath)) // downloads video if it isn't cached
        {
            await DownloadVideo();
        }
        return FilePath;
    }

    public void Stop()
    {
        mediaStream.Dispose();
        resampler.Dispose();
    }

    public void Play(int SampleRate)
    {
        var channelCount = Program._client.GetService<AudioService>().Config.Channels; // Get the number of AudioChannels our AudioService has been configured to use.
        WaveFormat OutFormat = new WaveFormat(SampleRate, 16, channelCount); // Create a new Output Format, using the spec that Discord will accept, and with the number of channels that our client supports.
        try
        {
            using (mediaStream = new MediaFoundationReader(this.FilePath))
            using (resampler = new MediaFoundationResampler(mediaStream, OutFormat)) // Create a Disposable Resampler, which will convert the read MP3 data to PCM, using our Output Format
            {
                resampler.ResamplerQuality = 60; // Set the quality of the resampler to 60, the highest quality
                int blockSize = OutFormat.AverageBytesPerSecond / 50; // Establish the size of our AudioBuffer
                byte[] buffer = new byte[blockSize];
                int byteCount;

                while ((byteCount = resampler.Read(buffer, 0, blockSize)) > 0) // Read audio into our buffer, and keep a loop open while data is present
                {
                    if (byteCount < blockSize)
                    {
                        // Incomplete Frame
                        for (int i = byteCount; i < blockSize; i++)
                            buffer[i] = 0;
                    }
                    Program._vClient.Send(buffer, 0, blockSize); // Send the buffer to Discord
                }
            }
        }
        catch (Exception ReaderException)
        {
            Console.WriteLine(ReaderException.Message); // Prints any errors to console
        }

        Program._vClient.Wait(); // Waits for the currently playing sound file to end.
    }
}