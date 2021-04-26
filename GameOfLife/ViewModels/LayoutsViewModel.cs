using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GameOfLife.Engine;
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

            var loadLayoutMessage = new LoadLayoutMessage(layout);
            var configMessage = new ConfigMessage(new GameConfiguration(layout.Layout.Width,
                layout.Layout.Height));

            Messenger.Default.Send(loadLayoutMessage);
            Messenger.Default.Send(configMessage, "Settings");
        }

        private async void LoadRandomLayout()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var randomLayout = Layouts.ElementAt(random.Next(Layouts.Count));
            var layoutFromDb = await _gameLayoutService.GetGameLayoutByTitleAsync(randomLayout.Title);
            var loadLayoutMessage = new LoadLayoutMessage(layoutFromDb);
            var configMessage = new ConfigMessage(new GameConfiguration(layoutFromDb.Layout.Width,
                layoutFromDb.Layout.Height));

            Messenger.Default.Send(loadLayoutMessage);
            Messenger.Default.Send(configMessage, "Settings");
        }

        private async void RemoveLayoutAsync()
        {
            IsBusy = true;
            await _gameLayoutService.RemoveGameLayoutAsync(SelectedLayout);
            IsBusy = false;

            Layouts.Remove(SelectedLayout);
        }

        private async void SaveLayout(GameLayout layout)
        {
            layout.Title = LayoutTitle;

            IsBusy = true;
            await _gameLayoutService.SaveGameLayoutAsync(layout);
            IsBusy = false;

            //clear layout as we store it empty
            layout.Layout = null;
            Layouts.Add(layout);
        }

        private async void LoadLayoutsAsync()
        {
            if (!_isLoaded)
            {
                IsBusy = true;
                var layouts = await _gameLayoutService.GetAllGameLayoutsAsync();
                IsBusy = false;

                Layouts = new ObservableCollection<GameLayout>(layouts);
                _isLoaded = true;
            }
        }
    }
}
