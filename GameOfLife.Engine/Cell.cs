namespace GameOfLife.Engine
{
    public class Cell
    {
        public Cell(int x, int y, bool alive = true)
        {
            X = x;
            Y = y;
            IsAlive = alive;
        }

        public bool IsAlive { get; set; }

        public int X { get; private set; }

        public int Y { get; private set; }
    }
}
