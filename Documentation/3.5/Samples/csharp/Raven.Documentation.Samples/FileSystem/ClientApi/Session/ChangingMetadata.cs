namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session
{
	using System;
	using System.Threading.Tasks;
	using Abstractions.FileSystem;
	using Client.FileSystem;

	public class ChangingMetadata
	{
		IAsyncFilesSession session = null;

		public async Task Foo()
		{
			#region update_metadata_1

			FileHeader file = await session.LoadFileAsync("/movies/intro.avi");

			file.Metadata["Copyright"] = "2015 Hibernating Rhinos. All rights reserved."; // modification of existing metadata
			file.Metadata.Add("Duration", TimeSpan.FromMinutes(22).ToString()); // adding a new one

			await session.SaveChangesAsync();
			#endregion
		}
	}
}