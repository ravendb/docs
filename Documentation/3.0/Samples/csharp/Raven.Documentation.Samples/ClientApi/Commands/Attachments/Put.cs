namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Attachments
{
	using System.IO;
	using System.Linq;

	using Raven.Abstractions.Data;
	using Raven.Abstractions.Indexing;
	using Raven.Client.Document;
	using Raven.Client.Indexes;
	using Raven.Json.Linq;

	public class Put
	{
		private interface IFoo
		{
			#region put_1
			void PutAttachment(string key, Etag etag, Stream data, RavenJObject metadata);
			#endregion
		}

		public Put()
		{
			using (var store = new DocumentStore())
			{
				#region put_2
				using (var file = File.Open("sea.png", FileMode.Open))
				{
					store
						.DatabaseCommands
						.PutAttachment("albums/holidays/sea.jpg", null, file, new RavenJObject
							                                           {
								                                           { "Description", "Holidays 2014" }
							                                           });
				}
				#endregion
			}
		}
	}
}