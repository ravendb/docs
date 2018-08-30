using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Web.Indexes
{
    public class DocumentationPages_ByKey : AbstractIndexCreationTask<DocumentationPage, DocumentationPages_ByKey.Result>
    {
        public class Result
        {
            public string Key { get; set; }

            public Dictionary<string, string> Ids { get; set; }

            public Dictionary<string, string[]> Languages { get; set; }
        }

        public DocumentationPages_ByKey()
        {
            Map =
                pages =>
                from page in pages
                select new
                {
                    Key = page.Key,
                    Ids = new Dictionary<string, string>
                                    {
                                        { page.Version + "/" + page.Language, page.Id }
                                    },
                    Languages = new Dictionary<string, string[]>
                                    {
                                        { page.Version, new [] { page.Language.ToString() } }
                                    }
                };

            Reduce = results => from result in results
                                group result by result.Key
                                into g
                                select new
                                {
                                    Key = g.Key,
                                    Ids = g.SelectMany(x => x.Ids).GroupBy(p => p.Key).ToDictionary(x => x.Key, x => x.First().Value),
                                    Languages = g.SelectMany(x => x.Languages).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.SelectMany(y => y.Value).Distinct())
                                };
        }
    }

    public class DocumentationPage_WithVersionsAndLanguages
    {
        public class Result
        {
            public Result(ProjectionResult result)
            {
                Page = result.Page;
                Languages = result.Languages.ToDictionary(x => x.Key, x => ConvertLanguages(x.Value));
            }

            public DocumentationPage Page { get; set; }

            public Dictionary<string, Language[]> Languages { get; set; }

            private static Language[] ConvertLanguages(string[] languages)
            {
                return languages
                    .Select(language => (Language)Enum.Parse(typeof(Language), language, ignoreCase: true))
                    .ToArray();
            }
        }

        public class ProjectionResult
        {
            public DocumentationPage Page { get; set; }

            public Dictionary<string, string[]> Languages { get; set; }
        }
    }
}
