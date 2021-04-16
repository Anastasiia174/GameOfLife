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

        public RelayCommand<object> StartCommand { get; private set; }

        public RelayCommand<object> PauseCommand { get; private set; }

        public RelayCommand<object> SaveCommand { get; private set; }

        public RelayCommand<object> ChangeConfigurationCommand { get; private set; }

        public RelayCommand<object> RandomizeCellsCommand { get; private set; }

        public RelayCommand<CellCoordinate> ToggleCellLifeStateCommand { get; private set; }
    }
}
