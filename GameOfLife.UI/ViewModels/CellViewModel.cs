using GameOfLife.Core;
using GameOfLife.UI.Infrastructure;

namespace GameOfLife.UI.ViewModels
{
    public class CellViewModel : BindableBase
    {
        private bool _isAlive;

        public CellViewModel(Cell cell)
        {
            Row = cell.Row;
            Column = cell.Column;
            IsAlive = cell.IsAlive;
        }

        public bool IsAlive
        {
            get
            {
                return _isAlive;
            }

            set
            {
                SetProperty(ref _isAlive, value);
            }
        }

        public int Row { get; private set; }

        public int Column { get; private set; }
    }
}
