using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Raven.Documentation.Parser;

namespace Raven.Documentation.Cli.Tasks
{
    internal abstract class CliTask<TOptions> where TOptions : class
    {
        protected readonly ILogger Logger;

        protected CliTask(ILogger logger)
        {
            Logger = logger;
        }

        protected const string DummyRootUrl = "https://example.com";

        protected static string GetSolutionPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\");

        protected readonly ParserOptions.GenerateImageUrl ImageUrlGenerator = (docVersion, lang, key, fileName) =>
            $"{DummyRootUrl}/image?v={docVersion}&lang={lang}&key={key}&fileName={fileName}";

        public abstract void Run(TOptions options = null);
    }
}
