using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes
{
	namespace Foo
	{
		#region indexing_related_documents_1
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

		#endregion

		#region indexing_related_documents_4
		public class Book
		{
			public string Id { get; set; }

			public string Name { get; set; }
		}

		public class Author
		{
			public string Id { get; set; }

			public string Name { get; set; }

			public IList<string> BookIds { get; set; }
		}

		#endregion

		#region indexing_related_documents_2
		public class SampleIndex : AbstractIndexCreationTask<Invoice>
		{
			public SampleIndex()
			{
				Map = invoices => from invoice in invoices
								  select new
								  {
									  CustomerId = invoice.CustomerId,
									  CustomerName = LoadDocument<Customer>(invoice.CustomerId).Name
								  };
			}
		}

		#endregion

		#region indexing_related_documents_5
		public class AnotherIndex : AbstractIndexCreationTask<Author>
		{
			public AnotherIndex()
			{
				Map = authors => from author in authors
								 select new
								 {
									 Name = author.Name,
									 Books = author.BookIds.Select(x => LoadDocument<Book>(x).Name)
								 };
			}
		}

		#endregion
	}

	public class IndexingRelatedDocuments
	{
		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region indexing_related_documents_3
				store.DatabaseCommands.PutIndex("SampleIndex", new IndexDefinition
				{
					Map = @"from invoice in docs.Invoices
							select new
							{
								CustomerId = invoice.CustomerId,
								CustomerName = LoadDocument(invoice.CustomerId).Name
							}"
				});

				#endregion

				#region indexing_related_documents_6
				store.DatabaseCommands.PutIndex("AnotherIndex", new IndexDefinition
				{
					Map = @"from author in docs.Authors
							select new
							{
								Name = author.Name,
								Books = author.BookIds.Select(x => LoadDocument(x).Id)
							}"
				});

				#endregion
			}
		}
	}
}
