using Raven.Client.Documents;
using Raven.TestDriver;
using Xunit;
using System;
using System.IO;
using System.Linq;
using Raven.Client.Documents.Indexes;

namespace RavenDBTestDriver
{
    public class RavenDBTestDriver : RavenTestDriver
    {
        #region test_driver_PreInitialize
        //This allows us to modify the conventions of the store we get from 'GetDocumentStore'
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 50;
        }
        #endregion

        #region test_driver_MyFirstTest
        [Fact]
        public void MyFirstTest()
        {
            using (var store = GetDocumentStore())
            {
                store.ExecuteIndex(new TestDocumentByName());
                using (var session = store.OpenSession())
                {
                    session.Store(new TestDocument { Name = "Hello world!" });
                    session.Store(new TestDocument { Name = "Goodbye..." });
                    session.SaveChanges();
                }
                // If we want to query documents, sometimes we need to wait for the indexes to catch up  
                // to prevent using stale indexes.
                WaitForIndexing(store);

                // Sometimes we want to debug the test itself. This method redirects us to the studio
                // so that we can see if the code worked as expected (in this case, created two documents).
                WaitForUserToContinueTheTest(store);

                using (var session = store.OpenSession())
                {
                    var query = session.Query<TestDocument, TestDocumentByName>().Where(x => x.Name == "hello").ToList();
                    Assert.Single(query);
                }
            }
        }
        #endregion

        [Fact]
        public void TestDriveConfigureServer()
        {
            #region test_driver_ConfigureServer

            var testServerOptions = new TestServerOptions
            {
                // Looks for the newest version on your machine including 3.1.15 and any newer patches
                // but not major new releases (default is .NET version at time of server release).
                FrameworkVersion = "3.1.15+",

                // Specifies where ravendb server binaries are located (Optional)
                ServerDirectory = "PATH_TO_RAVENDB_SERVER",

                // Specifies where ravendb data will be placed/located (Optional)
                DataDirectory = "PATH_TO_RAVENDB_DATADIR", 
            };

            ConfigureServer(testServerOptions);
            #endregion
        }


    }

    public class TestDocumentByName : AbstractIndexCreationTask<TestDocument>
    {
        public TestDocumentByName()
        {
            Map = docs => from doc in docs select new { doc.Name };
            Indexes.Add(x => x.Name, FieldIndexing.Search);
        }
    }

    public class TestDocument
    {
        public string Name { get; set; }
    }

}
