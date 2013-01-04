using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace RavenCodeSamples.ClientApi.Querying.StaticIndexes
{
	namespace Foo
	{

		public class Invoice
		{
			public string Id { get; set; }

			public string CustomerId { get; set; }
		}

		public class Customer
		{
			public string Id { get; set; }

			public string Name { get; set; }
		}

		public class SampleIndex : AbstractIndexCreationTask<Invoice>
		{
			public SampleIndex()
			{
				Map = invoices => from invoice in invoices
								  select new
								  {
									  CustomerId = invoice.CustomerId
								  };
			}
		}

	}

	public class IndexingRelatedDocuments : CodeSampleBase
	{

	}
}