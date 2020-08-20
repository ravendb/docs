using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Http;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide.Operations.Configuration;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Configuration
{
    public class PerSessionTopology
    {
        public PerSessionTopology()
        {
        }

        public DocumentStore GetDocumentStore()
        {
            DocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "TestDatabase"
            };
            store.Initialize();

            var parameters = new DeleteDatabasesOperation.Parameters
            {
                DatabaseNames = new[] { "TestDatabase" },
                HardDelete = true,
            };
            store.Maintenance.Server.Send(new DeleteDatabasesOperation(parameters));
            store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("TestDatabase")));

            return store;
        }

        public class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string AddressId { get; set; }
            public int Count { get; set; }
            public int Age { get; set; }
        }

        public void clientSessionLoadBalancing()
        {
            #region LoadBalanceBehavior
            string context = "usersTopology";

            using var store = new DocumentStore
            {
                Conventions = new DocumentConventions
                {
                    ReadBalanceBehavior = ReadBalanceBehavior.RoundRobin,
                    LoadBalanceBehavior = LoadBalanceBehavior.UseSessionContext,
                    LoadBalancerPerSessionContextSelector = db => context
                }
            }.Initialize();

            using (var session = store.OpenSession())
            {
                session.Advanced.SessionInfo.SetContext("usersTopology");
                session.Load<User>("users/1-A");
            }
            #endregion
        }


        public void AdministratorLoadBalancing()
        {
            using (var store = GetDocumentStore())
            {
                var requestExecutor = store.GetRequestExecutor();

                #region PutClientConfigurationOperation
                store.Maintenance.Send(
                    new PutClientConfigurationOperation(
                            new ClientConfiguration
                            {
                                ReadBalanceBehavior = ReadBalanceBehavior.RoundRobin,
                                LoadBalanceBehavior = LoadBalanceBehavior.UseSessionContext,
                                LoadBalancerContextSeed = 10,
                                Disabled = false
                            }));
                #endregion

                using (var session = store.OpenSession())
                {
                    session.Load<dynamic>("users/1-A"); // forcing client configuration update
                }

                #region PutServerWideClientConfigurationOperation
                store.Maintenance.Server.Send(new PutServerWideClientConfigurationOperation(
                    new ClientConfiguration
                    {
                        ReadBalanceBehavior = ReadBalanceBehavior.FastestNode,
                        LoadBalanceBehavior = LoadBalanceBehavior.UseSessionContext,
                        LoadBalancerContextSeed = 10,
                        Disabled = false
                    }
                    ));
                #endregion

                using (var session = store.OpenSession())
                {
                    session.Load<dynamic>("users/1-A"); // forcing client configuration update
                }

            }
        }

    }
}
