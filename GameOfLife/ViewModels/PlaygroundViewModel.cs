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
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GameOfLife.Engine;
using GameOfLife.Infrastructure;
using GameOfLife.Services;

namespace GameOfLife.ViewModels
{
    public class PlaygroundViewModel : MenuItemViewModel
    {
        private readonly IGameSaveService _gameSaveService;

        private readonly DispatcherTimer _timer;

        private readonly IGameEngine _gameEngine;

        private int _width;

        private int _height;

        private bool _isSaved;

        private UniverseConfiguration _universeConfiguration;

        public PlaygroundViewModel(GameConfiguration configuration, IGameSaveService gameSaveService, MainViewModel mainViewModel) : base(mainViewModel)
        {
            _gameSaveService = gameSaveService;
            _width = configuration.Width;
            _height = configuration.Height;
            _universeConfiguration = configuration.UniverseConfiguration;
            _isEditable = configuration.IsEditable;

            _gameEngine = new GameEngine(_width, _height, _universeConfiguration);

            PlaygroundImageSource = _gameEngine.Playground;
            Title = "New game*";

            _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
            _timer.Tick += UpdatePlayground;

            Messenger.Default.Register<ConfigMessage>(
                this, 
                "Playground",
                message => ChangeConfiguration(message.Configuration));

            Messenger.Default.Register<LoadSaveMessage>(
                this,
                message =>
                LoadGameSave(message.GameToSave));

            Messenger.Default.Register<SaveGameMessage>(
                this,
                message =>
                {
                    if (GameRunning)
                    {
                        message.ErrorAction("The game is running. Pause game before saving.");
                        return;
                    }

                    if (!_isSaved)
                    {
                        message.SaveAction(SaveGame(message.GameTitle));
                    }
                    else
                    {
                        message.ErrorAction("The game has been already saved.");
                    }
                });
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

        private int _generationNumber;

        public int GenerationNumber
        {
            get => _generationNumber;
            set
            {
                Set(() => GenerationNumber, ref _generationNumber, value);
            }
        }

        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                Set(() => Title, ref _title, value);
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
            _isSaved = false;
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
            GenerationNumber = _gameEngine.CurrentGenerationNumber;

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

            _gameEngine.ChangeUniverseState(new Cell(cellX, cellY));

            RaisePropertyChanged(() => PlaygroundImageSource);
            _isSaved = false;
        }

        private void ResetGame()
        {
            _gameEngine.ResetGame(_width, _height, _universeConfiguration);

            PlaygroundImageSource = _gameEngine.Playground;
            GenerationNumber = _gameEngine.CurrentGenerationNumber;
            GameEnded = _gameEngine.GameEnded;
            GameRunning = false;
            GameStarted = false;
            _isSaved = false;
        }

        private void RandomizeCells()
        {
            ResetGame();
            _gameEngine.RandomizeGame();

            PlaygroundImageSource = _gameEngine.Playground;
        }

        private void ChangeConfiguration(GameConfiguration configuration)
        {
            _width = configuration.Width;
            _height = configuration.Height;
            _universeConfiguration = configuration.UniverseConfiguration;
            IsEditable = configuration.IsEditable;

            ResetGame();
        }

        private GameSave SaveGame(string title)
        {
            Title = title;
            var gameSave = _gameEngine.SaveGame();
            gameSave.Title = Title;
            _isSaved = true;

            return gameSave;
        }

        private void LoadGameSave(GameSave save)
        {
            _gameEngine.LoadGame(save);

            _width = save.Playground.Width;
            _height = save.Playground.Height;
            GenerationNumber = _gameEngine.CurrentGenerationNumber;
            GameEnded = _gameEngine.GameEnded;
            PlaygroundImageSource = _gameEngine.Playground;
            Title = save.Title;
        }
    }
}
