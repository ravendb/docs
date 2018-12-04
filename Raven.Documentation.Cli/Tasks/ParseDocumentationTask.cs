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
    internal class ParseDocumentationTask : CliTask<ParseDocumentationTask.Options>
    {
        public ParseDocumentationTask(ILogger<ParseDocumentationTask> logger) : base(logger)
        {
        }

        private static string DefaultDocumentationDirectory => Path.Combine(GetSolutionPath, "Documentation\\");

        public override void Run(Options options = null)
        {
            if (options == null)
                options = new Options();

            Validate(options);

            Logger.LogInformation("Started parsing documentation pages.");

            var allPages = GetParsedPages(options);

            if (allPages.Count == 0)
                throw new InvalidOperationException("No documentation pages were parsed.");

            Logger.LogInformation($"Successfully parsed {allPages.Count} documentation pages.");
        }

        private static void Validate(Options options)
        {
            if (Directory.Exists(options.DocumentationDirectory) == false)
                throw new InvalidOperationException(
                    $"The provided article directory does not exist: {options.DocumentationDirectory}");
        }

        private List<DocumentationPage> GetParsedPages(Options options)
        {
            var parser =
                new DocumentationParser(
                    new ParserOptions
                    {
                        PathToDocumentationDirectory = options.DocumentationDirectory,
                        VersionsToParse = options.SpecificVersionsToParse,
                        RootUrl = DummyRootUrl,
                        ImageUrlGenerator = ImageUrlGenerator
                    }, new NoOpGitFileInformationProvider());

            var parserOutput = parser.Parse();
            return parserOutput.Pages.ToList();
        }

        public class Options
        {
            public string DocumentationDirectory { get; set; } = DefaultDocumentationDirectory;

            public List<string> SpecificVersionsToParse { get; set; } = new List<string>();
        }
    }
}
