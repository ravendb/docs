using Raven.Client.Documents;
using Raven.TestDriver;
using Xunit;
using System;
using System.IO;
using System.Linq;
using Raven.Client.Documents.Indexes;

namespace RavenDBTestDriver
{
    namespace Foo
    {
        #region RavenServerLocator
        public abstract class RavenServerLocator
        {
            /// <summary>
            /// Allows to fetch the server path
            /// </summary>
            public abstract string ServerPath { get; }

            /// <summary>
            /// Allows to fetch the command used to invoke the server.
            /// </summary>
            public virtual string Command => ServerPath;

            /// <summary>
            /// Allows to fetch the command arguments.
            /// </summary>
            public virtual string CommandArguments => string.Empty;
        }
        #endregion
    }

    public class RavenDBTestDriver : RavenTestDriver<MyRavenDBLocator>
    {
        #region test_driver_3
        //This allows us to modify the conventions of the store we get from 'GetDocumentStore'
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 50;
        }
        #endregion

        #region test_driver_4
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
    }

    #region test_driver_5
    public class MyRavenDBLocator : RavenServerLocator
    {
        private string _serverPath;
        private string _command = "dotnet";
        private readonly string RavenServerName = "Raven.Server";
        private string _arguments;

        public override string ServerPath
        {
            get
            {
                if (string.IsNullOrEmpty(_serverPath) == false)
                {
                    return _serverPath;
                }
                var path = Environment.GetEnvironmentVariable("Raven_Server_Test_Path");
                if (string.IsNullOrEmpty(path) == false)
                {
                    if (InitializeFromPath(path))
                        return _serverPath;
                }
                //If we got here we didn't have ENV:RavenServerTestPath setup for us maybe this is a CI environment
                path = Environment.GetEnvironmentVariable("Raven_Server_CI_Path");
                if (string.IsNullOrEmpty(path) == false)
                {
                    if (InitializeFromPath(path))
                        return _serverPath;
                }
                //We couldn't find Raven.Server in either environment variables let's look for it in the current directory
                foreach (var file in Directory.GetFiles(Environment.CurrentDirectory, $"{RavenServerName}.exe; {RavenServerName}.dll"))
                {
                    if (InitializeFromPath(file))
                        return _serverPath;
                }
                //Lets try some brut force
                foreach (var file in Directory.GetFiles(Directory.GetDirectoryRoot(Environment.CurrentDirectory), $"{RavenServerName}.exe; {RavenServerName}.dll", SearchOption.AllDirectories))
                {
                    if (InitializeFromPath(file))
                    {
                        try
                        {
                            //We don't want to override the variable if defined
                            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("Raven_Server_Test_Path")))
                                Environment.SetEnvironmentVariable("Raven_Server_Test_Path", file);
                        }
                        //We might not have permissions to set the enviroment variable
                        catch
                        {

                        }
                        return _serverPath;
                    }
                }
                throw new FileNotFoundException($"Could not find {RavenServerName} anywhere on the device.");
            }
        }

        private bool InitializeFromPath(string path)
        {
            if (Path.GetFileNameWithoutExtension(path) != RavenServerName)
                return false;
            var ext = Path.GetExtension(path);
            if (ext == ".dll")
            {
                _serverPath = path;
                _arguments = _serverPath;
                return true;
            }
            if (ext == ".exe")
            {
                _serverPath = path;
                _command = _serverPath;
                _arguments = string.Empty;
                return true;
            }
            return false;
        }

        public override string Command => _command;
        public override string CommandArguments => _arguments;
    }
    #endregion

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
