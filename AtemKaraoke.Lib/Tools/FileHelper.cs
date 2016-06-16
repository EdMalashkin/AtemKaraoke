using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AtemKaraoke.Lib.Tools
{
	public static class FileHelper
	{
		public static string GetTextFromFile(string FilePath)
		{
			String str = "";
			using (StreamReader sr = new StreamReader(FilePath))
			{
				str = sr.ReadToEnd();
			}
			return str;
		}

		public static string[] GetAllFilesList(string FolderPath, string SearchPattern)
		{
			return Directory.GetFiles(FolderPath, SearchPattern, SearchOption.AllDirectories);
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
