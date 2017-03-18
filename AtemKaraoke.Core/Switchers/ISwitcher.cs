namespace AtemKaraoke.Core
{
    public interface ISwitcher
    { 
        uint GetMediaFromPlayer();
        void SetMediaToPlayer(int Number);
        void SetMediaOffAir();
        void SetMediaOnAir();
        void SetMediaToPreview();
        void UploadMedia(string FilePath, int Slot);
    }
}
