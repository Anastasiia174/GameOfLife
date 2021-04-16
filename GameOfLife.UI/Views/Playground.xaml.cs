using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
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
        /// Builds the game of life playground grid
        /// </summary>
        private void BuildGrid()
        {
            for (var row = 0; row < _playgroundViewModel.Width; row++)
            {
                PlaygroundGrid.RowDefinitions.Add(new RowDefinition());

                for (var column = 0; column < _playgroundViewModel.Length; column++)
                {
                    if (row == 0)
                    {
                        PlaygroundGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    var cellRectangle = new Rectangle();
                    var border = new Border
                    {
                        BorderBrush = new SolidColorBrush(Colors.Gray),
                        BorderThickness = new Thickness(0.5),
                        Child = cellRectangle
                    };

                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, column);

                    PlaygroundGrid.Children.Add(border);
                }
            }
        }
    }
}
