using System.Collections.Generic;
using System.Text.RegularExpressions;
using AtemKaraoke.Lib.Tools;
using System;

namespace AtemKaraoke.Lib
{
	public class Song : Verse
	{
        public Song() {  }

        public Song(string FilePath) 
		{
			Text = FileHelper.GetTextFromFile(FilePath);
        }

		private string _Name;
		public new string Name
		{
			get
			{
				if (string.IsNullOrEmpty (_Name))
				{
					if (Text.Length > Config.Default.FileNameLength)
						_Name = Text.Substring(0, Config.Default.FileNameLength);
					else
						_Name = Text;

					_Name = string.Format("{0} {1}", Number, _Name).Trim();
				}
					
				return _Name;
			}
			set
			{
				_Name = value;
			}
		}

		private List<Verse> _Verses;
		public List<Verse> Verses
		{
			get
			{
                if (_Verses == null)
                {
                    _Verses = new List<Verse>();
                    string[] verses = Regex.Split(Text, Config.Default.Splitter);
                    int accumulatedLength = 0;
                    for (int i = 0; i < verses.Length; i++)
                    {
                        Verse v = new Verse();

                        v.Text = verses[i];
                        v.Number = i + 1;
                        v.StartPosition = Text.IndexOf(v.Text, accumulatedLength);

                        _Verses.Add(v);
                        accumulatedLength += v.Text.Length;
                    }
                }
				return _Verses;
			}
			set
			{
				_Verses = value;
			}
		}

        

        public int SelectVerse(int curPosition)
        {
            string splitter = Config.Default.Splitter;
            int resultNumber = -1;
            int selectionStart = Text.LastIndexOf(splitter, curPosition);
            int selectionLength = Text.IndexOf(splitter, curPosition);

            if (curPosition < Text.Length)
            {
                int startIndex = curPosition - splitter.Length / 2;
                if (startIndex < 0) return -1;
                string curTextToCheckIfSplitter = Text.Substring(startIndex, splitter.Length);
                if (splitter == curTextToCheckIfSplitter) return -1;
            }

            if (selectionStart == -1)
                selectionStart = 0;
            if (selectionLength == -1)
                selectionLength = Text.Length;

            selectionLength = selectionLength - selectionStart;

            foreach (Verse v in Verses)
            {
                if (v.StartPosition == selectionStart)
                {
                    VerseSelectedEventArgs e = new VerseSelectedEventArgs();
                    e.SelectionNumber = v.Number;
                    e.SelectionStart = v.StartPosition;
                    e.SelectionLength = v.EndPosition;
                    resultNumber = v.Number;
                    OnVerseSelected(this, e);
                    break;
                }
            }

            return resultNumber;
        }

        public event VerseSelectedEventHandler VerseSelected;
        private void OnVerseSelected(object sender, VerseSelectedEventArgs e)
        {
            VerseSelected(this, e);
        }
    }

    public delegate void VerseSelectedEventHandler(object source, VerseSelectedEventArgs args);
    public class VerseSelectedEventArgs : EventArgs
    {
        public int SelectionStart { get; set; }
        public int SelectionLength { get; set; }
        public int SelectionNumber { get; set; }
    }



    public class Verse
	{
		private string _Text;
		public string Text
		{
			get
			{
				return _Text;
			}
			set
			{
				_Text = TrimRows(value);
			}
		}

		private string _Name;
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
			}
		}

		private int _Number;
		public int Number
		{
			get
			{
				return _Number;
			}
			set
			{
				_Number = value;
			}
		}

        private int _startPosition;
        public int StartPosition
        {
            get
            {
                return _startPosition;
            }
            set
            {
                _startPosition = value;
            }
        }

        public int EndPosition
        {
            get
            {
                return StartPosition + Text.Length;
            }
        }

        private string _FilePath;
		public string FilePath
		{
			get
			{
				return _FilePath;
			}
			set
			{
				_FilePath = value;
			}
		}

        private string TrimRows(string text)
        {
            string[] rows = Regex.Split(text, Environment.NewLine);
            string newText = "";
            foreach (string r in rows)
            {
                newText += r.Trim() + Environment.NewLine;
            }
            return newText.Trim();
        }
    }
}