using System;
using System.Text;
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
                Console.OutputEncoding = Encoding.UTF8;
                App c = new App();

                if (args.Length == 0)
                {
                    List<Song> songs = c.ConvertSongsToImages();
                    c.UploadSongsToSwitcher(songs);
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
 
