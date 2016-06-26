using System;
using AtemKaraoke.Lib;
using AtemKaraoke.Lib.Tools;

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
                    c.ConvertSongsToImages();
                else if (args.Length == 1)
                {
                    string SourseFolder = args[0];
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
                Console.ReadLine();
            }
		}
	}
}
 