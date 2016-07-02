using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using AtemKaraoke.Lib.Tools;
using SwitcherLib;
using System.Threading;
using System.Text.RegularExpressions;

namespace AtemKaraoke.Lib
{
	public class Controller
	{
		public List<Song> ConvertSongsToImages(string SourceFolder, string SearchPattern, string DestinationFolder)
		{
			string[] files = FileHelper.GetAllFilesList(SourceFolder, SearchPattern);
			List<Song> songs = new List<Song>();

			int fileNumber = 0;
			foreach (string file in files)
			{
				Song s = new Song(file);
				fileNumber++;
				s.Number = fileNumber;
				songs.Add(s);
			}

			ConvertSongsToImages(songs, DestinationFolder);
            //UploadSongsToSwitcher(songs[0]);
            //SetSongToPlayer(5);
            return songs;
        }

		public List<Song> ConvertSongsToImages()
		{
			return ConvertSongsToImages(Config.Default.SourceFolder, Config.Default.SourceFolderPattern, Config.Default.DestinationFolder);
		}

        public void ConvertSongsToImages(string SourceFolder)
        {
            ConvertSongsToImages(SourceFolder, Config.Default.SourceFolderPattern, Config.Default.DestinationFolder);
        }

        public string ConvertSongsToImages(Song song)
        {
            string newFolder = "";
            if (song == null) return newFolder;
            
            foreach (Verse verse in song.Verses)
            {
                verse.FilePath = GetImageFilePath(verse.Text, verse.Number, song.Name, Config.Default.DestinationFolder);
                Bitmap bmp = GetImage(verse.Text);
                bmp.Save(verse.FilePath, System.Drawing.Imaging.ImageFormat.Png);
                newFolder = Path.GetDirectoryName(verse.FilePath);
            }
            return newFolder;
        }

        public void ConvertSongsToImages(List<Song> songs, string destinationFolder)
		{
			string imageFilePath = "";
			try
			{
				foreach (Song song in songs)
				{
                    ConvertSongsToImages(song);
				}
			}
			catch(Exception ex)
			{
				if (imageFilePath.Length == 0)
					imageFilePath = Path.Combine(destinationFolder, "Error.err");
				else
					imageFilePath += ".err";

				ExceptionHelper.HandleException(ex, imageFilePath);
			}
		}

		private Bitmap GetImage(string verseText)
		{
            using (Font font = new Font( Config.Default.FontName, Config.Default.FontSize, GraphicsUnit.Pixel))
			{
				StringFormat stringFormat = new StringFormat();
				Bitmap bmp = new Bitmap(Config.Default.HorizontalResolution, Config.Default.VerticalResolution);
				Graphics g = Graphics.FromImage(bmp);
				g.Clear(Color.Transparent);

				switch (Config.Default.VerticalAlignment)
				{
					case "Top":
						stringFormat.LineAlignment = StringAlignment.Near;
						break;
					case "Center":
						stringFormat.LineAlignment = StringAlignment.Center;
						break;
					default:
						stringFormat.LineAlignment = StringAlignment.Far;
						break;
				}

				switch (Config.Default.HorizontalAlignment)
				{
					case "Left":
						stringFormat.Alignment = StringAlignment.Near;
						break;
					case "Right":
						stringFormat.Alignment = StringAlignment.Far;
						break;
					default:
						stringFormat.Alignment = StringAlignment.Center;
						break;
				}
                
                int x = Config.Default.Padding;
				int y = Config.Default.Padding;
				int width = Config.Default.HorizontalResolution - Config.Default.Padding * 2;
				int height = Config.Default.VerticalResolution - Config.Default.Padding * 2; ;
				Rectangle rect = new Rectangle(x, y, width, height);

				Type t = typeof(Brushes);
				Brush brush = (Brush)t.GetProperty(Config.Default.FontColor).GetValue(null, null);

				g.DrawString(verseText, font, brush, rect, stringFormat);
				g.Flush();
				return bmp;
			}
		}

