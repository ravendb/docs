using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.HowTo
{
	public class FullUrl
	{
		private interface IFoo
		{
			#region full_url_1
			string UrlFor(string documentKey);
			#endregion
		}

		public FullUrl()
		{
			using (var store = new DocumentStore())
			{
				#region full_url_2
				// http://localhost:8080/databases/DB1/docs/people/1
				var url = store.DatabaseCommands.UrlFor("people/1");
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region full_url_3
				// http://localhost:8080/docs/people/1
				var url = store.DatabaseCommands.ForSystemDatabase().UrlFor("people/1");
				#endregion
			}
		}
	}
}