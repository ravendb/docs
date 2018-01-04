#region test_driver_1
using Raven.Client.Documents;
using Raven.TestDriver;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Xunit;
using System;
using System.Linq;
using Raven.Client.Documents.Indexes;

namespace RavenDBTestDriverFullExample
{
    public class RavenDBTestDriver : RavenTestDriver<MyRavenDBLocator>
    {
        //This allows us to generate a clean database for our test
        protected override void SetupDatabase(IDocumentStore documentStore)
        {
            var doc = new DatabaseRecord(documentStore.Database);
            try
            {
                documentStore.Maintenance.Server.Send(new CreateDatabaseOperation(doc));
            }
            catch (Exception e)
            {
                //Ignore if the database exists, we could also delete it and create a new one if we need
            }
        }

        //This allows us to modify the conventions of the store we get from 'GetDocumentStore'
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 50;
        }

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
    }

    public class MyRavenDBLocator : RavenServerLocator
    {
        public override string ServerPath
        {
            get
            {
                return Environment.GetEnvironmentVariable("RavenServerTestPath");
            }
        }
        public override string Command => "dotnet";
        public override string CommandArguments => ServerPath;
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
#endregion