        private Bitmap GetImage2(string verseText)
        {
            using (Font font = new Font(Config.Default.FontName, Config.Default.FontSize, GraphicsUnit.Pixel))
            {
                StringFormat stringFormat = new StringFormat();
                Bitmap bmp = new Bitmap(Config.Default.HorizontalResolution, Config.Default.VerticalResolution);
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.Transparent);

                //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
               // g.FillPath(Brushes.White, gp);
                //g.DrawPath(Pens.Black, gp);

                switch (Config.Default.VerticalAlignment)
                {
                    case "Top":
                        stringFormat.LineAlignment = StringAlignment.Near;
                        break;
                    case "Center":
                        stringFormat.LineAlignment = StringAlignment.Center;
                        break;
                    default:
                        stringFormat.LineAlignment = StringAlignment.Far;
                        break;
                }

                switch (Config.Default.HorizontalAlignment)
                {
                    case "Left":
                        stringFormat.Alignment = StringAlignment.Near;
                        break;
                    case "Right":
                        stringFormat.Alignment = StringAlignment.Far;
                        break;
                    default:
                        stringFormat.Alignment = StringAlignment.Center;
                        break;
                }

                int x = Config.Default.Padding;
                int y = Config.Default.Padding;
                int width = Config.Default.HorizontalResolution - Config.Default.Padding * 2;
                int height = Config.Default.VerticalResolution - Config.Default.Padding * 2; ;
                Rectangle rect = new Rectangle(x, y, width, height);

                Type t = typeof(Brushes);
                Brush brush = (Brush)t.GetProperty(Config.Default.FontColor).GetValue(null, null);

                g.DrawString(verseText, font, brush, rect, stringFormat);
                g.FillPath(Brushes.White, gp);
                g.DrawPath(Pens.Black, gp);
                g.Flush();
                return bmp;
            }
        }

        private string GetImageFilePath(string chunk, int chunkNumber, string songName, string destinationFolder)
		{
			destinationFolder = Path.Combine(destinationFolder, FileHelper.CleanIlligalFileNameChars(songName));
			if (chunkNumber == 1) FileHelper.GetCleanFolder(destinationFolder);
			
			string imageFileName = "";
			if (chunk.Length > Config.Default.FileNameLength)
				imageFileName = chunk.Substring(0, Config.Default.FileNameLength);
			else
				imageFileName = chunk;

			imageFileName = FileHelper.CleanIlligalFileNameChars(imageFileName);
			imageFileName = string.Format("{0} {1}{2}", chunkNumber.ToString(), imageFileName, ".png");
			string imageFilePath = Path.Combine(destinationFolder, imageFileName);
			return imageFilePath;
		}

		public void UploadSongsToSwitcher(List<Song> songs)
		{
			foreach (Song song in songs)
			{
                foreach (Verse verse in song.Verses)
                {
                    UploadSongsToSwitcher(song);
                }
            }
        }

        public void UploadSongsToSwitcher(Song song)
        {
                foreach (Verse verse in song.Verses)
                {
                    if (Config.Default.EmulateSwitcher == true)
                    {
                        Thread.Sleep(300);
                        continue;
                    }
                    else
                    {
                        UploadMediaToSwitcher(verse.FilePath, verse.Number - 1);
                    }
                }
        }

        public void UploadSongsToSwitcher(string FolderPath)
        {
            string[] files = FileHelper.GetAllFilesList(FolderPath, "*.png");
            List<Song> songs = new List<Song>();

            int slotNumber = 0;
            foreach (string file in files)
            {
                string shortFileName = Path.GetFileName(file);
                Console.WriteLine(shortFileName);
                slotNumber = int.Parse(Regex.Split(shortFileName, " ")[0]) - 1;
                UploadMediaToSwitcher(file, slotNumber);
            }
        }

        //public Upload.TransferCompletedDelegate transferCompleted;

        private void UploadMediaToSwitcher(string FilePath, int Slot)
        {
            Upload upload = new Upload(Switcher, FilePath, Slot);
            //upload.SetName(verse.Name);
            //upload.transferCompleted = transferCompleted;
            upload.Start();
            while (upload.InProgress())
            {
                SwitcherLib.Log.Info(String.Format("Progress: {0}%", upload.GetProgress().ToString()));
                Thread.Sleep(100);
            }
        }

        Switcher _switcher;
        private Switcher Switcher
        {
            get
            {
                if (_switcher == null) _switcher = new Switcher(Config.Default.SwitcherAddress);
                return _switcher;
            }
        }

        MediaPlayer _mediaPlayer;
        private MediaPlayer MediaPlayer
        {
            get
            {
                if (_mediaPlayer == null) _mediaPlayer = new MediaPlayer(Switcher);
                return _mediaPlayer;
            }
        }

        public void SetSongToPlayer(uint Number)
        {
            if (Config.Default.EmulateSwitcher == true) 
            {
                Thread.Sleep(300);
                return;
            }

            MediaPlayer.SetFirstMediaPlayerSource(Number);
        }

        public bool UseConsoleToUploadFromWinForm
        {
            get
            {
                return Config.Default.UseConsoleToUploadFromWinForm;
            }
        }
    }
}

