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

namespace MusicBot
{
    class YoutubeMusic : Music
    {
        private VideoDownloader downloader;
        private TaskCompletionSource<bool> DownloadFinishedEvent = new TaskCompletionSource<bool>();
        private VideoInfo Videoinfo;

        public YoutubeMusic(string URL)
        {
            Console.WriteLine($"Opening / creating output folder at: {Program.OutputFolder}");
            Directory.CreateDirectory(Program.OutputFolder); // Create video folder if not found
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(URL);
            Videoinfo = videoInfos.First();
            FilePath = Path.Combine(Program.OutputFolder, GetMD5(Videoinfo.Title) + Videoinfo.AudioExtension); // Grabs path to downloaded song
            Console.WriteLine($"Trying to find song at path: {FilePath}");
            if (!File.Exists(FilePath)) // downloads video if it isn't cached
            {
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                DownloadVideo();
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private static string GetMD5(string inputString) //Creates a MD5 hash from a input string
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(inputString);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();
            return encoded;
        }

        private async Task DownloadVideo() //Downloads a Youtube Video
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
    }
}
