using System.IO;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Session.Attachments
{
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
				Attachment attachment = store
					.DatabaseCommands
					.GetAttachment("albums/holidays/sea.jpg"); // null if does not exist

				Stream data = attachment.Data();
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region get_2_1
				AttachmentInformation[] attachments = store
					.DatabaseCommands
					.GetAttachments(start: 0, startEtag: Etag.Empty, pageSize: 10);
				#endregion
			}
		}
	}
}
