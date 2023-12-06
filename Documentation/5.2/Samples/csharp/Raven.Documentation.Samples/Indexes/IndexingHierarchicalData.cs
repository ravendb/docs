using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.Indexes
{
    public class IndexingHierarchicalData
    {
        #region indexes_1
        private class BlogPost
        {
            public string Author { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }

            // Blog post readers can leave comments
            public List<BlogPostComment> Comments { get; set; }
        }

        public class BlogPostComment
        {
            public string Author { get; set; }
            public string Text { get; set; }

            // Comments can be left recursively
            public List<BlogPostComment> Comments { get; set; }
        }
        #endregion

        #region indexes_2
        private class BlogPosts_ByCommentAuthor : 
            AbstractIndexCreationTask<BlogPost, BlogPosts_ByCommentAuthor.Result>
        {
            public class Result
            {
                public IEnumerable<string> Authors { get; set; }
            }

            public BlogPosts_ByCommentAuthor()
            {
                Map = blogposts => from blogpost in blogposts
                                   let authors = Recurse(blogpost, x => x.Comments)
                                   select new Result
                                   {
                                       Authors = authors.Select(x => x.Author)
                                   };
            }
        }
        #endregion

        public void Sample()
        {
            using (var store = new DocumentStore())
            {
                #region indexes_3
                store.Maintenance.Send(new PutIndexesOperation(
                    new IndexDefinition
                    {
                        Name = "BlogPosts/ByCommentAuthor",
                        Maps =
                        {
                            @"from blogpost in docs.BlogPosts
                              from comment in Recurse(blogpost, (Func<dynamic, dynamic>)(x => x.Comments))
                              select new
                              {
                                  Author = comment.Author
                              }"
                        }
                    }));
                #endregion

                using (var session = store.OpenSession())
                {
                    #region indexes_4
                    IList<BlogPost> results = session
                        .Query<BlogPosts_ByCommentAuthor.Result, BlogPosts_ByCommentAuthor>()
                        .Where(x => x.Authors.Any(a => a == "John"))
                        .OfType<BlogPost>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_5
                    IList<BlogPost> results = session
                        .Advanced
                        .DocumentQuery<BlogPost, BlogPosts_ByCommentAuthor>()
                        .WhereEquals("Authors", "John")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
