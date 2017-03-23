using AtemKaraoke.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtemKaraoke.Core
{   // decorator over Lyrics to load its content from files
    public class LyricsFromFile : ILyrics
    {
        string _folder;
        Lyrics _lyrics;

        public LyricsFromFile()
        {
            _folder = Config.Default.SourceFolder;
        }
        public LyricsFromFile(string folder)
        {
            _folder = folder;
        }

        private Lyrics Lyrics
        {
            get
            {
                if (_lyrics == null) _lyrics = CreateLyricsFromFiles();
                return _lyrics;
            }
        }

        private Lyrics CreateLyricsFromFiles()
        {
            IOrderedEnumerable<string> files = FileHelper.GetAllFilesList(_folder, Config.Default.SourceFolderPattern);
            List<Song> songs = new List<Song>();

            int fileNumber = 0;
            foreach (string file in files)
            {
                Song s = new Song(file);
                s.Number = fileNumber++;
                songs.Add(s);
            }
            return new Lyrics(songs);
        }

        public string Save()
        {
            return Lyrics.Save();
        }

        public void Send()
        {
            Lyrics.Send();
        }

        public void Select(VerseFile newVerseFile)
        {
            Lyrics.Select(newVerseFile);
        }
    }
}
