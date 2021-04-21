using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Infrastructure;

namespace GameOfLife.ViewModels
{
    public class SettingsViewModel : MenuItemViewModel
    {
        public SettingsViewModel(GameConfiguration configuration, MainViewModel mainViewModel) : base(mainViewModel)
        {
        }
    }
}
