﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using AtemKaraoke.Core.Tools;
using System;
using System.Text;

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

        public Verse FirstVerse
        {
            get
            {
                return VerseFiles[0].Verse;
            }
        }

        public Verse LastVerse
        {
            get
            {
                return VerseFiles[VerseFiles.Count - 1].Verse;
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var v in VerseFiles)
            {
                result.Append(v.Verse.ToString());
                result.Append(Environment.NewLine);
                result.Append(Environment.NewLine);
            }
            return result.ToString().Trim();
        }
    }

    public delegate void VerseSelectedEventHandler(object source, VerseSelectedEventArgs args);
    public class VerseSelectedEventArgs : EventArgs
    {
        public int SelectionStart { get; set; }
        public int SelectionLength { get; set; }
        public int SelectionNumber { get; set; }
    }

}