using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Documentation.Parser
{
	public interface IProvideLastCommitThatAffectedFile
	{
		string GetLastCommitThatAffectedFile(string path);
	}
}
