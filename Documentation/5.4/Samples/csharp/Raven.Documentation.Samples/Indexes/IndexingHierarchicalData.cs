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
        #region classes_1
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

        #region index_1
        public class BlogPosts_ByCommentAuthor : 
            AbstractIndexCreationTask<BlogPost, BlogPosts_ByCommentAuthor.IndexEntry>
        {
            public class IndexEntry
            {
                public IEnumerable<string> Authors { get; set; }
            }

            public BlogPosts_ByCommentAuthor()
            {
                Map = blogposts => 
                    from blogpost in blogposts
                    let authors = Recurse(blogpost, x => x.Comments)
                    select new IndexEntry
                    {
                        Authors = authors.Select(x => x.Author)
                    };
            }
        }
        #endregion

        #region index_2
        public class BlogPosts_ByCommentAuthor_JS : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string[] Authors { get; set; }
            }

            public BlogPosts_ByCommentAuthor_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('BlogPosts', function (blogpost) {

                          var authors =
                               recurse(blogpost.Comments, function(x) {
                                   return x.Comments;
                               })
                              .filter(function(comment) { 
                                   return comment.Author != null;
                               })
                              .map(function(comment) { 
                                  return comment.Author;
                               });

                          return {
                             Authors: authors
                          };
                    });"
                };
            }
        }
        #endregion

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                #region index_3
                store.Maintenance.Send(new PutIndexesOperation(
                    new IndexDefinition
                    {
                        Name = "BlogPosts/ByCommentAuthor",
                        Maps =
                        {
                            @"from blogpost in docs.BlogPosts
                              let authors = Recurse(blogpost, (Func<dynamic, dynamic>)(x => x.Comments))
                              let authorNames = authors.Select(x => x.Author)
                              select new
                              {
                                  Authors = authorNames
                              }"
                        }
                    }));
                #endregion

                using (var session = store.OpenSession())
                {
                    #region query_1
                    List<BlogPost> results = session
                        .Query<BlogPosts_ByCommentAuthor.IndexEntry, BlogPosts_ByCommentAuthor>()
                         // Query for all blog posts that contain comments by 'Moon':
                        .Where(x => x.Authors.Any(a => a == "Moon"))
                        .OfType<BlogPost>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_2
                    List<BlogPost> results = await asyncSession
                        .Query<BlogPosts_ByCommentAuthor.IndexEntry, BlogPosts_ByCommentAuthor>()
                         // Query for all blog posts that contain comments by 'Moon':
                        .Where(x => x.Authors.Any(a => a == "Moon"))
                        .OfType<BlogPost>()
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region query_3
                    List<BlogPost> results = session
                        .Advanced
                        .DocumentQuery<BlogPost, BlogPosts_ByCommentAuthor>()
                         // Query for all blog posts that contain comments by 'Moon':
                        .WhereEquals("Authors", "Moon")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
