using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using RavenDBSamples.BaseForSamples;

namespace RavenDBSamples.Indexing
{
	public class IndexSortOptions : SampleBase
	{
		public void PutIndexWithAnalyzer()
		{
			DocumentStore.DatabaseCommands.PutIndex("AnalyzersTestIdx", new IndexDefinitionBuilder<BlogPost, BlogPost>
				{
					Map = users => from doc in users select new {doc.Tags, doc.Content},
					Analyzers =
						{
							{x => x.Tags, "SimpleAnalyzer"},
							{x => x.Content, "SnowballAnalyzer"}
						},
				});
		}

		public void IndexingHierarchicalData()
		{
			DocumentStore.DatabaseCommands.PutIndex("SampleRecurseIndex", new IndexDefinition
				{
					Map = @"from post in docs.Posts
            from comment in Recurse(post, (Func<dynamic, dynamic>)(x => x.Comments))
            select new
            {
                Author = comment.Author,
                Text = comment.Text
            }"
				});
		}

		public void IndexWithTransformResults()
		{
			DocumentStore.DatabaseCommands.PutIndex("PurchesHistory", new IndexDefinitionBuilder<Order, Order>
				{
					Map = orders => from order in orders
					                from item in order.Items
					                select new
						                {
							                UserId = order.UserId,
							                ProductId = item.Id
						                },

					TransformResults = (database, orders) =>
					                   from order in orders
					                   from item in order.Items
					                   let product = database.Load<Product>(item.Id)
					                   where product != null
					                   select new
						                   {
							                   ProductId = item.Id,
							                   ProductName = product.Name
						                   }
				});
		}
	}

	public class SampleIndex1 : AbstractIndexCreationTask<Customer, Customer>
	{
		public SampleIndex1()
		{
			Map = users => from user in users select new { user.Age };

			Sort(x => x.Age, SortOptions.Short);
		}
	}

	public class SampleIndex2 : AbstractIndexCreationTask<Customer, Customer>
	{
		public SampleIndex2()
		{
			Map = users => from doc in users select new { doc.Name };

			Sort(x => x.Name, SortOptions.String);

			Analyzers.Add(x => x.Name, "Raven.Database.Indexing.Collation.Cultures.SvCollationAnalyzer, Raven.Database");
		}
	}

	public class StoresIndex : AbstractIndexCreationTask<BlogPost, BlogPost>
	{
		public StoresIndex()
		{
			Map = posts => from doc in posts
						   select new { doc.Tags, doc.Content };

			Stores.Add(x => x.Title, FieldStorage.Yes);

			Indexes.Add(x => x.Tags, FieldIndexing.NotAnalyzed);
			Indexes.Add(x => x.Comments, FieldIndexing.No);
		}
	}
}