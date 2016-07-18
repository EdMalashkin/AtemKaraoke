using System;
using BMDSwitcherAPI;
using System.Runtime.InteropServices;

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

        public uint GetFirstMediaPlayerSource() 
        {
            try
            {
                IBMDSwitcherMediaPlayer mediaPlayer = GetPlayer();

                uint index;
                _BMDSwitcherMediaPlayerSourceType t;
                mediaPlayer.GetSource(out t, out index);
                return index;
                //Debug.Print(string.Format("KeepConectionAlive: {0}", index));
                //mediaPlayer.SetSource(_BMDSwitcherMediaPlayerSourceType.bmdSwitcherMediaPlayerSourceTypeStill, Index);
            }
            catch (Exception ex)
            {
                throw new SwitcherLibException(ex.Message, ex);
            }
        }

        public void SetFirstMediaPlayerSource(uint index)
        {
            try
            {
                IBMDSwitcherMediaPlayer mediaPlayer = GetPlayer();
                mediaPlayer.SetSource(_BMDSwitcherMediaPlayerSourceType.bmdSwitcherMediaPlayerSourceTypeStill, index);
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
