namespace Raven.Documentation.Samples.FileSystem.ClientApi
{
	using System;
	using Client.FileSystem;

	public class CreatingFilesStore
	{
		public CreatingFilesStore()
		{
			#region create_fs_1
			using (IFilesStore fsStore = new FilesStore()
			{
				Url = "http://localhost:8080",
				DefaultFileSystem = "NorthwindFS"
			}.Initialize())
			{

			}
			#endregion
		}
	}

	#region create_fs_2
	public class FilesStoreHolder
	{
		private static readonly Lazy<IFilesStore> store = new Lazy<IFilesStore>(CreateStore);

		public static IFilesStore Store
		{
			get { return store.Value; }
		}

		private static IFilesStore CreateStore()
		{
			IFilesStore fsStore = new FilesStore()
			{
				Url = "http://localhost:8080",
				DefaultFileSystem = "NorthwindFS"
			}.Initialize();

			return fsStore;
		}
	}
	#endregion
}