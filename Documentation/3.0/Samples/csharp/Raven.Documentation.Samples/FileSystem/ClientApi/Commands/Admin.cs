namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using System.Threading.Tasks;
	using Abstractions.Data;
	using Abstractions.FileSystem;
	using Client.FileSystem;

	public class Admin
	{
		private interface IFoo
		{
			#region create_fs_1
			Task CreateFileSystemAsync(FileSystemDocument filesystemDocument, string newFileSystemName = null);
			#endregion

			#region create_or_update_fs_1
			Task CreateOrUpdateFileSystemAsync(FileSystemDocument filesystemDocument, string newFileSystemName = null);
			#endregion

			#region ensure_fs_exists_1
			Task EnsureFileSystemExistsAsync(string fileSystem);
			#endregion

			#region delete_fs_1
			Task DeleteFileSystemAsync(string fileSystemName = null, bool hardDelete = false);
			#endregion
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			#region create_fs_2

			await store.AsyncFilesCommands.Admin
				.CreateFileSystemAsync(new FileSystemDocument
				{
					Id = "Raven/FileSystems/NorthwindFS",
					Settings =
					{
						{ Constants.FileSystem.DataDirectory, "~/FileSystems/NorthwindFS" },
						{ Constants.ActiveBundles, "Versioning" }
					}
				}, "NorthwindFS");

			#endregion

			#region ensure_fs_exists_2

			await store.AsyncFilesCommands.Admin
				.EnsureFileSystemExistsAsync("NorthwindFS");

			#endregion

			#region delete_fs_2

			await store.AsyncFilesCommands.Admin
				.DeleteFileSystemAsync(hardDelete: true);

			#endregion
		}
	}
}