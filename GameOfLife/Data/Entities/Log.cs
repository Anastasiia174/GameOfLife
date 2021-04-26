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

        public string LogEvent { get; set; }

        public DateTime LogEventDateTime { get; set; }

        public byte[] LogData { get; set; }
    }
}
