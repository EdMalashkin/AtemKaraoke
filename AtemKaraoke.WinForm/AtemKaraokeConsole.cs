using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace AtemKaraoke.Core.Tools
{
	public class AtemKaraokeConsole
	{
		string _path;
		bool _sendSelected = false;

		public AtemKaraokeConsole(string path, bool sendSelected = false)
		{
			_path = path;
			_sendSelected = sendSelected;
		}
		public void Run()
		{
			if (_path.Trim().Length > 0)
			{
				using (var process = new Process())
				{
					process.StartInfo = new ProcessStartInfo(@"AtemKaraoke.Console.exe");
					process.StartInfo.Arguments = string.Format("\"{0}\"", _path);
					if (_sendSelected)
						process.StartInfo.Arguments += " sendSelected";
					Debug.Print(process.StartInfo.Arguments);
					process.Start();
					KeepWindowFocus();
				};
			}
		}

		private void KeepWindowFocus()
		{
			Thread.Sleep(200); // this is the key
			SetForegroundWindow(Process.GetCurrentProcess().MainWindowHandle);
		}

		[DllImport("user32.dll")]
		static extern bool SetForegroundWindow(IntPtr hWnd);
	}
}
