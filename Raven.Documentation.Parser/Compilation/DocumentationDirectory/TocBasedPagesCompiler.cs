using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser.Compilation.DocumentationDirectory
{
    internal class TocBasedPagesCompiler : DocumentationDirectoryCompiler
    {
        public TocBasedPagesCompiler(DocumentCompiler documentCompiler, ParserOptions options) : base(documentCompiler, options)
        {
        }

        public IEnumerable<DocumentationPage> Compile(TableOfContents tableOfContents)
        {
            foreach (var tableOfContentsItem in tableOfContents.Items)
            {
                var tocItemCompilationResult = CompileTableOfContentsItem(tableOfContentsItem, tableOfContents.Version);

                foreach (var documentationPage in tocItemCompilationResult)
                {
                    yield return documentationPage;
                }
            }
        }

        private IEnumerable<DocumentationPage> CompileTableOfContentsItem(TableOfContents.TableOfContentsItem tocItem, string version)
        {
            if (tocItem.IsFolder)
            {
                foreach (var innerTocItem in tocItem.Items)
                {
                    foreach (var innerTocItemCompiled in CompileTableOfContentsItem(innerTocItem, version))
                    {
                        yield return innerTocItemCompiled;
                    }
                }

                yield break;
            }

            ValidateTableOfContentsItem(tocItem, version);

            var sourceDirectory = tocItem.GetSourceBaseDirectory(Options);

            AssignDiscussionIds(tocItem, sourceDirectory, version);

            var pagesToCompile = GetPagesForTocItem(tocItem, sourceDirectory);

            foreach (var pageToCompile in pagesToCompile)
            {
                yield return CompileDocumentationPage(pageToCompile, sourceDirectory, version, pageToCompile.Mappings, tocItem.SourceVersion);
            }
        }

        private void AssignDiscussionIds(TableOfContents.TableOfContentsItem tocItem, string sourceDirectory, string version)
        {
            AssignDiscussionIdsInDocsFile(tocItem, sourceDirectory);
            var sourceDiscussionId = GetSourceDiscussionId(tocItem, sourceDirectory);
            AssignDiscussionIdToDerivedPages(tocItem, version, sourceDiscussionId);
        }

        private void AssignDiscussionIdsInDocsFile(TableOfContents.TableOfContentsItem tocItem, string directory, string discussionId = null)
        {
            var matchingFolderItem = GetFolderItemForTocItem(directory, tocItem);

            if (matchingFolderItem == null)
                return;

            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            DocumentationFileHelper.AssignDiscussionIdIfNeeded(docsFilePath, matchingFolderItem, discussionId);
        }

        private string GetSourceDiscussionId(TableOfContents.TableOfContentsItem tocItem, string directory)
        {
            var matchingFolderItem = GetFolderItemForTocItem(directory, tocItem);
            return matchingFolderItem.DiscussionId;
        }

        private void AssignDiscussionIdToDerivedPages(TableOfContents.TableOfContentsItem tocItem, string version, string sourceDiscussionId)
        {
            var directory = tocItem.GetDirectoryPath(version, Options);
            AssignDiscussionIdsInDocsFile(tocItem, directory, sourceDiscussionId);
        }

        private void ValidateTableOfContentsItem(TableOfContents.TableOfContentsItem tocItem, string version)
        {
            if (string.IsNullOrEmpty(tocItem.SourceVersion))
            {
                throw new InvalidOperationException($"Could not find markdown file for {tocItem.Key} version {version}.");
            }
        }

        private IEnumerable<FolderItem> GetPagesForTocItem(TableOfContents.TableOfContentsItem tocItem, string directory)
        {
            var matchingFolderItem = GetFolderItemForTocItem(directory, tocItem);

            var pages = GetPages(directory, matchingFolderItem);

            foreach (var folderItem in pages)
            {
                yield return folderItem;
            }
        }

        private FolderItem GetFolderItemForTocItem(string directory, TableOfContents.TableOfContentsItem tocItem)
        {
            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);

            if (File.Exists(docsFilePath) == false)
                return null;

            var keySuffix = tocItem.GetKeySuffix();

            var matchingFolderItem = DocumentationFileHelper.ParseFile(docsFilePath)
                .SingleOrDefault(x => x.Name == keySuffix);

            return matchingFolderItem;
        }
    }
}
