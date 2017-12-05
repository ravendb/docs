using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Attachments
{
	public class Delete
	{
		private interface IFoo
		{
			
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
