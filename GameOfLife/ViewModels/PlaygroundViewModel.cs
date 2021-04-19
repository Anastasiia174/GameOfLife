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
using GameOfLife.Core.Services;
using GameOfLife.Helpers;

namespace GameOfLife.ViewModels
{
    public class PlaygroundViewModel : ViewModelBase
    {
        private int _width;

        private int _height;

        private readonly IRenderService _imageService;

        public PlaygroundViewModel()
        : this(100, 100, new RenderService())
        {
        }

        public PlaygroundViewModel(int width, int height, IRenderService imageService)
        {
            _width = width;
            _height = height;
            _imageService = imageService;

            var path = _imageService.CreatePlayground(_width, _height);
            _playgroundImageSource = new Uri(Path.GetFullPath(path));
        }

        private Uri _playgroundImageSource;

        public Uri PlaygroundImageSource
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

        public RelayCommand StartCommand { get; private set; }

        public RelayCommand PauseCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand ChangeConfigurationCommand { get; private set; }

        public RelayCommand RandomizeCellsCommand { get; private set; }

        private RelayCommand<CellPosition> _toggleCellStateCommand;

        public RelayCommand<CellPosition> ToggleCellStateCommand
        {
            get
            {
                return _toggleCellStateCommand ??
                       (_toggleCellStateCommand = new RelayCommand<CellPosition>(ToggleCellState));
            }
        }

        private void ToggleCellState(CellPosition position)
        {
            SelectedCell = position;
        }
    }
}
