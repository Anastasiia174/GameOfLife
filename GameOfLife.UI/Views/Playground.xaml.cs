using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GameOfLife.Core;
using GameOfLife.UI.Infrastructure;
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

                    var cellRectangle = CreateRectangle(new CellViewModel(new Cell(row, column, false)));
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

        private Rectangle CreateRectangle(CellViewModel cell)
        {
            var rectangle = new Rectangle
            {
                DataContext = cell
            };
            rectangle.InputBindings.Add(CreateMouseClickInputBinding(cell));
            rectangle.SetBinding(Shape.FillProperty, CreateCellLifeStateBinding());

            return rectangle;
        }

        private InputBinding CreateMouseClickInputBinding(CellViewModel cell)
        {
            var cellRectangle = new InputBinding(
                _playgroundViewModel.ToggleCellLifeStateCommand,
                new MouseGesture(MouseAction.LeftClick)
            )
            {
                CommandParameter = new CellCoordinate(cell.Row, cell.Column)
            };

            return cellRectangle;
        }

        private static Binding CreateCellLifeStateBinding()
        {
            return new Binding
            {
                Path = new PropertyPath("IsAlive"),
                Mode = BindingMode.TwoWay,
                Converter = new LifeStateToColorConverter(
                    aliveColor: Brushes.Black,
                    deadColor: Brushes.White
                )
            };
        }
    }
}
