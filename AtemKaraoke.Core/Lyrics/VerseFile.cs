using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.IO;
using AtemKaraoke.Core.Tools;

namespace AtemKaraoke.Core
{
    public class VerseFile
    {
        VerseDrawing _verseDrawing;

        public VerseFile(VerseDrawing verseDrawing)
        {
            _verseDrawing = verseDrawing;
        }

        public Verse Verse
        {
            get
            {
                return _verseDrawing.Verse;
            }
        }

        public string Text // used when binding
        {
            get
            {
                return _verseDrawing.Verse.Text;
            }
        }

        private string _filePath;
        public string Save()
        {
            _filePath = GetImageFilePath(_verseDrawing.Verse.Text, _verseDrawing.Verse.Number, _verseDrawing.Verse.Song.Name, Config.Default.DestinationFolder);
            _verseDrawing.Image.Save(_filePath, System.Drawing.Imaging.ImageFormat.Png);
            Console.WriteLine("Generated: " + _filePath);
            return _filePath;
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
    }
}
