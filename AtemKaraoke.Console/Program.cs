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
                int verseNumber = -1;

                if (args.Length == 0)
                {
                    lyrics = new TextFileLyrics(); // generate Lyrics from text files
                    lyrics.Save();
                }                   
                else if (args.Length >= 1)
                {
                    string path = args[0];
                    Console.WriteLine(path);
                    lyrics = new BinaryFileLyrics(path); // get Lyrics by deserializing
                }

                if (args.Length == 2)
                {
                    int temp;
                    if (int.TryParse(args[1], out temp))
                        verseNumber = temp;
                    else
                        throw new Exception("The second argument (a verse number to send) must be an integer");
                }

                lyrics.Send(verseNumber);
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
                new Reader().ReadLine(5000);
            }
		}
	}
}
 
