namespace Raven.Documentation.Samples.FileSystem.Server.Bundles
{
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;
	using Client.FileSystem.Bundles.Versioning;
	using Database.Bundles.Versioning.Data;

	public class Versioning
	{
		public async Task Foo()
		{
			IFilesStore store = null;

			#region versioning_1
			await store
					.AsyncFilesCommands
					.Configuration
					.SetKeyAsync(
						"Raven/Versioning/DefaultConfiguration", 
						new FileVersioningConfiguration
						{
							Id = "Raven/Versioning/DefaultConfiguration", 
							MaxRevisions = 10
						});
			#endregion

			#region versioning_2
			await store
					.AsyncFilesCommands
					.Configuration
					.SetKeyAsync(
						"Raven/Versioning/documents/temp",
						new FileVersioningConfiguration
						{
							Id = "Raven/Versioning/documents/temp",
							Exclude = true
						});
			#endregion

			#region versioning_3
			await store
					.AsyncFilesCommands
					.Configuration
					.SetKeyAsync(
						"Raven/Versioning/documents/temp/drafts",
						new FileVersioningConfiguration
						{
							Id = "Raven/Versioning/documents/temp/drafts",
							Exclude = false,
							MaxRevisions = 5
						});
			#endregion

			#region versioning_4

			using (var session = store.OpenAsyncSession())
			{
				FileHeader[] revisions = await session
											.GetRevisionsForAsync(
												"/documents/temp/drafts/1.txt",
												start: 0, 
												pageSize: 5);
			}

			#endregion
		}
	}
}