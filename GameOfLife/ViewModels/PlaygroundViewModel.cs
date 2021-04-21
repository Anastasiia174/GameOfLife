using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class PlaygroundViewModel : MenuItemViewModel
    {
        private readonly DispatcherTimer _timer;

        private int _width;

        private int _height;

        private UniverseConfiguration _universeConfiguration;

        private IGameEngine _gameEngine;

        private Playground _playground;

        public PlaygroundViewModel(GameConfiguration configuration, MainViewModel mainViewModel) : base(mainViewModel)
        {
            _width = configuration.Width;
            _height = configuration.Height;
            _universeConfiguration = configuration.UniverseConfiguration;
            _isEditable = configuration.IsEditable;

            _playground = new Playground(_width, _height, _universeConfiguration);
            _gameEngine = new GameEngine(_playground);
            
            PlaygroundImageSource = _playground.Body;

            _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(1000) };
            _timer.Tick += UpdatePlayground;
        }

        private Bitmap _playgroundImageSource;

        public Bitmap PlaygroundImageSource
        {
            get => _playgroundImageSource;
            set
            {
                Set(() => PlaygroundImageSource, ref _playgroundImageSource, value);
            }
        }

        private CellPosition _selectedCell;

        public CellPosition SelectedCell
        {
            get => _selectedCell;
            set
            {
                Set(() => SelectedCell, ref _selectedCell, value);
            }
        }

        private bool _gameEnded;

        public bool GameEnded
        {
            get => _gameEnded;
            set
            {
                Set(() => GameEnded, ref _gameEnded, value);
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _gameRunning;

        public bool GameRunning
        {
            get => _gameRunning;
            set
            {
                Set(() => GameRunning, ref _gameRunning, value);
            }
        }

        private bool _gameStarted;

        public bool GameStarted
        {
            get => _gameStarted;
            set
            {
                Set(() => GameStarted, ref _gameStarted, value);
            }
        }

        private bool _isEditable;

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                Set(() => IsEditable, ref _isEditable, value);
            }
        }

        private RelayCommand _startCommand;

        public RelayCommand StartCommand =>
            _startCommand ??
            (_startCommand = new RelayCommand(StartGame, () => !GameEnded && !GameRunning));

        private RelayCommand _pauseCommand;

        public RelayCommand PauseCommand =>
            _pauseCommand ??
            (_pauseCommand = new RelayCommand(PauseGame, () => GameRunning));

        private RelayCommand _resetCommand;

        public RelayCommand ResetCommand =>
            _resetCommand ??
            (_resetCommand = new RelayCommand(ResetGame, () => !GameRunning));

        public RelayCommand SaveCommand { get; private set; }

        private RelayCommand _randomizeCellsCommand;

        public RelayCommand RandomizeCellsCommand =>
            _randomizeCellsCommand ??
            (_randomizeCellsCommand = new RelayCommand(RandomizeCells, () => !GameRunning && !GameStarted));

        private RelayCommand<PlaygroundState> _toggleCellStateCommand;

        public RelayCommand<PlaygroundState> ToggleCellStateCommand =>
            _toggleCellStateCommand ??
            (_toggleCellStateCommand = new RelayCommand<PlaygroundState>(
                ToggleCellState, 
                (state) => !GameStarted || (!GameRunning && IsEditable)));

        private void StartGame()
        {
            _timer.Start();
            GameRunning = true;
            GameStarted = true;
        }

        private async void UpdatePlayground(object sender, object e)
        {
            System.Diagnostics.Debug.WriteLine("Start updating playground...");
            var timer = Stopwatch.StartNew();
            await Task.Factory.StartNew(() =>
            {
                {
                    _gameEngine.MoveToNextGeneration();
                }
            });

            System.Diagnostics.Debug.WriteLine($"End updating playground... Updated in {timer.ElapsedMilliseconds}ms");
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

        private void ResetGame()
        {
            _playground = new Playground(_width, _height, _universeConfiguration);
            _gameEngine = new GameEngine(_playground);

            PlaygroundImageSource = _playground.Body;
            GameEnded = _gameEngine.GameEnded;
            GameRunning = false;
            GameStarted = false;
        }

        private void RandomizeCells()
        {
            var randomCells = new Cell[_width, _height];

            var random = new Random((int)DateTime.Now.Ticks);
            for (int x = 0; x < randomCells.GetLength(0); x++)
            {
                for (int y = 0; y < randomCells.GetLength(1); y++)
                {
                    randomCells[x, y] = new Cell(x, y, random.Next(2) == 1);
                }
            }

            _playground.LoadGameFromCells(randomCells);
            _gameEngine = new GameEngine(_playground);

            PlaygroundImageSource = _playground.Body;
        }

        private void ChangeConfiguration(GameConfiguration configuration)
        {
            _width = configuration.Width;
            _height = configuration.Height;
            _universeConfiguration = configuration.UniverseConfiguration;

            ResetGame();
        }
    }
}
