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
        public readonly bool IsRefrain = false;

        public Verse(Song s, string text, int number)
        {
            _song = s;
            _dirtyText = text;
            _number = number;
            IsRefrain = text.Contains("*");
        }

        public Song Song
        {
            get
            {
                return _song;
            }
        }

        private string _dirtyText;
        private string _cleanedText;
        public string Text
		{
			get
			{
                if (string.IsNullOrEmpty(_cleanedText))
                {
                    _cleanedText = CleanText(_dirtyText);
                }
                return _cleanedText;
			}
		}

        public bool Update(string newValue)
        {
            bool result = false;
            if (IsEdited(newValue))
            {
                _dirtyText = newValue;
                _cleanedText = CleanText(newValue);
                result = true;
            }
            return result;
        }

        private bool IsEdited(string newText)
        {
            return (_dirtyText.Trim() != newText.Trim());
        }

        private int _number;
		public int Number
		{
			get
			{
				return _number;
			}
		}

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
    }
}
