﻿using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{

    public class Compact
    {
        private interface IFoo
        {
            /*
            #region compact_1
            public CompactDatabaseOperation(CompactSettings compactSettings)
            #endregion
            */
        }

        private class Foo
        {
            #region compact_2
            public class CompactSettings
            {
                public string DatabaseName { get; set; }

                public bool Documents { get; set; }

                public string[] Indexes { get; set; }
            }
            #endregion
        }

        public Compact()
        {

            using (var store = new DocumentStore())
            {
                #region compact_3
                CompactSettings settings = new CompactSettings
                {
                    DatabaseName = "Northwind",
                    Documents = true,
                    Indexes = new[] { "Orders/Totals", "Orders/ByCompany" }
                };
                Operation operation = store.Maintenance.Server.Send(new CompactDatabaseOperation(settings));
                operation.WaitForCompletion();
                #endregion
            }
            using (var store = new DocumentStore())
            {
                #region compact_4
                // get all index names
                string[] indexNames = store.Maintenance.Send(new GetIndexNamesOperation(0, int.MaxValue));

                CompactSettings settings = new CompactSettings
                {
                    DatabaseName = "Northwind",
                    Documents = true,
                    Indexes = indexNames
                };
                // compact entire database: documents + all indexes
                Operation operation = store.Maintenance.Server.Send(new CompactDatabaseOperation(settings));
                operation.WaitForCompletion();
                #endregion
            }
        }

        // CompactDatabaseOperation automatically runs on the store's database.  
        // If we try to compact a different database that doesn't reside on the 
        // first online node, CompactDatabaseOperation won't find it and fail.  
        private static void WillNotWork()
        {
            using (var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "Northwind"
            }.Initialize())
            {
                var compactOperation = new CompactDatabaseOperation(new Raven.Client.ServerWide.CompactSettings
                {
                    DatabaseName = "NonDefaultDB",
                    Documents = true
                });

                var reqEx = store.GetRequestExecutor();

                store.Maintenance.Server.Send(compactOperation);
            }
        }

        // To compact a database other than the store's database, we need to explicitly provide 
        // its name to the store's Request Executor.  
        private static void locateDB()
        {
            #region compact_5
            using (var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "sampleDB" // the store's database
            }.Initialize())
            {
                store.GetRequestExecutor().GetPreferredNode().Wait();

                const string DBToCompact = "NonDefaultDB"; // the database we want to compact
                string[] indexNames = store.Maintenance.Send(new GetIndexNamesOperation(0, int.MaxValue));

                var compactOperation = new CompactDatabaseOperation(new Raven.Client.ServerWide.CompactSettings
                {
                    DatabaseName = DBToCompact,
                    Documents = true,
                    Indexes = indexNames
                });

                // Get request executor for our DB
                var reqEx = store.GetRequestExecutor(DBToCompact);

                using (reqEx.ContextPool.AllocateOperationContext(out var context))
                {
                    var compactCommand = compactOperation.GetCommand(store.Conventions, context);
                    reqEx.Execute(compactCommand, context);
                }
            }
            #endregion
        }
    }
}
