using System;
using System.Text.RegularExpressions;
using AtemKaraoke.Lib.Tools;

namespace AtemKaraoke.Lib
{
	public class Song
	{
		private string _Text;
		private string _Name;
		private int _Number;

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

		public string[] Chunks
		{
			get
			{
				string[] chunks = Regex.Split(Text, Config.Default.Splitter);
				for (int i = 0; i < chunks.Length; i++) chunks[i] = chunks[i].Trim();
				return chunks;
			}
		}
	}
}