using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
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
                // Backup & Restore procedures here
            }

            #endregion

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.RavenDBdom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {
                var config = new PeriodicBackupConfiguration
                {
                    LocalSettings = new LocalSettings
                    {
                        //Backup files local path
                        FolderPath = @"E:\RavenBackups"
                    },

                    //Full Backup period (Cron expression for a 3-hours period)
                    FullBackupFrequency = "0 */3 * * *",

                    //Type can be Backup or Snapshot
                    BackupType = BackupType.Backup,

                    //Task Name
                    Name = "fullBackupTask",

                    BackupEncryptionSettings = new BackupEncryptionSettings
                    {
                        #region use_database_encryption_key

                        //Use the same encryption key as the database
                        EncryptionMode = EncryptionMode.UseDatabaseKey

                        #endregion
                    }

                };
                var operation = new UpdatePeriodicBackupOperation(config);
                var result = await docStore.Maintenance.SendAsync(operation);
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.RavenDBdom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {
                #region use_database_encryption_key_full_sample

                var config = new PeriodicBackupConfiguration
                {
                    //additional settings here..
                    //..

                    //This is a logical-backup
                    BackupType = BackupType.Backup,

                    BackupEncryptionSettings = new BackupEncryptionSettings
                    {
                        //Use the same encryption key as the database
                        EncryptionMode = EncryptionMode.UseDatabaseKey
                    }
                };
                var operation = new UpdatePeriodicBackupOperation(config);
                var result = await docStore.Maintenance.SendAsync(operation);

                #endregion
            }

            using (var docStore = new DocumentStore())
            {
                var config = new PeriodicBackupConfiguration
                {
                    LocalSettings = new LocalSettings
                    {
                        //Backup files local path
                        FolderPath = @"E:\RavenBackups"
                    },

                    //Full Backup period (Cron expression for a 3-hours period)
                    FullBackupFrequency = "0 */3 * * *",

                    //Type can be Backup or Snapshot
                    BackupType = BackupType.Backup,

                    //Task Name
                    Name = "fullBackupTask",

                    BackupEncryptionSettings = new BackupEncryptionSettings
                    {
                        #region use_provided_encryption_key

                        //Use an encryption key of your choice
                        EncryptionMode = EncryptionMode.UseProvidedKey,
                        Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs="

                        #endregion
                    }
                };

                var operation = new UpdatePeriodicBackupOperation(config);
                var result = await docStore.Maintenance.SendAsync(operation);
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.RavenDBdom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {
                var config = new PeriodicBackupConfiguration
                {

                    // additional settings here..
                    //..

                    #region use_provided_encryption_key_full_sample

                    BackupEncryptionSettings = new BackupEncryptionSettings
                    {
                        //Use an encryption key of your choice
                        EncryptionMode = EncryptionMode.UseProvidedKey,
                        Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs="
                    }

                    #endregion
                };
                var operation = new UpdatePeriodicBackupOperation(config);
                var result = await docStore.Maintenance.SendAsync(operation);
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.RavenDBdom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {
                var config = new PeriodicBackupConfiguration
                {

                    // additional settings here..
                    //..

                    #region explicitly_choose_no_encryption

                    BackupEncryptionSettings = new BackupEncryptionSettings
                    {
                        //No encryption
                        EncryptionMode = EncryptionMode.None
                    }

                    #endregion
                };
                var operation = new UpdatePeriodicBackupOperation(config);
                var result = await docStore.Maintenance.SendAsync(operation);
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.RavenDBdom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {
                #region restore_encrypted_backup

                // restore encrypted database

                var restoreConfiguration = new RestoreBackupConfiguration();

                //New database name
                restoreConfiguration.DatabaseName = "newEncryptedDatabase";

                //Backup-file location
                var backupPath = @"C:\Users\RavenDB\2019-01-06-11-11.ravendb-encryptedDatabase-A-snapshot";
                restoreConfiguration.BackupLocation = backupPath;

                restoreConfiguration.BackupEncryptionSettings = new BackupEncryptionSettings
                {
                    Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs="
                };

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);

                #endregion
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.RavenDBdom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {
                // restore encrypted database

                var restoreConfiguration = new RestoreBackupConfiguration();

                //New database name
                restoreConfiguration.DatabaseName = "newEncryptedDatabase";

                //Backup-file location
                var backupPath = @"C:\Users\RavenDB\2019-01-06-11-11.ravendb-encryptedDatabase-A-snapshot";
                restoreConfiguration.BackupLocation = backupPath;

                #region restore_encrypted_database

                //Restore the database using the key you encrypted it with
                restoreConfiguration.BackupEncryptionSettings = new BackupEncryptionSettings
                {
                    Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs="
                };

                //Encrypt the restored database using this key
                restoreConfiguration.EncryptionKey = "1F0K2R/KkcwbkK7n4kYlv5eqisy/pMnSuJvZ2sJ/EKo=";

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);

                #endregion
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.RavenDBdom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {

                var restoreConfiguration = new RestoreBackupConfiguration();

                //New database name
                restoreConfiguration.DatabaseName = "newEncryptedDatabase";

                //Backup-file location
                var backupPath = @"C:\Users\RavenDB\2019-01-06-11-11.ravendb-encryptedDatabase-A-snapshot";
                restoreConfiguration.BackupLocation = backupPath;

                #region restore_unencrypted_database

                restoreConfiguration.BackupEncryptionSettings = new BackupEncryptionSettings
                {
                    //No encryption
                    EncryptionMode = EncryptionMode.None
                };

                #endregion

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.RavenDBdom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {

                // restore encrypted database

                // restore configuration
                var restoreConfiguration = new RestoreBackupConfiguration();

                //New database name
                restoreConfiguration.DatabaseName = "newEncryptedDatabase";

                // restoreConfiguration.EncryptionKey = "1F0K2R/KkcwbkK7n4kYlv5eqisy/pMnSuJvZ2sJ/EKo=";

                #region encrypting_logical_backup_with_new_key

                //Restore using your own key
                restoreConfiguration.BackupEncryptionSettings = new BackupEncryptionSettings
                {
                    Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs="
                };

                #endregion

                #region restore_encrypting_logical_backup_with_database_key

                //Restore using the DB-encryption key
                restoreConfiguration.EncryptionKey = "1F0K2R/KkcwbkK7n4kYlv5eqisy/pMnSuJvZ2sJ/EKo=";

                //Backup-file location
                var backupPath = @"C:\Users\RavenDB\2019-01-06-11-11.ravendb-encryptedDatabase-A-snapshot";
                restoreConfiguration.BackupLocation = backupPath;

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);

                #endregion
            }
        }

        public class Foo
        {
            #region BackupEncryptionSettings_definition
            public class BackupEncryptionSettings
            {
                public EncryptionMode EncryptionMode { get; set; }
                public string Key { get; set; }

                public BackupEncryptionSettings()
                {
                    Key = null;
                    EncryptionMode = EncryptionMode.None;
                }
            }
            #endregion

            #region EncryptionMode_definition
            public enum EncryptionMode
            {
                None,
                UseDatabaseKey,
                UseProvidedKey
            }
            #endregion
        }
    }
}
