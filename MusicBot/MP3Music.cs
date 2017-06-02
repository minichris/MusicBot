using TagLib;

namespace MusicBot
{
    class MP3Music : Music
    {
        public MP3Music(string FileLocation)
        {
            FilePath = FileLocation;

            File tagFile = File.Create(FileLocation);
            Name = $"{tagFile.Tag.Title} by {tagFile.Tag.FirstPerformer}";
        }
    }
}
