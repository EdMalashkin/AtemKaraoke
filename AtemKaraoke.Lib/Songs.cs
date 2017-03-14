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
		private string _Text;
		public Songs(string text)
		{
			_Text = text;
		}

        private List<Song> _List;
        public List<Song> List {
			get
			{
				if (_List == null)
				{
					_List = new List<Song>();
					string[] songs = Regex.Split(_Text, Config.Default.SongsSplitter);

					for (int i = 0; i < songs.Length; i++)
					{
						Song s = new Song(songs[i], i + 1);
						_List.Add(s);
					}
				}
				return _List;
			}
		}

        public List<Verse> Verses
        {
            get
            {
                List<Verse> list = new List<Verse>();
                foreach (var s in List)
                {
                    list.AddRange(s.Verses);
                }
                return list;
            }
        }

        public string Name
        {
            get
            {
                return List.First().Name;
            }
        }
    }
}
