using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using GameOfLife.Engine;
using GameOfLife.Infrastructure;

namespace GameOfLife.ViewModels
{
    public class PlaygroundViewModel : ViewModelBase
    {
        private int _width;

        private int _height;

        private IGameEngine _gameEngine;

        private Playground _playground;

        private readonly DispatcherTimer _timer;

        public PlaygroundViewModel()
        : this(10, 10)
        {
        }

        public PlaygroundViewModel(int width, int height)
        {
            _width = width;
            _height = height;

            _playground = new Playground(_width, _height);
            _gameEngine = new GameEngine(_playground);
            _playground.Configuration = UniverseConfiguration.Closed;
            PlaygroundImageSource = _playground.Body;

            _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
            _timer.Tick += UpdatePlayground;
        }

        private Bitmap _playgroundImageSource;

        public Bitmap PlaygroundImageSource
        {
            get
            {
                return _playgroundImageSource;
            }
            set
            {
                Set(() => PlaygroundImageSource, ref _playgroundImageSource, value);
            }
        }

        private CellPosition _selectedCell;

        public CellPosition SelectedCell
        {
            get
            {
                return _selectedCell;
            }
            set
            {
                Set(() => SelectedCell, ref _selectedCell, value);
            }
        }

        private bool _gameEnded;

        public bool GameEnded
        {
            get
            {
                return _gameEnded;
            }
            set
            {
                Set(() => GameEnded, ref _gameEnded, value);
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _gameRunning;

        public bool GameRunning
        {
            get
            {
                return _gameRunning;
            }
            set
            {
                Set(() => GameRunning, ref _gameRunning, value);
            }
        }

        private RelayCommand _startCommand;

        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand ??
                       (_startCommand = new RelayCommand(StartGame, CanStartGame));
            }

        }

        private RelayCommand _pauseCommand;

        public RelayCommand PauseCommand
        {
            get
            {
                return _pauseCommand ??
                       (_pauseCommand = new RelayCommand(PauseGame, CanPauseGame));
            }
        }

        private RelayCommand _resetCommand;

        public RelayCommand ResetCommand
        {
            get
            {
                return _resetCommand ??
                       (new RelayCommand(ResetGame));
            }
        }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand ChangeConfigurationCommand { get; private set; }

        public RelayCommand RandomizeCellsCommand { get; private set; }

        private RelayCommand<PlaygroundState> _toggleCellStateCommand;

        public RelayCommand<PlaygroundState> ToggleCellStateCommand
        {
            get
            {
                return _toggleCellStateCommand ??
                       (_toggleCellStateCommand = new RelayCommand<PlaygroundState>(ToggleCellState, CanToggleCellState));
            }
        }

        private void StartGame()
        {
            _timer.Start();
            GameRunning = true;
        }

        private bool CanStartGame()
        {
            return !GameEnded && !GameRunning;
        }

        private async void UpdatePlayground(object sender, object e)
        {
            await Task.Factory.StartNew(() =>
            {
                {
                    _gameEngine.MoveToNextGeneration();
                }
            });

            RaisePropertyChanged(() => PlaygroundImageSource);
            GameEnded = _gameEngine.GameEnded;

            if (GameEnded)
            {
                _timer.Stop();
                GameRunning = false;
            }
        }

        private void PauseGame()
        {
            _timer.Stop();
            GameRunning = false;
        }

        private bool CanPauseGame()
        {
            return GameRunning;
        }

        private void ToggleCellState(PlaygroundState playgroundState)
        {
            var cellWidth = playgroundState.Width / _width;
            var cellHeight = playgroundState.Height / _height;

            var cellX = (int)(playgroundState.ActiveCell.X / cellWidth);
            var cellY = (int)(playgroundState.ActiveCell.Y / cellHeight);

            SelectedCell = playgroundState.ActiveCell;

            _gameEngine.ChangeUniverseState(new Cell(cellX, cellY));

            RaisePropertyChanged(() => PlaygroundImageSource);
        }

        private bool CanToggleCellState(PlaygroundState playgroundState)
        {
            return !GameRunning;
        }

        private void ResetGame()
        {
            _playground = new Playground(_width, _height);
            _playground.Configuration = UniverseConfiguration.Closed;
            _gameEngine = new GameEngine(_playground);
            PlaygroundImageSource = _playground.Body;
            GameEnded = _gameEngine.GameEnded;
            GameRunning = false;
        }
    }
}
