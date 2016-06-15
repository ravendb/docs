namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session
{
	using System.Threading.Tasks;
	using Client.FileSystem;

	public class SavingChanges
	{
		interface IInterface
		{
			#region save_changes_1
			Task SaveChangesAsync();
			#endregion
		}

		public async Task Foo()
		{
			IAsyncFilesSession session = null;

			#region save_changes_2
			await session.SaveChangesAsync();
			#endregion
		}
	}
}