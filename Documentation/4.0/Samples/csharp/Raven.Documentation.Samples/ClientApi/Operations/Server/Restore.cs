using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class Restore
    {
        private class Foo
        {
            public class DeleteDatabasesOperation
            {
                /*
                #region restore_1
                public RestoreBackupOperation(RestoreBackupConfiguration restoreConfiguration)
                #endregion
                */

                #region restore_2
                public class RestoreBackupConfiguration
                {
                    public string DatabaseName { get; set; }

                    public string BackupLocation { get; set; }

                    public string LastFileNameToRestore { get; set; }

                    public string DataDirectory { get; set; }

                    public string EncryptionKey { get; set; }
                }
                #endregion
            }
        }

        public Restore()
        {
            using (var store = new DocumentStore())
            {
                #region restore_3
                RestoreBackupConfiguration config = new RestoreBackupConfiguration()
                {
                    BackupLocation = @"C:\backups\Northwind",
                    DatabaseName = "Northwind"
                };
                RestoreBackupOperation restoreOperation = new RestoreBackupOperation(config);
                store.Maintenance.Server.Send(restoreOperation)
                    .WaitForCompletion();
                #endregion
            }
        }
    }
}
