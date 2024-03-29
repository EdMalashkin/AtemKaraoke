﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AtemKaraoke.Core;
using System.Text;

namespace AtemKaraoke.Test
{
    [TestClass]
    public class GeneralTests
    {
        ISwitcher switcher = new FakeSwitcher();

        [TestMethod]
        public void TestVerseAmount()
        {
            string song = @"string 1
string 2
       
string 3
string 4";
            Lyrics lyrics = new Lyrics(song, switcher);
            Assert.AreEqual(lyrics.VerseFiles.Count, 2);
            Assert.AreEqual(lyrics.Songs.Count, 1);
        }

        [TestMethod]
        public void TestMaxVerseAmount()
        {
            string[] verses = new string[30];
            for (int i = 0; i < 30; i++)
            {
                verses[i] = "Verse " + i.ToString();
            }
            string song = string.Join("\r\n\r\n", verses);
            Lyrics lyrics = new Lyrics(song, switcher);
            Assert.AreEqual(lyrics.VerseFiles.Count, 19);
            Assert.AreEqual(lyrics.Songs.Count, 1);
        }

        [TestMethod]
        public void TestSongsAmount()
        {
            string song = @"Song 1
                        
                                
Song 2";
            Lyrics lyrics = new Lyrics(song, switcher);
            Assert.AreEqual(lyrics.VerseFiles.Count, 2);
            Assert.AreEqual(lyrics.Songs.Count, 2);
        }

        [TestMethod]
        public void TestContent()
        {
            string song = @"Song 1 Verse  1

    Song 1 Verse  2                  
                   
             

            Song 2 Verse    1     
 
     Song 2 Verse 2  ";
            Lyrics lyrics = new Lyrics(song, switcher);
            Assert.AreEqual(lyrics.VerseFiles[0].Text, "Song 1 Verse 1");
            Assert.AreEqual(lyrics.VerseFiles[1].Text, "Song 1 Verse 2");
            Assert.AreEqual(lyrics.VerseFiles[2].Text, "Song 2 Verse 1");
            Assert.AreEqual(lyrics.VerseFiles[3].Text, "Song 2 Verse 2");
            Assert.AreEqual(lyrics.Songs[0].Text.Contains("Song 1"), true);
            Assert.AreEqual(lyrics.Songs[0].Text.Contains("Song 2"), false);
            Assert.AreEqual(lyrics.Songs[1].Text.Contains("Song 1"), false);
            Assert.AreEqual(lyrics.Songs[1].Text.Contains("Song 2"), true);
        }

        [TestMethod]
        public void TestSerialization()
        {
            string song = @"string 1
string 2
       
string 3
string 4";
            var file1 = new BinaryFileLyrics(new Lyrics(song, switcher));
            var filePath = file1.Save();
            var file2 = new BinaryFileLyrics(filePath);
            Assert.AreEqual(file1.Lyrics, file2.Lyrics);
        }

        [TestMethod]
        public void TestLyricsToString()
        {
            string song = @"
string 1
string 2


string 3

string 4
  ";

            string result = @"string 1
string 2


string 3

string 4";
            Lyrics lyrics = new Lyrics(song, switcher);
            Assert.AreEqual(lyrics.ToString(), result);
        }


		[TestMethod]
		public void TestEditingVerseInSeveralSongs()
		{
			string song = @"Song 1
                        
                                
Song 2";
			Lyrics lyrics = new Lyrics(song, switcher);
			Assert.AreEqual(lyrics.Songs.Count, 2);
			lyrics.Songs[0].VerseFiles[0].Verse.Update("Song 1 edited");
			string songEdited = lyrics.ToString();

			string song2 = @"Song 1 edited
                        
                                
Song 2";
			Assert.AreEqual(lyrics.Songs.Count, 2);
			Assert.AreEqual(song2.Replace(" ", ""), songEdited.Replace(" ", ""));
		}
	}
}
