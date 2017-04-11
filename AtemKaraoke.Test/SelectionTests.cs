using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AtemKaraoke.Core;

namespace AtemKaraoke.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SelectionTests
    {
        ISwitcher switcher = new FakeSwitcher();

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestSelectVerse()
        {
            string song = @"
string 1
string 2
       
string 3
string 4";
            Lyrics lyrics = new Lyrics(song, switcher);
            lyrics.SelectFirstVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 0);
            lyrics.SelectFirstVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 0);

            lyrics.SelectNextVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
            lyrics.SelectNextVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);

            lyrics.SelectPrevVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 0);
            lyrics.SelectPrevVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 0);

            lyrics.SelectLastVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
            lyrics.SelectLastVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
        }

        [TestMethod]
        public void TestSelectKeyVerse()
        {
            string song = @"
string index 0
       
*refrain index 1

string index 2

string index 3

string index 4

refrain* index 5


next song string

next song refrain*
";
            Lyrics lyrics = new Lyrics(song, switcher);
            lyrics.SelectFirstVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 0);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
            lyrics.SelectNextVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 2);
            lyrics.SelectPrevKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 3);
            lyrics.SelectPrevKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 4);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 5);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 5);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 5);
            lyrics.SelectPrevKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
        }

        [TestMethod]
        public void TestSelectKeyVerseInSong2()
        {
            string song = @"
song 0 string 0

song 0 refrain 1*

song 0 string 2


song 1 string 3

song 1 string 4

song 1 refrain 5*
";
            Lyrics lyrics = new Lyrics(song, switcher);
            lyrics.SelectFirstVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 0);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
            lyrics.SelectNextVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 2);

            lyrics.SelectNextVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 3);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 5);
            lyrics.SelectPrevKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 4);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 5);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 5);
        }

        [TestMethod]
        public void TestSelectDoubleKeyVerse()
        {
            string song = @"
refrain index 0-A*

refrain index 1-A*

refrain index 2-A*

verse index 3

verse index 4

verse index 5

refrain index 6-B*

refrain index 7-B*

refrain index 8-B*
";
            Lyrics lyrics = new Lyrics(song, switcher);
            lyrics.SelectFirstVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 0);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
            lyrics.SelectNextVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 2);

            lyrics.SelectNextVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 3);

            lyrics.SelectPrevKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 0);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
            lyrics.SelectNextVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 2);

            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 4);

            lyrics.SelectPrevKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 0);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 1);
            lyrics.SelectNextVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 2);

            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 5);

            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 6);
            lyrics.SelectNextKeyVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 7);
            lyrics.SelectNextVerse();
            Assert.AreEqual(lyrics.SelectedVerse.LyricsIndexBasedOnZero, 8);
        }
    }
}
