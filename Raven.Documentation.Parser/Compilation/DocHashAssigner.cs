using System.Diagnostics;
using System.IO;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser.Compilation
{
    public class DocHashAssigner
    {
        private readonly ParserOptions _options;

        public DocHashAssigner(ParserOptions options)
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
                AssignHashesInDirectory(directory);
            }
        }

        private string GetDocumentationVersion(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);
            var version = directoryInfo.Name;
            return version;
        }

        private void AssignHashesInDirectory(string directory)
        {
            if (Directory.Exists(directory) == false)
                return;

            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);

            if (File.Exists(docsFilePath) == false)
                return;

            DocumentationFileHelper.AssignDiscussionHashIfNeeded(docsFilePath);

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                if (item.IsFolder == false)
                    continue;

                var innerDirectoryPath = Path.Combine(directory, item.Name);
                AssignHashesInDirectory(innerDirectoryPath);
            }
        }
    }
}
