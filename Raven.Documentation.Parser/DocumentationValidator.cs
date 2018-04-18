using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser
{
    public class DocumentationValidator
    {
        private readonly ParserOptions _options;

        private readonly Language _currentLanguage;

        public DocumentationValidator(ParserOptions options, Language currentLanguage)
        {
            _options = options;
            _currentLanguage = currentLanguage;
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
                            results.Add(ValidatePageLinks(client, page));
                    }
                });

            return results;
        }

        private PageLinksValidationResult ValidatePageLinks(HttpClient client, DocumentationPage page)
        {
            void ValidateLinks(HttpClient httpClient, List<HtmlNode> toValidate, Uri baseUri, PageLinksValidationResult results)
            {
                foreach (var link in toValidate)
                {
                    var hrefAttribute = link.Attributes["href"];
                    var href = hrefAttribute.Value.Trim();

                    try
                    {
                        var isValid = false;
                        Uri uri = null;
                        if (string.IsNullOrEmpty(href) == false)
                        {
                            uri = href.StartsWith("http")
                                ? new Uri(href, UriKind.Absolute)
                                : new Uri(baseUri + href, UriKind.Absolute);
                            var indexOfHash = uri.AbsoluteUri.IndexOf("#", StringComparison.InvariantCultureIgnoreCase);
                            if (indexOfHash != -1)
                            {
                                var potentialGuid = uri.AbsoluteUri.Substring(indexOfHash + 1);
                                if (Guid.TryParse(potentialGuid, out Guid _))
                                    continue;
                            }

                            isValid = ValidatePageLink(httpClient, uri);
                        }

                        results.Links[$"[{link.InnerText}][{uri}]"] = isValid;
                    }
                    catch (Exception)
                    {
                        results.Links[href] = false;
                    }
                }
            }

            var result = new PageLinksValidationResult(page.Key, page.Language == Language.All ? _currentLanguage : page.Language, page.Version);

            var htmlDocument = HtmlHelper.ParseHtml(page.HtmlContent);
            var linksToValidate = new List<HtmlNode>();

            var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
            if (links != null)
                linksToValidate.AddRange(links);

            if (string.IsNullOrWhiteSpace(page.RelatedArticlesContent) == false)
            {
                htmlDocument = HtmlHelper.ParseHtml(page.RelatedArticlesContent);
                links = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
                if (links != null)
                    linksToValidate.AddRange(links);
            }

            if (linksToValidate.Count == 0)
                return result;

            var currentUri = new Uri(_options.RootUrl + page.Key + "/../");

            ValidateLinks(client, linksToValidate, currentUri, result);

            return result;
        }

        private static bool ValidatePageLink(HttpClient client, Uri uri)
        {
            try
            {
                var url = uri.AbsoluteUri;

                var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url)).Result;
                if (response.IsSuccessStatusCode == false)
                    return false;

                var content = response.Content.ReadAsStringAsync().Result;
                if (content.Contains("The article you are looking for does not exist."))
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static readonly MappingVersion[] MappingVersionsToCheck = {
            new MappingVersion { FloatValue = 1.0f, StringValue = "1.0"},
            new MappingVersion { FloatValue = 2.0f, StringValue = "2.0"},
            new MappingVersion { FloatValue = 2.5f, StringValue = "2.5"},
            new MappingVersion { FloatValue = 3.0f, StringValue = "3.0"},
            new MappingVersion { FloatValue = 3.5f, StringValue = "3.5"}
        };

        public IEnumerable<PageMappingsValidationResult> ValidateMappings(IList<DocumentationPage> pages)
        {
            var results = new ConcurrentBag<PageMappingsValidationResult>();

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
                        {
                            if (page.Key.EndsWith("/index") || page.Key == "index") // legacy index.markdown files
                                continue;

                            var result = new PageMappingsValidationResult(page.Key, page.Language == Language.All ? _currentLanguage : page.Language, page.Version);

                            foreach (var versionToCheck in MappingVersionsToCheck.Where(x => x.StringValue != page.Version))
                            {
                                var explicitMapping = page.Mappings.FirstOrDefault(x => Math.Abs(x.Version - versionToCheck.FloatValue) < float.Epsilon);

                                DocumentationMapping inheritedMapping = null;

                                if (explicitMapping == null)
                                {
                                    if (versionToCheck.FloatValue < float.Parse(page.Version)) // switching from higher to lower version
                                    {
                                        inheritedMapping =
                                            page.Mappings.OrderBy(x => x.Version).LastOrDefault(y => y.Version < versionToCheck.FloatValue);
                                    }
                                    else if (page.Mappings.Any(x => x.Version >= float.Parse(page.Version)))
                                    // when switching from lower to higher version, make sure we don't get too lower mapping
                                    {
                                        inheritedMapping =
                                            page.Mappings.OrderByDescending(x => x.Version).FirstOrDefault(y => y.Version < versionToCheck.FloatValue);
                                    }
                                }

                                Uri uriToCheck;

                                var versionSpecificRootUrl = _options.RootUrl.Replace(page.Version, versionToCheck.StringValue);

                                if (explicitMapping != null)
                                {
                                    uriToCheck = new Uri(versionSpecificRootUrl + explicitMapping.Key);
                                }
                                else if (inheritedMapping != null)
                                {
                                    uriToCheck = new Uri(versionSpecificRootUrl + inheritedMapping.Key);
                                }
                                else
                                {
                                    uriToCheck = new Uri(versionSpecificRootUrl + page.Key);
                                }

                                result.Mappings.Add(versionToCheck.StringValue + " " + uriToCheck, ValidatePageLink(client, uriToCheck));
                            }

                            results.Add(result);
                        }
                    }
                });

            return results;
        }
    }
}
