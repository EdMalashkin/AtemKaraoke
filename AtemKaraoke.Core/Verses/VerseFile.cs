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
    [Serializable]
    public class VerseFile
    {
        VerseDrawing _verseDrawing;
        Lyrics _lyrics;
        internal int GlobalNumber;

        public VerseFile(VerseDrawing verseDrawing, Lyrics lyrics)
        {
            _verseDrawing = verseDrawing;
            _lyrics = lyrics;
        }

        public Verse Verse
        {
            get
            {
                return _verseDrawing.Verse;
            }
        }

        public VerseDrawing VerseDrawing
        {
            get
            {
                return _verseDrawing;
            }
        }

        public int LyricsIndexBasedOnZero
        {
            get
            {
                return GlobalNumber - 1;
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
        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        public string Save()
        {
            _filePath = GetImageFilePath(Config.Default.DestinationFolder, _lyrics.Name, _verseDrawing.Verse.Song.Name, _verseDrawing.Verse.Number, _verseDrawing.Verse.Text);
            if (_verseDrawing.Verse.Text.Length > 0) // just not to have trash in the folder
            {
                _verseDrawing.Image.Save(_filePath, System.Drawing.Imaging.ImageFormat.Png);
                Console.WriteLine("Generated: " + _filePath);
            }
            return _filePath;
        }

        private string GetImageFilePath(string destinationFolderPath, string folderName, string innerFolderName, int chunkNumber, string chunk)
        {
            destinationFolderPath = Path.Combine(destinationFolderPath,
                                                FileHelper.CleanIlligalFileNameChars(folderName),
                                                FileHelper.CleanIlligalFileNameChars(innerFolderName));
            if (chunkNumber == 1) FileHelper.GetCleanFolder(destinationFolderPath);

            string imageFileName = "";
            if (chunk.Length > Config.Default.FileNameLength)
                imageFileName = chunk.Substring(0, Config.Default.FileNameLength);
            else
                imageFileName = chunk;

            imageFileName = FileHelper.CleanIlligalFileNameChars(imageFileName);
            imageFileName = string.Format("{0} {1}{2}", chunkNumber.ToString(), imageFileName, ".png");
            string imageFilePath = Path.Combine(destinationFolderPath, imageFileName);
            return imageFilePath;
        }

        public override bool Equals(object obj)
        {
            var file = obj as VerseFile;
            bool res = (file != null
                        && file.FilePath != null
                        && file.FilePath == this.FilePath
                        && file.Text == this.Text
                        );
            return res;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
