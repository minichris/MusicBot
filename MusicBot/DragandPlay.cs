using Discord;
using Discord.Audio;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicBot
{
    public partial class DragandPlay : Form
    {
        Song SongToPlay;
        Channel UsersVoiceChannel = null;
        Boolean FirstConnectionEstablished;
        string[] CustomButtonData;
        public DragandPlay()
        {
            InitializeComponent();
            CustomButtonData = File.ReadAllLines("CustomButton.txt");
            GeneralButton.Text = CustomButtonData[2];
            DragBox.AllowDrop = true;
            DragBox.DragEnter += DragBox_DragEnter;
            DragBox.DragDrop += DragBox_DragDrop;
            FindButton.Click += FindButton_Click;
            Scanner.RunWorkerAsync();
            foreach(Control ControlObj in this.Controls)
            {
                ControlObj.Enabled = false;
            }
            EnableControls();
        }

        private async void EnableControls()
        {
            while (!FirstConnectionEstablished)
            {
                await Task.Delay(25);
            }
            foreach (Control ControlObj in this.Controls)
            {
                ControlObj.Enabled = true;
            }
            FindButton.Focus();
            YoutubeGetButton.Enabled = false;
            YoutubeBox.Enabled = false;
        }

        private void DragBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void DragBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false); //get a list of files which the user has dropped
            if (FileList.Length > 1) //if the user is trying to ram more in then we can take
            {
                MessageBox.Show("One at a time, please!"); //display a message telling the user to only drop one file at a time
            }
            SongToPlay = new Song();
            SongToPlay.FilePath = FileList.First();
            SongPlayer.RunWorkerAsync();
            MessageBox.Show($"Will now attempt to play {FileList.First()}");
        }

        private async void FindButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Looking for user {NameBox.Text}"); 
            foreach (Server ServerObj in Program._client.Servers)
            {
                Console.WriteLine($"Checking Server {ServerObj.Name}");
                foreach (Channel ChannelObj in ServerObj.AllChannels)
                {
                    if (ChannelObj.Type == ChannelType.Voice)
                    {
                        Console.WriteLine($"--- Checking Channel {ChannelObj.Name}");
                        foreach (User UserObj in ChannelObj.Users)
                        {
                            Console.WriteLine($"------User: {UserObj.Name}");
                            if (UserObj.Name == NameBox.Text || UserObj.Nickname == NameBox.Text)
                            {
                                UsersVoiceChannel = ChannelObj; //We found them!
                                Console.WriteLine($"User {UserObj.Nickname} found on server {ServerObj.Name}, {ChannelObj.Name}");
                            }
                            else
                            {
                                Console.WriteLine($"Checking {UserObj.Nickname} on server {ServerObj.Name}, {ChannelObj.Name}");
                            }
                        }
                    }
                }
            }
            if (UsersVoiceChannel != null) //Found a user
            {
                Program._vClient = await Program._client.GetService<AudioService>().Join(UsersVoiceChannel); // Join the Voice Channel, and return the IAudioClient.
            }
            else
            {
                MessageBox.Show("We couldn't find you :(\nMaybe try leaving and rejoining the voice channel you're in.");
            }
        }

        private void Scanner_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
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
                FirstConnectionEstablished = true;
            });
        }

        private void SongPlayer_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SongToPlay.Play(Convert.ToInt32(SampleRateBox.Value));
        }

        private async void GeneralButton_Click(object sender, EventArgs e)
        {
            foreach (Server ServerObj in Program._client.Servers)
            {
                if (ServerObj.Name == CustomButtonData[0])
                {
                    foreach (Channel ChannelObj in ServerObj.AllChannels)
                    {
                        if (ChannelObj.Id == ulong.Parse(CustomButtonData[1]))
                        {
                            Program._vClient = await Program._client.GetService<AudioService>().Join(ChannelObj); // Join the Voice Channel, and return the IAudioClient.
                            UsersVoiceChannel = ChannelObj;
                        }
                    }
                }
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            if (UsersVoiceChannel != null)
            {
                Program._client.GetService<AudioService>().Leave(UsersVoiceChannel);
                UsersVoiceChannel = null;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            SongToPlay.Stop();
        }
    }
}
