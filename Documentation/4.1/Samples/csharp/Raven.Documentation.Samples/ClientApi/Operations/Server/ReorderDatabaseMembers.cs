using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Http;
using Raven.Client.ServerWide.Operations;
using Sparrow.Json;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class ReorderDatabaseMembers
    {
        private class ReorderDatabaseMembersOperation : IServerOperation
        {
            #region syntax
            public ReorderDatabaseMembersOperation(string database, List<string> order)
            #endregion
            { }

            public RavenCommand GetCommand(DocumentConventions conventions, JsonOperationContext context)
            {
                throw new System.NotImplementedException();
            }
        }

        public ReorderDatabaseMembers()
        {
            using (var store = new DocumentStore())
            {
                #region example_1
                // Assume that the current order of database group nodes is : ["A", "B", "C"]

                // Change the order of database group nodes to : ["C", "A", "B"]

                store.Maintenance.Server.Send(new ReorderDatabaseMembersOperation("Northwind", 
                    new List<string>
                    {
                        "C", "A", "B"
                    }));
                #endregion

                #region example_2
                // Get the current DatabaseTopology from database record
                var topology = store.Maintenance.Server.Send(new GetDatabaseRecordOperation("Northwind")).Topology;

                // Reverse the order of database group nodes
                topology.Members.Reverse();
                store.Maintenance.Server.Send(new ReorderDatabaseMembersOperation("Northwind", topology.Members));
                #endregion

            }
        }
    }
}
