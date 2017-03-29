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
                ILyrics lyrics = null;

                if (args.Length == 0)
                {
                    lyrics = new TextFileLyrics();
                    lyrics.Save();
                }                   
                else if (args.Length == 1)
                {
                    string path = args[0];
                    Console.WriteLine(path);
                    lyrics = new BinaryFileLyrics(path);
                    
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
 
