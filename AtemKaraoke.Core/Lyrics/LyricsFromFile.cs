using AtemKaraoke.Core.Tools;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtemKaraoke.Core
{   // decorator over Lyrics to load its content from files
    public class LyricsFromFile : ILyrics
    {
        string _path;
        Lyrics _lyrics;

        public LyricsFromFile()
        {
            _path = Config.Default.SourceFolder;
        }
        public LyricsFromFile(string path)
        {
            _path = path;
        }

        private Lyrics Lyrics
        {
            get
            {
                if (_lyrics == null) _lyrics = CreateLyrics();
                return _lyrics;
            }
        }

        private Lyrics CreateLyrics()
        {
            Lyrics lyr;
            if (System.IO.File.Exists(_path))
            {
                lyr = CreateLyricsFromFile();
            }
            else
            {
                lyr = CreateLyricsFromFolder();
            }
            return lyr;
        }

        private Lyrics CreateLyricsFromFile()
        {
            List<Song> songs = new List<Song>();
            Song s = new Song(_path, 1);
            songs.Add(s);
            return new Lyrics(songs);
        }

        private Lyrics CreateLyricsFromFolder()
        {
            IOrderedEnumerable<string> files = FileHelper.GetAllFilesList(_path, Config.Default.SourceFolderPattern);
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
