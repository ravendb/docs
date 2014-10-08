using System;
using System.Collections.Generic;
using System.IO;

using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser
{
	public class ParserOptions
	{
		private string _pathToDocumentationDirectory;

		public ParserOptions()
		{
			PathToDocumentationDirectory = "..\\..\\..\\Documentation";
			RootUrl = "";
			VersionsToParse = new List<string>();
		}

		public string PathToDocumentationDirectory
		{
			get
			{
				return _pathToDocumentationDirectory;
			}
			set
			{
				_pathToDocumentationDirectory = Path.GetFullPath(value);
			}
		}

		public string GetPathToDocumentationPagesDirectory(string documentationVersion)
		{
			return Path.Combine(PathToDocumentationDirectory, documentationVersion, "Raven.Documentation.Pages");
		}

		public string GetPathToDocumentationSamplesDirectory(Language language, string documentationVersion)
		{
			switch (language)
			{
				case Language.Csharp:
					return Path.Combine(PathToDocumentationDirectory, documentationVersion, "Samples", "csharp", "Raven.Documentation.Samples");
				case Language.Java:
					return Path.Combine(PathToDocumentationDirectory, documentationVersion, "Samples", "java", "src", "test", "java", "net", "ravendb");
				default:
					throw new NotSupportedException(language.ToString());
			}
		}

		public string RootUrl { get; set; }

		public string ImagesUrl { get; set; }

		public List<string> VersionsToParse { get; set; }
	}
}