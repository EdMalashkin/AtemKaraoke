using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Text;

namespace AtemKaraoke.Core
{
    [Serializable]
    public class Lyrics : ILyrics
    {
		private string _text;

        private Lyrics()
        {
            _selection = new Selection(this);
        }

        // from form
        public Lyrics(string text) : this()
        {
            _text = text;
        }

        // from unit test
        public Lyrics(string text, ISwitcher switcher) : this()
        {
			_text = text;
            _switcher = switcher;
        }

        public Lyrics(List<Song> songs) : this()
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
			set
			{
				_switcher = value;
			}
        }

        private List<Song> _songs;
        public List<Song> Songs {
			get
			{
				if (_songs == null)
				{
					_songs = new List<Song>();
					string[] songs = Regex.Split(_text.Trim(), Config.Default.SongSplitter);
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

                int selectionNumber = 1;
                int absoluteNumber = 1;
                list.ForEach(f => 
                {
                    if (!f.Verse.IsCommentOnly) f.NumberToSelect = selectionNumber++;
                    f.NumberInGrid = absoluteNumber++;
                });

                return list;
            }
        }

        public List<VerseFile> VerseFilesSelectable
        {
            get
            {
                return VerseFiles.Where(v => v.NumberToSelect > 0).ToList();
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

        private Selection _selection;
        public Selection Selection
        {
            get
            {
                return _selection;
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
                Switcher.UploadMedia(f.FilePath, f.NumberToSelect.Value);
            }
        }

        public void SendSelected()
        {
            if (Selection.CurrentVerse != null)
            {
                Console.WriteLine(Selection.CurrentVerse.FilePath);
                Switcher.UploadMedia(Selection.CurrentVerse.FilePath, Selection.CurrentVerse.NumberToSelect.Value);
            }
            else
            {
                throw new Exception("No selected verse");
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
                result.Append(s.ToString());
                result.Append(GetSongSplitter());
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
