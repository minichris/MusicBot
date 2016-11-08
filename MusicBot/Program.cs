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

class Program
{
    static void Main(string[] args) => new Program().Start();

    public static string botPrefix = "m!"; // Defines voting variable
    public static Discord.Audio.IAudioClient _vClient;

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
                await e.Channel.SendMessage($"Available commands: {botPrefix}help, {botPrefix}info, {botPrefix}summon, {botPrefix}disconnect");
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
        };

        string token = File.ReadAllText("token.txt");
        _client.ExecuteAndWait(async () => {
            await _client.Connect(token, TokenType.Bot);
            _client.SetGame("some music");
        });

    }
}
