namespace RavenCodeSamples.Appendixes
{
	using System.Linq;
	using Raven.Abstractions.Indexing;
	using Raven.Client;
	using Raven.Client.Indexes;
	using Raven.Client.Linq;

	public class Article
	{
		public string Id { get; set; }
		public string Text { get; set; }
	}

	public class LuceneScore : CodeSampleBase
	{
		public LuceneScore()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region index_def_and_storing_items
					store.DatabaseCommands.PutIndex("Articles/ByText", new IndexDefinitionBuilder<Article, Article>
					{
						Map = articles => from article in articles select new { article.Text },
						Indexes =
						{
							{ x => x.Text, FieldIndexing.Analyzed }
						}
					});

					using (var s = store.OpenSession())
					{
						s.Store(new Article() {Text = "Lorem ipsum is simply text."}); // articles/1
						s.Store(new Article() {Text = "Ipsum lorem ipsum is simply text. Lorem Ipsum."}); // articles/2
						s.Store(new Article() {Text = "Lorem ipsum. Ipsum is simply text."}); // articles/3
						s.SaveChanges();
					}
					#endregion
				}

				#region order_by_and_get_score
				using (var session = store.OpenSession())
				{
					var articles = session.Query<Article>("Articles/ByText")
										  .Customize(x => x.WaitForNonStaleResults())
										  .Where(x => x.Text == "ipsum")
										  .OrderByScore()
										  .ToList();

					var articlesWithLuceneScore = articles.Select(x =>
						new
						{
							x.Id,
							x.Text,
							Score = session.Advanced.GetMetadataFor(x).Value<double>("Temp-Index-Score"),
						}).ToList();
				}
				#endregion
			}
		}
	}
}