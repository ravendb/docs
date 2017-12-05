using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
	public class CreateDeleteDatabase
	{
		public CreateDeleteDatabase()
		{
			using (var store = new DocumentStore())
			{
                #region CreateDatabase
			    store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("MyNewDatabase")));
                #endregion
                
			    #region DeleteDatabase
			    store.Maintenance.Server.Send(new DeleteDatabasesOperation("MyNewDatabase", hardDelete: true, fromNode: null, timeToWaitForConfirmation: null));
			    #endregion

			    #region DeleteDatabases
			    var parameters = new DeleteDatabasesOperation.Parameters
			    {
			        DatabaseNames = new[] { "MyNewDatabase", "OtherDatabaseToDelete" },
			        HardDelete = true,
			        FromNodes = new[] { "A", "C" },     // optional
			        TimeToWaitForConfirmation = TimeSpan.FromSeconds(30)    // optional
                };
			    store.Maintenance.Server.Send(new DeleteDatabasesOperation(parameters));
                #endregion
            }
        }

		public async Task CreateDeleteDatabaseAsync()
		{
			using (var store = new DocumentStore())
			{
			    #region CreateDatabaseAsync
			    await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord("MyNewDatabase")));
                #endregion

                #region DeleteDatabaseAsync
			    await store.Maintenance.Server.SendAsync(new DeleteDatabasesOperation("MyNewDatabase", hardDelete: true, fromNode: null, timeToWaitForConfirmation: null));
                #endregion

			    #region DeleteDatabasesAsync
			    var parameters = new DeleteDatabasesOperation.Parameters
			    {
			        DatabaseNames = new[] {"MyNewDatabase", "OtherDatabaseToDelete"},
			        HardDelete = true,
			        FromNodes = new[] {"A", "C"},   // optional
                    TimeToWaitForConfirmation = TimeSpan.FromSeconds(30)    // optional
                };
			    await store.Maintenance.Server.SendAsync(new DeleteDatabasesOperation(parameters));
			    #endregion
            }
        }
	}
}
