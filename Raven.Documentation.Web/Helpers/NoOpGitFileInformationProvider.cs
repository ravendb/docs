using Raven.Documentation.Parser;

namespace Raven.Documentation.Web.Helpers
{
	public class NoOpGitFileInformationProvider : IProvideGitFileInformation
	{
		public string GetLastCommitThatAffectedFile(string path)
		{
			return string.Empty;
		}

		public string MakeRelativePathInRepository(string toPath)
		{
			return string.Empty;
		}
	}
}