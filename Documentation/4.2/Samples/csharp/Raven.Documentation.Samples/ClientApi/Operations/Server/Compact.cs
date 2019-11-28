using System;
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
    }
}
