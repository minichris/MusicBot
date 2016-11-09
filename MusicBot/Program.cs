using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using System.IO;
using System.Timers;
using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using WrapYoutubeDl;

class Program
{
    static void Main(string[] args) => new Program().Start();

    public static string botPrefix = "m!"; // Defines voting variable
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

                    var urlToDownload = input;
                    var newFilename = Guid.NewGuid().ToString();
                    var mp3OutputFolder = $"{Directory.GetCurrentDirectory()}\\videos";

                    var downloader = new AudioDownloader(input, newFilename, mp3OutputFolder);
                    downloader.ProgressDownload += downloader_ProgressDownload;
                    downloader.FinishedDownload += downloader_FinishedDownload;
                    downloader.Download();

                    videoName = downloader.OutputName;

                    string filePath = $"{mp3OutputFolder}\\{newFilename}.mp3"; // Grab music file to play

                    var channelCount = _client.GetService<AudioService>().Config.Channels; // Get the number of AudioChannels our AudioService has been configured to use.
                    var OutFormat = new WaveFormat(48000, 16, channelCount); // Create a new Output Format, using the spec that Discord will accept, and with the number of channels that our client supports.
                    using (var MP3Reader = new Mp3FileReader(filePath)) // Create a new Disposable MP3FileReader, to read audio from the filePath parameter
                    using (var resampler = new MediaFoundationResampler(MP3Reader, OutFormat)) // Create a Disposable Resampler, which will convert the read MP3 data to PCM, using our Output Format
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

                    _vClient.Wait(); // Waits for the currently playing sound file to end.
                }
            }
        };

        string token = File.ReadAllText("token.txt");
        _client.ExecuteAndWait(async () => {
            await _client.Connect(token, TokenType.Bot);
            _client.SetGame("some music");
        });

    }

    static void downloader_FinishedDownload(object sender, DownloadEventArgs e)
    {
        playMessage.Channel.SendMessage($"Finished downloading! Now playing {videoName}.");
    }

    static void downloader_ProgressDownload(object sender, ProgressEventArgs e)
    {
        // i have nothing to put here
    }
}
