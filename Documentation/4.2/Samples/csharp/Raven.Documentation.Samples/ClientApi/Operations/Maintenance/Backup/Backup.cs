using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Backup
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
                        //Local path for storing the backup
                        FolderPath = @"E:\RavenBackups"
                    },

                    //Full Backup period (Cron expression for a 3-hours period)
                    FullBackupFrequency = "0 */3 * * *",

                    //Set backup type to Logical-Backup
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
                        //Local path for storing the backup
                        FolderPath = @"E:\RavenBackups"
                    },

                    //Full Backup period (Cron expression for a 3-hours period)
                    FullBackupFrequency = "0 */3 * * *",

                    //Set backup type to Logical-Backup
                    BackupType = BackupType.Backup,

                    //Task Name
                    Name = "fullBackupTask"
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
                        FolderPath = @"E:\RavenBackups"
                    },

                    #region backup_type_snapshot
                    //Set backup type to Snapshot
                    BackupType = BackupType.Snapshot,
                    #endregion

                    #region backup_full_backup
                    //A full-backup will run every 6-hours (Cron expression)
                    FullBackupFrequency = "0 */6 * * *",
                    #endregion

                    #region backup_incremental_backup
                    //An incremental-backup will run every 20 minutes (Cron expression)
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
                        FolderPath = @"E:\RavenBackups"
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
                    },

                    //Amazon S3 bucket settings.
                    S3Settings = new S3Settings
                    {
                        AwsAccessKey = "your access key here",
                        AwsSecretKey = "your secret key here",
                        AwsRegionName = "OPTIONAL",
                        BucketName = "john-bucket"
                    },

                    //Amazon Glacier settings.
                    GlacierSettings = new GlacierSettings
                    {
                        AwsAccessKey = "your access key here",
                        AwsSecretKey = "your secret key here",
                        AwsRegionName = "OPTIONAL",
                        VaultName = "john-glacier",
                        RemoteFolderName = "john/backups"
                    },

                    //Google Cloud Backup settings
                    GoogleCloudSettings = new GoogleCloudSettings
                    {
                        BucketName = "RavenBucket",
                        RemoteFolderName = "BackupFolder",
                        GoogleCredentialsJson = "GoogleCredentialsJson"
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
                        FolderPath = @"E:\RavenBackups"
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
                #region initiate_immediate_backup_execution
                //Create a new backup task
                var operation = new UpdatePeriodicBackupOperation(config);
                var result = await docStore.Maintenance.SendAsync(operation);

                //Run the backup task immediately
                await docStore.Maintenance.SendAsync(new StartBackupOperation(true, result.TaskId));
                    #endregion

                #region get_backup_execution_results
                //Pass the the ongoing backup task ID to GetPeriodicBackupStatusOperation  
                var backupStatus = new GetPeriodicBackupStatusOperation(result.TaskId);
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
                var backupPath = @"C:\Users\RavenDB\backups\2018-12-26-16-17.ravendb-Products-A-backup";
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
                //Restore to the specified directory path
                var dataPath = @"C:\Users\RavenDB\backups\2018-12-26-16-17.ravendb-Products-A-backup\restoredDatabaseLocation";
                restoreConfiguration.DataDirectory = dataPath;
                #endregion

                #region restore_disable_ongoing_tasks_true
                //Do or do not run ongoing tasks after restoration.
                //Default setting is FALSE, to allow tasks' execution when the backup is restored.
                restoreConfiguration.DisableOngoingTasks = true;
                #endregion
            }

            #region encrypted_database
            // path to the certificate you received during the server setup
            var cert = new X509Certificate2(@"C:\Users\RavenDB\authentication_key\admin.client.certificate.RavenDBdom.pfx");
            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.RavenDBdom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {
                // Backup & Restore here
            }
            #endregion

            // path to the authentication key you received during the server setup
            cert = new X509Certificate2(@"C:\Users\RavenDB\authentication_key\admin.client.certificate.RavenDBdom.pfx");
            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.RavenDBdom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {
                #region restore_encrypted_database
                //Restore an encrypted database from an encrypted snapshot

                var restoreConfiguration = new RestoreBackupConfiguration();

                //New database name
                restoreConfiguration.DatabaseName = "newEncryptedDatabase";

                //Backup-file location
                var backupPath = @"C:\Users\RavenDB\2019-01-06-11-11.ravendb-encryptedDatabase-A-snapshot";
                restoreConfiguration.BackupLocation = backupPath;

                //Specify the key that was used to encrypt the backup file
                restoreConfiguration.EncryptionKey = "1F0K2R/KkcwbkK7n4kYlv5eqisy/pMnSuJvZ2sJ/EKo=";

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);
                #endregion
            }
        }
        public class Foo
        {
            public class RestoreBackupOperation
            {
                #region restore_restorebackupoperation
                public RestoreBackupOperation(RestoreBackupConfiguration restoreConfiguration)
                #endregion
                { }

                #region restore_restorebackupconfiguration
                public class RestoreBackupConfiguration
                {
                    public string DatabaseName { get; set; }
                    public string BackupLocation { get; set; }
                    public string LastFileNameToRestore { get; set; }
                    public string DataDirectory { get; set; }
                    public string EncryptionKey { get; set; }
                    public bool DisableOngoingTasks { get; set; }
                    public bool SkipIndexes { get; set; }
                }
                #endregion
            }

            #region periodic_backup_status
            public class PeriodicBackupStatus : IDatabaseTaskStatus
            {
                public long TaskId { get; set; }
                public BackupType BackupType { get; set; }
                public bool IsFull { get; set; }
                public string NodeTag { get; set; }
                public DateTime? LastFullBackup { get; set; }
                public DateTime? LastIncrementalBackup { get; set; }
                public DateTime? LastFullBackupInternal { get; set; }
                public DateTime? LastIncrementalBackupInternal { get; set; }
                public LocalBackup LocalBackup { get; set; }
                public UploadToS3 UploadToS3;
                public UploadToGlacier UploadToGlacier;
                public UploadToAzure UploadToAzure;
                public UploadToFtp UploadToFtp;
                public long? LastEtag { get; set; }
                public LastRaftIndex LastRaftIndex { get; set; }
                public string FolderName { get; set; }
                public long? DurationInMs { get; set; }
                public long Version { get; set; }
                public Error Error { get; set; }
                public long? LastOperationId { get; set; }
            }
            #endregion

            public class StartBackupOperation
            {
                #region start_backup_operation
                public StartBackupOperation(bool isFullBackup, long taskId)
                #endregion
                {
                }
            }
        }
    }
}

