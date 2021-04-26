namespace GameOfLife.Infrastructure
{
    public class ConfigMessage
    {
        public ConfigMessage(GameConfiguration configuration)
        {
            Configuration = configuration;
        }

        public GameConfiguration Configuration { get; set; }
    }
}
