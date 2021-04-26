namespace GameOfLife.Infrastructure
{
    public class PlaygroundState
    {
        public PlaygroundState(double width, double height, CellPosition activeCell)
        {
            Width = width;
            Height = height;
            ActiveCell = activeCell;
        }

        public double Width { get; set; }

        public double Height { get; set; }

        public CellPosition ActiveCell { get; set; }
    }
}
