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

        public abstract IEnumerable<TPage> Parse();

        public abstract IEnumerable<TableOfContents> GenerateTableOfContents();
    }
}