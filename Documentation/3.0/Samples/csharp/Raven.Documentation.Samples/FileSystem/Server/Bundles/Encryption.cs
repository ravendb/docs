namespace Raven.Documentation.Samples.FileSystem.Server.Bundles
{
	using Abstractions.FileSystem;
	using Client.FileSystem;

	public class Encryption
	{
		public Encryption()
		{

			using (var store = new FilesStore())
			{
				#region encryption_1
				store
					.AsyncFilesCommands
					.Admin
					.CreateFileSystemAsync(
						new FileSystemDocument
						{
							Id = "EncryptedFS",
							// Other configuration options omitted for simplicity
							Settings =
							{
								// ...
								{
									"Raven/ActiveBundles", "Encryption"
								},
								{
									"Raven/Encryption/Algorithm",
									"System.Security.Cryptography.DESCryptoServiceProvider, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
								}
							},
							SecuredSettings =
							{
								// ...
								{
									"Raven/Encryption/Key", "<key_here>"
								}
							}
						});

				#endregion
			}
		}
	}
}