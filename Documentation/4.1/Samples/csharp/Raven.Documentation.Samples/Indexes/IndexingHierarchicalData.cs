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
        public class BlogPost
        {
            public string Author { get; set; }

            public string Title { get; set; }

            public string Text { get; set; }

            public List<BlogPostComment> Comments { get; set; }
        }

        public class BlogPostComment
        {
            public string Author { get; set; }

            public string Text { get; set; }

            public List<BlogPostComment> Comments { get; set; }
        }
        #endregion

        #region indexes_2
        public class BlogPosts_ByCommentAuthor : AbstractIndexCreationTask<BlogPost>
        {
            public class Result
            {
                public string[] Authors { get; set; }
            }

            public BlogPosts_ByCommentAuthor()
            {
                Map = posts => from post in posts
                               select new
                               {
                                   Authors = Recurse(post, x => x.Comments).Select(x => x.Author)
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
                            @"from post in docs.Posts
                              from comment in Recurse(post, (Func<dynamic, dynamic>)(x => x.Comments))
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
                        .Where(x => x.Authors.Any(a => a == "Ayende Rahien"))
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
                        .WhereEquals("Authors", "Ayende Rahien")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
