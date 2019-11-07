using System;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance
{
    class CleanChangeVector
    {
        static void Main(string[] args)
        {
            /*
            #region syntax_1
            public UpdateUnusedDatabasesOperation(string database, HashSet<string> unusedDatabaseIds)
            #endregion
            */

            var documentStore = new DocumentStore
            {
                Urls = new[]
                {
                    //"http://live-test.ravendb.net"
                    "http://localhost:8080"
                },
                //Database = "Demo"
                Database = "nwww"
            }.Initialize();

            using (var session = documentStore.OpenSession())
            {
                #region example_sync
                documentStore.Maintenance.Server.Send(
                                new UpdateUnusedDatabasesOperation(documentStore.Database, new HashSet<string>
                {
                    "0N64iiIdYUKcO+yq1V0cPA",
                    "xwmnvG1KBkSNXfl7/0yJ1A"
                }));
                #endregion
            }

            using (var session = documentStore.OpenAsyncSession())
            {
                /*
                #region example_async
                await documentStore.Maintenance.Server.SendAsync(
                                new UpdateUnusedDatabasesOperation(documentStore.Database, new HashSet<string>
                {
                    "0N64iiIdYUKcO+yq1V0cPA",
                    "xwmnvG1KBkSNXfl7/0yJ1A"
                }));
                #endregion
                */
            }
        }
    }
}
