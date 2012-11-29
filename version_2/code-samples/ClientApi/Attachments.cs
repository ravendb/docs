namespace RavenCodeSamples.ClientApi
{
	using System.IO;

	using Raven.Json.Linq;

	using System.Collections.Generic;

	public class Attachments : CodeSampleBase
	{
		public void SimpleAttachments()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region retrieving_attachment
				Raven.Abstractions.Data.Attachment attachment = 
					documentStore.DatabaseCommands.GetAttachment("videos/1");

				#endregion

				#region putting_attachment
				Stream data = new MemoryStream(new byte[] { 1, 2, 3 }); // don't forget to load the data from a file or something!
				documentStore.DatabaseCommands.PutAttachment("videos/2", null, data,
				                                             new RavenJObject {{"Description", "Kids play in the garden"}});

				#endregion

				#region deleting_attachment
				documentStore.DatabaseCommands.DeleteAttachment("videos/1", null);

				#endregion

				#region retrieving_attachment_metadata
				Raven.Abstractions.Data.Attachment attachementMetadata = 
					documentStore.DatabaseCommands.HeadAttachment("Description");

				#endregion

				#region retrieving_attachment_headers_with_prefix
				IEnumerable<Raven.Abstractions.Data.Attachment> attachmentsMetadata =
					documentStore.DatabaseCommands.GetAttachmentHeadersStartingWith("videos", 0, 10);

				#endregion

				#region updating_attachment_metadata
				documentStore.DatabaseCommands.UpdateAttachmentMetadata("videos/1", null, new RavenJObject
					                                                                          {
						                                                                          { "Description", "Kids play in the bathroom" }
					                                                                          });

				#endregion
			}
		}
	}
}
