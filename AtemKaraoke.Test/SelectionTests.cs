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
            lyrics.Selection.ToFirstVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 0);
            lyrics.Selection.ToFirstVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 0);

            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);

            lyrics.Selection.ToPrevVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 0);
            lyrics.Selection.ToPrevVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 0);

            lyrics.Selection.ToLastVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
            lyrics.Selection.ToLastVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
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
            lyrics.Selection.ToFirstVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 0);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 2);
            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 3);
            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 4);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 5);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 5);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 5);
            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
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
            lyrics.Selection.ToFirstVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 0);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 2);

            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 3);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 5);
            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 4);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 5);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 5);
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
            lyrics.Selection.ToFirstVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 0);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 2);

            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 3);

            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 0);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 2);

            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 4);

            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 0);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 2);

            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 5);

            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 6);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 7);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.LyricsIndexBasedOnZero, 8);
        }
    }
}
