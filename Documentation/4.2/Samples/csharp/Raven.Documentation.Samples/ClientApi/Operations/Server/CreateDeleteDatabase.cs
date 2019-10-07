using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class CreateDeleteDatabase
    {
        private class Foo
        {
            public class DeleteDatabasesOperation
            {
                #region delete_database_syntax
                public DeleteDatabasesOperation(
                    string databaseName,
                    bool hardDelete,
                    string fromNode = null,
                    TimeSpan? timeToWaitForConfirmation = null)
                {
                }

                public DeleteDatabasesOperation(DeleteDatabasesOperation.Parameters parameters)
                {
                }

                public class Parameters
                {
                    public string[] DatabaseNames { get; set; }

                    public bool HardDelete { get; set; }

                    public string[] FromNodes { get; set; }

                    public TimeSpan? TimeToWaitForConfirmation { get; set; }
                }
                #endregion
            }

            public class CreateDatabaseOperation
            {
                #region create_database_syntax
                public CreateDatabaseOperation(
                    DatabaseRecord databaseRecord,
                    int replicationFactor = 1)
                {
                }
                #endregion
            }
        }

        #region EnsureDatabaseExists
        public void EnsureDatabaseExists(IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
        {
            database = database ?? store.Database;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

            try
            {
                store.Maintenance.ForDatabase(database).Send(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                if (createDatabaseIfNotExists == false)
                    throw;

                try
                {
                    store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));
                }
                catch (ConcurrencyException)
                {
                    // The database was already created before calling CreateDatabaseOperation
                }

            }
        }
        #endregion

        #region EnsureDatabaseExistsAsync
        public async Task EnsureDatabaseExistsAsync(IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
        {
            database = database ?? store.Database;

            if (string.IsNullOrWhiteSpace(database))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

            try
            {
                await store.Maintenance.ForDatabase(database).SendAsync(new GetStatisticsOperation());
            }
            catch (DatabaseDoesNotExistException)
            {
                if (createDatabaseIfNotExists == false)
                    throw;

                try
                {
                    await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(database)));
                }
                catch (ConcurrencyException)
                {
                }
            }
        }
        #endregion

        public CreateDeleteDatabase()
        {
            using (var store = new DocumentStore())
            {
                #region CreateDatabase
                store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("MyNewDatabase")));
                #endregion

                #region CreateEncryptedDatabase
                store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("MyEncryptedDatabase")
                {
                    Encrypted = true
                }));
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

                #region CreateEncryptedDatabaseAsync
                await store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord("MyEncryptedDatabase")
                {
                    Encrypted = true
                }));
                #endregion

                #region DeleteDatabaseAsync
                await store.Maintenance.Server.SendAsync(new DeleteDatabasesOperation("MyNewDatabase", hardDelete: true, fromNode: null, timeToWaitForConfirmation: null));
                #endregion

                #region DeleteDatabasesAsync
                var parameters = new DeleteDatabasesOperation.Parameters
                {
                    DatabaseNames = new[] { "MyNewDatabase", "OtherDatabaseToDelete" },
                    HardDelete = true,
                    FromNodes = new[] { "A", "C" },   // optional
                    TimeToWaitForConfirmation = TimeSpan.FromSeconds(30)    // optional
                };
                await store.Maintenance.Server.SendAsync(new DeleteDatabasesOperation(parameters));
                #endregion
            }
        }
    }
}
