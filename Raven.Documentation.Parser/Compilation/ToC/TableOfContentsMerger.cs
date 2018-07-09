using System.Collections.Generic;
using System.Linq;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser.Compilation.ToC
{
    internal class TableOfContentsMerger
    {
        public static void Merge(List<TableOfContents> tableOfContents)
        {
            var duplicatedTocs = tableOfContents.GroupBy(x => new { x.Version, x.Category })
                .Where(g => g.Count() > 1)
                .ToList();

            foreach (var duplicatedTocGroup in duplicatedTocs)
            {
                var mergedToc = MergeTocs(duplicatedTocGroup.ToList());
                ReplaceToc(mergedToc, tableOfContents);
            }
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
                var matchedItem = destination.SingleOrDefault(x => x.Key == sourceItem.Key);

                if (matchedItem == null)
                    destination.Add(sourceItem);
                else
                    MergeTocItems(sourceItem.Items, matchedItem.Items);
            }
        }

        private static void ReplaceToc(TableOfContents mergedToc, List<TableOfContents> tableOfContents)
        {
            tableOfContents.RemoveAll(x => x.Version == mergedToc.Version && x.Category == mergedToc.Category);
            tableOfContents.Add(mergedToc);
        }
    }
}
