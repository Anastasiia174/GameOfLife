using System;
using System.Collections.Generic;
using System.Text;
using GameOfLife.UI.Infrastructure;

namespace GameOfLife.UI.ViewModels
{
    class PlaygroundViewModel : BindableBase
    {
        public PlaygroundViewModel(int width, int length)
        {
            this.Length = length;
            this.Width = width;
        }

        public int Width { get; private set; }

        public int Length { get; private set; }

        public RelayCommand StartCommand { get; private set; }

        public RelayCommand PauseCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand ChangeConfigurationCommand { get; private set; }

        public RelayCommand RandomizeCellsCommand { get; private set; }
    }
}
