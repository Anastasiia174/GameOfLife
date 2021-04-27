using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GameOfLife.Infrastructure;
using GameOfLife.Services;

namespace GameOfLife.ViewModels
{
    public class LayoutsViewModel : MenuItemViewModel
    {
        private readonly IGameLayoutService _gameLayoutService;

        private readonly IDialogService _dialogService;

        private bool _isLoaded;

        public LayoutsViewModel(IGameLayoutService gameLayoutService, IDialogService dialogService, MainViewModel mainViewModel) : base(mainViewModel)
        {
            _gameLayoutService = gameLayoutService;
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

        private ObservableCollection<GameLayout> _layouts;

        public ObservableCollection<GameLayout> Layouts
        {
            get => _layouts ?? (_layouts = new ObservableCollection<GameLayout>());
            set
            {
                Set(() => Layouts, ref _layouts, value);
            }
        }

        private GameLayout _selectedLayout;

        public GameLayout SelectedLayout
        {
            get => _selectedLayout;
            set
            {
                Set(() => SelectedLayout, ref _selectedLayout, value);
                LoadCommand.RaiseCanExecuteChanged();
            }
        }

        private string _layoutTitle;

        public string LayoutTitle
        {
            get => _layoutTitle;
            set
            {
                Set(() => LayoutTitle, ref _layoutTitle, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _loadLayoutsCommand;

        public RelayCommand LoadLayoutsCommand =>
            _loadLayoutsCommand
            ?? (_loadLayoutsCommand = new RelayCommand(LoadLayoutsAsync));

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand =>
            _saveCommand ??
            (_saveCommand = new RelayCommand(SaveLayout, () => !string.IsNullOrEmpty(LayoutTitle)));

        private RelayCommand _loadCommand;

        public RelayCommand LoadCommand =>
            _loadCommand ??
            (_loadCommand = new RelayCommand(LoadLayout, () => SelectedLayout != null));

        private RelayCommand _loadRandomCommand;

        public RelayCommand LoadRandomCommand =>
            _loadRandomCommand ??
            (_loadRandomCommand = new RelayCommand(LoadRandomLayout, () => Layouts.Any()));

        private RelayCommand _deleteCommand;

        public RelayCommand DeleteCommand =>
            _deleteCommand ??
            (_deleteCommand = new RelayCommand(RemoveLayoutAsync, () => SelectedLayout != null));

        public override void Cleanup()
        {
            _gameLayoutService?.Dispose();
            Layouts?.Clear();

            base.Cleanup();
        }

        private void SaveLayout()
        {
            if (_layouts.Any(s => s.Title == LayoutTitle))
            {
                _dialogService.ShowMessage($"The layout with title \"{LayoutTitle}\" already exists");
                return;
            }

            var saveLayoutMessage = new SaveLayoutMessage()
            {
                SaveAction = SaveLayout,
                ErrorAction = error => _dialogService.ShowMessage(error)
            };

            Messenger.Default.Send(saveLayoutMessage);
        }

        private async void LoadLayout()
        {
            var layout = await _gameLayoutService.GetGameLayoutByTitleAsync(SelectedLayout.Title);

            if (layout != null)
            {
                var loadLayoutMessage = new LoadLayoutMessage(layout);
                var configMessage = new ConfigMessage(new GameConfiguration(layout.Layout.Width,
                    layout.Layout.Height));

                Messenger.Default.Send(loadLayoutMessage);
                Messenger.Default.Send(configMessage, "Settings");
            }
            else
            {
                _dialogService.ShowMessage($"Could not load layout \"{SelectedLayout.Title}\" from database");
            }
        }

        private async void LoadRandomLayout()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var randomLayoutTitle = Layouts.ElementAt(random.Next(Layouts.Count)).Title;
            var layoutFromDb = await _gameLayoutService.GetGameLayoutByTitleAsync(randomLayoutTitle);

            if (layoutFromDb != null)
            {
                var loadLayoutMessage = new LoadLayoutMessage(layoutFromDb);
                var configMessage = new ConfigMessage(new GameConfiguration(layoutFromDb.Layout.Width,
                    layoutFromDb.Layout.Height));

                Messenger.Default.Send(loadLayoutMessage);
                Messenger.Default.Send(configMessage, "Settings");
            }
            else
            {
                _dialogService.ShowMessage($"Could not load layout \"{randomLayoutTitle}\" from database");
            }
        }

        private async void RemoveLayoutAsync()
        {
            IsBusy = true;
            bool result = await _gameLayoutService.RemoveGameLayoutAsync(SelectedLayout);
            IsBusy = false;

            if (result)
            {
                Layouts.Remove(SelectedLayout);
            }
            else
            {
                _dialogService.ShowMessage($"Could not remove layout \"{SelectedLayout.Title}\" from database");
            }
        }

        private async void SaveLayout(GameLayout layout)
        {
            layout.Title = LayoutTitle;

            IsBusy = true;
            bool result = await _gameLayoutService.SaveGameLayoutAsync(layout);
            IsBusy = false;

            if (result)
            {
                //clear layout as we store it empty
                layout.Layout = null;
                Layouts.Add(layout);
            }
            else
            {
                _dialogService.ShowMessage($"Could not save layout \"{layout.Title}\" to database");
            }
        }

        private async void LoadLayoutsAsync()
        {
            if (!_isLoaded)
            {
                IsBusy = true;
                var layouts = await _gameLayoutService.GetAllGameLayoutsAsync();
                IsBusy = false;

                if (layouts != null)
                {
                    Layouts = new ObservableCollection<GameLayout>(layouts);
                }
                else
                {
                    _dialogService.ShowMessage("Could not load layouts from database");
                }
                
                _isLoaded = true;
            }
        }
    }
}
