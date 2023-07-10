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
    class Program2
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
                        FolderPath = @"E:/RavenBackups"
                    },

                    //Azure Backup settings
                    AzureSettings = new AzureSettings
                    {
                        StorageContainer = "storageContainer",
                        RemoteFolderName = "remoteFolder",
                        AccountName = "JohnAccount",
                        AccountKey = "key"
                    },

                    //Amazon S3 bucket settings
                    S3Settings = new S3Settings
                    {
                        AwsAccessKey = "your access key here",
                        AwsSecretKey = "your secret key here",
                        AwsRegionName = "OPTIONAL",
                        BucketName = "john-bucket"
                    },

                    // Google Cloud bucket settings
                    GoogleCloudSettings = new GoogleCloudSettings
                    {
                        BucketName = "your bucket name here",
                        RemoteFolderName = "remoteFolder",
                        GoogleCredentialsJson = "your credentials here"
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
                    // Shard Number - which shard to restore to.
                    // Please make sure that each shard is given 
                    // the same number it had when it was backed up.
                    ShardNumber = 0,
                    // Node Tag - which node to restore to
                    NodeTag = "A",
                    // Backups Folder Name
                    FolderName = "E:/RavenBackups/2023-02-12-09-52-27.ravendb-Books$0-A-backup"
                });

                // Second shard
                restoreSettings.Shards.Add(1, new SingleShardRestoreSetting
                {
                    ShardNumber = 1,
                    NodeTag = "B",
                    FolderName = "E:/RavenBackups/2023-02-12-09-52-27.ravendb-Books$1-B-backup"
                });

                // Third shard
                restoreSettings.Shards.Add(2, new SingleShardRestoreSetting
                {
                    ShardNumber = 2,
                    NodeTag = "C",
                    FolderName = "E:/RavenBackups/2023-02-12-09-52-27.ravendb-Books$2-C-backup",
                });

                var restoreBackupOperation = new RestoreBackupOperation(new RestoreBackupConfiguration
                {
                    // Database Name
                    DatabaseName = "Books",
                    // Paths to backup files
                    ShardRestoreSettings = restoreSettings
                });

                var operation = await docStore.Maintenance.Server.SendAsync(restoreBackupOperation);
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
                    // Shard Number - which shard to restore to.
                    // Please make sure that each shard is given 
                    // the same number it had when it was backed up.
                    ShardNumber = 0,
                    // Node Tag - which node to restore to
                    NodeTag = "A",
                    // Backups Folder Name
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$0-A-backup"
                });

                // Second shard
                restoreSettings.Shards.Add(1, new SingleShardRestoreSetting
                {
                    ShardNumber = 1,
                    NodeTag = "B",
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$1-B-backup"
                });

                // Third shard
                restoreSettings.Shards.Add(2, new SingleShardRestoreSetting
                {
                    ShardNumber = 2,
                    NodeTag = "C",
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$2-C-backup",
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

                var operation = await docStore.Maintenance.Server.SendAsync(restoreBackupOperation);
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
                    // Shard Number - which shard to restore to.
                    // Please make sure that each shard is given 
                    // the same number it had when it was backed up.
                    ShardNumber = 0,
                    // Node Tag - which node to restore to
                    NodeTag = "A",
                    // Backups Folder Name
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$0-A-backup"
                });

                // Second shard
                restoreSettings.Shards.Add(1, new SingleShardRestoreSetting
                {
                    ShardNumber = 1,
                    NodeTag = "B",
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$1-B-backup"
                });

                // Third shard
                restoreSettings.Shards.Add(2, new SingleShardRestoreSetting
                {
                    ShardNumber = 2,
                    NodeTag = "C",
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$2-C-backup",
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

                var operation = await docStore.Maintenance.Server.SendAsync(restoreBackupOperation);
                #endregion
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
                #region restore_google-cloud-settings
                // Create a dictionary with paths to shard backups
                var restoreSettings = new ShardedRestoreSettings
                {
                    Shards = new Dictionary<int, SingleShardRestoreSetting>(),
                };

                // First shard
                restoreSettings.Shards.Add(0, new SingleShardRestoreSetting
                {
                    // Shard Number - which shard to restore to.
                    // Please make sure that each shard is given 
                    // the same number it had when it was backed up.
                    ShardNumber = 0,
                    // Node Tag - which node to restore to
                    NodeTag = "A",
                    // Backups Folder Name
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$0-A-backup"
                });

                // Second shard
                restoreSettings.Shards.Add(1, new SingleShardRestoreSetting
                {
                    ShardNumber = 1,
                    NodeTag = "B",
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$1-B-backup"
                });

                // Third shard
                restoreSettings.Shards.Add(2, new SingleShardRestoreSetting
                {
                    ShardNumber = 2,
                    NodeTag = "C",
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$2-C-backup",
                });

                var restoreBackupOperation = new RestoreBackupOperation(new RestoreFromGoogleCloudConfiguration
                {
                    // Database Name
                    DatabaseName = "Books",
                    // Paths to backup files
                    ShardRestoreSettings = restoreSettings,
                    // Google Cloud settings
                    Settings = new GoogleCloudSettings
                    {
                        BucketName = "your bucket name here",
                        RemoteFolderName = "", // Replaced by restoreSettings.Shards.FolderName 
                        GoogleCredentialsJson = "your credentials here"
                    }
                });

                var operation = await docStore.Maintenance.Server.SendAsync(restoreBackupOperation);
                #endregion
            }

            #region SingleShardRestoreSetting
            var shard0 = new SingleShardRestoreSetting
            {
                ShardNumber = 0,
                NodeTag = "A",
                FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$0-A-backup"
            };

            var shard1 = new SingleShardRestoreSetting
            {
                ShardNumber = 1,
                NodeTag = "B",
                FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$1-B-backup"
            };
            var shard2 = new SingleShardRestoreSetting
            {
                ShardNumber = 2,
                NodeTag = "C",
                FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$2-C-backup"
            };
            #endregion
        }


        static async Task singleShardSettings()
        {
            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
                #region singleShardSettings_restore_local-settings
                // Create a dictionary with paths to shard backups
                var restoreSettings = new ShardedRestoreSettings
                {
                    Shards = new Dictionary<int, SingleShardRestoreSetting>(),
                };

                // First shard
                restoreSettings.Shards.Add(0, new SingleShardRestoreSetting
                {
                    ShardNumber = 0,
                    NodeTag = "A",
                    // Backups Folder Name
                    FolderName = "E:/RavenBackups/2023-02-12-09-52-27.ravendb-Books$0-A-backup",
                    // Last incremental backup to restore
                    LastFileNameToRestore = "2023-02-12-10-30-00.ravendb-incremental-backup"
                });

                var restoreBackupOperation = new RestoreBackupOperation(new RestoreBackupConfiguration
                {
                    // Database Name
                    DatabaseName = "Books",
                    // Paths to backup files
                    ShardRestoreSettings = restoreSettings
                });

                var operation = await docStore.Maintenance.Server.SendAsync(restoreBackupOperation);
                #endregion
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
                #region singleShardSettings_restore_s3-settings
                // Create a dictionary with paths to shard backups
                var restoreSettings = new ShardedRestoreSettings
                {
                    Shards = new Dictionary<int, SingleShardRestoreSetting>(),
                };

                // First shard
                restoreSettings.Shards.Add(0, new SingleShardRestoreSetting
                {
                    ShardNumber = 0,
                    NodeTag = "A",
                    // Backups Folder Name
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$0-A-backup",
                    // Last incremental backup to restore
                    LastFileNameToRestore = "2023-02-12-10-30-00.ravendb-incremental-backup"
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

                var operation = await docStore.Maintenance.Server.SendAsync(restoreBackupOperation);
                #endregion
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
                #region singleShardSettings_restore_azure-settings
                // Create a dictionary with paths to shard backups
                var restoreSettings = new ShardedRestoreSettings
                {
                    Shards = new Dictionary<int, SingleShardRestoreSetting>(),
                };

                // First shard
                restoreSettings.Shards.Add(0, new SingleShardRestoreSetting
                {
                    ShardNumber = 0,
                    NodeTag = "A",
                    // Backups Folder Name
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$0-A-backup",
                    // Last incremental backup to restore
                    LastFileNameToRestore = "2023-02-12-10-30-00.ravendb-incremental-backup"
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

                var operation = await docStore.Maintenance.Server.SendAsync(restoreBackupOperation);
                #endregion
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
                #region singleShardSettings_restore_google-cloud-settings
                // Create a dictionary with paths to shard backups
                var restoreSettings = new ShardedRestoreSettings
                {
                    Shards = new Dictionary<int, SingleShardRestoreSetting>(),
                };

                // First shard
                restoreSettings.Shards.Add(0, new SingleShardRestoreSetting
                {
                    ShardNumber = 0,
                    NodeTag = "A",
                    // Backups Folder Name
                    FolderName = "RavenBackups/2023-02-12-09-52-27.ravendb-Books$0-A-backup",
                    // Last incremental backup to restore
                    LastFileNameToRestore = "2023-02-12-10-30-00.ravendb-incremental-backup"
                });

                var restoreBackupOperation = new RestoreBackupOperation(new RestoreFromGoogleCloudConfiguration
                {
                    // Database Name
                    DatabaseName = "Books",
                    // Paths to backup files
                    ShardRestoreSettings = restoreSettings,
                    // Google Cloud settings
                    Settings = new GoogleCloudSettings
                    {
                        BucketName = "your bucket name here",
                        RemoteFolderName = "", // Replaced by restoreSettings.Shards.FolderName 
                        GoogleCredentialsJson = "your credentials here"
                    }
                });

                var operation = await docStore.Maintenance.Server.SendAsync(restoreBackupOperation);
                #endregion
            }
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
                    SingleShardRestoreSetting> Shards 
                                            { get; set; }
                }
                #endregion

                #region restore_SingleShardRestoreSetting
                public class SingleShardRestoreSetting
                {
                    // Shard number 
                    public int ShardNumber { get; set; }
                    // Node tag
                    public string NodeTag { get; set; }
                    // Folder name
                    public string FolderName { get; set; }
                    // Restore up to (including) this incremental backup file
                    public string LastFileNameToRestore { get; set; }
                }
                #endregion
            }
        }
    }
}

