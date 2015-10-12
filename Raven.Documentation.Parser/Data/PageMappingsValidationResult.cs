using System.Collections.Generic;

namespace Raven.Documentation.Parser.Data
{
	public class PageMappingsValidationResult
	{
		public string PageKey { get; private set; }

		public Language PageLanguage { get; private set; }

		public string PageVersion { get; private set; }

		public Dictionary<string, bool> Mappings { get; private set; }

		public PageMappingsValidationResult(string key, Language language, string version)
		{
			PageKey = key;
			PageLanguage = language;
			PageVersion = version;
			Mappings = new Dictionary<string, bool>();
		}
	}
}