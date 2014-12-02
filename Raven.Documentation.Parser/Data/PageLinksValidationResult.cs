namespace Raven.Documentation.Parser.Data
{
	using System.Collections.Generic;

	public class PageLinksValidationResult
	{
		public string PageKey { get; private set; }

		public Language PageLanguage { get; private set; }

		public string PageVersion { get; private set; }

		public Dictionary<string, bool> Links { get; private set; }

		public PageLinksValidationResult(string key, Language language, string version)
		{
			PageKey = key;
			PageLanguage = language;
			PageVersion = version;
			Links = new Dictionary<string, bool>();
		}
	}
}