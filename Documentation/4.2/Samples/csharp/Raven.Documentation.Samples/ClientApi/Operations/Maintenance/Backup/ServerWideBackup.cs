using System.Threading.Tasks;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.ServerWide.Operations.Configuration;
using Raven.TestDriver;

namespace SlowTests.Server.Documents.PeriodicBackup
{
    public class ServerWideBackup : RavenTestDriver
    {
        public async Task CanStoreServerWideBackup()
        {
            using (var store = GetDocumentStore())
            {
                #region server_wide_backup_configuration
                var putConfiguration = new ServerWideBackupConfiguration
                {
                    Disabled = true,
                    FullBackupFrequency = "0 2 * * 0",
                    IncrementalBackupFrequency = "0 2 * * 1",

                    //Backups are stored in this folder first, and sent from it to remote destinations (if defined).
                    LocalSettings = new LocalSettings
                    {
                        FolderPath = "localFolderPath"
                    },

                    //FTP settings
                    FtpSettings = new FtpSettings
                    {
                        Url = "ftps://localhost/john/backups"
                    },

                    //Microsoft Azure settings.
                    AzureSettings = new AzureSettings
                    {
                        AccountKey = "Azure Account Key",
                        AccountName = "Azure Account Name",
                        RemoteFolderName = "john/backups"
                    },

                    //Amazon S3 bucket settings.
                    S3Settings = new S3Settings
                    {
                        AwsAccessKey = "Amazon S3 Access Key",
                        AwsSecretKey = "Amazon S3 Secret Key",
                        AwsRegionName = "Amazon S3 Region Name",
                        BucketName = "john-bucket",
                        RemoteFolderName = "john/backups"
                    },

                    //Amazon Glacier settings.
                    GlacierSettings = new GlacierSettings
                    {
                        AwsAccessKey = "Amazon Glacier Access Key",
                        AwsSecretKey = "Amazon Glacier Secret Key",
                        AwsRegionName = "Amazon Glacier Region Name",
                        VaultName = "john-glacier",
                        RemoteFolderName = "john/backups"
                    },

                    //Google Cloud Backup settings
                    GoogleCloudSettings = new GoogleCloudSettings
                    {
                        BucketName = "Google Cloud Bucket",
                        RemoteFolderName = "BackupFolder",
                        GoogleCredentialsJson = "GoogleCredentialsJson"
                    }
                };

                var result = await store.Maintenance.Server.SendAsync(new PutServerWideBackupConfigurationOperation(putConfiguration));
                var serverWideConfiguration = await store.Maintenance.Server.SendAsync(new GetServerWideBackupConfigurationOperation(result.Name));
                #endregion 
            }
        }
    }
}
