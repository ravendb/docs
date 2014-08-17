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
			var result = new PageLinksValidationResult(page.Key, page.Language, page.Version);

			var htmlDocument = HtmlHelper.ParseHtml(page.HtmlContent);
			var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
			if (links == null)
				return result;

			var currentUri = new Uri(_options.RootUrl + page.Key + "/");

			foreach (var link in links)
			{
				var hrefAttribute = link.Attributes["href"];
				var href = hrefAttribute.Value.Trim();

				try
				{
					var isValid = false;
					Uri uri = null;
					if (string.IsNullOrEmpty(href) == false)
					{
						uri = href.StartsWith("http") ? new Uri(href, UriKind.Absolute) : new Uri(currentUri + href, UriKind.Absolute);
						isValid = ValidatePageLink(uri, pages);
					}

					result.Links[string.Format("[{0}][{1}]", link.InnerText, uri)] = isValid;
				}
				catch (Exception)
				{
					result.Links[href] = false;
				}
			}

			return result;
		}

		private bool ValidatePageLink(Uri uri, IEnumerable<DocumentationPage> pages)
		{
			try
			{
				var url = uri.AbsoluteUri;

				var response = _client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url)).Result;
				return response.IsSuccessStatusCode;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}