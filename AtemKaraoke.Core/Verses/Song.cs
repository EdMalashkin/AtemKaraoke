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

		private List<VerseFile> _verseFiles;
		public List<VerseFile> VerseFiles
		{
			get
			{
                if (_verseFiles == null)
                {
                    _verseFiles = new List<VerseFile>();
                    string[] verses = Regex.Split(Text, Config.Default.Splitter);
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

        public VerseFile FirstVerseFile
        {
            get
            {
                return VerseFiles[0];
            }
        }

        public VerseFile LastVerseFile
        {
            get
            {
                return VerseFiles[VerseFiles.Count - 1];
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
                    for (int i = curIndex + 1; i < VerseFiles.Count; i++)
                    {
                        if (VerseFiles[i].Verse.IsRefrain)
                        {
                            result = VerseFiles[i];
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

                        if (VerseFiles[i].Verse.IsRefrain
                            && (    i == 0
                                    ||  VerseFiles[i-1] == null 
                                    || !VerseFiles[i-1].Verse.IsRefrain)  // refrain may consists of several verses
                                )
                        {
                            result = VerseFiles[i];
                            break;
                        }

                    }
                }
                return result;
            }
        }

        private int GetCurrentSelectedVerseIndex()
        {
            return VerseFiles.FindIndex(v => v.GlobalNumber == _lyrics.Selection.CurrentVerse.GlobalNumber && _lyrics.Selection.CurrentVerse.Verse.Song == this);
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
    }
}