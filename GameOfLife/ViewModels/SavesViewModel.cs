using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GameOfLife.Engine;
using GameOfLife.Infrastructure;
using GameOfLife.Services;

namespace GameOfLife.ViewModels
{
    public class SavesViewModel : MenuItemViewModel
    {
        private readonly IGameSaveService _gameSaveService;

        public SavesViewModel(IGameSaveService gameSaveService, MainViewModel mainViewModel) : base(mainViewModel)
        {
            _gameSaveService = gameSaveService;
        }

        private ObservableCollection<GameSave> _saves;

        public ObservableCollection<GameSave> Saves
        {
            get => _saves ?? (_saves = new ObservableCollection<GameSave>(_gameSaveService.GetAllGameSaves()));
            set
            {
                Set(() => Saves, ref _saves, value);
            }
        }

        private GameSave _selectedSave;

        public GameSave SelectedSave
        {
            get => _selectedSave;
            set
            {
                Set(() => SelectedSave, ref _selectedSave, value);
                LoadCommand.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _loadCommand;

        public RelayCommand LoadCommand =>
            _loadCommand ??
            (_loadCommand = new RelayCommand(LoadSave, () => SelectedSave != null));

        private RelayCommand _loadRandomCommand;

        public RelayCommand LoadRandomCommand =>
            _loadRandomCommand ??
            (_loadRandomCommand = new RelayCommand(LoadRandom, () => Saves.Any()));

        private RelayCommand _removeCommand;

        public RelayCommand RemoveCommand =>
            _removeCommand ??
            (_removeCommand = new RelayCommand(RemoveSave));

        private void LoadSave()
        {
            var saveMessage = new LoadSaveMessage(SelectedSave);
            var configMessage = new ConfigMessage(new GameConfiguration(SelectedSave.Playground.Width,
                SelectedSave.Playground.Height, SelectedSave.UniverseConfiguration));

            Messenger.Default.Send(saveMessage);
            Messenger.Default.Send(configMessage, "Settings");
        }

        private void LoadRandom()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var randomSave = Saves.ElementAt(random.Next(Saves.Count));
            var saveMessage = new LoadSaveMessage(randomSave);
            var configMessage = new ConfigMessage(new GameConfiguration(randomSave.Playground.Width,
                randomSave.Playground.Height, randomSave.UniverseConfiguration));

            Messenger.Default.Send(saveMessage);
            Messenger.Default.Send(configMessage, "Settings");
        }

        private void RemoveSave()
        {
            _gameSaveService.RemoveGameSave(SelectedSave);
            Saves.Remove(SelectedSave);
        }
    }
}
