using System.Collections.Generic;
using System.Linq;
using Raven.Client.Indexes;
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
									Ids = g.SelectMany(x => x.Ids).ToDictionary(x => x.Key, x => x.Value),
									Languages = g.SelectMany(x => x.Languages).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.SelectMany(y => y.Value).Distinct())
								};
		}
	}

	public class DocumentationPage_WithVersionsAndLanguages : AbstractTransformerCreationTask<DocumentationPages_ByKey.Result>
	{
		public class Result
		{
			public DocumentationPage Page { get; set; }

			public Dictionary<string, Language[]> Languages { get; set; }
		}

		public DocumentationPage_WithVersionsAndLanguages()
		{
			TransformResults = results => from result in results
										  let version = Parameter("Version").Value<string>()
										  let language = Parameter("Language").Value<string>()
										  let id = result.Ids[version + "/" + language] ?? result.Ids[version + "/All"]
										  where id != null
										  select new
										  {
											  Page = LoadDocument<DocumentationPage>(id),
											  Languages = result.Languages
										  };
		}
	}
}
