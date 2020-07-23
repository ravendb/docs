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

            var resultTocs = mergeContext.CompiledTocs
                .SelectMany(x => x.Value)
                .Select(x => MergeToTableOfContents(x, mergeContext))
                .ToList();
            
            return resultTocs;
        }

        private static TableOfContents MergeToTableOfContents(MergeContext.RootItemEntry rootItem, MergeContext mergeContext)
        {
            if (rootItem.ContainsMultipleItems)
            {
                var orderedDuplicates = OrderDuplicateTocsByPriority(rootItem.Entries);
                var mergedToc = MergeTocs(orderedDuplicates);
                AssignTocPosition(mergedToc, mergeContext);
                
                return mergedToc;
            }
            
            var singleItem = rootItem.Entries.Single();
            var toc = singleItem.TableOfContents;
            AssignTocPosition(toc, mergeContext);
            
            return toc;
        }

        private static List<TableOfContents> OrderDuplicateTocsByPriority(List<MergeContext.Entry> duplicateEntries)
        {
            return duplicateEntries.OrderByDescending(x => x.SourceDirectoryVersion).Select(x => x.TableOfContents).ToList();
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

        private static void AssignTocPosition(TableOfContents toc, MergeContext mergeContext)
        {
            var categoryIndex = mergeContext.GetCategoryIndex(toc.Version, toc.Category);
            toc.Position = categoryIndex;
        }

        private class MergeContext
        {
            private readonly Dictionary<string, List<Category>> _orderedCategories = new Dictionary<string, List<Category>>();

            public Dictionary<string, List<RootItemEntry>> CompiledTocs { get; } = new Dictionary<string, List<RootItemEntry>>();

            public void AddCompiled(TableOfContentsCompiler.TocFolderCompilationResult folderCompilationResult)
            {
                var sourceDirVersion = folderCompilationResult.SourceDirectoryVersion;

                foreach (var toc in folderCompilationResult.CompiledTocs)
                {
                    AddCompiled(sourceDirVersion, toc);
                }
            }

            private void AddCompiled(string sourceDirectoryVersion, TableOfContents tableOfContents)
            {
                AddToOrderedCategories(sourceDirectoryVersion, tableOfContents);
                
                var version = tableOfContents.Version;

                var entry = new Entry
                {
                    TableOfContents = tableOfContents,
                    SourceDirectoryVersion = sourceDirectoryVersion
                };
                
                if (CompiledTocs.ContainsKey(version) == false)
                    CompiledTocs[version] = new List<RootItemEntry>();

                var existingRootItem = CompiledTocs[version].SingleOrDefault(x => x.Category == tableOfContents.Category);

                if (existingRootItem == null)
                {
                    var rootItem = new RootItemEntry {Category = tableOfContents.Category};
                    rootItem.Entries.Add(entry);
                    CompiledTocs[version].Add(rootItem);
                }
                else
                {
                    existingRootItem.Entries.Add(entry);
                }
            }

            private void AddToOrderedCategories(string sourceDirectoryVersion, TableOfContents tableOfContents)
            {
                var category = tableOfContents.Category;

                if (_orderedCategories.ContainsKey(sourceDirectoryVersion) == false)
                    _orderedCategories[sourceDirectoryVersion] = new List<Category>();
                
                if (_orderedCategories[sourceDirectoryVersion].Contains(category) == false)
                    _orderedCategories[sourceDirectoryVersion].Add(category);
            }

            public int GetCategoryIndex(string version, Category category)
            {
                var categories = _orderedCategories[version];
                return categories.IndexOf(category);
            }

            internal class RootItemEntry
            {
                public Category Category { get; set; }

                public List<Entry> Entries { get; } = new List<Entry>();

                public bool ContainsMultipleItems => Entries.Count > 1;
            }

            internal class Entry
            {
                public TableOfContents TableOfContents { get; set; }

                public string SourceDirectoryVersion { get; set; }
            }
        }
    }
}
