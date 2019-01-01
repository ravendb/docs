using System;
using Raven.Client.Documents;
using Raven.Client;
using System.Linq;
using System.IO;
using System.Collections.Generic;
//using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.Documents.Smuggler;
using Raven.Client.ServerWide.Operations;

namespace Rvn.Ch02
{
    public class User
    {
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var a = MainInternal();
            a.Wait();
        }

        static async Task MainInternal()
        {
            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
                #region logical_full_backup_every_3_hours
                var config = new PeriodicBackupConfiguration
                {
                    LocalSettings = new LocalSettings
                    {
                        //Backup files local path
                        FolderPath = @"C:\Users\Beth\backups"
                    },

                    //Full Backup period (Cron expression for a 3-hours period)
                    FullBackupFrequency = "0 */3 * * *",

                    //Type can be Backup or Snapshot
                    BackupType = BackupType.Backup,

                    //Task Name
                    Name = "fullBackupTask",
                };
                var operation = new UpdatePeriodicBackupOperation(config);
                var result = await docStore.Maintenance.SendAsync(operation);
                #endregion
            }
            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
                var config = new PeriodicBackupConfiguration
                {
                    LocalSettings = new LocalSettings
                    {
                        FolderPath = @"C:\Users\Beth\backups"
                    },

                    #region backup_type_snapshot
                    //Type can be Backup or Snapshot
                    BackupType = BackupType.Snapshot,
                    #endregion

                    #region backup_full_backup
                    //Full Backup period (Cron expression for a 6-hours period)
                    FullBackupFrequency = "0 */6 * * *",
                    #endregion

                    #region backup_incremental_backup
                    //Cron expression: set incremental backup period ("*/20 * * * *" is a 20-minutes period)
                    IncrementalBackupFrequency = "*/20 * * * *",
                    #endregion

                };
            }
            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
                #region backup_remote_destinations
                var config = new PeriodicBackupConfiguration
                {
                    LocalSettings = new LocalSettings
                    {
                        FolderPath = @"C:\Users\Beth\backups"
                    },

                    //FTP Backup settings
                    FtpSettings = new FtpSettings
                    {
                        Url = "192.168.10.4",
                        Port = 8080,
                        UserName = "Beth",
                        Password = "Bethlehem38"
                    },

                    //Azure Backup settings
                    AzureSettings = new AzureSettings
                    {
                        StorageContainer = "storageContainer",
                        RemoteFolderName = "remoteFolder",
                        AccountName = "BethAccount",
                        AccountKey = "key"
                    }
                };
                var operation = new UpdatePeriodicBackupOperation(config);
                var result = await docStore.Maintenance.SendAsync(operation);
                #endregion
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {


                #region backup_restore_DisableOngoingTasks
                //Do or do not run ongoing tasks after restoration.
                //Default setting is FALSE, to allow tasks' execution when backup is restored.
                restoreConfiguration.DisableOngoingTasks = true;
                #endregion

                #region backup_restore
                var backupPath = @"C:\Users\Beth\backups\2018-12-26-16-17.ravendb-Products-A-backup";

                var dataPath = @"C:\Users\Beth\backups\2018-12-26-16-17.ravendb-Products-A-backup\restoredDatabaseLocation";

                var restoreConfiguration = new RestoreBackupConfiguration();

                //New database name
                restoreConfiguration.DatabaseName = "newProductsDatabase";

                //Backup location (directory only)
                restoreConfiguration.BackupLocation = backupPath;

                //New database directory
                //restoreConfiguration.DataDirectory = dataPath;

                //Disable or Enable ongoing tasks after restoration.
                //Default setting is FALSE, so tasks DO run when backup is restored.
                restoreConfiguration.DisableOngoingTasks = false;

                //Last incremental backup file to restore from
                restoreConfiguration.LastFileNameToRestore = @"2018-12-26-09-50.ravendb-incremental-backup";

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);
                #endregion
            }
        }
    }
}
