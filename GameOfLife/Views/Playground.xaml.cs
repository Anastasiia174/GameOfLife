using System.Windows.Controls;

namespace GameOfLife.Views
{
    /// <summary>
    /// Interaction logic for Playground.xaml
    /// </summary>
    public partial class Playground : UserControl
    {
        public Playground()
        {
            InitializeComponent();
            Image.MouseWheel += Image_MouseWheel;
        }

        private void Image_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                ImageScale.ScaleX = ImageScale.ScaleX * 1.10;
                ImageScale.ScaleY = ImageScale.ScaleY * 1.10;
            }
            if (e.Delta < 0)
            {
                ImageScale.ScaleX = ImageScale.ScaleX / 1.10;
                ImageScale.ScaleY = ImageScale.ScaleY / 1.10;
            }
        }
    }
}
