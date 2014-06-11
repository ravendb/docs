namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Attachments
{
	using System.IO;
	using System.Linq;

	using Raven.Abstractions.Data;
	using Raven.Abstractions.Indexing;
	using Raven.Client.Document;
	using Raven.Client.Indexes;
	using Raven.Json.Linq;

	public class Delete
	{
		private interface IFoo
		{
			#region delete_1
			void DeleteAttachment(string key, Etag etag);
			#endregion
		}

		public Delete()
		{
			using (var store = new DocumentStore())
			{
				#region delete_2
					store
						.DatabaseCommands
						.DeleteAttachment("albums/holidays/sea.jpg", null);
				#endregion
			}
		}
	}
}