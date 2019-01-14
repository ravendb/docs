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
                        FolderPath = @"C:\Users\John\backups"
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
                #region encrypted_logical_full_backup
                var config = new PeriodicBackupConfiguration
                {
                    LocalSettings = new LocalSettings
                    {
                        //Backup files local path
                        FolderPath = @"C:\Users\John\backups"
                    },

                    //Full Backup period (Cron expression for a 3-hours period)
                    FullBackupFrequency = "0 */3 * * *",

                    //Type can be Backup or Snapshot
                    BackupType = BackupType.Backup,

                    //Task Name
                    Name = "fullBackupTask",

                    BackupEncryptionSettings = new BackupEncryptionSettings
                    {
                        Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs="
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
                var config = new PeriodicBackupConfiguration
                {
                    LocalSettings = new LocalSettings
                    {
                        FolderPath = @"C:\Users\John\backups"
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
                        FolderPath = @"C:\Users\John\backups"
                    },

                    //FTP Backup settings
                    FtpSettings = new FtpSettings
                    {
                        Url = "192.168.10.4",
                        Port = 8080,
                        UserName = "John",
                        Password = "JohnDoe38"
                    },

                    //Azure Backup settings
                    AzureSettings = new AzureSettings
                    {
                        StorageContainer = "storageContainer",
                        RemoteFolderName = "remoteFolder",
                        AccountName = "JohnAccount",
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
                #region restore_to_single_node
                var restoreConfiguration = new RestoreBackupConfiguration();

                //New database name
                restoreConfiguration.DatabaseName = "newProductsDatabase";

                //Local path with a backup file
                var backupPath = @"C:\Users\John\backups\2018-12-26-16-17.ravendb-Products-A-backup";
                restoreConfiguration.BackupLocation = backupPath;

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);
                #endregion

                #region restore_disable_ongoing_tasks_false
                //Disable or Enable ongoing tasks after restoration.
                //Default setting is FALSE, so tasks DO run when backup is restored.
                restoreConfiguration.DisableOngoingTasks = false;
                #endregion

                #region restore_last_file_name_to_restore
                //Last incremental backup file to restore from
                restoreConfiguration.LastFileNameToRestore = @"2018-12-26-12-00.ravendb-incremental-backup";
                #endregion

                #region restore_to_specific__data_directory
                //Restore to a pre-chosen folder
                var dataPath = @"C:\Users\John\backups\2018-12-26-16-17.ravendb-Products-A-backup\restoredDatabaseLocation";
                restoreConfiguration.DataDirectory = dataPath;
                #endregion

                #region restore_disable_ongoing_tasks_true
                //Do or do not run ongoing tasks after restoration.
                //Default setting is FALSE, to allow tasks' execution when backup is restored.
                restoreConfiguration.DisableOngoingTasks = true;
                #endregion
            }

            #region encrypted_database
            // path to the certificate you received during the server setup
            var cert = new X509Certificate2(@"C:\Users\John\authentication_key\admin.client.certificate.johndom.pfx");
            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.johndom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {
                // Backup & Restore here
            }
            #endregion

            // path to the authentication key you received during the server setup
            var cert = new X509Certificate2(@"C:\Users\John\authentication_key\admin.client.certificate.johndom.pfx");
            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.johndom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {
                #region restore_encrypted_database
                // restore encrypted database

                // restore configuration
                var restoreConfiguration = new RestoreBackupConfiguration();

                //New database name
                restoreConfiguration.DatabaseName = "newEncryptedDatabase";

                //Backup-file location
                var backupPath = @"C:\Users\John\2019-01-06-11-11.ravendb-encryptedDatabase-A-snapshot";
                restoreConfiguration.BackupLocation = backupPath;

                restoreConfiguration.EncryptionKey = "1F0K2R/KkcwbkK7n4kYlv5eqisy/pMnSuJvZ2sJ/EKo=";

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);
                #endregion
            }

            * private class Foo
            {
                public class DeleteDatabasesOperation
                {
                    #region restore_restorebackupoperation
                    public RestoreBackupOperation(RestoreBackupConfiguration restoreConfiguration)
                    #endregion

                    #region restore_restorebackupconfiguration
                    public class RestoreBackupConfiguration
                    {
                        public string DatabaseName { get; set; }
                        public string BackupLocation { get; set; }
                        public string LastFileNameToRestore { get; set; }
                        public string DataDirectory { get; set; }
                        public string EncryptionKey { get; set; }
                        public bool DisableOngoingTasks { get; set; }
                    }

                    #endregion
                }
            }
        }
    }
}
