using System.Collections.Generic;
using System.Text.RegularExpressions;
using AtemKaraoke.Core.Tools;
using System;
using System.Text;
using System.Linq;

namespace AtemKaraoke.Core
{
    [Serializable]
    public class Song
	{
        public Song() {  }
        private Lyrics _lyrics;

        public Song(Lyrics lyrics, string filePath)
		{
            _text = FileHelper.GetTextFromFile(filePath);
            _lyrics = lyrics;
        }

        public Song(Lyrics lyrics, string songText, int songNumber)
		{
            _text = songText.Trim();
            _number = songNumber;
            _lyrics = lyrics;
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
        }

        private int _number;
        public int Number
        {
            get
            {
                return _number;
            }
        }

        private string _name;
		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty (_name))
				{
					if (Text.Length > Config.Default.FileNameLength)
						_name = Text.Substring(0, Config.Default.FileNameLength);
					else
						_name = Text;

					_name = string.Format("{0} {1}", Number, _name).Trim();
				}
					
				return _name;
			}
		}

        string _title;
        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    string[] rows = Regex.Split(Text, Environment.NewLine);
                    if (rows.Length > 0)
					{
                        _title = rows.Where(r => !r.TrimStart().StartsWith(Config.Default.CommentSign)).FirstOrDefault();
					}
                }
                return _title.Replace(Config.Default.RefrainSign, "");
            }
        }

        private List<VerseFile> _verseFiles;
		public List<VerseFile> VerseFiles
		{
			get
			{
                if (_verseFiles == null)
                {
                    _verseFiles = new List<VerseFile>();
                    string[] verses = Regex.Split(Text, Config.Default.VerseSplitter);
                    int maxArray = Config.Default.MaxAmountOfVerses;
                    int versesCount = (verses.Length >= maxArray) ? maxArray : verses.Length;
                    for (int i = 0; i < versesCount; i++)
                    {
                        var v = new VerseFile(new VerseDrawing(new Verse(   this, 
                                                                            verses[i], 
                                                                            i+1)
                                                                            ),
                                                               _lyrics
                                             );
                        _verseFiles.Add(v);
                    }
                }
				return _verseFiles;
			}
		}

        private List<VerseFile> VerseFilesSelectable
        {
            get
            {
                return VerseFiles.Where(v => v.NumberToSelect > 0).ToList();
            }
        }

        public VerseFile FirstVerseFile
        {
            get
            {
                return VerseFilesSelectable[0];
            }
        }

        public VerseFile LastVerseFile
        {
            get
            {
                return VerseFilesSelectable[VerseFilesSelectable.Count - 1];
            }
        }

        public VerseFile NextRefrain
        {
            get
            {
                var result = _lyrics.Selection.CurrentVerse;
                int curIndex = GetCurrentSelectedVerseIndex();
                if (curIndex >= 0)
                {
                    for (int i = curIndex + 1; i < VerseFilesSelectable.Count; i++)
                    {
                        if (VerseFilesSelectable[i].Verse.IsRefrain)
                        {
                            result = VerseFilesSelectable[i];
                            break;
                        }
                    }
                }
                return result;
            }
        }

        public VerseFile PrevRefrain
        {
            get
            {
                var result = _lyrics.Selection.CurrentVerse;
                int curIndex = GetCurrentSelectedVerseIndex();
                if (curIndex >= 0)
                {
                    for (int i = curIndex - 1; i >= 0; i--)
                    {

                        if (VerseFilesSelectable[i].Verse.IsRefrain
                            && (    i == 0
                                    ||  VerseFilesSelectable[i-1] == null 
                                    || !VerseFilesSelectable[i-1].Verse.IsRefrain)  // refrain may consists of several verses
                                )
                        {
                            result = VerseFilesSelectable[i];
                            break;
                        }

                    }
                }
                return result;
            }
        }

        private int GetCurrentSelectedVerseIndex()
        {
            return VerseFilesSelectable.FindIndex(v => v.NumberToSelect == _lyrics.Selection.CurrentVerse.NumberToSelect && _lyrics.Selection.CurrentVerse.Verse.Song == this);
        }

        private string GetVerseSplitter()
        {
            //string newLine = "\r\n";
            ////int count = Regex.Matches(Config.Default.Splitter, newLine).Count;
            //int count = Regex.Split(Config.Default.Splitter, newLine).Length - 1;
            //return String.Concat(Enumerable.Repeat(Environment.NewLine, count));
            return String.Concat(Enumerable.Repeat(Environment.NewLine, 2)); // to do - make 2 dynamic
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var v in VerseFiles)
            {
                result.Append(v.Verse.ToString());
                result.Append(GetVerseSplitter());
            }
            return result.ToString().Trim();
        }

		public int GetFirstCharPosition()
		{
			return _lyrics.ToString().IndexOf(this.ToString());
		}

		public int GetLastCharPosition()
		{
			return _lyrics.ToString().LastIndexOf(this.ToString());
		}
	}
}