using System.Collections.Generic;
using System.IO;

using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser
{
    public class ArticleParser : ParserBase<ArticlePage>
    {
        private readonly ArticleDirectoryCompiler _directoryCompiler;

        public ArticleParser(ParserOptions options, IProvideGitFileInformation repoAnalyzer)
            : base(options)
        {
            var articleCompiler = new ArticleCompiler(Parser, options, repoAnalyzer);
            _directoryCompiler = new ArticleDirectoryCompiler(articleCompiler, options);
        }

        public override IEnumerable<ArticlePage> Parse()
        {
            var articleDirectory = new DirectoryInfo(Options.PathToDocumentationDirectory);
            var compilationContext = new DocumentationCompilation.Context();

            return _directoryCompiler.Compile(articleDirectory, compilationContext);
        }

        public override IEnumerable<TableOfContents> GenerateTableOfContents()
        {
            yield break;
        }
    }
}