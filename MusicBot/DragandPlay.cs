using Discord;
using Discord.Audio;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicBot
{
    public partial class DragandPlay : Form
    {
		Music CurrentSong;
		Channel BotCurrentChannel = null;
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
            foreach(Control ControlObj in this.Controls)
            {
                ControlObj.Enabled = false;
            }
            EnableControls();
        }

        private async void EnableControls() //called to enable the controls after the Discord Client connects
        {
            while (!Program.FirstConnectionEstablished) //while were not connected
            {
                await Task.Delay(25); //wait a bit
            }
            //at this point we have connected at least once
            foreach (Control ControlObj in this.Controls) //for all of the controls in the form
            {
                ControlObj.Enabled = true; //enable the control
            }
            FindButton.Focus();
        }

        private void DragBox_DragEnter(object sender, DragEventArgs e) //when something is dragged over the DragBox
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) //if its a File
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void DragBox_DragDrop(object sender, DragEventArgs e) //when something is dropped onto the DragBox
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false); //get a list of files which the user has dropped
            if (FileList.Length > 1) //if the user is trying to ram more in then we can take
            {
                MessageBox.Show("One at a time, please!"); //display a message telling the user to only drop one file at a time
            }
            CurrentSong = new Music(); //create a new song to hold the data and allow us to play it
            CurrentSong.FilePath = FileList.First(); //set the file path to the path of the dragged in file
            SongPlayer.RunWorkerAsync(); //run the song playing worker
            MessageBox.Show($"Will now attempt to play {FileList.First()}");
        }

        private async void FindButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Looking for user {NameBox.Text}"); 
            foreach (Server ServerObj in Program._client.Servers) //searching all of the servers
            {
                Console.WriteLine($"Checking Server {ServerObj.Name}");
                foreach (Channel ChannelObj in ServerObj.AllChannels) //and all of the channel in those servers
                {
                    if (ChannelObj.Type == ChannelType.Voice) //and of those channels, the ones with voice enabled
                    {
                        Console.WriteLine($"--- Checking Channel {ChannelObj.Name}");
                        foreach (User UserObj in ChannelObj.Users) //and all of the users
                        {
                            Console.WriteLine($"------User: {UserObj.Name}");
                            if (UserObj.Name == NameBox.Text || UserObj.Nickname == NameBox.Text) //for a user with the same name or nickname as the one in the box
                            {
                                BotCurrentChannel = ChannelObj; //We found them!
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
            if (BotCurrentChannel != null) //Found a user
            {
                Program._vClient = await Program._client.GetService<AudioService>().Join(BotCurrentChannel); // Join the Voice Channel, and return the IAudioClient.
            }
            else
            {
                MessageBox.Show("We couldn't find you :(\nMaybe try leaving and rejoining the voice channel you're in.");
            }
        }       

        private void SongPlayer_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) //when SongPlayer.RunWorkerAsync(); is called
        {
            Console.WriteLine($"Attempting to play song in location {CurrentSong.FilePath}");
            CurrentSong.Play(Convert.ToInt32(SampleRateBox.Value)); //play the song at the sample rate in "SampleRateBox"
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
                            BotCurrentChannel = ChannelObj;
                        }
                    }
                }
            }
        }

        private void DisconnectButton_Click(object sender, EventArgs e) //disconnect button pressed
        {
            if (BotCurrentChannel != null) //if the bot is in a voice channel
            {
                Program._client.GetService<AudioService>().Leave(BotCurrentChannel); //leave the voice channel
                BotCurrentChannel = null; //set our voice channel to null
            }
        }

        private void StopButton_Click(object sender, EventArgs e) //stop button pressed
        {
            CurrentSong.Stop(); //stop the currently playing song
        }

        private void DragandPlay_Deactivate(object sender, EventArgs e) //when this form is closed
        {
            Program.DiscordClientThread.Abort(); //abort the discord client thread
        }

        private void YoutubeGetButton_Click(object sender, EventArgs e)
        {
            GetYoutube.RunWorkerAsync();
        }

        private void GetYoutube_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            YoutubeMusic NextSong = new YoutubeMusic(YoutubeBox.Text); //create a new song to hold the data and allow us to play it
            if (CurrentSong != null) //if there is a current song to stop
            { 
                CurrentSong.Stop(); //stop the currently playing song
            }
            CurrentSong = NextSong;
            SongPlayer.RunWorkerAsync(); //run the song playing worker
        }
    }
}
