using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Data.Entities
{
    public class Layout
    {
        public int LayoutId { get; set; }

        public string LayoutTitle { get; set; }

        public byte[] LayoutData { get; set; }
    }
}
