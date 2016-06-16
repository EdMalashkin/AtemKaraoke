using System;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace AtemKaraoke.Lib.Tools
{
	public static class Log
	{
		public static void WriteInEventLog(string message, EventLogEntryType type)
		{
			string SourceName = "Application";
			//if (!EventLog.SourceExists(SourceName))
			//{
			//	EventLog.CreateEventSource(SourceName, " ");
			//}

			EventLog.WriteEntry(
				SourceName,      // Registered event source
				message,         // Event entry message
				type // Event type               
			);
		}

		public static void WriteInFile(string message, string filepath)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("=============================" + Environment.NewLine);
			sb.Append("Log record" + Environment.NewLine);
			sb.Append(message + Environment.NewLine);
			FileHelper.AppendFile(sb.ToString(), filepath);
		}

	
	}
}
