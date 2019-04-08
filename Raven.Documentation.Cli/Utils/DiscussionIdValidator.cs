using System;
using System.IO;
using System.Text;
using Raven.Documentation.Parser;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Cli.Utils
{
    public class DiscussionIdValidator
    {
        private readonly ParserOptions _options;

        public DiscussionIdValidator(ParserOptions options)
        {
            _options = options;
        }

        public void Run()
        {
            var documentationDirectories = Directory.GetDirectories(_options.PathToDocumentationDirectory);

            foreach (var documentationDirectory in documentationDirectories)
            {
                var version = GetDocumentationVersion(documentationDirectory);

                if (_options.CanCompileVersion(version) == false)
                    continue;

                var directory = _options.GetPathToDocumentationPagesDirectory(version);
                ValidateHashesInDirectory(directory);
            }
        }

        private string GetDocumentationVersion(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);
            var version = directoryInfo.Name;
            return version;
        }

        private void ValidateHashesInDirectory(string directory)
        {
            if (Directory.Exists(directory) == false)
                return;

            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);

            if (File.Exists(docsFilePath) == false)
                return;

            ValidateHashesInDocsJson(docsFilePath);

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                if (item.IsFolder == false)
                    continue;

                var innerDirectoryPath = Path.Combine(directory, item.Name);
                ValidateHashesInDirectory(innerDirectoryPath);
            }
        }

        private void ValidateHashesInDocsJson(string docsFilePath)
        {
            var folderItems = DocumentationFileHelper.ParseFile(docsFilePath);

            foreach (var item in folderItems)
            {
                if (item.IsFolder)
                    continue;

                if (string.IsNullOrEmpty(item.DiscussionId))
                {
                    var errorMessage = new StringBuilder();
                    errorMessage.Append($"{nameof(DocumentationPage.DiscussionId)} is missing in {docsFilePath} for item: {item.Name}. ");
                    errorMessage.Append("Please generate the documentation again and make sure to commit the .docs.json file changes.");

                    throw new InvalidOperationException(errorMessage.ToString());
                }
            }
        }
    }
}
