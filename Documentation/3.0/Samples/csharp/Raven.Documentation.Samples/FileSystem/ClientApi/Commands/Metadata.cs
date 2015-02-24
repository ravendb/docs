namespace Raven.Documentation.Samples.FileSystem.ClientApi.Commands
{
	using System.Threading.Tasks;
	using Client.FileSystem;
	using Json.Linq;

	public class Metadata
	{
		interface IFoo
		{
			#region get_metadata_1
			Task<RavenJObject> GetMetadataForAsync(string filename);
			#endregion

			#region update_metadata_1
			Task UpdateMetadataAsync(string filename, RavenJObject metadata);
			#endregion
		}

		public async Task Foo()
		{
			IFilesStore store = null;

			#region get_metadata_2
			var metadata = await store
				.AsyncFilesCommands
				.GetMetadataForAsync("/movies/intro.avi");
			#endregion

			#region update_metadata_2

			await store
				.AsyncFilesCommands
				.UpdateMetadataAsync(
					"/movies/intro.avi",
					new RavenJObject()
					{
						{
							"AllowRead", "None"
						}
					});

			#endregion
		}
	}
}