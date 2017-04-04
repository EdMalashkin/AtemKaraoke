using AtemKaraoke.Core.Tools;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtemKaraoke.Core
{   // decorator over Lyrics to load its content from text files
    [Serializable]
    public class TextFileLyrics : ILyrics
    {
        string _path;
        Lyrics _lyrics;

        public TextFileLyrics()
        {
            _path = Config.Default.SourceFolder;
        }
        public TextFileLyrics(string path)
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

        private Lyrics CreateLyrics(string songsText)
        {
            var lyr = new Lyrics(songsText); // like from the form
            return lyr;
        }

        private Lyrics CreateLyricsFromFile()
        {
            var songsText = new StringBuilder();
            songsText.Append(FileHelper.GetTextFromFile(_path).Trim());
            songsText.Append(GetSongSplitter());
            return CreateLyrics(songsText.ToString());
        }

        private Lyrics CreateLyricsFromFolder()
        {
            IOrderedEnumerable<string> files = FileHelper.GetAllFiles(_path, Config.Default.SourceFolderPattern);
            var songsText = new StringBuilder();

            foreach (var filePath in files)
            {
                songsText.Append(FileHelper.GetTextFromFile(filePath).Trim());
                songsText.Append(GetSongSplitter());
            }
            
            return CreateLyrics(songsText.ToString());
        }

        private string GetSongSplitter()
        {
            return String.Concat(Enumerable.Repeat(Environment.NewLine, Config.Default.SongSplitterInPresenter));
        }

        public string Save()
        {
            return Lyrics.Save();
        }

        public void Send()
        {
            Lyrics.Send();
        }

        public void SendSelected()
        {
            Lyrics.SendSelected();
        }

        public void Select(VerseFile newVerseFile)
        {
            Lyrics.Select(newVerseFile);
        }
    }
}
