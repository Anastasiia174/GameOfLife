using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MahApps.Metro.Controls;

namespace GameOfLife.ViewModels
{
    public class MenuItemViewModel : ViewModelBase, IHamburgerMenuItemBase
    {
        private object _icon;
        private object _label;
        private bool _isVisible = true;

        public MenuItemViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }

        public MainViewModel MainViewModel { get; }

        public object Icon
        {
            get => _icon;
            set => Set(() => Icon,ref _icon, value);
        }

        public object Label
        {
            get => _label;
            set => Set(() => Label,ref _label, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => Set(() => IsVisible, ref _isVisible, value);
        }
    }
}
