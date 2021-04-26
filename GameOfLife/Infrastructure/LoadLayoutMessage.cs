using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Infrastructure
{
    public class LoadLayoutMessage
    {
        public LoadLayoutMessage(GameLayout layout)
        {
            GameLayout = layout;
        }

        public GameLayout GameLayout { get; set; }
    }
}
