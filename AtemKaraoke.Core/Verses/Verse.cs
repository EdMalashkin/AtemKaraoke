using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AtemKaraoke.Core
{
    [Serializable]
    public class Verse
	{
        private Song _song;
        private int _accumulatedLength; // may be deleted I think
        public readonly bool IsRefrain = false;
        public Verse(Song s, string text, int number, int accumulatedLength)
        {
            _song = s;
            _accumulatedLength = accumulatedLength;
            IsRefrain = text.Contains("*");
            Text = text;
            Number = number;
        }

        public Song Song
        {
            get
            {
                return _song;
            }
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
				_Text = CleanText(value);
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

  //      public int StartPosition
		//{
		//	get
  //          {
  //              return Song.Text.IndexOf(this.Text, _accumulatedLength);
  //          }
		//}

		//public int EndPosition
		//{
		//	get
		//	{
		//		return StartPosition + Text.Length;
		//	}
		//}

        private string CleanText(string text)
		{
			string[] rows = Regex.Split(text, Environment.NewLine);
			string newText = "";
			foreach (string r in rows)
			{
				newText += r.Trim() + Environment.NewLine;
			}

            //http://stackoverflow.com/questions/206717/how-do-i-replace-multiple-spaces-with-a-single-space-in-c
            Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
            newText = regex.Replace(newText, " ");

            newText = newText.Replace("*", ""); //refrain symbol removal
            return newText.Trim();
		}

        public override string ToString()
        {
            string result = Text;
            if (IsRefrain) result = "*" + result;
            return result.Trim();
        }

        public bool Update(string newValue)
        {
            bool result = false;
            if (IsEdited(newValue))
            {
                Text = newValue;
                result = true;
            }
            return result;
        }

        private bool IsEdited(string newValue)
        {
            return (Text.Trim() != newValue.Trim());
        }
    }
}
