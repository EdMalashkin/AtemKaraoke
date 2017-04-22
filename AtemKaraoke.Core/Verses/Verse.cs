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

        public Verse(Song s, string text, int number)
        {
            _song = s;
            _dirtyText = text;
            _number = number;
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
                if (IsRefrain)
                {
                    _dirtyText = Config.Default.RefrainSign + newValue;
                }
                else
                {
                    _dirtyText = newValue;
                }
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
            StringBuilder newText = new StringBuilder();
            char[] charsToTrim = {' ', '\t' };
            string[] rows = Regex.Split(text, Environment.NewLine);

			foreach (string r in rows)
			{
                newText.Append(r.Trim(charsToTrim));
                newText.Append(Environment.NewLine);
			}
            return CutRefrainSign(MultipleSpacesToSingle(newText.ToString().TrimEnd()));
        }

        private string MultipleSpacesToSingle(string str)
        {
            //http://stackoverflow.com/questions/206717/how-do-i-replace-multiple-spaces-with-a-single-space-in-c
            Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
            return regex.Replace(str, " ");
        }

        private string CutRefrainSign(string str)
        {
            return str.Replace(Config.Default.RefrainSign, ""); //refrain symbols removal;
        }

        public bool IsRefrain
        {
            get
            {
                return _dirtyText.Contains(Config.Default.RefrainSign);
            }
        }

        internal bool IsCommentOnly
        {
            get
            {
                bool anyContent = false;
                string[] rows = Regex.Split(Text, Environment.NewLine);
                for (int i = 0; i < rows.Count(); i++)
                {
                    if (!rows[i].TrimStart().StartsWith(Config.Default.CommentSign))
                    {
                        anyContent = true;
                        break;
                    }
                }
                return !anyContent;
            }
        }
        public override string ToString()
        {
            return _dirtyText;
        }
    }
}
