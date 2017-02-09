using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Audio;
using System.IO;
using NAudio.Wave;
using YoutubeExtractor;

class Program
{
    static void Main(string[] args) => new Program().Start();

    public static string botPrefix = "/"; // Defines voting variable
    public static Discord.Audio.IAudioClient _vClient;
    public static Message playMessage;
    public static string videoName = "Rick Astley - Never Gonna Give You Up";

    private DiscordClient _client;

    public void Start()
    {
        _client = new DiscordClient();

        _client.UsingAudio(x => // Opens an AudioConfigBuilder so we can configure our AudioService
        {
            x.Mode = AudioMode.Outgoing; // Tells the AudioService that we will only be sending audio
        });

        _client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}"); // Log errors/info to console


        _client.MessageReceived += async (s, e) =>
        {
            if (e.Message.Text == $"{botPrefix}help")
            {
                await e.Channel.SendMessage($"Available commands: {botPrefix}help, {botPrefix}info, {botPrefix}summon, {botPrefix}disconnect, {botPrefix}play");
            }
            else if (e.Message.Text == $"{botPrefix}info")
                await e.Channel.SendMessage($"Hiya! I'm SharpTunes, a Discord Music Bot written in C# using Discord.Net. You can see my list of commands with `{botPrefix}help` and check out my source code at <https://github.com/Noahkiq/MusicBot>.");
            else if (e.Message.Text == $"{botPrefix}summon") // Detect if message is m!summon
            {
                if (e.User.VoiceChannel == null) // Checks if 'userVC' is null
                {
                    await e.Channel.SendMessage($"You must be in a voice channel to use this command!"); // Give error message if 'userVC' is null
                }
                else
                {
                    string userVC = e.User.VoiceChannel.Name; // Define 'userVC' variable
                    var voiceChannel = _client.FindServers(e.Server.Name).FirstOrDefault().FindChannels(userVC).FirstOrDefault(); // Grabs VC object
                    _vClient = await _client.GetService<AudioService>() // We use GetService to find the AudioService that we installed earlier. In previous versions, this was equivelent to _client.Audio()
                            .Join(voiceChannel); // Join the Voice Channel, and return the IAudioClient.
                    await e.Channel.SendMessage($"👌");
                }
            }
            else if (e.Message.Text == $"{botPrefix}disconnect")
            {
                if (_vClient != null)
                {
                    await _vClient.Disconnect();
                    _vClient = null;
                    await e.Channel.SendMessage($"👋");
                }
                else
                {
                    await e.Channel.SendMessage($"The bot is not currently in a voice channel.");
                }
            }
            else if (e.Message.Text.StartsWith($"{botPrefix}play"))
            {
                if (e.Message.Text == $"{botPrefix}play")
                     await e.Channel.SendMessage($"Proper usage: `{botPrefix}play [youtube video url]`");
                 else
                 {
                     string rawinput = e.Message.RawText.Replace($"{botPrefix}play ", ""); // Grab raw video input
                     string filtering = rawinput.Replace("<", ""); // Remove '<' from input
                     string input = filtering.Replace(">", ""); // Remove '>' from input
                     playMessage = e.Message; // Set 'playMessage' ID

                     var newFilename = Guid.NewGuid().ToString(); // Create file name
                     var mp3OutputFolder = $"{Directory.GetCurrentDirectory()}\\videos"; // Grab video folder
                     Directory.CreateDirectory(mp3OutputFolder); // Create video folder if not found

                     IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(rawinput);

                     VideoInfo Video = videoInfos.First();
                     //construct downloader
                     var downloader = new VideoDownloader(Video, Path.Combine(mp3OutputFolder, Video.Title + Video.AudioExtension));
                     downloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);
                     try
                     {
                         downloader.Execute();
                     }
                     catch(Exception DownloadException)
                     {
                         Console.WriteLine(DownloadException.Message);
                     }

                    string filePath = Path.Combine(mp3OutputFolder, Video.Title + Video.AudioExtension); // Grab music file to play
                     
                    var channelCount = _client.GetService<AudioService>().Config.Channels; // Get the number of AudioChannels our AudioService has been configured to use.
                    var OutFormat = new WaveFormat(48000, 16, channelCount); // Create a new Output Format, using the spec that Discord will accept, and with the number of channels that our client supports.
                    try
                    {
                    using (var vorbisStream = new MediaFoundationReader(filePath))
                        using (MediaFoundationResampler resampler = new MediaFoundationResampler(vorbisStream, OutFormat)) // Create a Disposable Resampler, which will convert the read MP3 data to PCM, using our Output Format
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
                                _vClient.Send(buffer, 0, blockSize); // Send the buffer to Discord
                            }
                        }
                    }
                    catch(Exception ReaderException)
                    {
                        Console.WriteLine(ReaderException.Message);
                    }
                
                     _vClient.Wait(); // Waits for the currently playing sound file to end.
                }
            }
        };

        string[] AccountData = File.ReadAllLines("account.txt");
        _client.ExecuteAndWait(async () => {
            await _client.Connect(AccountData[0], AccountData[1]);
            _client.SetGame("some music");
        });

    }
}
