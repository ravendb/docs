namespace RavenCodeSamples.ClientApi.Querying.StaticIndexes
{
	using System.Linq;

	using Raven.Abstractions.Indexing;
	using Raven.Client.Indexes;

	public class ConfiguringIndexOptions : CodeSampleBase
	{
		public void BasicStaticIndexes()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				#region analyzers1
				documentStore.DatabaseCommands.PutIndex("AnalyzersTestIdx", new IndexDefinitionBuilder<BlogPost, BlogPost>
																				{
																					Map =
																						users =>
																						from doc in users select new { doc.Tags, doc.Content },
																					Analyzers =
				                                                            			{
				                                                            				{x => x.Tags, "SimpleAnalyzer"},
				                                                            				{x => x.Content, "SnowballAnalyzer"}
				                                                            			},
																				});

				#endregion
			}
		}

		#region stores1
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

		#endregion
	}
}
