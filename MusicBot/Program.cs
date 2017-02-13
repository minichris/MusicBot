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
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

class Song
{
    public string FilePath;
    public VideoInfo Videoinfo;
    private MediaFoundationResampler resampler;
    private MediaFoundationReader mediaStream;

    public static string GetMD5(string inputString)
    {
        HashAlgorithm algorithm = MD5.Create();
        return Convert.ToString(algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString)));
    }

    private void DownloadVideo()
    {
        VideoDownloader downloader = new VideoDownloader(Videoinfo, FilePath);
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

    public async Task<string> SongViaUrl(string RawURL)
    {
        Directory.CreateDirectory(Program.OutputFolder); // Create video folder if not found
        IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(RawURL);
        Videoinfo = videoInfos.First();
        FilePath = Path.Combine(Program.OutputFolder, GetMD5(Videoinfo.Title) + Videoinfo.AudioExtension); // Grabs path to downloaded song
        if (!File.Exists(FilePath)) // downloads video if it isn't cached
        {
            DownloadVideo();
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

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ThreadStart DiscordClientThreadStart = new ThreadStart(DiscordClientHandler);
        DiscordClientThread = new Thread(DiscordClientThreadStart);
        DiscordClientThread.Start();
        Application.Run(new DragandPlay());
    }

    public static string botPrefix = "/"; // Defines prefix variable
	public static string OutputFolder = $"{Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar}videos"; // Output folder for songs
    public static DiscordClient _client;
    public static IAudioClient _vClient;
    public static bool FirstConnectionEstablished;
    public static Thread DiscordClientThread;

    static void DiscordClientHandler()
    {
        Program._client = new DiscordClient();

        Program._client.UsingAudio(x => // Opens an AudioConfigBuilder so we can configure our AudioService
        {
            x.Mode = AudioMode.Outgoing; // Tells the AudioService that we will only be sending audio
        });

        Program._client.Log.Message += (s, MessageEvent) => Console.WriteLine($"[{MessageEvent.Severity}] {MessageEvent.Source}: {MessageEvent.Message}"); // Log errors/info to console

        Program._client.MessageReceived += async (s, MessageEvent) =>
        {
            if (MessageEvent.Message.Text == $"{Program.botPrefix}info")
                await MessageEvent.Channel.SendMessage($"Hiya! I'm ChrisRadio C# Edition, a Discord Music Bot written in C# using Discord.Net. Check out my source code at <https://github.com/minichris/MusicBot/>.");
        };

        string[] AccountData = File.ReadAllLines("account.txt");
        Program._client.ExecuteAndWait(async () =>
        {
            await Program._client.Connect(AccountData[0], AccountData[1]);
            Program._client.SetGame("some music");
            Console.WriteLine("Connected!");
            Program.FirstConnectionEstablished = true;
        });
    }
}
