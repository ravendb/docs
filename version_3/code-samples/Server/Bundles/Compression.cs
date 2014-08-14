using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.Server.Bundles
{
	public class Compression
	{
		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region compression_1
				store
					.DatabaseCommands
					.GlobalAdmin
					.CreateDatabase(
						new DatabaseDocument
							{
								Id = "CompressedDB",
								// Other configuration options omitted for simplicity
								Settings =
									{
										// ...
										{ "Raven/ActiveBundles", "Compression" }
									}
							});

				#endregion
			}
		}
	}
}