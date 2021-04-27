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

        private bool _isLoaded;

        public LogsViewModel(IGameLogService gameLogService, MainViewModel mainViewModel) : base(mainViewModel)
        {
            _gameLogService = gameLogService;

            Messenger.Default.Register<LogEventMessage>(
                this,
                message =>
                {
                    _gameLogService.SaveGameLogAsync(message.Log);
                    message.Log.Playground = null;
                    Logs.Add(message.Log);
                });
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

        private async void LoadLogsAsync()
        {
            if (!_isLoaded)
            {
                IsBusy = true;
                var logs = await _gameLogService.GetAllGameLogsAsync();
                IsBusy = false;

                Logs = new ObservableCollection<GameLog>(logs);
                FilteredLogs = new ListCollectionView(Logs) { Filter = Filter };
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
    }
}
