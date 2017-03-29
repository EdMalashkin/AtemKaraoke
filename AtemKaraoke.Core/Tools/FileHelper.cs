using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AtemKaraoke.Core.Tools
{
	public static class FileHelper
	{
		public static string GetTextFromFile(string FilePath)
		{
			String str = "";
			//using (StreamReader sr = new StreamReader(FilePath, Encoding.Default))
			using (StreamReader sr = OpenStreamReaderWithEncoding(FilePath))
			{
				str = sr.ReadToEnd();
                sr.Close();

            }
			return str;
		}

		/// <summary>
		/// Detects the byte order mark of a file and returns
		/// an appropriate encoding for the file.
		/// </summary>
		/// <param name="srcFile"></param>
		/// <returns></returns>
		public static Encoding GetFileEncoding(string srcFile)
		{
			// *** Use Default of Encoding.Default (Ansi CodePage)
			Encoding enc = Encoding.Default;

			// *** Detect byte order mark if any - otherwise assume default
			byte[] buffer = new byte[5];
			FileStream file = new FileStream(srcFile, FileMode.Open);
			file.Read(buffer, 0, 5);
			file.Close();

			if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
				enc = Encoding.UTF8;
			else if (buffer[0] == 0xfe && buffer[1] == 0xff)
				enc = Encoding.Unicode;
			else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
				enc = Encoding.UTF32;
			else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
				enc = Encoding.UTF7;
			return enc;
		}

		/// <summary>
		/// Opens a stream reader with the appropriate text encoding applied.
		/// </summary>
		/// <param name="srcFile"></param>
		public static StreamReader OpenStreamReaderWithEncoding(string srcFile)
		{
			Encoding enc = GetFileEncoding(srcFile);
			return new StreamReader(srcFile, enc);
		}

		public static IOrderedEnumerable<string> GetAllFiles(string FolderPath, string SearchPattern)
		{
            return Directory.GetFiles(FolderPath, SearchPattern, SearchOption.AllDirectories).OrderBy(f => new FileInfo(f).CreationTime);
        }

        public static void GetCleanFolder(string FolderPath)
		{
			if (!Directory.Exists(FolderPath))
			{
				Directory.CreateDirectory(FolderPath);
			}
			else
			{
				System.IO.DirectoryInfo di = new DirectoryInfo(FolderPath);

				foreach (FileInfo file in di.GetFiles())
				{
					file.Delete();
				}
				foreach (DirectoryInfo dir in di.GetDirectories())
				{
					dir.Delete(true);
				}
			}
		}

		public static string CleanIlligalFileNameChars(string fileName)
		{
			string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
			fileName = r.Replace(fileName, " ");
			fileName = fileName.Replace("  ", " ").Trim();
			return fileName;
		}

		public static void AppendFile(string fileContent, string filePath)
		{
			if (Directory.Exists(Path.GetDirectoryName(filePath)))
			{
				while (true)
				{
					try
					{
						if (File.Exists(filePath))
						{
							fileContent = Environment.NewLine + fileContent;
						}

						FileStream txtFile = new FileStream(filePath, FileMode.Append);
						UTF8Encoding utf8 = new UTF8Encoding(false);
						byte[] encodedBytes = utf8.GetBytes(fileContent);
						string decodedString = utf8.GetString(encodedBytes);

						StreamWriter strwrite = new StreamWriter(txtFile);
						strwrite.Write(decodedString);
						strwrite.Close();
						txtFile.Close();

						break;
					}
					catch (UnauthorizedAccessException uae)
					{
						//replace current folder to temp and try again
						string path = string.Format(@"{0}\AtemKaraoke", Environment.GetEnvironmentVariable("TEMP"));

						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}

						string newPath = Path.Combine(path, Path.GetFileName(filePath));

						if (filePath != newPath)
						{
							filePath = newPath;
						}
						else
						{
							throw uae;
						}
					}
				}
			}
			else
			{
				throw new Exception("The folder '" + Path.GetDirectoryName(filePath) + "' is not found.");
			}

		}
	}


}
