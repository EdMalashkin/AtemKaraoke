using System;
using System.Threading;

namespace AtemKaraoke.Core
{
    [Serializable]
    public class FakeSwitcher : ISwitcher
    {
        public uint GetMediaFromPlayer()
        {
            Thread.Sleep(300);
            return 0;
        }

        public void SetMediaToPlayer(int Number)
        {
            Thread.Sleep(100);
        }

        public void SetMediaOffAir()
        {
            Thread.Sleep(300);
        }

        public void SetMediaOnAir()
        {
            Thread.Sleep(300);
        }

        public void SetMediaToPreview()
        {
            Thread.Sleep(300);
        }
        public void UploadMedia(string FilePath, int Slot)
        {
            Thread.Sleep(300);
        }
    }
}
