using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GameOfLife.Engine;
using GameOfLife.Helpers.Async;
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
            get => _logs ?? (_logs = new ObservableCollection<GameLog>());
            set
            {
                Set(() => Logs, ref _logs, value);
            }
        }

        private ICommand _loadLogsCommand;

        public ICommand LoadLogsCommand =>
            _loadLogsCommand
            ?? (_loadLogsCommand = new AwaitableDelegateCommand(LoadLogsAsync));

        private async Task LoadLogsAsync()
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
