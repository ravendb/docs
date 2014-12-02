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
        public static IDocumentStore MyDocStore =
            new DocumentStore {Url = "http://ravendb.mydomain.com/"};

        // Embedded
        public static IDocumentStore EmbeddedDocStore =
            new EmbeddableDocumentStore {DataDirectory = "~/DataDir"};

        #endregion

        public void Steps()
        {

            #region Step 2

            using (var session = MyDocStore.OpenSession())
            {
            	session.Store(new BlogPost {Title = "Sample"});
                session.SaveChanges();
            }

            #endregion

            #region Step 3

            using (var session = MyDocStore.OpenSession())
            {
                var blogPosts = from post in session.Query<BlogPost>()
                               where post.PublishedAt > new DateTime(2011, 8, 31)
                               select post;

                Console.WriteLine(blogPosts.Count());
            }

            #endregion
        }
    }
}