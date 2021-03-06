using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        private readonly IDialogService _dialogService;

        private bool _isLoaded;

        public SavesViewModel(IGameSaveService gameSaveService, IDialogService dialogService, MainViewModel mainViewModel) : base(mainViewModel)
        {
            _gameSaveService = gameSaveService;
            _dialogService = dialogService;
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Set(() => IsBusy, ref _isBusy, value);
            }
        }

        private ObservableCollection<GameSave> _saves;

        public ObservableCollection<GameSave> Saves
        {
            get => _saves ?? (_saves = new ObservableCollection<GameSave>());
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
                RemoveCommand.RaiseCanExecuteChanged();
            }
        }

        private string _gameTitle;

        public string GameTitle
        {
            get => _gameTitle;
            set
            {
                Set(() => GameTitle, ref _gameTitle, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _loadSavesCommand;

        public RelayCommand LoadSavesCommand =>
            _loadSavesCommand
            ?? (_loadSavesCommand = new RelayCommand(LoadSavesAsync));

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand =>
            _saveCommand ??
            (_saveCommand = new RelayCommand(SaveGame, () => !string.IsNullOrEmpty(GameTitle)));

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
            (_removeCommand = new RelayCommand(RemoveSaveAsync, () => SelectedSave != null));

        public override void Cleanup()
        {
            _gameSaveService?.Dispose();
            Saves?.Clear();

            base.Cleanup();
        }

        private void SaveGame()
        {
            if (_saves.Any(s => s.Title == GameTitle))
            {
                _dialogService.ShowMessage($"The game with title \"{GameTitle}\" already exists");
                return;
            }

            var saveGameMessage = new SaveGameMessage()
            {
                GameTitle = GameTitle,
                SaveAction = SaveGame,
                ErrorAction = error => _dialogService.ShowMessage(error)
            };

            Messenger.Default.Send(saveGameMessage);
        }

        private async void LoadSave()
        {
            var save = await _gameSaveService.GetGameSaveByTitleAsync(SelectedSave.Title);

            if (save != null)
            {
                var saveMessage = new LoadSaveMessage(save);
                var configMessage = new ConfigMessage(new GameConfiguration(save.Playground.Width,
                    save.Playground.Height, save.UniverseConfiguration));

                Messenger.Default.Send(saveMessage);
                Messenger.Default.Send(configMessage, "Settings");
            }
            else
            {
                _dialogService.ShowMessage($"Could not load save \"{SelectedSave.Title}\" from database");
            }
        }

        private async void LoadRandom()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var randomSaveTitle = Saves.ElementAt(random.Next(Saves.Count)).Title;
            var save = await _gameSaveService.GetGameSaveByTitleAsync(randomSaveTitle);

            if (save != null)
            {
                var saveMessage = new LoadSaveMessage(save);
                var configMessage = new ConfigMessage(new GameConfiguration(save.Playground.Width,
                    save.Playground.Height, save.UniverseConfiguration));

                Messenger.Default.Send(saveMessage);
                Messenger.Default.Send(configMessage, "Settings");
            }
            else
            {
                _dialogService.ShowMessage($"Could not load save \"{randomSaveTitle}\" from database");
            }
        }

        private async void RemoveSaveAsync()
        {
            IsBusy = true;
            var result = await _gameSaveService.RemoveGameSaveAsync(SelectedSave);
            IsBusy = false;

            if (result)
            {
                Saves.Remove(SelectedSave);
            }
            else
            {
                _dialogService.ShowMessage($"Could not remove save \"{SelectedSave.Title}\" from database");
            }
        }

        private async void SaveGame(GameSave save)
        {
            IsBusy = true;
            var result = await _gameSaveService.SaveGameSaveAsync(save);
            IsBusy = false;

            if (result)
            {
                //clear playground
                save.Playground = null;
                Saves.Add(save);
            }
            else
            {
                _dialogService.ShowMessage($"Could not save \"{SelectedSave.Title}\" to database");
            }
        }

        private async void LoadSavesAsync()
        {
            if (!_isLoaded)
            {
                IsBusy = true;
                var saves = await _gameSaveService.GetAllGameSavesAsync();
                IsBusy = false;

                if (saves != null)
                {
                    Saves = new ObservableCollection<GameSave>(saves);
                    LoadRandomCommand.RaiseCanExecuteChanged();
                }
                else
                {
                    _dialogService.ShowMessage("Could not load game saves from database");
                }

                _isLoaded = true;
            }
        }
    }
}
