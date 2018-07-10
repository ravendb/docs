using System.Collections.Generic;
using System.Linq;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser.Compilation.ToC
{
    internal class TocConverter
    {
        public TableOfContents ConvertToTableOfContents(List<TableOfContentsCompiler.CompilationResult> compilationResults, string version, Category category)
        {
            var tocItems = ConvertToTableOfContentItems(compilationResults).ToList();

            var tableOfContents = new TableOfContents
            {
                Version = version,
                Category = category,
                Items = tocItems
            };
            return tableOfContents;
        }

        public IEnumerable<TableOfContents.TableOfContentsItem> ConvertToTableOfContentItems(IEnumerable<TableOfContentsCompiler.CompilationResult> compilationResults)
        {
            foreach (var compilationResult in compilationResults)
            {
                var tocItem = ConvertToTableOfContentsItem(compilationResult);
                tocItem.Items = ConvertToTableOfContentItems(compilationResult.Items).ToList();

                yield return tocItem;
            }
        }

        public TableOfContents.TableOfContentsItem ConvertToTableOfContentsItem(TableOfContentsCompiler.CompilationResult compilationResult)
        {
            var tocItem = new TableOfContents.TableOfContentsItem
            {
                Key = compilationResult.Key,
                Title = compilationResult.Title,
                SourceVersion = compilationResult.SourceVersion,
                IsFolder = compilationResult.IsFolder,
                Languages = compilationResult.Languages
            };
            return tocItem;
        }
    }
}
