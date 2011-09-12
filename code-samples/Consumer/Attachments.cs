using System;
using Raven.Json.Linq;

namespace RavenCodeSamples.Consumer
{
	public class Attachments : CodeSampleBase
	{
		public void SimpleAttachments()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region retrieving_attachment
				Raven.Abstractions.Data.Attachment attachment = documentStore.DatabaseCommands.GetAttachment("videos/1");
				#endregion

				#region putting_attachment
				byte[] data = new byte[] {1, 2, 3}; // don't forget to load the data from a file or something!
				documentStore.DatabaseCommands.PutAttachment("videos/2", null, data,
				                                             new RavenJObject {{"Description", "Kids play in the garden"}});
				#endregion

				#region deleting_attachment
				documentStore.DatabaseCommands.DeleteAttachment("videos/1", null);
				#endregion
			}
		}
	}
}
