using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using BMDSwitcherAPI;
using System.IO;
using System.Runtime.InteropServices;
using SwitcherLib.Callbacks;

namespace SwitcherLib
{
    public class MediaPlayer
    {

        private Switcher switcher;

        public MediaPlayer(Switcher switcher)
        {
            this.switcher = switcher;
        }

        public void SetFirstMediaPlayerSource(int Index)
        {
            try
            {
                IBMDSwitcherMediaPlayer mediaPlayer = GetPlayer();
                mediaPlayer.SetSource(MDSwitcherMediaPlayerSourceType.bmdSwitcherMediaPlayerSourceTypeStill, Index);
            }
            catch (Exception ex)
            {
                throw new SwitcherLibException(ex.Message, ex);
            }
    }
}

        private IBMDSwitcherMediaPlayer GetPlayer()
        {
            IntPtr mediaPlayerIteratorPtr;
            Guid mediaIteratorIID = typeof(IBMDSwitcherMediaPlayerIterator).GUID;
            this.switcher.CreateIterator(ref mediaIteratorIID, out mediaPlayerIteratorPtr);
            IBMDSwitcherMediaPlayerIterator mediaPlayerIterator = (IBMDSwitcherMediaPlayerIterator)Marshal.GetObjectForIUnknown(mediaPlayerIteratorPtr);

            IBMDSwitcherMediaPlayer mediaPlayer;
            mediaPlayerIterator.Next(out mediaPlayer);
            int num1 = 1;
            while (mediaPlayer != null)
            {
                return mediaPlayer;

                num1++;
                mediaPlayerIterator.Next(out mediaPlayer);
            }
            return mediaPlayer;

        }
    }
}
