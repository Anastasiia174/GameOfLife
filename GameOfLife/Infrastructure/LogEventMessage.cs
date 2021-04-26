namespace GameOfLife.Infrastructure
{
    public class LogEventMessage
    {
        public LogEventMessage(GameLog log)
        {
            Log = log;
        }

        public GameLog Log { get; set; }
    }
}
