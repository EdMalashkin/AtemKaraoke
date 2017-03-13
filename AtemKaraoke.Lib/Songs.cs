using System;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using AtemKaraoke.Lib.Tools;
using SwitcherLib;
using System.Threading;
using System.Text.RegularExpressions;
using System.Linq;

namespace AtemKaraoke.Lib
{
	public class Songs
	{
		private string _songsText;
		private List<Song> _songsList;

		public Songs(string text)
		{
			_songsText = text;
		}

		public List<Song> List {
			get
			{
				if (_songsList == null)
				{
					_songsList = new List<Song>();
					string[] songs = Regex.Split(_songsText, Config.Default.SongsSplitter);

					for (int i = 0; i < songs.Length; i++)
					{
						Song s = new Song(songs[i], i + 1);
						_songsList.Add(s);
					}
				}
				return _songsList;
			}
		}

		public Song Current
		{
			get
			{
				return List.First(); //temp
			}
		}
	}
}
