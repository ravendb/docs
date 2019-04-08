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

        private static string DefaultDocumentationDirectory => Path.Combine(GetSolutionPath, "Documentation");

        public override void Run(Options options = null)
        {
            if (options == null)
                options = new Options();

            var parserOptions = GetParserOptions(options);

            Validate(options, parserOptions);

            Logger.LogInformation("Started parsing documentation pages.");

            var allPages = GetParsedPages(parserOptions);

            if (allPages.Count == 0)
                throw new InvalidOperationException("No documentation pages were parsed.");

            Logger.LogInformation($"Successfully parsed {allPages.Count} documentation pages.");
        }

        private ParserOptions GetParserOptions(Options options)
        {
            return new ParserOptions
            {
                PathToDocumentationDirectory = options.DocumentationDirectory,
                VersionsToParse = options.SpecificVersionsToParse,
                RootUrl = DummyRootUrl,
                ImageUrlGenerator = ImageUrlGenerator
            };
        }

        private void Validate(Options options, ParserOptions parserOptions)
        {
            Logger.LogInformation("Starting validation.");

            if (Directory.Exists(options.DocumentationDirectory) == false)
                throw new InvalidOperationException(
                    $"The provided article directory does not exist: {options.DocumentationDirectory}");

            var discussionIdValidator = new DiscussionIdValidator(parserOptions);
            discussionIdValidator.Run();

            Logger.LogInformation("Validation successful.");
        }

        private List<DocumentationPage> GetParsedPages(ParserOptions parserOptions)
        {
            var parser = new DocumentationParser(parserOptions, new NoOpGitFileInformationProvider());
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
