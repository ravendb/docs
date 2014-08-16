namespace Raven.Documentation.CodeSamples.ClientApi.Commands.Attachments
{
	using System.Collections.Generic;

	using Raven.Abstractions.Data;
	using Raven.Client;
	using Raven.Client.Document;
	using Raven.Database.Data;
	using Raven.Json.Linq;

	public class Get
	{
		private interface IFoo
		{
			#region get_1_0
			Attachment GetAttachment(string key);
			#endregion

			#region get_2_0
			AttachmentInformation[] GetAttachments(int start, Etag startEtag, int pageSize);
			#endregion
		}

		public Get()
		{
			using (var store = new DocumentStore())
			{
				#region get_1_1
				var attachment = store
					.DatabaseCommands
					.GetAttachment("albums/holidays/sea.jpg"); // null if does not exist

				var data = attachment.Data();
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region get_2_1
				var attachments = store
					.DatabaseCommands
					.GetAttachments(start: 0, startEtag: Etag.Empty, pageSize: 10);
				#endregion
			}
		}
	}
}