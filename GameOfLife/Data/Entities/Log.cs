using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Data.Entities
{
    public class Log
    {
        public int LogId { get; set; }

        public string LogTitle { get; set; }

        public DateTime LogStart { get; set; }

        public DateTime LogEnd{ get; set; }

        public byte[] LogData { get; set; }
    }
}
