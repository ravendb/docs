using Raven.Abstractions.Data;
using Raven.Client.Extensions;
using Raven.Json.Linq;

namespace RavenCodeSamples.Server.Bundles
{
    public class PeriodicBackup : CodeSampleBase
    {
        public void Sample()
        {
            using (var store = NewDocumentStore())
            {
                #region periodic_backups_1
                store.DatabaseCommands.CreateDatabase(new DatabaseDocument
                    {
                        Id = "BackupedDB",
                        Settings =
                            {
                                {"Raven/ActiveBundles", "PeriodicBackup"}
                            }
                    });

                #endregion
            }

            using (var store = NewDocumentStore())
            {
                #region periodic_backups_2
                store.DatabaseCommands.CreateDatabase(new DatabaseDocument
                {
                    Id = "BackupedDB",
                    Settings =
                            {
                                {"Raven/ActiveBundles", "PeriodicBackup"},
                                {"Raven/AWSAccessKey", "<key_here>"}
                            },
                    SecuredSettings =
                                {
                                    {"Raven/AWSSecretKey", "<key_here>"}
                                }
                });

                #endregion
            }



            using (var store = NewDocumentStore())
            {
                #region periodic_backups_3
                store.DatabaseCommands.Put("Raven/Backup/Periodic/Setup",
                                           null,
                                           RavenJObject.FromObject(new PeriodicBackupSetup
                                               {
                                                   AwsRegionEndpoint = "eu-west-1", // if not specified default is 'us-east-1'
                                                   GlacierVaultName = "your_glacier_vault_name",
												   IntervalMilliseconds = 60 * 1000,
                                                   S3BucketName = "your_s3_bucket_name"
                                               }),
                                           new RavenJObject());

                #endregion
            }
        }
    }
}