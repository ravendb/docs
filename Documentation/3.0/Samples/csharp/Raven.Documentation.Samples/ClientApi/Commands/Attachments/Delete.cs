using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Attachments
{
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