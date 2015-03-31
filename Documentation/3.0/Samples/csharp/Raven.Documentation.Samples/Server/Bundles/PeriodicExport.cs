using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.Server.Bundles
{
	public class PeriodicExport
	{
		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region periodic_backups_1
				store
					.DatabaseCommands
					.GlobalAdmin
					.CreateDatabase(
						new DatabaseDocument
							{
								Id = "Northwind", 
								Settings =
									{
										{ "Raven/ActiveBundles", "PeriodicExport" }
									}
							});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region periodic_backups_2
				store
					.DatabaseCommands
					.GlobalAdmin
					.CreateDatabase(
						new DatabaseDocument
							{
								Id = "Northwind",
								Settings =
									{
										{ "Raven/ActiveBundles", "PeriodicExport" }, 
										{ "Raven/AWSAccessKey", "<key_here>" }
									},
								SecuredSettings =
									{
										{ "Raven/AWSSecretKey", "<key_here>" }
									}
							});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region periodic_backups_3
				store
					.DatabaseCommands
					.Put(
						"Raven/Backup/Periodic/Setup",
						null,
						RavenJObject.FromObject(
							new PeriodicExportSetup
								{
									AwsRegionEndpoint = "eu-west-1", // if not specified default is 'us-east-1'
									GlacierVaultName = "your_glacier_vault_name",
									IntervalMilliseconds = 60 * 60 * 1000, // 60 minutes
									S3BucketName = "your_s3_bucket_name",
									S3RemoteFolderName = "your_s3_remote_folder_name" // if not specified, then root folder will be assumed
								}),
						new RavenJObject());
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region periodic_backups_4
				store
					.DatabaseCommands
					.Put(
						"Raven/Backup/Periodic/Setup",
						null,
						RavenJObject.FromObject(
							new PeriodicExportSetup
							{
								LocalFolderName = @"full_path_to_backup_folder"
							}),
						new RavenJObject());
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region periodic_backups_5
				store
					.DatabaseCommands
					.GlobalAdmin
					.CreateDatabase(
						new DatabaseDocument
						{
							Id = "Northwind",
							Settings =
									{
										{ "Raven/ActiveBundles", "PeriodicExport" }, 
										{ "Raven/AzureStorageAccount", "<name_here>" }
									},
							SecuredSettings =
								{
									{ "Raven/AzureStorageKey", "<key_here>" }
								}
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region periodic_backups_6
				store
					.DatabaseCommands
					.Put(
						"Raven/Backup/Periodic/Setup",
						null,
						RavenJObject.FromObject(
							new PeriodicExportSetup
							{
								AzureStorageContainer = "your_container_name",
								AzureRemoteFolderName = "your_azure_remote_folder_name", // if not specified, then root folder will be assumed
								IntervalMilliseconds = 60 * 60 * 1000, // 60 minutes
							}),
						new RavenJObject());
				#endregion
			}
		}
	}
}