using AtemKaraoke.Core.Tools;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AtemKaraoke.Core
{   // decorator over Lyrics to load its content from binary files
    [Serializable]
    public class BinaryFileLyrics : ILyrics
    {
        string _path;
        Lyrics _lyrics;

        public BinaryFileLyrics()
        {
            _path = Config.Default.SourceFolder;
        }
        public BinaryFileLyrics(string path)
        {
            _path = path;
        }
        public BinaryFileLyrics(string path, Lyrics lyrics)
        {
            _lyrics = lyrics;
            _path = path;
        }
        public BinaryFileLyrics(Lyrics lyrics)
        {
            _lyrics = lyrics;
        }

        public Lyrics Lyrics
        {
            get
            {
                if (_lyrics == null) _lyrics = CreateLyricsFromBinaryFile();
                return _lyrics;
            }
        }

        private Lyrics CreateLyricsFromBinaryFile()
        {
            Lyrics lyr;
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(_path,
                                                  FileMode.Open,
                                                  FileAccess.Read,
                                                  FileShare.Read))
            {
                lyr = (Lyrics)formatter.Deserialize(stream);
                stream.Close();
            }

            return lyr;
        }

        private string SaveLyricsToBinaryFile()
        {
            _path = Path.GetTempFileName();
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(_path,
                                                    FileMode.Create,
                                                    FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, Lyrics);
                stream.Close();
            }

            return _path;
        }

        public string Save()
        {
            Lyrics.Save();
            return SaveLyricsToBinaryFile();
        }

        public void Send()
        {
            Lyrics.Send();
        }

        public void Send(int verseNumber)
        {
            Lyrics.Send(verseNumber);
        }

        public void Select(VerseFile newVerseFile)
        {
            Lyrics.Select(newVerseFile);
        }
    }
}
