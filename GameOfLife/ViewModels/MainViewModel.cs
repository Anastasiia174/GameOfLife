using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MahApps.Metro.IconPacks;

namespace GameOfLife.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<MenuItemViewModel> _menuItems;
        private ObservableCollection<MenuItemViewModel> _menuOptionItems;

        public MainViewModel()
        {
        }

        public void CreateMenuItems(IEnumerable<MenuItemViewModel> menuItems, IEnumerable<MenuItemViewModel> menuOptionsItems)
        {
            MenuItems = new ObservableCollection<MenuItemViewModel>(menuItems);
            MenuOptionItems = new ObservableCollection<MenuItemViewModel>(menuOptionsItems);
        }

        public ObservableCollection<MenuItemViewModel> MenuItems
        {
            get => _menuItems;
            set => Set(() => MenuItems, ref _menuItems, value);
        }

        public ObservableCollection<MenuItemViewModel> MenuOptionItems
        {
            get => _menuOptionItems;
            set => Set(() => MenuOptionItems,ref _menuOptionItems, value);
        }
    }
}
