namespace Raven.Documentation.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Http;

	using Raven.Documentation.Parser.Data;
	using Raven.Documentation.Parser.Helpers;

	public class DocumentationValidator
	{
		private readonly ParserOptions _options;

		private readonly HttpClient _client;

		public DocumentationValidator(ParserOptions options)
		{
			_client = new HttpClient();
			_options = options;
		}

		public IEnumerable<PageLinksValidationResult> ValidateLinks(IList<DocumentationPage> pages)
		{
			return pages.Select(page => ValidatePageLinks(page, pages));
		}

		private PageLinksValidationResult ValidatePageLinks(DocumentationPage page, IList<DocumentationPage> pages)
		{
			var result = new PageLinksValidationResult(page.Key);

			var htmlDocument = HtmlHelper.ParseHtml(page.HtmlContent);
			var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
			if (links == null)
				return result;

			var currentUri = new Uri(_options.RootUrl + page.Key + "/");

			foreach (var link in links)
			{
				var hrefAttribute = link.Attributes["href"];
				var href = hrefAttribute.Value.Trim();
				var isValid = false;
				Uri uri = null;
				if (string.IsNullOrEmpty(href) == false)
				{
					uri = href.StartsWith("http") ? new Uri(href, UriKind.Absolute) : new Uri(currentUri + href, UriKind.Absolute);
					isValid = ValidatePageLink(uri, pages);
				}

				result.Links[string.Format("[{0}][{1}]", link.InnerText, uri)] = isValid;
			}

			return result;
		}

		private bool ValidatePageLink(Uri uri, IEnumerable<DocumentationPage> pages)
		{
			var url = uri.AbsoluteUri;
			//if (url.StartsWith(_options.RootUrl, StringComparison.OrdinalIgnoreCase))
			//{
			//	var key = url.Substring(_options.RootUrl.Length);
			//	var hashIndex = key.IndexOf("#", StringComparison.OrdinalIgnoreCase);
			//	if (hashIndex != -1)
			//		key = key

			//	return pages.Any(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
			//}

			var response = _client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url)).Result;
			return response.IsSuccessStatusCode;
		}
	}
}