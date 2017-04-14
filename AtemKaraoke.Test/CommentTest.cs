using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AtemKaraoke.Core;

namespace AtemKaraoke.Test
{
    [TestClass]
    public class CommentTest
    {
        ISwitcher switcher = new FakeSwitcher();

        [TestMethod]
        public void TestCommentCut()
        {
            string song = @"
    //comment
string 1
string 2

string 3
string 4
 //   comment
";
            string result1 = @"string 1
string 2";

            string result2 = @"string 3
string 4";
            Lyrics lyrics = new Lyrics(song, switcher);
            Assert.AreEqual(lyrics.VerseFiles[0].VerseDrawing.Text, result1);
            Assert.AreEqual(lyrics.VerseFiles[1].VerseDrawing.Text, result2);
        }
    }
}
