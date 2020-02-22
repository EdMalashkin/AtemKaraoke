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
                bool sendSelected = false;

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

                if (args.Length == 2 && args[1].Trim() == "sendSelected")
                {
                    sendSelected = true;
                }

                if (sendSelected)
                    lyrics.SendSelected();
                else
                    lyrics.Send();
            }
			catch(Exception ex)
			{
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("!!!!!!!!!!!!!!!!!");
                Console.WriteLine(ex.Message);
                Console.WriteLine("!!!!!!!!!!!!!!!!!");
                ExceptionHelper.HandleException(ex);
			}
            finally
            {
                Console.ResetColor();
                Console.WriteLine("Finished");
                //Console.ReadLine();
                new Reader().ReadLine(3000);
            }
		}
	}
}
 
