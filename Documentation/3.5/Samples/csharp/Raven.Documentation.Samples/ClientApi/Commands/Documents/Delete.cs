using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents
{
	public class Delete
	{
		private interface IFoo
		{
			#region delete_1
			void Delete(string key, Etag etag);
			#endregion
		}

		public Delete()
		{
			using (var store = new DocumentStore())
			{
				#region delete_2
				store.DatabaseCommands.Delete("employees/1", null);
				#endregion
			}
		}
	}
}