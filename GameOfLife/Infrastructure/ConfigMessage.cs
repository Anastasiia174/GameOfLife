using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Infrastructure
{
    public class ConfigMessage
    {
        public ConfigMessage(GameConfiguration configuration)
        {
            Configuration = configuration;
        }

        public GameConfiguration Configuration { get; set; }
    }
}
