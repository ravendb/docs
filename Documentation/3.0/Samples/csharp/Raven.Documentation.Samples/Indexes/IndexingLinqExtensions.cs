using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq.Indexing;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	public class IndexingLinqExtensions
	{
		#region indexes_3
		public class Article
		{
			public string Id { get; set; }

			public string Title { get; set; }

			public string ContentHtml { get; set; }
		}
		#endregion

		#region indexes_4
		public class Employees_ByReversedFirstName : AbstractIndexCreationTask<Employee>
		{
			public Employees_ByReversedFirstName()
			{
				Map = employees => from employee in employees
							select new
							{
								FirstName = employee.FirstName.Reverse()
							};
			}
		}
		#endregion

		#region indexes_1
		public class Article_Search : AbstractIndexCreationTask<Article>
		{
			public class Result
			{
				public string Content { get; set; }
			}

			public Article_Search()
			{
				Map = articles => from article in articles
							select new
							{
								Content = article.ContentHtml.StripHtml()
							};

				Index(x => x.ContentHtml, FieldIndexing.Analyzed);
			}
		}
		#endregion

		public IndexingLinqExtensions()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region indexes_2
					IList<Article> results = session
						.Query<Article_Search.Result, Article_Search>()
						.Search(x => x.Content, "Raven*", escapeQueryOptions: EscapeQueryOptions.AllowPostfixWildcard)
						.OfType<Article>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_5
					IList<Employee> results = session
						.Query<Employee, Employees_ByReversedFirstName>()
						.Where(x => x.FirstName == "treboR")
						.ToList();
					#endregion
				}
			}
		}
	}
}