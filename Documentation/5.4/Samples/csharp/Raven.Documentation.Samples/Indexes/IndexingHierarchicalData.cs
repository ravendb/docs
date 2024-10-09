using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.Indexes
{
    public class IndexingHierarchicalData
    {
        #region indexes_1
        public class BlogPost
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

            // Allow nested comments, enabling replies to existing comments
            public List<BlogPostComment> Comments { get; set; }
        }
        #endregion

        #region indexes_2
        private class BlogPosts_ByCommentAuthor : 
            AbstractIndexCreationTask<BlogPost, BlogPosts_ByCommentAuthor.IndexEntry>
        {
            public class IndexEntry
            {
                public IEnumerable<string> Authors { get; set; }
            }

            public BlogPosts_ByCommentAuthor()
            {
                Map = blogposts => from blogpost in blogposts
                                   let authors = Recurse(blogpost, x => x.Comments)
                                   select new IndexEntry
                                   {
                                       Authors = authors.Select(x => x.Author)
                                   };
            }
        }
        #endregion

        public async Task Sample()
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
                        .Query<BlogPosts_ByCommentAuthor.IndexEntry, BlogPosts_ByCommentAuthor>()
                        .Where(x => x.Authors.Any(a => a == "John"))
                        .OfType<BlogPost>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region indexes_4_async
                    IList<BlogPost> results = await asyncSession
                        .Query<BlogPosts_ByCommentAuthor.IndexEntry, BlogPosts_ByCommentAuthor>()
                        .Where(x => x.Authors.Any(a => a == "John"))
                        .OfType<BlogPost>()
                        .ToListAsync();
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
