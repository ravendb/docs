using Raven.Documentation.Cli.Tasks;

namespace Raven.Documentation.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Setup();

            ParseArticles();
            ParseDocumentation();
        }

        private static void Setup()
        {
            ServiceLocator.Configure();

            ServiceLocator.Resolve<Startup>()
                .Configure();
        }

        private static void ParseArticles()
        {
            var task = ServiceLocator.Resolve<ParseArticlesTask>();
            task.Run();
        }

        private static void ParseDocumentation()
        {
            var task = ServiceLocator.Resolve<ParseDocumentationTask>();
            task.Run();
        }
    }
}
