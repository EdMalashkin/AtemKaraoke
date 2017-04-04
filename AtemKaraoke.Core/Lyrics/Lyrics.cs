using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using AtemKaraoke.Core.Tools;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AtemKaraoke.Core
{
    [Serializable]
    public class Lyrics : ILyrics
    {
		private string _text;
        // from form
        public Lyrics(string text)
        {
            _text = text;
        }

        // from unit test
        public Lyrics(string text, ISwitcher switcher)
		{
			_text = text;
            _switcher = switcher;
        }

        public Lyrics(List<Song> songs)
        {
            _songs = songs;
        }

        private ISwitcher _switcher;
        public ISwitcher Switcher
        {
            get
            {
                if (_switcher == null)
                {
                    if (Config.Default.EmulateSwitcher == true)
                    {
                        _switcher = new FakeSwitcher();
                    }
                    else
                    {
                        _switcher = new ComSwitcher();
                    }
                }
                return _switcher;
            }
        }

        private List<Song> _songs;
        public List<Song> Songs {
			get
			{
				if (_songs == null)
				{
					_songs = new List<Song>();
					string[] songs = Regex.Split(_text.Trim(), Config.Default.SongSplitterInEditor);
					for (int i = 0; i < songs.Length; i++)
					{
						Song s = new Song(this, songs[i], i + 1);
						_songs.Add(s);
					}
				}
				return _songs;
			}
		}

        public List<VerseFile> VerseFiles
        {
            get
            {
                List<VerseFile> list = new List<VerseFile>();
                Songs.ForEach(s => list.AddRange(s.VerseFiles));

                int i = 0;
                list.ForEach(f => f.GlobalNumber = i++);

                return list;
            }
        }

        string _name;
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    string name = Songs.First().Text;
                    if (name.Length > Config.Default.FileNameLength)
                        name = name.Substring(0, 20);
                    _name = name;
                }
                return _name;
            }
        }

        public Config Configuration
        {
            get
            {
                return Config.Default;
            }
        }

        public string Save()
        {
            string filePath = "";
            foreach (var f in VerseFiles)
            {
                filePath = f.Save();
            }
            return Directory.GetParent(Directory.GetParent(filePath).FullName).FullName;
        }

        public void Send()
        {
            foreach (var f in VerseFiles)
            {
                Console.WriteLine(f.FilePath);
                Switcher.UploadMedia(f.FilePath, f.GlobalNumber);
            }
        }

        public void Send(int verseNumber)
        { // the method is not a good practice - could be removed when avoidance of "console workaround" is found
            if (verseNumber >= 0)
            {
                var verseToSend = VerseFiles.Single(v => v.GlobalNumber == verseNumber);
                if (verseToSend != null)
                {
                    Console.WriteLine(verseToSend.FilePath);
                    Switcher.UploadMedia(verseToSend.FilePath, verseToSend.GlobalNumber);
                }
                else
                {
                    throw new Exception("Cannot find the verse");
                }
            }
            else
            {
                Send();
            }
        }

        private int _selectedNumber = -1;
        public void Select(VerseFile newVerseFile)
        {
            if (_selectedNumber != newVerseFile.GlobalNumber)
            {
                _selectedNumber = newVerseFile.GlobalNumber;
                Switcher.SetMediaToPlayer(_selectedNumber);
            }
        }

        public string GetSongSplitter()
        {
            return String.Concat(Enumerable.Repeat(Environment.NewLine, Config.Default.SongSplitterInPresenter));
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var s in Songs)
            {
                result.Append(s.ToString() + GetSongSplitter());
            }
            return result.ToString().Trim();
        }

        public override bool Equals(object obj)
        {
            var lyr = obj as Lyrics;
            bool res = (lyr != null
                        && lyr.Songs.Count == this.Songs.Count
                        && lyr.ToString() == this.ToString()
                        && lyr.VerseFiles.SequenceEqual(this.VerseFiles)
                        );
            return res;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
