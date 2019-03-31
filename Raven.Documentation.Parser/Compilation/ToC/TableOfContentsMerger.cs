using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser.Compilation.ToC
{
    internal class TableOfContentsMerger
    {
        public static List<TableOfContents> Merge(List<TableOfContentsCompiler.TocFolderCompilationResult> folderCompilationResults)
        {
            var mergeContext = new MergeContext();

            foreach (var folderCompilationResult in folderCompilationResults)
            {
                mergeContext.AddCompiled(folderCompilationResult);
            }

            var resultTocs = mergeContext.GetUnique();

            var duplicatedEntries = mergeContext.GetDuplicated();

            foreach (var duplicateGroup in duplicatedEntries)
            {
                var orderedDuplicates = OrderDuplicateTocsByPriority(duplicateGroup.Value);
                var mergedToc = MergeTocs(orderedDuplicates);
                resultTocs.Add(mergedToc);
            }

            return resultTocs;
        }

        private static List<TableOfContents> OrderDuplicateTocsByPriority(List<MergeContext.Entry> duplicateEntries)
        {
            return duplicateEntries.OrderByDescending(x => x.SourceDirectoryVerion).Select(x => x.TableOfContents).ToList();
        }

        private static TableOfContents MergeTocs(List<TableOfContents> tocs)
        {
            var toc = tocs.First();

            for (var i = 1; i < tocs.Count; i++)
            {
                MergeTocItems(tocs[i].Items, toc.Items);
            }

            return toc;
        }

        private static void MergeTocItems(List<TableOfContents.TableOfContentsItem> source, List<TableOfContents.TableOfContentsItem> destination)
        {
            foreach (var sourceItem in source)
            {
                var matchedItems = destination.Where(x => x.Key == sourceItem.Key).ToList();
                if (matchedItems.Count == 0)
                {
                    destination.Add(sourceItem);
                    continue;
                }

                if (matchedItems.Count > 1)
                    throw new InvalidOperationException($"Detected more than one matching element during ToC merging. Key: '{sourceItem.Key}'.");

                var matchedItem = matchedItems.First();

                if (matchedItem.SourceVersion == null)
                {
                    matchedItem.SourceVersion = sourceItem.SourceVersion;
                    matchedItem.Languages = sourceItem.Languages;
                }

                MergeTocItems(sourceItem.Items, matchedItem.Items);
            }
        }

        private class MergeContext
        {
            private readonly Dictionary<string, List<Entry>> _compiledTocs = new Dictionary<string, List<Entry>>();

            private static string GetKey(TableOfContents toc) => $"{toc.Version}/{toc.Category}";

            public void AddCompiled(TableOfContentsCompiler.TocFolderCompilationResult folderCompilationResult)
            {
                var sourceDirVersion = folderCompilationResult.SourceDirectoryVersion;

                foreach (var toc in folderCompilationResult.CompiledTocs)
                {
                    AddCompiled(sourceDirVersion, toc);
                }
            }

            private void AddCompiled(string sourceDirectoryVerion, TableOfContents tableOfContents)
            {
                var key = GetKey(tableOfContents);
                var entry = new Entry
                {
                    TableOfContents = tableOfContents,
                    SourceDirectoryVerion = sourceDirectoryVerion
                };

                if (_compiledTocs.ContainsKey(key))
                    _compiledTocs[key].Add(entry);
                else
                    _compiledTocs[key] = new List<Entry> { entry };
            }

            public List<KeyValuePair<string, List<Entry>>> GetDuplicated()
            {
                return _compiledTocs.Where(x => x.Value.Count > 1).ToList();
            }

            public List<TableOfContents> GetUnique()
            {
                return _compiledTocs
                    .Where(x => x.Value.Count == 1)
                    .SelectMany(x => x.Value)
                    .Select(x => x.TableOfContents)
                    .ToList();
            }

            internal class Entry
            {
                public TableOfContents TableOfContents { get; set; }

                public string SourceDirectoryVerion { get; set; }
            }
        }
    }
}
