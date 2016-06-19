using System.Collections.Generic;
using System.Text.RegularExpressions;
using AtemKaraoke.Lib.Tools;

namespace AtemKaraoke.Lib
{
	public class Song : Verse
	{
		public Song(string FilePath)
		{
			Text = FileHelper.GetTextFromFile(FilePath);

			string[] verses = Regex.Split(Text, Config.Default.Splitter);
			for (int i = 0; i < verses.Length; i++)
			{
				Verse v = new Verse();
				v.Text = verses[i].Trim();
				v.Number = i + 1;

				Verses.Add(v);
			}
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
				if (_Verses == null) _Verses = new List<Verse>();
				return _Verses;
			}
			set
			{
				_Verses = value;
			}
		}
	}







	public class Verse
	{
		protected string _Text;
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
	}
}