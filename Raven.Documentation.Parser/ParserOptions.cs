namespace Raven.Documentation.Parser
{
	using System;
	using System.Globalization;
	using System.IO;

	using Raven.Documentation.Parser.Data;

	public class ParserOptions
	{
		private string _pathToDocumentationDirectory;

		public ParserOptions()
		{
			PathToDocumentationDirectory = "..\\..\\..\\Documentation";
			RootUrl = "";
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

		public string GetPathToDocumentationPagesDirectory(double documentationVersion)
		{
			return Path.Combine(PathToDocumentationDirectory, documentationVersion.ToString("#.0", CultureInfo.InvariantCulture), "Raven.Documentation.Pages");
		}

		public string GetPathToDocumentationSamplesDirectory(Language language, double documentationVersion)
		{
			switch (language)
			{
				case Language.Csharp:
					return Path.Combine(PathToDocumentationDirectory, documentationVersion.ToString("#.0", CultureInfo.InvariantCulture), "Samples", "csharp", "Raven.Documentation.Samples");
				case Language.Java:
					return Path.Combine(PathToDocumentationDirectory, documentationVersion.ToString("#.0", CultureInfo.InvariantCulture), "Samples", "java", "Raven.Documentation.Samples");
				default:
					throw new NotSupportedException(language.ToString());
			}
		}

		public string RootUrl { get; set; }

		public string ImagesUrl { get; set; }
	}
}