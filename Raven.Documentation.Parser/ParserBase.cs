namespace Raven.Documentation.Parser
{
    using System.Collections.Generic;

    using MarkdownDeep;

    using Raven.Documentation.Parser.Data;

    public abstract class ParserBase<TPage> 
        where TPage : DocumentationPage
    {
        protected readonly ParserOptions Options;

        protected readonly Markdown Parser;

        protected ParserBase(ParserOptions options)
        {
            Options = options;

            Parser = new Markdown
            {
                AutoHeadingIDs = true,
                ExtraMode = true,
                NoFollowLinks = false,
                SafeMode = false,
                HtmlClassTitledImages = "figure text-center",
                UrlRootLocation = options.RootUrl
            };
        }

        public abstract ParserOutput Parse();

        public class ParserOutput
        {
            public ParserOutput()
            {
                TableOfContents = new List<TableOfContents>();
                Pages = new List<TPage>();
            }

            public List<TableOfContents> TableOfContents { get; set; }

            public IEnumerable<TPage> Pages { get; set; }
        }
    }
}
