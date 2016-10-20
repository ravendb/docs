using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Abstractions.Replication;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Bundles
{
	public class HowClientIntegratesWithReplicationBundle
	{
		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region client_integration_1
				store.Conventions.FailoverBehavior = FailoverBehavior.FailImmediately;
				#endregion

				#region client_integration_4
				store.Conventions.FailoverBehavior = FailoverBehavior.ReadFromAllServers 
					| FailoverBehavior.AllowReadsFromSecondariesAndWritesToSecondaries;
                #endregion
                
                using (var session = store.OpenSession())
                {
                    #region client_integration_5
                    session.Store(new ReplicationDocument
                    {
                        ClientConfiguration = new ReplicationClientConfiguration
                        {
                            FailoverBehavior = FailoverBehavior.ReadFromLeaderWriteToLeader
                        }

                    }, "Raven/Replication/Destinations");
                    #endregion
                    session.SaveChanges();
                }

                #region client_integration_2
                store
                    .DatabaseCommands
					.Put(
						"Raven/ServerPrefixForHilo",
						null,
						new RavenJObject
						{
							{ "ServerPrefix", "NorthServer/" }
						},
						new RavenJObject());
				#endregion
			}
		}

		public HowClientIntegratesWithReplicationBundle()
		{
			using (var store = new DocumentStore())
			{
				#region client_integration_3
				store.FailoverServers = new FailoverServers();
				store.FailoverServers.ForDefaultDatabase = new[]
				{
					new ReplicationDestination
						{
							Url = "http://localhost:8078", 
							ApiKey = "apikey"
						},
					new ReplicationDestination
						{
							Url = "http://localhost:8077/",
							Database = "test",
							Username = "user",
							Password = "secret"
						}
				};

				store.FailoverServers.ForDatabases = new Dictionary<string, ReplicationDestination[]>
				{
					{
						"Northwind",
						new[]
							{
								new ReplicationDestination
									{
										Url = "http://localhost:8076"
									}
							}
					}
				};
				#endregion
			}

			using (var store = new DocumentStore())
			{
				/*
				#region client_integration_5
				Url = http://localhost:59233;
					// Primary server url
				Failover = { Url:'http://localhost:8078'}; 
					// Failover for DefaultDatabase
				Failover = { Url:'http://localhost:8077/', Database:'test'};
					// Failover for DefaultDatabase with non-default database
				Failover = Northwind|{ Url:'http://localhost:8076/'};
					// Failover for 'Northwind' database
				Failover= { Url:'http://localhost:8075', Username:'user', Password:'secret'};
					// Failover for DefaultDatabase with Username and Password
				Failover= { Url:'http://localhost:8074', ApiKey:'d5723e19-92ad-4531-adad-8611e6e05c8a'}
					// Failover for DefaultDatabase with ApiKey
				#endregion
				*/
			}
		}
	}
}