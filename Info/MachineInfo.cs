using System;

namespace BioMetrixCore
{
    public class MachineInfo
    {
        internal bool dwEnrollNumber1;

        public int MachineNumber { get; set; }
        public int IndRegID { get; set; }
        public int dwInOutMode { get; set; }
        public string DateTimeRecord { get; set; }

        public DateTime DateOnlyRecord
        {
            get { return DateTime.Parse(DateTime.Parse(DateTimeRecord).ToString("yyyy-MM-dd")); }
        }
        public DateTime TimeOnlyRecord
        {
            get { return DateTime.Parse(DateTime.Parse(DateTimeRecord).ToString("hh:mm:ss tt")); }
        }

    }
}
