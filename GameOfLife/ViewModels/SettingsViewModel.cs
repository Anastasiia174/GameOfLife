using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GameOfLife.Engine;
using GameOfLife.Infrastructure;

namespace GameOfLife.ViewModels
{
    public class SettingsViewModel : MenuItemViewModel
    {
        public SettingsViewModel(GameConfiguration configuration, MainViewModel mainViewModel) : base(mainViewModel)
        {
            Width = configuration.Width;
            Height = configuration.Height;
            IsEditable = configuration.IsEditable;
            UniverseConfiguration = configuration.UniverseConfiguration;
        }

        private int _width;

        public int Width
        {
            get => _width;
            set
            {
                Set(() => Width, ref _width, value);
            }
        }

        private int _height;

        public int Height
        {
            get => _height;
            set
            {
                Set(() => Height, ref _height, value);
            }
        }

        private bool _isEditable;

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                Set(() => IsEditable, ref _isEditable, value);
            }
        }

        private UniverseConfiguration _universeConfiguration;

        public UniverseConfiguration UniverseConfiguration
        {
            get => _universeConfiguration;
            set
            {
                Set(() => UniverseConfiguration, ref _universeConfiguration, value);
            }
        }

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand =>
            _saveCommand ??
            (_saveCommand = new RelayCommand(SaveConfiguration));

        private void SaveConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}
