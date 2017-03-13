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
		public Song(string songText, int songNumber)
		{
			Text = songText;
			Number = songNumber;
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
                    int versesCount = (verses.Length > 20) ? 20 : verses.Length;
                    for (int i = 0; i < versesCount; i++)
                    {
                        Verse v = new Verse();

                        v.Text = verses[i];
                        v.Number = i + 1;
                        v.StartPosition = Text.IndexOf(v.Text, accumulatedLength);

                        //if (v.Number == 21)
                        //{
                        //    throw new Exception("ATEM cannot recieve more than 20 images");
                        //}

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

        private Verse SelectedVerse;

        public void SelectVerse(Verse newVerse)
        {
            if (newVerse == null) return;
            if (SelectedVerse != null && SelectedVerse.Number == newVerse.Number) return;

            SelectedVerse = newVerse;

            VerseSelectedEventArgs e = new VerseSelectedEventArgs();
            e.SelectionNumber = newVerse.Number;
            e.SelectionStart = newVerse.StartPosition;
            e.SelectionLength = newVerse.EndPosition;
            OnVerseSelected(this, e);
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
                    SelectVerse(v);

                    resultNumber = v.Number;
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

}