using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using NAudio.Wave;

namespace MusicBot
{
    static class MusicPlayer
    {
        public static IAudioClient _vClient;
        private static MediaFoundationResampler resampler;
        private static MediaFoundationReader mediaStream;
        public static int SampleRate;

        public static void Play(Music MusicToPlay)
        {
            var channelCount = Program._client.GetService<AudioService>().Config.Channels; // Get the number of AudioChannels our AudioService has been configured to use.
            WaveFormat OutFormat = new WaveFormat(SampleRate, 16, channelCount); // Create a new Output Format, using the spec that Discord will accept, and with the number of channels that our client supports.
            try
            {
                using (mediaStream = new MediaFoundationReader(MusicToPlay.FilePath))
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
                        _vClient.Send(buffer, 0, blockSize); // Send the buffer to Discord
                    }
                }
            }
            catch (Exception ReaderException)
            {
                Console.WriteLine(ReaderException.Message); // Prints any errors to console
            }

            _vClient.Wait(); // Waits for the currently playing sound file to end.
        }

        public static void Stop()
        {
            mediaStream.Dispose();
            resampler.Dispose();
        }
    }
}
