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
                WaitForIndexing(store); //If we want to query documents sometime we need to wait for the indexes to catch up
                WaitForUserToContinueTheTest(store);//Sometimes we want to debug the test itself, this redirect us to the studio
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
                FrameworkVersion = "3.1.15+", // Look for newest version on machine including 3.1.15 and up (default is set at time of server release).  
                ServerDirectory = "PATH_TO_RAVENDB_SERVER", // Specify where ravendb server binaries are located (Optional)
                DataDirectory = "PATH_TO_RAVENDB_DATADIR", // Specify where ravendb data will be placed/located (Optional)
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
