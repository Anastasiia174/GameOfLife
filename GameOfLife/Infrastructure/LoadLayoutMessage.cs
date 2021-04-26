namespace GameOfLife.Infrastructure
{
    public class LoadLayoutMessage
    {
        public LoadLayoutMessage(GameLayout layout)
        {
            GameLayout = layout;
        }

        public GameLayout GameLayout { get; set; }
    }
}
