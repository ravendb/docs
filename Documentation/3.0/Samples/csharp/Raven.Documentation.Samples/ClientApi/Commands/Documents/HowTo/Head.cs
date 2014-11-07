using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents.HowTo
{
	public class Head
	{
		private interface IFoo
		{
			#region head_1
			JsonDocumentMetadata Head(string key);
			#endregion
		}

		public Head()
		{
			using (var store = new DocumentStore())
			{
				#region head_2
				var metadata = store.DatabaseCommands.Head("employees/1"); // null if does not exist
				#endregion
			}
		}
	}
}