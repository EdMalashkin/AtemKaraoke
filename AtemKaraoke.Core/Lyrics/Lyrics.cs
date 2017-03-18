using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using AtemKaraoke.Core.Tools;
using System.IO;

namespace AtemKaraoke.Core
{
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
					string[] songs = Regex.Split(_text, Config.Default.SongsSplitter);
					for (int i = 0; i < songs.Length; i++)
					{
						Song s = new Song(songs[i], i + 1);
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
                foreach (var s in Songs)
                {
                    list.AddRange(s.VerseFiles);
                }
                return list;
            }
        }

        public string Name
        {
            get
            {
                return Songs.First().Name;
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
            return Path.GetDirectoryName(filePath);
        }

        public void Send()
        {
            string filePath;
            foreach (var f in VerseFiles)
            {
                filePath = f.Save();
                Switcher.UploadMedia(filePath, f.Verse.Number);
            }
        }

        private int _selectedNumber = -1;
        public void Select(VerseFile newVerseFile)
        {
            if (_selectedNumber != newVerseFile.Verse.Number)
            {
                _selectedNumber = newVerseFile.Verse.Number;
                Switcher.SetMediaToPlayer(_selectedNumber);
            }
        }
    }
}
