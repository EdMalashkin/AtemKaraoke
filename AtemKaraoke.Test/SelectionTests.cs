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
        // comment

string 1
string 2
       
        // comment

string 3
string 4

        // comment
";
            Lyrics lyrics = new Lyrics(song, switcher);
            lyrics.Selection.ToFirstVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 0);
            lyrics.Selection.ToFirstVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 0);

            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);

            lyrics.Selection.ToPrevVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 0);
            lyrics.Selection.ToPrevVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 0);

            lyrics.Selection.ToLastVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
            lyrics.Selection.ToLastVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
        }

        [TestMethod]
        public void TestSelectKeyVerse()
        {
            string song = @"
        // comment
        // comment

string index 0
       
        // comment

*refrain index 1

        // comment

string index 2

        // comment

string index 3

        // comment

string index 4

        // comment

refrain* index 5

        // comment

    
        //next song
next song string

        // comment

next song refrain*

        // comment
";
            Lyrics lyrics = new Lyrics(song, switcher);
            lyrics.Selection.ToFirstVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 0);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 2);
            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 3);
            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 4);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 5);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 5);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 5);
            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
        }

        [TestMethod]
        public void TestSelectKeyVerseInSong2()
        {
            string song = @"
        // comment

song 0 string 0

        // comment

song 0 refrain 1*

        // comment

song 0 string 2

        // comment


// next song
song 1 string 3

        // comment

song 1 string 4

        // comment

song 1 refrain 5*

        // comment
";
            Lyrics lyrics = new Lyrics(song, switcher);
            lyrics.Selection.ToFirstVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 0);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 2);

            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 3);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 5);
            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 4);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 5);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 5);
        }

        [TestMethod]
        public void TestSelectDoubleKeyVerse()
        {
            string song = @"
        // comment

refrain index 0-A*

        // comment

refrain index 1-A*

        // comment

refrain index 2-A*

        // comment

verse index 3

        // comment

verse index 4

        // comment

verse index 5

        // comment

refrain index 6-B*

        // comment

refrain index 7-B*

        // comment

refrain index 8-B*
        // comment

";
            Lyrics lyrics = new Lyrics(song, switcher);
            lyrics.Selection.ToFirstVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 0);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 2);

            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 3);

            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 0);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 2);

            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 4);

            lyrics.Selection.ToPrevKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 0);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 1);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 2);

            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 5);

            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 6);
            lyrics.Selection.ToNextKeyVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 7);
            lyrics.Selection.ToNextVerse();
            Assert.AreEqual(lyrics.Selection.CurrentVerse.IndexBasedOnZeroToSelect, 8);
        }
    }
}
