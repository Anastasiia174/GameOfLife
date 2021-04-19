using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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

            PlaygroundImageSource = _playground.Body;
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

        private RelayCommand _startCommand;

        public RelayCommand StartCommand
        {
            get
            {
                return _startCommand ??
                       (_startCommand = new RelayCommand(StartGame));
            }

        }

        public RelayCommand PauseCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand ChangeConfigurationCommand { get; private set; }

        public RelayCommand RandomizeCellsCommand { get; private set; }

        private RelayCommand<PlaygroundState> _toggleCellStateCommand;
        

        public RelayCommand<PlaygroundState> ToggleCellStateCommand
        {
            get
            {
                return _toggleCellStateCommand ??
                       (_toggleCellStateCommand = new RelayCommand<PlaygroundState>(ToggleCellState));
            }
        }

        private void StartGame()
        {
            _gameEngine.MoveToNextGeneration();

            RaisePropertyChanged(() => PlaygroundImageSource);
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
    }
}
