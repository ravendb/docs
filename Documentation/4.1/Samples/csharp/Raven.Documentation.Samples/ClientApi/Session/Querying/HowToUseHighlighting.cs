using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries.Highlighting;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToUseHighlighting
    {
        public class ContentSearchIndex : AbstractIndexCreationTask<SearchItem>
        {
            public ContentSearchIndex()
            {
                Map = docs => from doc in docs
                              select new
                              {
                                  Text = doc.Text
                              };

                Index(x => x.Text, FieldIndexing.Search);
                Store(x => x.Text, FieldStorage.Yes);
                TermVector(x => x.Text, FieldTermVector.WithPositionsAndOffsets);
            }
        }

        public class SearchItem
        {
            public string Id { get; set; }

            public string Text { get; set; }
        }

        private class Foo
        {
            #region options
            public string GroupKey { get; set; }

            public string[] PreTags { get; set; }

            public string[] PostTags { get; set; }
            #endregion
        }

        private interface IFoo<T>
        {
            #region highlight_1
            IRavenQueryable<T> Highlight(
                string fieldName,
                int fragmentLength,
                int fragmentCount,
                out Highlightings highlightings);

            IRavenQueryable<T> Highlight(
                string fieldName,
                int fragmentLength,
                int fragmentCount,
                HighlightingOptions options,
                out Highlightings highlightings);

            IRavenQueryable<T> Highlight(
                Expression<Func<T, object>> path,
                int fragmentLength,
                int fragmentCount,
                out Highlightings highlightings);

            IRavenQueryable<T> Highlight(
                Expression<Func<T, object>> path,
                int fragmentLength,
                int fragmentCount,
                HighlightingOptions options,
                out Highlightings highlightings);
            #endregion
        }

        public HowToUseHighlighting()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region highlight_2
                    SearchItem[] results = session
                        .Query<SearchItem, ContentSearchIndex>()
                        .Highlight(x => x.Text, 128, 1, out Highlightings highlightings)
                        .Search(x => x.Text, "raven")
                        .ToArray();

                    StringBuilder builder = new StringBuilder()
                        .AppendLine("<ul>");

                    foreach (SearchItem result in results)
                    {
                        string[] fragments = highlightings.GetFragments(result.Id);
                        builder.AppendLine($"<li>{fragments.First()}</li>");
                    }

                    string ul = builder
                        .AppendLine("</ul>")
                        .ToString();
                    #endregion
                }
            }
        }
    }
}
