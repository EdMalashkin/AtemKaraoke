using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AtemKaraoke.Core
{
    [Serializable]
    public class Selection
    {
        private Lyrics _lyrics;
        public Selection(Lyrics l)
        {
            _lyrics = l;
        }

        public void ToPrevKeyVerse()
        {
            ToVerse(GetPrevKeyVerses().FirstOrDefault());
        }

        public void ToNextKeyVerse()
        {
            ToVerse(GetNextKeyVerses().FirstOrDefault());
        }

        public void ToFirstVerse()
        {
            ToVerse(_lyrics.VerseFiles.FirstOrDefault());
        }

        public void ToLastVerse()
        {
            ToVerse(_lyrics.VerseFiles.Last());
        }

        public void ToPrevVerse()
        {
            ToVerse(PrevVerseFile);
        }

        public void ToNextVerse()
        {
            ToVerse(NextVerseFile);
        }

        private VerseFile _previouslySelectedKeyVerse;
        private VerseFile _previouslySelectedVerse;
        private VerseFile GetNextToPreviouslySelectedVerse()
        {
            return _lyrics.VerseFiles.Where(v => _previouslySelectedKeyVerse != null
                                    && v.Verse.Song == _previouslySelectedKeyVerse.Verse.Song
                                    && v.LyricsIndexBasedOnZero == _previouslySelectedKeyVerse.LyricsIndexBasedOnZero + 1)
                             .FirstOrDefault();
        }

        private List<VerseFile> GetPrevKeyVerses()
        {
            return new List<VerseFile>() {
                GetNextToPreviouslySelectedVerse(),
                CurrentVerse.Verse.Song.PrevRefrain
            }
            .Where(v => v != null && v.LyricsIndexBasedOnZero < CurrentVerse.LyricsIndexBasedOnZero)
            .OrderBy(v => v.LyricsIndexBasedOnZero)
            .ToList();
        }

        private List<VerseFile> GetNextKeyVerses()
        {
            return new List<VerseFile>() {
                GetNextToPreviouslySelectedVerse(),
                CurrentVerse.Verse.Song.NextRefrain
            }
            .Where(v => v != null && v.LyricsIndexBasedOnZero > CurrentVerse.LyricsIndexBasedOnZero)
            .OrderBy(v => v.LyricsIndexBasedOnZero)
            .ToList();
        }

        private VerseFile PrevVerseFile
        {
            get
            {
                return _lyrics.VerseFiles.Find(v => v.GlobalNumber == CurrentVerse.GlobalNumber - 1);
            }
        }

        private VerseFile NextVerseFile
        {
            get
            {
                return _lyrics.VerseFiles.Find(v => v.GlobalNumber == CurrentVerse.GlobalNumber + 1);
            }
        }

        private VerseFile _currentVerse;
        public VerseFile CurrentVerse
        {
            get
            {
                return _currentVerse;
            }
        }

        public event EventHandler OnVerseSelected;

        public void ToVerse(VerseFile newVerseFile)
        {
            if (newVerseFile != null && _currentVerse != newVerseFile)
            {
                // if a previous verse was also a refrain then keep _previouslySelectedVerse in _previouslySelectedKeyVerse to use it later
                // so only if 2 refrains go in a row 
                if (!(newVerseFile.Verse.IsRefrain
                    && _currentVerse != null
                    && _currentVerse.Verse.IsRefrain
                    && Math.Abs(_currentVerse.LyricsIndexBasedOnZero - newVerseFile.LyricsIndexBasedOnZero) == 1))
                {
                    _previouslySelectedKeyVerse = _currentVerse;
                }
                _previouslySelectedVerse = _currentVerse;
                _currentVerse = newVerseFile;

                _lyrics.Switcher.SetMediaToPlayer(newVerseFile.GlobalNumber);

                if (_previouslySelectedVerse == null)
                {
                    Debug.Print("Verse index {0} is selected",
                                                        newVerseFile.LyricsIndexBasedOnZero);
                }
                else
                {
                    Debug.Print("Verse index {0} is selected after {1}",
                                                        newVerseFile.LyricsIndexBasedOnZero,
                                                        _previouslySelectedVerse.LyricsIndexBasedOnZero);
                }

                //if (OnVerseSelected != null)
                //{
                //    OnVerseSelected(newVerseFile, null);
                //}
                OnVerseSelected?.Invoke(newVerseFile, null);
            }
        }
    }
}
