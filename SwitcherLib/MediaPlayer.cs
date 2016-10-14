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


		public uint GetFirstMediaPlayerSource()
		{
			try
			{
				IBMDSwitcherMediaPlayer mediaPlayer = GetPlayer();

				uint index;
				_BMDSwitcherMediaPlayerSourceType t;
				mediaPlayer.GetSource(out t, out index);
				return index;
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

		private IBMDSwitcherTransitionParameters GetTransition1Parameters()
		{
			return (BMDSwitcherAPI.IBMDSwitcherTransitionParameters)Switcher.m_mixEffectBlock1;
		}

		private IBMDSwitcherKey GetKey1()
		{
			IntPtr keyIteratorPtr;
			Guid keyIteratorIID = typeof(IBMDSwitcherKeyIterator).GUID;
			Switcher.m_mixEffectBlock1.CreateIterator(ref keyIteratorIID, out keyIteratorPtr);
			IBMDSwitcherKeyIterator keyIterator = (IBMDSwitcherKeyIterator)Marshal.GetObjectForIUnknown(keyIteratorPtr);
			IBMDSwitcherKey key;
			keyIterator.Next(out key);
			return key;
		}

		public void SetSongToPreview()
		{
			try
			{
				IBMDSwitcherKey key = GetKey1();
				key.SetOnAir(0);
				key.SetInputFill(3010); //http://skaarhoj.com/fileadmin/BMDPROTOCOL.html
				GetTransition1Parameters().SetNextTransitionSelection(_BMDSwitcherTransitionSelection.bmdSwitcherTransitionSelectionKey1);
			}
			catch (Exception ex)
			{
				throw new SwitcherLibException(ex.Message, ex);
			}
		}

		public void SetSongOnAir()
		{
			try
			{
				IBMDSwitcherKey key = GetKey1();
				key.SetInputFill(3010); //http://skaarhoj.com/fileadmin/BMDPROTOCOL.html
				GetTransition1Parameters().SetNextTransitionSelection(_BMDSwitcherTransitionSelection.bmdSwitcherTransitionSelectionBackground);
				key.SetOnAir(1);
			}
			catch (Exception ex)
			{
				throw new SwitcherLibException(ex.Message, ex);
			}
		}

		public void SetSongOffAir()
		{
			try
			{
				GetTransition1Parameters().SetNextTransitionSelection(_BMDSwitcherTransitionSelection.bmdSwitcherTransitionSelectionBackground);
				IBMDSwitcherKey key = GetKey1();
				key.SetOnAir(0);
			}
			catch (Exception ex)
			{
				throw new SwitcherLibException(ex.Message, ex);
			}
		}

		public void SetDownstreamKeyOnAir()
		{
			try
			{
				IBMDSwitcherDownstreamKey key = GetDownstreamKey();
				key.SetInputFill(6); //1-6 works
				int yes = 1;
				key.SetOnAir(yes);
			}
			catch (Exception ex)
			{
				throw new SwitcherLibException(ex.Message, ex);
			}
		}

		private IBMDSwitcherMixEffectBlock GetMixEffectBlock()
		{
			IntPtr mixEffectBlockPtr;
			Guid mixEffectBlockIID = typeof(IBMDSwitcherMixEffectBlock).GUID;
			switcher.GetSwitcher().CreateIterator(ref mixEffectBlockIID, out mixEffectBlockPtr);
			IBMDSwitcherMixEffectBlockIterator mixEffectBlockIterator = (IBMDSwitcherMixEffectBlockIterator)Marshal.GetObjectForIUnknown(mixEffectBlockPtr);
			IBMDSwitcherMixEffectBlock mixEffectBlock;
			mixEffectBlockIterator.Next(out mixEffectBlock);
			return mixEffectBlock;
		}

		private IBMDSwitcherDownstreamKey GetDownstreamKey()
		{
			IntPtr keyIteratorPtr;
			Guid keyIteratorIID = typeof(IBMDSwitcherDownstreamKeyIterator).GUID;
			switcher.GetSwitcher().CreateIterator(ref keyIteratorIID, out keyIteratorPtr);
			IBMDSwitcherDownstreamKeyIterator keyIterator = (IBMDSwitcherDownstreamKeyIterator)Marshal.GetObjectForIUnknown(keyIteratorPtr);
			IBMDSwitcherDownstreamKey key;
			keyIterator.Next(out key);
			return key;
		}
	}
}
