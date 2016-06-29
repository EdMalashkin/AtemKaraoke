using System;
using System.Windows.Forms;
using BMDSwitcherAPI;
using SwitcherLib.Callbacks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitcherLib
{
    public class SwitcherForm : Form
    {
        public IBMDSwitcherLockCallback lockCallback;
        public Upload upload;
        public void LockCallback()
        {
            IBMDSwitcherStillsCallback callback = (IBMDSwitcherStillsCallback)new Stills(this.upload); //EDM: commented for test
            this.upload.stills.AddCallback(callback);
            this.upload.stills.Upload((uint)this.upload.uploadSlot, this.upload.GetName(), this.upload.frame);
        }

        public void TransferCompleted()
        {
            Log.Debug("Completed upload");
            try
            {
                this.upload.stills.Unlock(this.lockCallback); //EDM: commented for test
            }
            catch
            {

            }

            this.upload.currentStatus = Upload.Status.Completed;
        }
    }
}
