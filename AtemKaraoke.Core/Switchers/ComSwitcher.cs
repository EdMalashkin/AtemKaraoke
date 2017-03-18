using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwitcherLib;
using System.Threading;

namespace AtemKaraoke.Core
{
    public class ComSwitcher : ISwitcher
    {
        Switcher _switcher;
        private Switcher Switcher
        {
            get
            {
                if (_switcher == null) _switcher = new Switcher(Config.Default.SwitcherAddress);
                return _switcher;
            }
        }

        IMediaPlayer _mediaPlayer;
        private IMediaPlayer MediaPlayer
        {
            get
            {
                if (_mediaPlayer == null)
                {
                    _mediaPlayer = new MediaPlayer(Switcher);
                }
                return _mediaPlayer;
            }
        }

        public void SetMediaOnAir()
        {
            MediaPlayer.SetSongOnAir();
            //MediaPlayer.SetDownstreamKeyOnAir();
        }

        public void SetMediaOffAir()
        {
            MediaPlayer.SetSongOffAir();
        }

        public void SetMediaToPreview()
        {
            MediaPlayer.SetSongToPreview();
        }

        public uint GetMediaFromPlayer()
        {
            uint result = 999;
            result = MediaPlayer.GetFirstMediaPlayerSource();
            return result;
        }

        public void SetMediaToPlayer(int Number)
        {
            MediaPlayer.SetFirstMediaPlayerSource((uint)Number);
        }

        private void Reconnect()
        {
            // if it works move it to a new Switcher.Dispose method
            // disposing added because when a switcher fails to answer, reconnecting doesn't help. So I had to restart the form.
            if (_mediaPlayer != null)
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(_mediaPlayer);
                _mediaPlayer = null;
            }
            if (_switcher != null)
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(_switcher);
                _switcher = null;
            }

            // After all of the COM objects have been released and set to null, do the following:
            GC.Collect(); // Start .NET CLR Garbage Collection
            GC.WaitForPendingFinalizers(); // Wait for Garbage Collection to finish
            //// twice - https://www.add-in-express.com/creating-addins-blog/2013/11/05/release-excel-com-objects/
            GC.Collect(); // Start .NET CLR Garbage Collection
            GC.WaitForPendingFinalizers(); // Wait for Garbage Collection to finish

            if (Config.Default.EmulateSwitcher == false)
                Switcher.Connect();
            else
                Thread.Sleep(1000);
        }

        public void UploadMedia(string FilePath, int Slot)
        {
            if (Config.Default.EmulateSwitcher == true)
            {
                Thread.Sleep(300);
            }
            else
            {
                Upload upload = new Upload(Switcher, FilePath, Slot);
                //upload.SetName(verse.Name);
                //upload.transferCompleted = transferCompleted;
                upload.Start();
                while (upload.InProgress())
                {
                    SwitcherLib.Log.Info(String.Format("Progress: {0}%", upload.GetProgress().ToString()));
                    Thread.Sleep(100);
                }
            }
        }
    }
}
