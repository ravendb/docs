namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using System.Threading.Tasks;
	using Client.FileSystem;

	public class Storage
	{
		private interface IFoo
		{
			#region clean_up_1
			Task CleanUpAsync();
			#endregion

			#region retry_renaming_1
			Task RetryRenamingAsync();
			#endregion
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			#region clean_up_2

			await store.AsyncFilesCommands.Storage
					.CleanUpAsync();
			#endregion

			#region retry_renaming_2

			await store.AsyncFilesCommands.Storage
					.RetryRenamingAsync();
			#endregion
		}
	}
}