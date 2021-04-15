using System.Windows;
using GameOfLife.UI.ViewModels;

namespace GameOfLife.UI.Views
{
    /// <summary>
    /// Interaction logic for Playground.xaml
    /// </summary>
    public partial class Playground : Window
    {
        private const int DefaultWidth = 80;

        private const int DefaultLength = 100;

        private readonly PlaygroundViewModel _playgroundViewModel;

        public Playground()
        {
            InitializeComponent();

            _playgroundViewModel = new PlaygroundViewModel(DefaultWidth, DefaultLength);

            BuildGrid();

            DataContext = _playgroundViewModel;
        }

        /// <summary>
        /// Builds the game of life user interface.
        /// </summary>
        private void BuildGrid()
        {
        }
    }
}
