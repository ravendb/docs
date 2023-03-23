using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Linq;
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

    class Program1
    {
        static void Main(string[] args)
        {
        }

        static async Task MainInternal()
        {
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
                
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
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
                
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
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
                
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
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
                
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "http://127.0.0.1:8080" },
                Database = "Products"
            }.Initialize())
            {
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
                
            }

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
            
        }
        public class Foo
        {
            public class AddNodeToOrchestratorTopologyOperation
            {
                #region AddNodeToOrchestratorTopologyOperation_Definition
                public AddNodeToOrchestratorTopologyOperation(string databaseName, string node = null)
                #endregion
                {
                }
            }

            public class RemoveNodeFromOrchestratorTopologyOperation
            {
                #region RemoveNodeFromOrchestratorTopologyOperation_Definition
                public RemoveNodeFromOrchestratorTopologyOperation(string databaseName, string node)
                #endregion
                {
                }
            }

            #region ModifyOrchestratorTopologyResult
            public class ModifyOrchestratorTopologyResult
            {
                public string Name; // Database Name
                public OrchestratorTopology OrchestratorTopology; // Database Topology
                public long RaftCommandIndex;
            }
            #endregion

            public class AddDatabaseNodeOperation
            {
                #region AddDatabaseNodeOperation_Definition
                public AddDatabaseNodeOperation(string databaseName, string node = null)
                #endregion
                {
                }
            }

            #region DatabasePutResult
            public class DatabasePutResult
            {
                public long RaftCommandIndex { get; set; }

                public string Name { get; set; }
                public DatabaseTopology Topology { get; set; }
                public List<string> NodesAddedTo { get; set; }

                public bool ShardsDefined { get; set; }
            }
            #endregion

            public class DeleteDatabasesOperation
            {
                #region DeleteDatabasesOperation_Definition
                public DeleteDatabasesOperation(
                    string databaseName, 
                    int shardNumber, 
                    bool hardDelete, 
                    string fromNode, 
                    TimeSpan? timeToWaitForConfirmation = null)
                #endregion
                {
                }
            }

            #region DeleteDatabaseResult
            public class DeleteDatabaseResult
            {
                public long RaftCommandIndex { get; set; }
                public string[] PendingDeletes { get; set; }
            }
            #endregion

            
            internal class AddDatabaseShardOperation
            {
                #region AddDatabaseShardOperation_Overload-1
                public AddDatabaseShardOperation(string databaseName, int? shardNumber = null)
                #endregion
                {
                }

                #region AddDatabaseShardOperation_Overload-2
                public AddDatabaseShardOperation(string databaseName, string[] nodes, int? shardNumber = null)
                #endregion
                {
                }

                #region AddDatabaseShardOperation_Overload-3
                public AddDatabaseShardOperation(string databaseName, int? replicationFactor, int? shardNumber = null)
                #endregion
                {
                }
            }

            #region AddDatabaseShardResult
            public class AddDatabaseShardResult
            {
                public string Name { get; set; }
                public int ShardNumber { get; set; }
                public DatabaseTopology ShardTopology { get; set; }
                public long RaftCommandIndex { get; set; }
            }
            #endregion

            public class PromoteDatabaseNodeOperation
            {
                #region PromoteDatabaseNodeOperation_Definition
                public PromoteDatabaseNodeOperation(string databaseName, int shard, string node)
                #endregion
                {
                }
            }
        }
    }
}

