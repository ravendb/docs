using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Raven.Documentation.Parser
{
	using System;
	using System.Collections.Generic;
	using System.Net.Http;

	using Raven.Documentation.Parser.Data;
	using Raven.Documentation.Parser.Helpers;

	public class DocumentationValidator
	{
		private readonly ParserOptions _options;

		public DocumentationValidator(ParserOptions options)
		{
			_options = options;
		}

		public IEnumerable<PageLinksValidationResult> ValidateLinks(IList<DocumentationPage> pages)
		{
			var results = new ConcurrentBag<PageLinksValidationResult>();

			var p = new List<DocumentationPage>[2];

			var half = pages.Count / 2;
			p[0] = pages.Take(half).ToList();
			p[1] = pages.Skip(half).ToList();

			Parallel.For(
				0,
				2,
				i =>
				{
					using (var client = new HttpClient())
					{
						var pagesToCheck = p[i];
						foreach (var page in pagesToCheck)
							results.Add(ValidatePageLinks(client, page, pages));
					}
				});

			return results;
		}

		private PageLinksValidationResult ValidatePageLinks(HttpClient client, DocumentationPage page, IList<DocumentationPage> pages)
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
						isValid = ValidatePageLink(client, uri, pages);
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

		private static bool ValidatePageLink(HttpClient client, Uri uri, IEnumerable<DocumentationPage> pages)
		{
			try
			{
				var url = uri.AbsoluteUri;

				var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url)).Result;
				return response.IsSuccessStatusCode;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}