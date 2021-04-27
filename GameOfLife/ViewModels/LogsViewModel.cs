using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GameOfLife.Infrastructure;
using GameOfLife.Services;

namespace GameOfLife.ViewModels
{
    public class LogsViewModel : MenuItemViewModel
    {
        private readonly IGameLogService _gameLogService;

        private readonly IDialogService _dialogService;

        private bool _isLoaded;

        public LogsViewModel(IGameLogService gameLogService, IDialogService dialogService, MainViewModel mainViewModel) : base(mainViewModel)
        {
            _gameLogService = gameLogService;
            _dialogService = dialogService;

            Logs = new ObservableCollection<GameLog>();

            // subscribe to log messages only if service exists
            if (_gameLogService != null)
            {
                Messenger.Default.Register<LogEventMessage>(
                    this,
                    SaveLog);
            }
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

        private DateTime? _selectedStartDateTime;

        public DateTime? SelectedStartDateTime
        {
            get => _selectedStartDateTime;
            set
            {
                Set(() => SelectedStartDateTime, ref _selectedStartDateTime, value);
                FilteredLogs.Refresh();
            }
        }

        private DateTime? _selectedEndDateTime;

        public DateTime? SelectedEndDateTime
        {
            get => _selectedEndDateTime;
            set
            {
                Set(() => SelectedEndDateTime, ref _selectedEndDateTime, value);
                FilteredLogs.Refresh();
            }
        }

        private ObservableCollection<GameLog> _logs;

        public ObservableCollection<GameLog> Logs
        {
            get => _logs;
            set
            {
                Set(() => Logs, ref _logs, value);
            }
        }

        private ListCollectionView _filteredLogs;

        public ListCollectionView FilteredLogs
        {
            get => _filteredLogs;
            set
            {
                Set(() => FilteredLogs, ref _filteredLogs, value);
            }
        }

        private RelayCommand _loadLogsCommand;

        public RelayCommand LoadLogsCommand =>
            _loadLogsCommand
            ?? (_loadLogsCommand = new RelayCommand(LoadLogsAsync));

        public override void Cleanup()
        {
            _gameLogService?.Dispose();
            Logs?.Clear();
            FilteredLogs = null;

            base.Cleanup();
        }

        private async void LoadLogsAsync()
        {
            if (!_isLoaded)
            {
                IsBusy = true;
                var logs = await _gameLogService.GetAllGameLogsAsync();
                IsBusy = false;

                //if any logs was added before loading logs from DB - save them to DB
                if (Logs.Any())
                {
                    foreach (var gameLog in Logs)
                    {
                        await _gameLogService.SaveGameLogAsync(gameLog);
                    }
                }

                if (logs != null)
                {
                    Logs = new ObservableCollection<GameLog>(logs.Concat(Logs));
                    FilteredLogs = new ListCollectionView(Logs) { Filter = Filter };
                }
                else
                {
                    _dialogService.ShowMessage("Could not load game logs from database");
                }

                _isLoaded = true;
            }
        }

        private bool Filter(object obj)
        {
            bool result = true;

            if (obj is GameLog current)
            {
                if (SelectedStartDateTime != null && SelectedEndDateTime == null)
                {
                    result = current.EventDateTime >= SelectedStartDateTime.Value;
                }
                else if (SelectedStartDateTime != null && SelectedEndDateTime != null)
                {
                    result = current.EventDateTime >= SelectedStartDateTime.Value &&
                             current.EventDateTime <= SelectedEndDateTime.Value;
                }
                else if (SelectedStartDateTime == null && SelectedEndDateTime != null)
                {
                    result = current.EventDateTime <= SelectedEndDateTime.Value;
                }
            }
            return result;
        }

        private async void SaveLog(LogEventMessage message)
        {
            if (_isLoaded)
            {
                var result = await _gameLogService.SaveGameLogAsync(message.Log);
                if (!result)
                {
                    _dialogService.ShowMessage("Could not save log to database.");
                }

                message.Log.Playground = null;
            }
            
            Logs.Add(message.Log);
        }
    }
}
