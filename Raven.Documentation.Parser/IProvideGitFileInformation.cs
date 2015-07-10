using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Documentation.Parser
{
	public interface IProvideGitFileInformation
	{
		string GetLastCommitThatAffectedFile(string path);

		string MakeRelativePathInRepository(string toPath);
	}
}
