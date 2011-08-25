using System;
using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Linq;

namespace RavenCodeSamples
{
    public class HomepageSlides
    {

        #region Step 1

        // Client / Server
        public static IDocumentStore MyDocStore = new DocumentStore {Url = "http://ravendb.mydomain.com/"};

        // Embedded
        public static IDocumentStore EmbeddedDocStore = new EmbeddableDocumentStore {DataDirectory = "~/DataDir"};

        #endregion

        public void Steps()
        {

            #region Step 2

            using (var session = MyDocStore.OpenSession())
            {
                session.Store(new BlogPost
                                  {
                                      Title = "Sample",
                                      Content = "Some HTML content",
                                      Comments = new[]
                                                     {
                                                         new BlogComment {Title = "First comment", Content = "foo"},
                                                         new BlogComment {Title = "Second comment"}
                                                     }
                                  });
                session.SaveChanges();
            }

            #endregion
            #region Step 3

            using (var session = MyDocStore.OpenSession())
            {
                var blogPosts = from post in session.Query<BlogPost>()
                               from tag in post.Tags
                               where "RavenDB".Equals(tag)
                               select post;

                Console.WriteLine(blogPosts.Count());
            }

            #endregion
        }
    }
}