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
            this.CreateMenuItems();
        }

        public void CreateMenuItems()
        {
            MenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new PlaygroundViewModel(this)
                {
                    Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Gamepad },
                    Label = "Play game",
                },
                new SavesViewModel(this)
                {
                    Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.ContentSaveAll },
                    Label = "Saved games",
                },
                new LogsViewModel(this)
                {
                    Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Server },
                    Label = "Game logs",
                },
            };

            MenuOptionItems = new ObservableCollection<MenuItemViewModel>
            {
                new SettingsViewModel(this)
                {
                    Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Cog },
                    Label = "Settings",
                }
            };
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
