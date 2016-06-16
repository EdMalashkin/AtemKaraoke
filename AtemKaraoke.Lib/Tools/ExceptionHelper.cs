using System;
using AtemKaraoke.Lib.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtemKaraoke.Lib
{
	public static class ExceptionHelper
	{
		public static string GetError(Exception e)
		{
			string res = e.Message;
			Exception InnerExc = e.InnerException;
			while (InnerExc != null)
			{
				res += Environment.NewLine + InnerExc.Message;
				InnerExc = InnerExc.InnerException;
			};

			res = string.Format("[{0}] {1} \nSource: {2} \n{3}", e.GetType().ToString(), res, e.Source, e.StackTrace);
			return res;
		}

		public static string HandleException(Exception e)
		{
			string ErrMessage = GetError(e);
			Log.WriteInEventLog(ErrMessage, System.Diagnostics.EventLogEntryType.Error);
			return ErrMessage;
		}

		public static string HandleException(Exception e, string FilePath)
		{
			string ErrMessage = GetError(e);
			Log.WriteInFile(ErrMessage, FilePath);
			return ErrMessage;
		}
	}
}
