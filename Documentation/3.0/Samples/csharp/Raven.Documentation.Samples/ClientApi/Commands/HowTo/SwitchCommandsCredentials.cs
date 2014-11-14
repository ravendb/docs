using System.Net;

using Raven.Client.Connection;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
	public class SwitchCommandsCredentials
	{
		private interface IFoo
		{
			#region with_1
			IDatabaseCommands With(ICredentials credentialsForSession);
			#endregion
		}

		public SwitchCommandsCredentials()
		{
			using (var store = new DocumentStore())
			{
				#region with_2
				IDatabaseCommands commands = store
					.DatabaseCommands
					.With(new NetworkCredential("otherUserName", "otherPassword"));
				#endregion
			}
		}
	}
}