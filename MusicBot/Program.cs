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

    public static string botPrefix = "m!";

    private DiscordClient _client;

    public void Start()
    {
        _client = new DiscordClient();

        _client.UsingAudio(x => // Opens an AudioConfigBuilder so we can configure our AudioService
        {
            x.Mode = AudioMode.Outgoing; // Tells the AudioService that we will only be sending audio
        });

        _client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");


        _client.MessageReceived += async (s, e) =>
        {
            if ((e.Channel.Name.Contains("music")) && (e.Message.Text.StartsWith(botPrefix)))
            {
                if (e.Message.Text.StartsWith($"{botPrefix}summon"))
                {
                    if (!string.IsNullOrWhiteSpace(e.User.VoiceChannel.Name))
                    {
                        string userVC = e.User.VoiceChannel.Name;
                        var voiceChannel = _client.FindServers(e.Server.Name).FirstOrDefault().FindChannels(userVC).FirstOrDefault();

                        var _vClient = await _client.GetService<AudioService>() // We use GetService to find the AudioService that we installed earlier. In previous versions, this was equivelent to _client.Audio()
                                .Join(voiceChannel); // Join the Voice Channel, and return the IAudioClient.
                    }
                    else
                    {
                        await e.Channel.SendMessage($"You must be in a voice channel to use this command!");
                    }
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
