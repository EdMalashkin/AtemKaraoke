using System.Collections.Generic;
using System.Text.RegularExpressions;
using AtemKaraoke.Core.Tools;
using System;
using System.Text;

namespace AtemKaraoke.Core
{
	public class Song
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

        private string _Text;
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value.Trim();
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

        private string _Name;
		public string Name
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

		private List<VerseFile> _VerseFiles;
		public List<VerseFile> VerseFiles
		{
			get
			{
                if (_VerseFiles == null)
                {
                    _VerseFiles = new List<VerseFile>();
                    string[] verses = Regex.Split(Text, Config.Default.Splitter);
                    int accumulatedLength = 0;
                    int maxArray = Config.Default.MaxAmountOfVerses;
                    int versesCount = (verses.Length >= maxArray) ? maxArray : verses.Length;
                    for (int i = 0; i < versesCount; i++)
                    {
                        var v = new VerseFile(new VerseDrawing(new Verse(   this, 
                                                                            verses[i], 
                                                                            i+1,
                                                                            accumulatedLength)
                                                                            )
                                             );
                        _VerseFiles.Add(v);
                        accumulatedLength += v.Verse.Text.Length;
                    }
                }
				return _VerseFiles;
			}
			set
			{
				_VerseFiles = value;
			}
		}

        private Verse SelectedVerse;
        public void SelectVerse(Verse newVerse)
        {
            if (newVerse == null) return;
            if (SelectedVerse != null && SelectedVerse.Number == newVerse.Number) return;

            SelectedVerse = newVerse;

            //VerseSelectedEventArgs e = new VerseSelectedEventArgs();
            //e.SelectionNumber = newVerse.Number;
            //e.SelectionStart = newVerse.StartPosition;
            //e.SelectionLength = newVerse.EndPosition;
            //OnVerseSelected(this, e);
        }

        //public int SelectVerse(int curPosition)
        //{
        //    string splitter = Config.Default.Splitter;
        //    int resultNumber = -1;
        //    int selectionStart = Text.LastIndexOf(splitter, curPosition);
        //    int selectionLength = Text.IndexOf(splitter, curPosition);

        //    if (curPosition < Text.Length)
        //    {
        //        int startIndex = curPosition - splitter.Length / 2;
        //        if (startIndex < 0) return -1;
        //        string curTextToCheckIfSplitter = Text.Substring(startIndex, splitter.Length);
        //        if (splitter == curTextToCheckIfSplitter) return -1;
        //    }

        //    if (selectionStart == -1)
        //        selectionStart = 0;
        //    if (selectionLength == -1)
        //        selectionLength = Text.Length;

        //    selectionLength = selectionLength - selectionStart;

        //    foreach (var v in VerseFiles)
        //    {
        //        if (v.Verse.StartPosition == selectionStart)
        //        {
        //            SelectVerse(v.Verse);

        //            resultNumber = v.Verse.Number;
        //            break;
        //        }
        //    }

        //    return resultNumber;
        //}

        //public event VerseSelectedEventHandler VerseSelected;
        //private void OnVerseSelected(object sender, VerseSelectedEventArgs e)
        //{
        //    VerseSelected(this, e);
        //}

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