using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.HowTo
{
	public class BuildNumber
	{
		private interface IFoo
		{
			#region build_number_1
			BuildNumber GetBuildNumber();
			#endregion
		}

		public BuildNumber()
		{
			using (var store = new DocumentStore())
			{
				#region build_number_2
				var buildNumber = store
					.DatabaseCommands
					.GlobalAdmin
					.GetBuildNumber();
				#endregion
			}
		}
	}
}