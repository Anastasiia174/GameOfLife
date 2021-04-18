using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GameOfLife.Services;

namespace GameOfLife.ViewModels
{
    public class PlaygroundViewModel : ViewModelBase
    {
        private int _width;

        private int _height;

        private readonly IImageService _imageService;

        public PlaygroundViewModel()
        : this(10, 10, new ImageService())
        {
        }

        public PlaygroundViewModel(int width, int height, IImageService imageService)
        {
            _width = width;
            _height = height;
            _imageService = imageService;


            _playgroundImageSource = _imageService.RenderGrid(_width, _height);
        }

        private ImageSource _playgroundImageSource;

        public ImageSource PlaygroundImageSource
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

        public RelayCommand StartCommand { get; private set; }

        public RelayCommand PauseCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand ChangeConfigurationCommand { get; private set; }

        public RelayCommand RandomizeCellsCommand { get; private set; }

    }
}
