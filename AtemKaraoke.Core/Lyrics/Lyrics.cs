using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using AtemKaraoke.Core.Tools;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

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

                int i = 1;
                list.ForEach(f => f.GlobalNumber = i++);

                return list;
            }
        }

        private VerseFile _previouslySelectedVerse;
        private VerseFile GetNextToPreviouslySelectedVerse()
        {
            return VerseFiles.Where(v => _previouslySelectedVerse != null 
                                    && v.Verse.Song == _previouslySelectedVerse.Verse.Song
                                    && v.LyricsIndexBasedOnZero == _previouslySelectedVerse.LyricsIndexBasedOnZero + 1)
                             .FirstOrDefault();
        }

        private List<VerseFile> GetPrevKeyVerses()
        {
            return new List<VerseFile>() {
                GetNextToPreviouslySelectedVerse(),
                SelectedVerse.Verse.Song.PrevRefrain
            }
            .Where(v => v != null && v.LyricsIndexBasedOnZero < SelectedVerse.LyricsIndexBasedOnZero)
            .OrderBy(v => v.LyricsIndexBasedOnZero)
            .ToList();
        }

        private List<VerseFile> GetNextKeyVerses()
        {
            return new List<VerseFile>() {
                GetNextToPreviouslySelectedVerse(),
                SelectedVerse.Verse.Song.NextRefrain
            }
            .Where(v => v!=null && v.LyricsIndexBasedOnZero > SelectedVerse.LyricsIndexBasedOnZero)
            .OrderBy(v => v.LyricsIndexBasedOnZero)
            .ToList();
        }

        public void SelectPrevKeyVerse()
        {
            Select(GetPrevKeyVerses().FirstOrDefault());
        }

        public void SelectNextKeyVerse()
        {
            Select(GetNextKeyVerses().FirstOrDefault());
        }

        public void SelectFirstVerse()
        {
            Select(VerseFiles.FirstOrDefault());
        }

        public void SelectLastVerse()
        {
            Select(VerseFiles.Last());
        }

        public void SelectPrevVerse()
        {
            Select(PrevVerseFile);
        }

        public void SelectNextVerse()
        {
            Select(NextVerseFile);
        }

        private VerseFile PrevVerseFile
        {
            get
            {
                return VerseFiles.Find(v => v.GlobalNumber == SelectedVerse.GlobalNumber - 1);
            }
        }

        private VerseFile NextVerseFile
        {
            get
            {
                return VerseFiles.Find(v => v.GlobalNumber == SelectedVerse.GlobalNumber + 1);
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

        public void SendSelected()
        {
            if (SelectedVerse != null)
            {
                Console.WriteLine(SelectedVerse.FilePath);
                Switcher.UploadMedia(SelectedVerse.FilePath, SelectedVerse.GlobalNumber);
            }
            else
            {
                throw new Exception("No selected verse");
            }
        }

        private VerseFile _selectedVerse;
        public VerseFile SelectedVerse
        {
            get
            {
                return _selectedVerse;
            }
        }

        private void Select(VerseFile refraineVerse, VerseFile cachedVerse)
        {

        }

        public void Select(VerseFile newVerseFile)
        {
            if (newVerseFile != null && _selectedVerse != newVerseFile)
            {
                _previouslySelectedVerse = _selectedVerse;
                _selectedVerse = newVerseFile;

                Switcher.SetMediaToPlayer(newVerseFile.GlobalNumber);

                if (_previouslySelectedVerse == null)
                {
                    Debug.Print("Verse index {0} is selected",
                                                        newVerseFile.LyricsIndexBasedOnZero);
                }
                else
                {
                    Debug.Print("Verse index {0} is selected after {1}",
                                                        newVerseFile.LyricsIndexBasedOnZero,
                                                        _previouslySelectedVerse.LyricsIndexBasedOnZero);
                }

                //if (OnVerseSelected != null)
                //{
                //    OnVerseSelected(newVerseFile, null);
                //}
                OnVerseSelected?.Invoke(newVerseFile, null);
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

        public event EventHandler OnVerseSelected;
    }
}
