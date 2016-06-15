namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session
{
	using System.Threading.Tasks;
	using Abstractions.Data;
	using Abstractions.FileSystem;
	using Client.FileSystem;

	public class DeletingFiles
	{
		interface IInterface
		{
			#region register_delete_1
			void RegisterFileDeletion(string path, Etag etag = null);
			void RegisterFileDeletion(FileHeader file, Etag etag = null);
			#endregion

			#region register_deletion_query_1
			void RegisterDeletionQuery(string query);
			#endregion
		}

		public async Task Foo()
		{
			using (IAsyncFilesSession session = null)
			{
				#region register_delete_2
				session.RegisterFileDeletion("/movies/intro.avi");
				session.RegisterFileDeletion(await session.LoadFileAsync("/txt/1.txt"));

				await session.SaveChangesAsync();
				#endregion
			}

			using (IAsyncFilesSession session = null)
			{
				#region register_deletion_query_2
				session.RegisterDeletionQuery("__rfileName:tfard.*");

				await session.SaveChangesAsync();
				#endregion

				#region register_deletion_query_3
				session.Query()
					.WhereEndsWith(x => x.Name, ".draft")
					.RegisterResultsForDeletion();

				await session.SaveChangesAsync();
				#endregion
			}
		}
	}
}