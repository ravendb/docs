namespace Raven.Documentation.Cli
{
    public class Startup
    {
        public Startup(Settings settings)
        {
            Settings = settings;
        }

        public Settings Settings { get; }

        public void Configure()
        {
        }
    }
}
