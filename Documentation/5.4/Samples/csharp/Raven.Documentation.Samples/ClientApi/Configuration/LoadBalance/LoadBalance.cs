using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Http;
using Raven.Client.ServerWide.Operations.Configuration;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Configuration.LoadBalance
{
    public class LoadBalanceExamples
    {
        public LoadBalanceExamples()
        {
            #region LoadBalance_1
            // Initialize 'LoadBalanceBehavior' on the client:
            var documentStore = new DocumentStore
            {
                Urls = new[] {"ServerURL_1", "ServerURL_2", "..."},
                Database = "DefaultDB",
                Conventions = new DocumentConventions
                {
                    // Enable the session-context feature
                    // If this is not enabled then a context string set in a session will be ignored 
                    LoadBalanceBehavior = LoadBalanceBehavior.UseSessionContext,
                    
                    // Assign a method that sets the default context string
                    // This string will be used for sessions that do Not provide a context string
                    // A sample GetDefaultContext method is defined below
                    LoadBalancerPerSessionContextSelector = GetDefaultContext,
                    
                    // Set a seed
                    // The seed is 0 by default, provide any number to override
                    LoadBalancerContextSeed = 5 
                }
            }.Initialize();
            #endregion
            
            #region LoadBalance_2
            // Open a session that will use the DEFAULT store values:
            using (var session = documentStore.OpenSession())
            {
                // For all Read & Write requests made in this session,
                // node to access is calculated from string & seed values defined on the store
                var employee = session.Load<Employee>("employees/1-A"); 
            }
            #endregion
            
            #region LoadBalance_3
            // Open a session that will use a UNIQUE context string:
            using (var session = documentStore.OpenSession())
            {
                // Call SetContext, pass a unique context string for this session
                session.Advanced.SessionInfo.SetContext("SomeOtherContext");
                
                // For all Read & Write requests made in this session,
                // node to access is calculated from the unique string & the seed defined on the store
                var employee = session.Load<Employee>("employees/1-A");
            }
            #endregion
            
            #region LoadBalance_4
            // Setting 'LoadBalanceBehavior' on the server by sending an operation:
            using (documentStore)
            {
                // Define the client configuration to put on the server
                var configurationToSave = new ClientConfiguration
                {
                    // Enable the session-context feature
                    // If this is not enabled then a context string set in a session will be ignored 
                    LoadBalanceBehavior = LoadBalanceBehavior.UseSessionContext,
                    
                    // Set a seed
                    // The seed is 0 by default, provide any number to override
                    LoadBalancerContextSeed = 10,
                    
                    // NOTE:
                    // The session's context string is Not set on the server
                    // You still need to set it on the client:
                    //   * either as a convention on the document store
                    //   * or pass it to 'SetContext' method on the session
                    
                    // Configuration will be in effect when Disabled is set to false
                    Disabled = false
                };
                
                // Define the put configuration operation for the DEFAULT database
                var putConfigurationOp = new PutClientConfigurationOperation(configurationToSave);
                
                // Execute the operation by passing it to Maintenance.Send
                documentStore.Maintenance.Send(putConfigurationOp);
                
                // After the operation has executed:
                // all Read & Write requests, per session, will address the node calculated from:
                //   * the seed set on the server &
                //   * the session's context string set on the client
            }
            #endregion
            
            #region LoadBalance_5
            // Setting 'LoadBalanceBehavior' on the server by sending an operation:
            using (documentStore)
            {
                // Define the client configuration to put on the server
                var configurationToSave = new ClientConfiguration
                {
                    // Enable the session-context feature
                    // If this is not enabled then a context string set in a session will be ignored 
                    LoadBalanceBehavior = LoadBalanceBehavior.UseSessionContext,
                    
                    // Set a seed
                    // The seed is 0 by default, provide any number to override
                    LoadBalancerContextSeed = 10,
                    
                    // NOTE:
                    // The session's context string is Not set on the server
                    // You still need to set it on the client:
                    //   * either as a convention on the document store
                    //   * or pass it to 'SetContext' method on the session
                    
                    // Configuration will be in effect when Disabled is set to false
                    Disabled = false
                };
                
                // Define the put configuration operation for ALL databases
                var putConfigurationOp = new PutServerWideClientConfigurationOperation(configurationToSave);
                
                // Execute the operation by passing it to Maintenance.Server.Send
                documentStore.Maintenance.Server.Send(putConfigurationOp);
                
                // After the operation has executed:
                // all Read & Write requests, per session, will address the node calculated from:
                //   * the seed set on the server &
                //   * the session's context string set on the client
            }
            #endregion
        }

        #region LoadBalance_6
        // A customized method for getting a default context string 
        private string GetDefaultContext(string dbName)
        {
            // Method is invoked by RavenDB with the database name
            // Use that name - or return any string of your choice
            return "DefaultContextString";
        }
        #endregion
    }
}
