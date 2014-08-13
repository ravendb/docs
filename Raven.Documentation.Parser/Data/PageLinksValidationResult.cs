namespace Raven.Documentation.Parser.Data
{
	using System.Collections.Generic;

	public class PageLinksValidationResult
	{
		public string PageKey { get; private set; }

		public Dictionary<string, bool> Links { get; private set; }

		public PageLinksValidationResult(string key)
		{
			PageKey = key;
			Links = new Dictionary<string, bool>();
		}
	}
}