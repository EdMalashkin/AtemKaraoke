using System;
using AtemKaraoke.Lib;
using AtemKaraoke.Lib.Tools;
using System.Collections.Generic;

namespace AtemKaraoke
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Controller c = new Controller();
				//c.ConvertSongsToImages("C:\\Projects\\AtemKaraoke\\AtemKaraoke\\Songs\\", "*.txt", "C:\\Projects\\AtemKaraoke\\AtemKaraoke\\Songs\\");

                if (args.Length == 0)
                {
                    List<Song> songs = c.ConvertSongsToImages();
                    c.UploadSongsToSwitcher(songs);

                    //c.UploadMediaToSwitcher("D:\\1.png", 0);
                }                   
                else if (args.Length == 1)
                {
                    string SourseFolder = args[0];
                    Console.WriteLine(SourseFolder);
                    c.UploadSongsToSwitcher(SourseFolder);
                }

            }
			catch(Exception ex)
			{
				Console.Write(ex.Message);
				ExceptionHelper.HandleException(ex);
			}
            finally
            {
                Console.WriteLine("Finished");
                Console.ReadLine();
            }
		}
	}
}
 
