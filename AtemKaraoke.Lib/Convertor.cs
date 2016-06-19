using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using AtemKaraoke.Lib.Tools;
using SwitcherLib;

namespace AtemKaraoke.Lib
{
	public class Convertor
	{
		public void ConvertSongsToImages(string SourceFolder, string SearchPattern, string DestinationFolder)
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
			//UploadSongsToSwitcher(songs);
		}

		public void ConvertSongsToImages()
		{
			ConvertSongsToImages(Config.Default.SourceFolder, Config.Default.SourceFolderPattern, Config.Default.DestinationFolder);
		}

		public void ConvertSongsToImages(List<Song> songs, string destinationFolder)
		{
			string imageFilePath = "";
			try
			{
				foreach (Song song in songs)
				{
					foreach (Verse verse in song.Verses)
					{
						verse.FilePath = GetImageFilePath(verse.Text, verse.Number, song.Name, destinationFolder);
						Bitmap bmp = GetImage(verse.Text);
						bmp.Save(verse.FilePath, System.Drawing.Imaging.ImageFormat.Png);
					}
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

		private Bitmap GetImage(string chunk)
		{
			using (Font font = new Font(Config.Default.FontName, Config.Default.FontSize, GraphicsUnit.Pixel))
			{
				StringFormat sf = new StringFormat();
				Bitmap bmp = new Bitmap(Config.Default.HorizontalResolution, Config.Default.VerticalResolution);
				Graphics g = Graphics.FromImage(bmp);
				g.Clear(Color.Transparent);

				switch (Config.Default.VerticalAlignment)
				{
					case "Top":
						sf.LineAlignment = StringAlignment.Near;
						break;
					case "Center":
						sf.LineAlignment = StringAlignment.Center;
						break;
					default:
						sf.LineAlignment = StringAlignment.Far;
						break;
				}

				switch (Config.Default.HorizontalAlignment)
				{
					case "Left":
						sf.Alignment = StringAlignment.Near;
						break;
					case "Right":
						sf.Alignment = StringAlignment.Far;
						break;
					default:
						sf.Alignment = StringAlignment.Center;
						break;
				}

				int x = Config.Default.Padding;
				int y = Config.Default.Padding;
				int width = Config.Default.HorizontalResolution - Config.Default.Padding * 2;
				int height = Config.Default.VerticalResolution - Config.Default.Padding * 2; ;
				Rectangle rect = new Rectangle(x, y, width, height);

				Type t = typeof(Brushes);
				Brush b = (Brush)t.GetProperty(Config.Default.FontColor).GetValue(null, null);

				g.DrawString(chunk, font, b, rect, sf);
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
			Switcher switcher = new Switcher("192.168.88.5");
			foreach (Song song in songs)
			{
				foreach (Verse verse in song.Verses)
				{
					Upload upload = new Upload(switcher, verse.FilePath, verse.Number);
					upload.SetName(verse.Name);
					upload.Start();
					while (upload.InProgress())
					{
						//Log.Info(String.Format("Progress: {0}%", upload.GetProgress().ToString()));
						//Thread.Sleep(100);
					}
				}
			}
		}
	}
}
