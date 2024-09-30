using System.Collections.Generic;
using System.IO;
using MarkdownDeep;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers.DocumentBuilding;

namespace Raven.Documentation.Parser
{
    public class DocumentBuilder
    {
        private readonly Markdown _parser;
        private readonly ParserOptions _options;
        private string _content;
        private readonly string _sourceDocumentationVersion;

        private IDictionary<string, string> _rawHtmlBlocks;
        private IDictionary<string, string> _codeBlocks;
        private bool _transformBlocks;

        public DocumentBuilder(Markdown parser, ParserOptions options, string sourceDocumentationVersion, string content)
        {
            _parser = parser;
            _options = options;
            _content = content;
            _sourceDocumentationVersion = sourceDocumentationVersion;
        }

        public void TransformRawHtmlBlocks()
        {
            _content = LegacyBlockHelper.ReplaceRawHtmlWithPlaceholders(_content, out _rawHtmlBlocks);
        }

        public void TransformLegacyBlocks(FileInfo file)
        {
            _content = LegacyBlockHelper.GenerateLegacyBlocks(Path.GetDirectoryName(file.FullName), _content);
        }

        public void TransformBlocks()
        {
            _transformBlocks = true;
            
            _content = CodeBlockHelper.ReplaceCodeBlocksWithPlaceholders(_content, out _codeBlocks); 
        }

        public string Build(FolderItem page)
        {
            _content = _parser.Transform(_content);

            if (_transformBlocks)
            {
                _content = NoteBlockHelper.GenerateNoteBlocks(_content);
                _content = PanelBlockHelper.GeneratePanelBlocks(_content);
                _content = CodeBlockHelper.GenerateContentFrames(_content);
            }

            if (_codeBlocks != null)
                _content = CodeBlockHelper.GenerateCodeBlocks(_content, _sourceDocumentationVersion, _options, _codeBlocks);

            if (_rawHtmlBlocks != null)
                _content = LegacyBlockHelper.ReplaceRawHtmlPlaceholdersAfterMarkdownTransformation(_content, _rawHtmlBlocks);

            string expectedPageUrl = null;
            page.Metadata?.TryGetValue("url", out expectedPageUrl);

            _content = SocialMediaBlockHelper.ReplaceSocialMediaBlocks(_content, expectedPageUrl);
            _content = InternalUrlsHelper.ConvertHttpToHttps(_content);

            return _content;
        }
    }
}
