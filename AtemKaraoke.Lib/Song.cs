using System;
using System.Text.RegularExpressions;
using AtemKaraoke.Lib.Tools;

namespace AtemKaraoke.Lib
{
	public class Song
	{
		private string _Text;
		private string _Name;

		public Song(string FilePath)
		{
			_Text = FileHelper.GetTextFromFile(FilePath);
		}

		public string Text
		{
			get
			{
				return _Text;
			}
			set
			{
				_Text = value;
			}
		}

		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty (_Name))
				{
					if (Text.Length > 40)
						_Name = Text.Substring(0, 40);
					else
						_Name = Text;
				}
					
				return _Name;
			}
			set
			{
				_Name = value;
			}
		}

		public string[] Chunks
		{
			get
			{
				return Regex.Split(Text, "\r\n\r\n");
			}
		}
	}
}