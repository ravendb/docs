namespace RavenCodeSamples.Appendixes
{
	using Lucene.Net.Analysis;

	using Raven.Abstractions.Indexing;
	using Raven.Client.Document;

	public class LuceneIndexesUsage
	{
		public void UsingCustomAnalyzers()
		{
			var store = new DocumentStore();

			#region using_custom_analyzers
			store.DatabaseCommands.PutIndex("Movies",
				new IndexDefinition
				{
					Map = "from movie in docs.Movies select new { movie.Name, movie.Tagline }",
					Analyzers =
					{
						{"Name", typeof (SimpleAnalyzer).AssemblyQualifiedName},
						{"Tagline", typeof (StopAnalyzer).AssemblyQualifiedName},
						{"Other", typeof (MyCustomAnalyzer).AssemblyQualifiedName},
					}
				});
			#endregion
		}

		public class MyCustomAnalyzer
		{
		}
	}
}