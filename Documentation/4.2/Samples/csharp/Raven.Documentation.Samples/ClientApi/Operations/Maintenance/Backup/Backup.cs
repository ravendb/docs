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

                    #region use_database_encryption_key
                    BackupEncryptionSettings = new BackupEncryptionSettings
                    {
                        //Use the same encryption key as the database
                        EncryptionMode = EncryptionMode.UseDatabaseKey
                    }
                    #endregion

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
                #region use_database_encryption_key_full_sample
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
                        //Use the same encryption key as the database
                        EncryptionMode = EncryptionMode.UseDatabaseKey
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
                        //Backup files local path
                        FolderPath = @"C:\Users\John\backups"
                    },

                    //Full Backup period (Cron expression for a 3-hours period)
                    FullBackupFrequency = "0 */3 * * *",

                    //Type can be Backup or Snapshot
                    BackupType = BackupType.Backup,

                    //Task Name
                    Name = "fullBackupTask",

                    #region use_provided_encryption_key
                    BackupEncryptionSettings = new BackupEncryptionSettings
                    {
                        //Use an encryption key of your choice
                        Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs=",
                        EncryptionMode = EncryptionMode.UseProvidedKey
                    }
                    #endregion

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
                #region use_provided_encryption_key_full_sample
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
                        //Use an encryption key of your choice
                        Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs=",
                        EncryptionMode = EncryptionMode.UseProvidedKey
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
                #region demonstrate_BackupEncryptionSettings_for_backup
                var config = new PeriodicBackupConfiguration
                {
                    //Insert other backup settings here
                    //..

                    BackupEncryptionSettings = new BackupEncryptionSettings
                    {
                        //Use an encryption key of your choice
                        Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs=",
                        EncryptionMode = EncryptionMode.UseProvidedKey
                    }
                };
                var operation = new UpdatePeriodicBackupOperation(config);
                var result = await docStore.Maintenance.SendAsync(operation);
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

                var restoreConfiguration = new RestoreBackupConfiguration();

                restoreConfiguration.BackupEncryptionSettings = new BackupEncryptionSettings
                {
                    Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs="
                };

                //New database name
                restoreConfiguration.DatabaseName = "newEncryptedDatabase";

                //Backup-file location
                var backupPath = @"C:\Users\John\2019-01-06-11-11.ravendb-encryptedDatabase-A-snapshot";
                restoreConfiguration.BackupLocation = backupPath;

                //Use the same encryption key used to encrypt the database
                restoreConfiguration.EncryptionKey = "1F0K2R/KkcwbkK7n4kYlv5eqisy/pMnSuJvZ2sJ/EKo=";

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);
                #endregion
            }

            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.johndom.development.run" },
                Database = "encryptedDatabase",
                Certificate = cert
            }.Initialize())
            {

                #region demonstrate_BackupEncryptionSettings_for_restore
                var restoreConfiguration = new RestoreBackupConfiguration();

                restoreConfiguration.BackupEncryptionSettings = new BackupEncryptionSettings
                {
                    Key = "OI7Vll7DroXdUORtc6Uo64wdAk1W0Db9ExXXgcg5IUs="
                };

                //New database name
                restoreConfiguration.DatabaseName = "newEncryptedDatabase";

                //Backup-file location
                var backupPath = @"C:\Users\John\2019-01-06-11-11.ravendb-encryptedDatabase-A-snapshot";
                restoreConfiguration.BackupLocation = backupPath;

                //Use the same encryption key used to encrypt the database
                restoreConfiguration.EncryptionKey = "1F0K2R/KkcwbkK7n4kYlv5eqisy/pMnSuJvZ2sJ/EKo=";

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);
                #endregion
            }


            // using dedicated encryption key for backup
            // path to the authentication key you received during the server setup
            var cert = new X509Certificate2(@"C:\Users\John\authentication_key\admin.client.certificate.johndom.pfx");
            using (var docStore = new DocumentStore
            {
                Urls = new[] { "https://a.johndom.development.run" },
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
                var backupPath = @"C:\Users\John\2019-01-06-11-11.ravendb-encryptedDatabase-A-snapshot";
                restoreConfiguration.BackupLocation = backupPath;

                var restoreBackupTask = new RestoreBackupOperation(restoreConfiguration);
                docStore.Maintenance.Server.Send(restoreBackupTask);
                #endregion
            }


            public class Foo
            {
                public class bbb
                {
                    public class xxx
                    {
                        public string ttt { get; set; }
                    }

                    public class yyy
                    {
                        public string ttt { get; set; }
                    }
                }
            }
        }
    }
}
