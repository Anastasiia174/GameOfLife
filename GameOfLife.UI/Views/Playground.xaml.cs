using System.Windows;
using GameOfLife.UI.ViewModels;

namespace GameOfLife.UI.Views
{
    /// <summary>
    /// Interaction logic for Playground.xaml
    /// </summary>
    public partial class Playground : Window
    {
        public Playground()
        {
            InitializeComponent();

            DataContext = new PlaygroundViewModel();
        }
    }
}
