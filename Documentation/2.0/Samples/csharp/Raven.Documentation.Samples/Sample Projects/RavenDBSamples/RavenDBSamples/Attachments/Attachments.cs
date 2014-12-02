using System.IO;
using Raven.Json.Linq;
using RavenDBSamples.BaseForSamples;

namespace RavenDBSamples.Attachments
{
	public class Attachments : SampleBase
	{
		public void GetAttachment()
		{
			var attachment = DocumentStore.DatabaseCommands.GetAttachment("videos/1");
		}

		public void PutAttachment()
		{
			var data = new MemoryStream(new byte[] { 1, 2, 3 }); // don't forget to load the data from a file or something!
			DocumentStore.DatabaseCommands.PutAttachment("videos/2", null, data,
			                                             new RavenJObject {{"Description", "Kids play in the garden"}});
		}

		public void DeleteAttachment()
		{
			DocumentStore.DatabaseCommands.DeleteAttachment("videos/1", null);
		}

		public void GetAttachmentMetadata()
		{
			var attachmentMetadata1 = DocumentStore.DatabaseCommands.HeadAttachment("videos/1");
		}

		public void GetAttachmentsMetadata()
		{
			var attachmentsMetadata = DocumentStore.DatabaseCommands.GetAttachmentHeadersStartingWith("video", start: 0,
			                                                                                          pageSize: 10);
		}

		public void UpdateMetadata()
		{
			DocumentStore
				.DatabaseCommands
				.UpdateAttachmentMetadata("videos/1", null, new RavenJObject
					{
						{"Description", "Kids play in the bathroom"}
					});
		}
	}
}
