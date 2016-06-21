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
            this.switcher.Connect();
        }

        public void SetFirstMediaPlayerSource(uint Index)
        {
            try
            {
                IBMDSwitcherMediaPlayer mediaPlayer = GetPlayer();
                //_BMDSwitcherMediaPlayerSourceType t;
                //mediaPlayer.GetSource(out t, out Index);
                mediaPlayer.SetSource(_BMDSwitcherMediaPlayerSourceType.bmdSwitcherMediaPlayerSourceTypeStill, Index);
            }
            catch (Exception ex)
            {
                throw new SwitcherLibException(ex.Message, ex);
            }
        }

        private IBMDSwitcherMediaPlayer GetPlayer()
        {
            IntPtr mediaPlayerIteratorPtr;
            Guid mediaIteratorIID = typeof(IBMDSwitcherMediaPlayerIterator).GUID;
            switcher.GetSwitcher().CreateIterator(ref mediaIteratorIID, out mediaPlayerIteratorPtr);
            IBMDSwitcherMediaPlayerIterator mediaPlayerIterator = (IBMDSwitcherMediaPlayerIterator)Marshal.GetObjectForIUnknown(mediaPlayerIteratorPtr);

            IBMDSwitcherMediaPlayer mediaPlayer;
            mediaPlayerIterator.Next(out mediaPlayer);

            return mediaPlayer;

        }
    }
}
