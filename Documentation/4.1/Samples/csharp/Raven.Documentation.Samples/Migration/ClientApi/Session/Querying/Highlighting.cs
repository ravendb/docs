using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents.Indexes;

#region highlighting_2
using Raven.Client.Documents;
using Raven.Client.Documents.Queries.Highlighting;
#endregion

/*
#region highlighting_1
using Raven.Client;
using Raven.Client.Document;
#endregion
*/

namespace Raven.Documentation.Samples.Migration.ClientApi.Session.Querying
{
    public class Highlighting
    {
        private class Foo
        {
            public Foo()
            {
            }
        }

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

        public Highlighting()
        {
            using (var store = new DocumentStore())
            {
                /*
                #region highlighting_3
                SearchItem[] results = session
                    .Query<SearchItem>("ContentSearchIndex")
                    .Customize(x => x.Highlight("Text", 128, 1, out FieldHighlightings highlightings))
                    .Search(x => x.Text, "raven")
                    .ToArray();
                #endregion
                */

                using (var session = store.OpenSession())
                {
                    #region highlighting_4
                    List<SearchItem> results = session
                        .Query<SearchItem, ContentSearchIndex>()
                        .Highlight(x => x.Text, 128, 1, out Highlightings highlightings)
                        .Search(x => x.Text, "raven")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region highlighting_5
                    List<SearchItem> results = session
                        .Advanced
                        .DocumentQuery<SearchItem, ContentSearchIndex>()
                        .Highlight(x => x.Text, 128, 1, out Highlightings highlightings)
                        .Search(x => x.Text, "raven")
                        .ToList();
                    #endregion
                }

                /*
                #region highlighting_6
                SearchItem[] results = session
                    .Query<SearchItem>("ContentSearchIndex")
                    .Customize(x => x.Highlight("Text", 128, 1, out FieldHighlightings highlightings))
                    .SetHighlighterTags("**", "**")
                    .Search(x => x.Text, "raven")
                    .ToArray();
                #endregion
                */

                using (var session = store.OpenSession())
                {
                    #region highlighting_7
                    List<SearchItem> results = session
                        .Query<SearchItem, ContentSearchIndex>()
                        .Highlight(x => x.Text, 128, 1, new HighlightingOptions
                        {
                            PreTags = new[] { "**" },
                            PostTags = new[] { "**" }
                        }, out Highlightings highlightings)
                        .Search(x => x.Text, "raven")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region highlighting_8
                    List<SearchItem> results = session
                        .Advanced
                        .DocumentQuery<SearchItem, ContentSearchIndex>()
                        .Highlight(x => x.Text, 128, 1, new HighlightingOptions
                        {
                            PreTags = new[] { "**" },
                            PostTags = new[] { "**" }
                        }, out Highlightings highlightings)
                        .Search(x => x.Text, "raven")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
