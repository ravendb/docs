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

        public override ParserOutput Parse()
        {
            return new ParserOutput
            {
                Pages = GenerateArticles()
            };
        }

        private IEnumerable<ArticlePage> GenerateArticles()
        {
            var articleDirectory = new DirectoryInfo(Options.PathToDocumentationDirectory);

            return _directoryCompiler.Compile(articleDirectory);
        }
    }
}
