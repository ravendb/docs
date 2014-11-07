using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Commands.Attachments.HowTo
{
	public class Update
	{
		private interface IFoo
		{
			#region update_1
			void UpdateAttachmentMetadata(string key, Etag etag, RavenJObject metadata);
			#endregion
		}

		public Update()
		{
			using (var store = new DocumentStore())
			{
				#region update_2
				var attachment = store.DatabaseCommands.Head("albums/holidays/sea.jpg");
				var metadata = attachment.Metadata;
				metadata["Description"] = "Holidays 2012";

				store
					.DatabaseCommands
					.UpdateAttachmentMetadata("albums/holidays/sea.jpg", attachment.Etag, metadata);
				#endregion
			}
		}
	}
}