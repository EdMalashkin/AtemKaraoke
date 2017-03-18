using System;
using System.Text;
using AtemKaraoke.Core;
using AtemKaraoke.Core.Tools;
using System.Collections.Generic;

namespace AtemKaraoke
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
                Console.OutputEncoding = Encoding.UTF8;
                //App app = new App();
                ILyrics lyrics = null;

                if (args.Length == 0)
                {
                    //List<Song> songs = app.ConvertSongsToImages();
                    //app.UploadSongsToSwitcher(songs);
                    lyrics = new LyricsFromFile();
                }                   
                else if (args.Length == 1)
                {
                    string sourseFolder = args[0];
                    Console.WriteLine(sourseFolder);
                    lyrics = new LyricsFromFile(sourseFolder);
                    //app.UploadSongsToSwitcher(sourseFolder);
                }
                lyrics.Send();
            }
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				ExceptionHelper.HandleException(ex);
			}
            finally
            {
                Console.WriteLine("Finished");
                //Console.ReadLine();
            }
		}
	}
}
 
