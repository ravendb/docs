namespace RavenCodeSamples.ClientApi.Querying.StaticIndexes
{
	using System.Linq;

	using Raven.Abstractions.Indexing;
	using Raven.Client.Indexes;

	public class CustomizingResultsOrder : CodeSampleBase
	{
		#region static_sorting1
		public class SampleIndex1 : AbstractIndexCreationTask<Customer, Customer>
		{
			public SampleIndex1()
			{
				Map = users => from user in users select new { user.Age };

				Sort(x => x.Age, SortOptions.Short);
			}
		}

		#endregion

		#region static_sorting2
		public class SampleIndex2 : AbstractIndexCreationTask<Customer, Customer>
		{
			public SampleIndex2()
			{
				Map = users => from doc in users select new { doc.Name };

				Sort(x => x.Name, SortOptions.String);

				Analyzers.Add(x => x.Name, "Raven.Database.Indexing.Collation.Cultures.SvCollationAnalyzer, Raven.Database");
			}
		}

		#endregion
	}
}