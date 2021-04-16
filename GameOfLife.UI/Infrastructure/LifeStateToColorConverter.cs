using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace GameOfLife.UI.Infrastructure
{
    internal class LifeStateToColorConverter : IValueConverter
    {
        /// <summary>
        /// Gets the color that represents alive cell
        /// </summary>
        public SolidColorBrush AliveColor { get; private set; }

        /// <summary>
        /// Gets the color that represents dead cell
        /// </summary>
        public SolidColorBrush DeadColor { get; private set; }

        public LifeStateToColorConverter(SolidColorBrush aliveColor, SolidColorBrush deadColor)
        {
            AliveColor = aliveColor;
            DeadColor = deadColor;
        }

        /// <summary>
        /// Converts a boolean value that indicates a cell's living status to a color.
        /// </summary>
        /// <param name="value">A boolean value indicating a cell's living status.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>A SolidColorBrush instance (AliveCellColor or DeadCellColor).</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var alive = false;

            if (value is bool)
                alive = (bool)value;

            return alive ? AliveColor : DeadColor;
        }

        /// <summary>
        /// Converts a cell's color back to a boolean that indicates whether the cell is dead or alive.
        /// </summary>
        /// <param name="value">A SolidColorBrush indicating a cell's living status.</param>
        /// <param name="targetType">Target type.</param>
        /// <param name="parameter">Parameter.</param>
        /// <param name="culture">Culture.</param>
        /// <returns>A boolean value that indicates a cell's living status.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush)
                return ((SolidColorBrush)value) == AliveColor;

            return false;
        }
    }
}
