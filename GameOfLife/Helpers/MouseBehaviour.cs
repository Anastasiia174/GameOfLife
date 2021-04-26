using System.Windows;
using System.Windows.Input;
using GameOfLife.Infrastructure;

namespace GameOfLife.Helpers
{
    public class MouseBehaviour : System.Windows.Interactivity.Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty MouseUpCommandProperty =
            DependencyProperty.RegisterAttached("MouseUpCommand", typeof(ICommand),
                typeof(MouseBehaviour), new FrameworkPropertyMetadata(
                    new PropertyChangedCallback(MouseUpCommandChanged)));

        private static void MouseUpCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;

            element.MouseUp += new MouseButtonEventHandler(element_MouseUp);
        }

        static void element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var element = (FrameworkElement)sender;

            var command = GetMouseUpCommand(element);
            var pos = e.GetPosition(element);

            var width = element.ActualWidth;
            var height = element.ActualHeight;

            command.Execute(new PlaygroundState(width, height, new CellPosition() { X = (int)pos.X, Y = (int)pos.Y }));
        }

        public static void SetMouseUpCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseUpCommandProperty, value);
        }

        public static ICommand GetMouseUpCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseUpCommandProperty);
        }
    }
}
