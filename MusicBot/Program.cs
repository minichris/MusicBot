using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Audio;
using System.IO;
using NAudio.Wave;
using YoutubeExtractor;
using MusicBot;
using System.Windows.Forms;

class Song
{
    public string FilePath;
    public VideoInfo Videoinfo;

    public string SongViaUrl(string RawURL)
    {
        Directory.CreateDirectory(Program.OutputFolder); // Create video folder if not found
        IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(RawURL);
        Videoinfo = videoInfos.First();
        string FinalFilePath = Path.Combine(Program.OutputFolder, Videoinfo.Title + Videoinfo.AudioExtension); // Grabs path to downloaded song
        if(!File.Exists(FinalFilePath)) // downloads video if it isn't cached
        {
            //construct downloader
            VideoDownloader downloader = new VideoDownloader(Videoinfo, FinalFilePath);
            downloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);
            try
            {
                downloader.Execute();
            }
            catch (Exception DownloadException)
            {
                Console.WriteLine(DownloadException.Message);
                Console.WriteLine(DownloadException);
            }
        }
        this.FilePath = FinalFilePath;
        return FilePath;
    }
    private MediaFoundationResampler resampler;
    private MediaFoundationReader mediaStream;

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

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new DragandPlay());
    }

    public static string botPrefix = "/"; // Defines prefix variable
	public static string OutputFolder = $"{Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar}videos"; // Output folder for songs
    public static DiscordClient _client;
    public static IAudioClient _vClient;
}
