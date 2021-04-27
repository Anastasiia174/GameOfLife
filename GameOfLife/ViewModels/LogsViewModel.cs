using System.Collections.ObjectModel;
using System.Linq;
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
            _logs = new ObservableCollection<GameLog>();
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

        private ObservableCollection<GameLog> _logs;

        public ObservableCollection<GameLog> Logs
        {
            get => _logs;
            set
            {
                Set(() => Logs, ref _logs, value);
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
                _isLoaded = true;
            }
        }
    }
}
