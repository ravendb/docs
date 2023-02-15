using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.Documents.Operations.Backups.Sharding;
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
                #region backup_remote_destinations
                var config = new PeriodicBackupConfiguration
                {
                    LocalSettings = new LocalSettings
                    {
                        FolderPath = @"E:\RavenBackups"
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
                #region restore_local-settings
                // Create a dictionary with paths to shard backups
                var restoreSettings = new ShardedRestoreSettings
                {
                    Shards = new Dictionary<int, SingleShardRestoreSetting>(),
                };

                // First shard
                restoreSettings.Shards.Add(0, new SingleShardRestoreSetting
                {
                    // Shard Number
                    // Please make sure that each shard is given 
                    // the same number it had when it was backed up.
                    ShardNumber = 0,
                    // Node Tag
                    // Please make sure that each shard is restored
                    // to the same node it was backed-up by.
                    NodeTag = "A",
                    FolderName = "d:/backups/shard0-backup-folder"
                });

                // Second shard
                restoreSettings.Shards.Add(1, new SingleShardRestoreSetting
                {
                    ShardNumber = 1,
                    NodeTag = "B",
                    FolderName = "d:/backups/shard1-backup-folder"
                });

                // Third shard
                restoreSettings.Shards.Add(2, new SingleShardRestoreSetting
                {
                    ShardNumber = 2,
                    NodeTag = "C",
                    FolderName = "d:/backups/shard2-backup-folder",
                });

                var restoreBackupOperation = new RestoreBackupOperation(new RestoreBackupConfiguration
                {
                    // Database Name
                    DatabaseName = "Books",
                    // Paths to backup files
                    ShardRestoreSettings = restoreSettings
                });
                #endregion
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
                #region restore_s3-settings
                // Create a dictionary with paths to shard backups
                var restoreSettings = new ShardedRestoreSettings
                {
                    Shards = new Dictionary<int, SingleShardRestoreSetting>(),
                };

                // First shard
                restoreSettings.Shards.Add(0, new SingleShardRestoreSetting
                {
                    // Shard Number
                    // Please make sure that each shard is given 
                    // the same number it had when it was backed up.
                    ShardNumber = 0,
                    NodeTag = "A",
                    FolderName = "backups/2023-02-12-09-52-27.ravendb-Books$0-A-backup"
                });

                // Second shard
                restoreSettings.Shards.Add(1, new SingleShardRestoreSetting
                {
                    ShardNumber = 1,
                    NodeTag = "B",
                    FolderName = "backups/2023-02-12-09-52-27.ravendb-Books$1-B-backup"
                });

                // Third shard
                restoreSettings.Shards.Add(2, new SingleShardRestoreSetting
                {
                    ShardNumber = 2,
                    NodeTag = "C",
                    FolderName = "backups/2023-02-12-09-52-27.ravendb-Books$2-C-backup",

                });

                var restoreBackupOperation = new RestoreBackupOperation(new RestoreFromS3Configuration
                {
                    // Database Name
                    DatabaseName = "Books",
                    // Paths to backup files
                    ShardRestoreSettings = restoreSettings,
                    // S3 Bucket settings
                    Settings = new S3Settings 
                    {
                        AwsRegionName = "us-east-1", // Optional
                        BucketName = "your bucket name here",
                        RemoteFolderName = "", // Replaced by restoreSettings.Shards.FolderName 
                        AwsAccessKey = "your access key here",
                        AwsSecretKey = "your secret key here",
                    } 
                });
                #endregion
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
                #region restore_azure-settings
                // Create a dictionary with paths to shard backups
                var restoreSettings = new ShardedRestoreSettings
                {
                    Shards = new Dictionary<int, SingleShardRestoreSetting>(),
                };

                // First shard
                restoreSettings.Shards.Add(0, new SingleShardRestoreSetting
                {
                    // Shard Number
                    // Please make sure that each shard is given 
                    // the same number it had when it was backed up.
                    ShardNumber = 0,
                    NodeTag = "A",
                    FolderName = "backups/2023-02-12-09-52-27.ravendb-Books$0-A-backup"
                });

                // Second shard
                restoreSettings.Shards.Add(1, new SingleShardRestoreSetting
                {
                    ShardNumber = 1,
                    NodeTag = "B",
                    FolderName = "backups/2023-02-12-09-52-27.ravendb-Books$1-B-backup"
                });

                // Third shard
                restoreSettings.Shards.Add(2, new SingleShardRestoreSetting
                {
                    ShardNumber = 2,
                    NodeTag = "C",
                    FolderName = "backups/2023-02-12-09-52-27.ravendb-Books$2-C-backup",

                });

                var restoreBackupOperation = new RestoreBackupOperation(new RestoreFromAzureConfiguration
                {
                    // Database Name
                    DatabaseName = "Books",
                    // Paths to backup files
                    ShardRestoreSettings = restoreSettings,
                    // Azure Blob settings
                    Settings = new AzureSettings
                    {
                        StorageContainer = "storageContainer",
                        RemoteFolderName = "", // Replaced by restoreSettings.Shards.FolderName 
                        AccountName = "your account name here",
                        AccountKey = "your account key here",
                    }
                  });
                #endregion
            }

            #region SingleShardRestoreSetting
            var shard0 = new SingleShardRestoreSetting
            {
                ShardNumber = 0,
                NodeTag = "A",
                FolderName = "backups/2023-02-12-09-52-27.ravendb-Books$0-A-backup"
            };

            var shard1 = new SingleShardRestoreSetting
            {
                ShardNumber = 1,
                NodeTag = "B",
                FolderName = "backups/2023-02-12-09-52-27.ravendb-Books$1-B-backup"
            };
            var shard2 = new SingleShardRestoreSetting
            {
                ShardNumber = 2,
                NodeTag = "C",
                FolderName = "backups/2023-02-12-09-52-27.ravendb-Books$2-C-backup"
            };
            #endregion
        }
        public class Foo
        {
            public class RestoreBackupOperation
            {
                #region restore_RestoreBackupOperation
                public RestoreBackupOperation(RestoreBackupConfigurationBase restoreConfiguration)
                #endregion
                { }

                #region restore_RestoreBackupConfigurationBase
                public abstract class RestoreBackupConfigurationBase
                {
                    public string DatabaseName { get; set; }

                    public string LastFileNameToRestore { get; set; }

                    public string DataDirectory { get; set; }

                    public string EncryptionKey { get; set; }

                    public bool DisableOngoingTasks { get; set; }

                    public bool SkipIndexes { get; set; }

                    public ShardedRestoreSettings ShardRestoreSettings { get; set; }

                    public BackupEncryptionSettings BackupEncryptionSettings { get; set; }

                }
                #endregion

                #region restore_ShardedRestoreSettings
                public class ShardedRestoreSettings
                {
                  public Dictionary<int, 
                    SingleShardRestoreSetting> Shards { get; set; }
                }
                #endregion

                #region restore_SingleShardRestoreSetting
                public class SingleShardRestoreSetting
                {
                    // Shard number 
                    public int ShardNumber { get; set; }
                    // Node tag
                    public string NodeTag { get; set; }
                    // Backup file/s folder name
                    public string FolderName { get; set; }
                }
                #endregion
            }
        }
    }
}

