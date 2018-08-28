using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.Highlighting;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Highlights
    {
        #region highlights_1
        public class BlogPosts_ByContent : AbstractIndexCreationTask<BlogPost>
        {
            public class Result
            {
                public string Content { get; set; }
            }

            public BlogPosts_ByContent()
            {
                Map = posts => from post in posts
                               select new
                               {
                                   post.Content
                               };

                Index(x => x.Content, FieldIndexing.Search);
                Store(x => x.Content, FieldStorage.Yes);
                TermVector(x => x.Content, FieldTermVector.WithPositionsAndOffsets);
            }
        }
        #endregion

        #region highlights_7
        public class BlogPosts_ByCategory_Content : AbstractIndexCreationTask<BlogPost, BlogPosts_ByCategory_Content.Result>
        {
            public class Result
            {
                public string Category { get; set; }

                public string Content { get; set; }
            }

            public BlogPosts_ByCategory_Content()
            {
                Map = posts => from post in posts
                               select new
                               {
                                   post.Category,
                                   post.Content
                               };

                Reduce = results => from result in results
                                    group result by result.Category into g
                                    select new
                                    {
                                        Category = g.Key,
                                        Content = string.Join(" ", g.Select(r => r.Content))
                                    };

                Index(x => x.Content, FieldIndexing.Search);
                Store(x => x.Content, FieldStorage.Yes);
                TermVector(x => x.Content, FieldTermVector.WithPositionsAndOffsets);
            }
        }
        #endregion

        public Highlights()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region highlights_2
                    BlogPost[] results = session
                        .Advanced
                        .DocumentQuery<BlogPost, BlogPosts_ByContent>()
                        .Highlight("Content", 128, 1, out Highlightings highlightings)
                        .Search("Content", "raven")
                        .ToArray();

                    StringBuilder builder = new StringBuilder()
                        .AppendLine("<ul>");

                    foreach (BlogPost result in results)
                    {
                        string[] fragments = highlightings.GetFragments(result.Id);
                        builder.AppendLine($"<li>{fragments.First()}</li>");
                    }

                    string ul = builder
                        .AppendLine("</ul>")
                        .ToString();

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region highlights_6_1
                    BlogPosts_ByContent.Result[] results = session
                        .Query<BlogPosts_ByContent.Result, BlogPosts_ByContent>()
                        .Highlight("Content", 128, 1, new HighlightingOptions
                        {
                            PreTags = new[] { "**" },
                            PostTags = new[] { "**" }
                        }, out Highlightings highlightings)
                        .Search(x => x.Content, "raven")
                        .ProjectInto<BlogPosts_ByContent.Result>()
                        .ToArray();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region highlights_6_2
                    BlogPosts_ByContent.Result[] results = session
                        .Advanced
                        .DocumentQuery<BlogPost, BlogPosts_ByContent>()
                        .Highlight("Content", 128, 1, new HighlightingOptions
                        {
                            PreTags = new[] { "**" },
                            PostTags = new[] { "**" }
                        }, out Highlightings highlightings)
                        .Search("Content", "raven")
                        .SelectFields<BlogPosts_ByContent.Result>()
                        .ToArray();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region highlights_8_1
                    // highlighting 'Content', but marking 'Category' as key
                    BlogPosts_ByCategory_Content.Result[] results = session
                        .Query<BlogPosts_ByCategory_Content.Result, BlogPosts_ByCategory_Content>()
                        .Highlight("Content", 128, 1, new HighlightingOptions
                        {
                            PreTags = new[] { "**" },
                            PostTags = new[] { "**" },
                            GroupKey = "Category"
                        }, out Highlightings highlightings)
                        .Search(x => x.Content, "raven")
                        .ToArray();

                    // get fragments for 'News' category
                    var newsHighlightings = highlightings.GetFragments("News");
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region highlights_8_2
                    // highlighting 'Content', but marking 'Category' as key
                    BlogPosts_ByCategory_Content.Result[] results = session
                        .Advanced
                        .DocumentQuery<BlogPosts_ByCategory_Content.Result, BlogPosts_ByCategory_Content>()
                        .Highlight("Content", 128, 1, new HighlightingOptions
                        {
                            PreTags = new[] { "**" },
                            PostTags = new[] { "**" },
                            GroupKey = "Category"
                        }, out Highlightings highlightings)
                        .Search("Content", "raven")
                        .ToArray();

                    // get fragments for 'News' category
                    var newsHighlightings = highlightings.GetFragments("News");
                    #endregion
                }
            }
        }
    }
}
