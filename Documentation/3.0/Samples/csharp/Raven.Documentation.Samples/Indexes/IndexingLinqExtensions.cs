using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq.Indexing;
using Raven.Documentation.CodeSamples.Orders;

using Xunit;

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

		#region indexes_6
		public class Item_Parse : AbstractIndexCreationTask<Item>
		{
			public class Result
			{
				public int MajorWithDefault { get; set; }

				public int MajorWithCustomDefault { get; set; }
			}

			public Item_Parse()
			{
				Map = items => from item in items
				            let parts = item.Version.Split('.')
							select new
							{
								MajorWithDefault = parts[0].ParseInt(),			// will return default(int) in case of parsing failure
								MajorWithCustomDefault = parts[0].ParseInt(-1)	// will return -1 in case of parsing failure
							};

				StoreAllFields(FieldStorage.Yes);
			}
		}
		#endregion

		#region indexes_7
		public class Item
		{
			public string Version { get; set; }
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

				using (var session = store.OpenSession())
				{
					#region indexes_8
					session.Store(new Item { Version = "3.0.1" });
					session.Store(new Item { Version = "Unknown" });

					session.SaveChanges();

					var results = session
						.Query<Item_Parse.Result, Item_Parse>()
						.ToList();

					Assert.Equal(2, results.Count);
					Assert.True(results.Any(x => x.MajorWithDefault == 3));
					Assert.True(results.Any(x => x.MajorWithCustomDefault == 3));
					Assert.True(results.Any(x => x.MajorWithDefault == 0));
					Assert.True(results.Any(x => x.MajorWithCustomDefault == -1));
					#endregion
				}
			}
		}
	}
}