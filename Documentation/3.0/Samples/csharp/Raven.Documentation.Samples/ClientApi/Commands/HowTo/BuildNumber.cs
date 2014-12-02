using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
	public class BuildNumber2
	{
		private interface IFoo
		{
			#region build_number_1
			BuildNumber GetBuildNumber();
			#endregion
		}

		public BuildNumber2()
		{
			using (var store = new DocumentStore())
			{
				#region build_number_2
				BuildNumber buildNumber = store
					.DatabaseCommands
					.GlobalAdmin
					.GetBuildNumber();
				#endregion
			}
		}
	}
}