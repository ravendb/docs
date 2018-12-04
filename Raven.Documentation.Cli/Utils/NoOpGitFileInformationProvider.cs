using Raven.Documentation.Parser;

namespace Raven.Documentation.Cli.Utils
{
    internal class NoOpGitFileInformationProvider : IProvideGitFileInformation
    {
        public string GetLastCommitThatAffectedFile(string path) => string.Empty;

        public string MakeRelativePathInRepository(string toPath) => string.Empty;
    }
}
