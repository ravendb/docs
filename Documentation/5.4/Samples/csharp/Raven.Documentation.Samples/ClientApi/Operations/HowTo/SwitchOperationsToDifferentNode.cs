using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Http;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.HowTo
{
    public class SwitchOperationsToDifferentNode
    {
        private interface OperationsForDatabaseSyntax
        {
            #region syntax_1
            ServerOperationExecutor ForNode(string nodeTag);
            #endregion
        }

        public void SwitchOperationToDifferentNode()
        {
            #region for_node_1
            // Default node access can be defined on the store
            var documentStore = new DocumentStore
            {
                Urls = new[] { "ServerURL_1", "ServerURL_2", "..." },
                Database = "DefaultDB",
                Conventions = new DocumentConventions
                {
                    // For example:
                    // With ReadBalanceBehavior set to: 'FastestNode':
                    // Client READ requests will address the fastest node
                    // Client WRITE requests will address the preferred node
                    ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                }
            }.Initialize();
            
            using (documentStore)
            {
                // Use 'ForNode' to override the default node configuration
                // The Maintenance.Server operation will be executed on the specified node
                var dbNames = documentStore.Maintenance.Server.ForNode("C")
                    .Send(new GetDatabaseNamesOperation(0, 25));
            }
            #endregion
        }
    }
}
