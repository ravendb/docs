namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session.Querying
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;

	public class Basics
	{
		interface IInterface
		{
			#region query_1
			IAsyncFilesQuery<FileHeader> Query();
			#endregion
		}

		public async Task Foo()
		{
			IAsyncFilesSession session = null;

			#region query_2
			List<FileHeader> files = await session.Query().ToListAsync();
			#endregion

			#region query_3
			FileHeader file = await session
								.Query()
								.WhereLessThan(x => x.TotalSize, 100)
								.FirstOrDefaultAsync();

			#endregion
		}
	}
}