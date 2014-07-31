using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi
{
	public class SetupConnectionString
	{
		public SetupConnectionString()
		{
			#region connection_string_1
			var store = new DocumentStore
							{
								ConnectionStringName = "MyRavenConnectionStringName"
							};
			#endregion
		}
	}
}