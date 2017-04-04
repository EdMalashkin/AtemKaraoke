using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtemKaraoke
{ //http://stackoverflow.com/questions/57615/how-to-add-a-timeout-to-console-readline
    public class Reader
    {
        private Thread inputThread;
        private AutoResetEvent getInput, gotInput;
        private string input;

        public Reader()
        {
            getInput = new AutoResetEvent(false);
            gotInput = new AutoResetEvent(false);
            inputThread = new Thread(reader);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private void reader()
        {
            while (true)
            {
                getInput.WaitOne();
                input = Console.ReadLine();
                gotInput.Set();
            }
        }

        public string ReadLine(int timeOutMillisecs)
        {
            getInput.Set();
            bool success = gotInput.WaitOne(timeOutMillisecs);
            if (success)
                return input;
            else
                return "";
                //throw new TimeoutException("User did not provide input within the timelimit.");
        }
    }
}
