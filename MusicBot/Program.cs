using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Audio;
using System.IO;
using MusicBot;
using System.Windows.Forms;
using System.Threading;

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
