using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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

            IsChanged = false;
        }

        private int _width;

        public int Width
        {
            get => _width;
            set
            {
                Set(() => Width, ref _width, value);
                IsChanged = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private int _height;

        public int Height
        {
            get => _height;
            set
            {
                Set(() => Height, ref _height, value);
                IsChanged = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isEditable;

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                Set(() => IsEditable, ref _isEditable, value);
                IsChanged = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private UniverseConfiguration _universeConfiguration;

        public UniverseConfiguration UniverseConfiguration
        {
            get => _universeConfiguration;
            set
            {
                Set(() => UniverseConfiguration, ref _universeConfiguration, value);
                IsChanged = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isChanged;

        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                Set(() => IsChanged, ref _isChanged, value);
            }
        }

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand =>
            _saveCommand ??
            (_saveCommand = new RelayCommand(SaveConfiguration, () => IsChanged));

        private void SaveConfiguration()
        {
            var newConfiguration = new GameConfiguration(Width, Height, UniverseConfiguration, IsEditable);
            var message = new ConfigMessage(newConfiguration);

            Messenger.Default.Send(message);

            IsChanged = false;
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
