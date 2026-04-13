using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace DataAccessLayer
{
    public class clsLogging
    {
        public static void ErrorLogExceptions(string message)
        {
            string Source = "DVLD";

            if(!EventLog.Exists(Source))
            {
                EventLog.CreateEventSource(Source, "Applications");
            }

            EventLog.WriteEntry(Source, message, EventLogEntryType.Error);
        }
    }
}
