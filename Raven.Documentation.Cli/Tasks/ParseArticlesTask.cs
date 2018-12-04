using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Raven.Documentation.Cli.Utils;
using Raven.Documentation.Parser;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Cli.Tasks
{
    internal class ParseArticlesTask : CliTask<ParseArticlesTask.Options>
    {
        public ParseArticlesTask(ILogger<ParseArticlesTask> logger) : base(logger)
        {
        }

        private static string DefaultArticleDirectory => Path.Combine(GetSolutionPath, "Articles");

        public override void Run(Options options = null)
        {
            if (options == null)
                options = new Options();

            Validate(options);

            Logger.LogInformation("Started parsing article pages.");

            var allPages = GetParsedPages(options);

            if (allPages.Count == 0)
                throw new InvalidOperationException("No article pages were parsed.");

            Logger.LogInformation($"Successfully parsed {allPages.Count} article pages.");
        }

        private void Validate(Options options)
        {
            if (Directory.Exists(options.ArticleDirectory) == false)
                throw new InvalidOperationException($"The provided article directory does not exist: {options.ArticleDirectory}");
        }

        private List<ArticlePage> GetParsedPages(Options options)
        {
            var parser =
                new ArticleParser(
                    new ParserOptions
                    {
                        PathToDocumentationDirectory = options.ArticleDirectory,
                        RootUrl = DummyRootUrl,
                        ImageUrlGenerator = ImageUrlGenerator
                    }, new NoOpGitFileInformationProvider());

            var parserOutput = parser.Parse();
            return parserOutput.Pages.ToList();
        }

        public class Options
        {
            public string ArticleDirectory { get; set; } = DefaultArticleDirectory;
        }
    }
}
