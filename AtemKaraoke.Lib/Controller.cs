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
                Bitmap bmp = GetImage2(verse.Text);
                bmp.Save(verse.FilePath, System.Drawing.Imaging.ImageFormat.Png);
                newFolder = Path.GetDirectoryName(verse.FilePath);
                Console.WriteLine("Generated: " + verse.FilePath);
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

                SetStringFormat(stringFormat);

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
            using (Font font = new Font(Config.Default.FontName, Config.Default.FontSize, Config.Default.FontStyle, GraphicsUnit.Pixel))
            using (StringFormat stringFormat = new StringFormat())
            using (GraphicsPath graphicsPath = new GraphicsPath())
            using (Pen pen = new Pen(Config.Default.FontBorderColor, Config.Default.FontBorderSize))
            {
                SetStringFormat(stringFormat);

                int x = Config.Default.Padding;
                int y = Config.Default.Padding;
                int width = Config.Default.HorizontalResolution - Config.Default.Padding * 2;
                int height = Config.Default.VerticalResolution - Config.Default.Padding * 2; ;
                Rectangle rect = new Rectangle(x, y, width, height);

                graphicsPath.AddString(
                    verseText,                  // text to draw
                    font.FontFamily,            // or any other font family
                    (int)font.Style,            // font style (bold, italic, etc.)
                    font.Size,                  // em size
                    rect,                       // a rectangle where the text is drawn in
                    stringFormat);              // set options here (e.g. center alignment)

                Type t = typeof(Brushes);
                Brush brush = (Brush)t.GetProperty(Config.Default.FontColor).GetValue(null, null);

                Bitmap bmp = new Bitmap(Config.Default.HorizontalResolution, Config.Default.VerticalResolution);

                //http://stackoverflow.com/questions/4200843/outline-text-with-system-drawing
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.High;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawPath(pen, graphicsPath);
                g.FillPath(brush, graphicsPath);
                g.Flush();
                g.Dispose();
               
                return bmp;
            }
        }

        private StringFormat SetStringFormat(StringFormat stringFormat)
        {
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
            return stringFormat;
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
                   UploadMediaToSwitcher(verse.FilePath, verse.Number - 1);
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
                Console.WriteLine("");
                Console.WriteLine(shortFileName);
                slotNumber = int.Parse(Regex.Split(shortFileName, " ")[0]) - 1;
                UploadMediaToSwitcher(file, slotNumber);
            }
        }

        //public Upload.TransferCompletedDelegate transferCompleted;

        private void UploadMediaToSwitcher(string FilePath, int Slot)
        {
            if (Config.Default.EmulateSwitcher == true)
            {
                Thread.Sleep(300);
            }
            else
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

        public void ReconnectToSwitcher()
        {
            // if it works move it to a new Switcher.Dispose method
            // disposing added because when a switcher fails to answer, reconnecting doesn't help. So I had to restart the form.
            if (_mediaPlayer != null)
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(_mediaPlayer);
                _mediaPlayer = null;
            }
            if (_switcher != null)
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(_switcher);
                _switcher = null;
            }

            // After all of the COM objects have been released and set to null, do the following:
            GC.Collect(); // Start .NET CLR Garbage Collection
            GC.WaitForPendingFinalizers(); // Wait for Garbage Collection to finish
            // twice - https://www.add-in-express.com/creating-addins-blog/2013/11/05/release-excel-com-objects/
            GC.Collect(); // Start .NET CLR Garbage Collection
            GC.WaitForPendingFinalizers(); // Wait for Garbage Collection to finish

            if (Config.Default.EmulateSwitcher == false)
                Switcher.Connect();
            else
                Thread.Sleep(1000);
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

