#### Indexing Related Documents

To extend indexing capabilities and simplify many scenarios, we have introduced the possibility to index related documents. 
For start, lets consider a simple `Customer - Invoice` scenario where you want to lookup for an `Invoice` by `Customer Name`.

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

Without this feature, the index that had to be created would be a fairly complex multiple map-reduce index and this is why the `LoadDocument` function was introduced.

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

Alternative way is to use `PutIndex` command:

	store.DatabaseCommands.PutIndex("SampleIndex", new IndexDefinition
	{
	    Map = @"from invoice in docs.Invoices
	            select new
	            {
	                CustomerId = invoice.CustomerId,
	                CustomerName = LoadDocument(invoice.CustomerId).Name
	            }"
	});

Now we will be able to search for invoices using `Customer Name` as a parameter.

Our next scenario will show us that indexing more complex relationships is also trivial. Lets consider a case:

	public class Book
	{
	    public string Id { get; set; }
	}
	 
	public class Author
	{
	    public string Id { get; set; }
	 
	    public string Name { get; set; }
	 
	    public IList<string> BookIds { get; set; }
	}

Now to create an index with `Author Name` and list of `Book Names` we need to create it as follows:

	public class AnotherIndex : AbstractIndexCreationTask<Author>
	{
	    public AnotherIndex()
	    {
	        Map = authors => from author in authors
	                         select new
	                             {
	                                 Name = author.Name,
	                                 Books = author.BookIds.Select(x => LoadDocument<Book>(x).Id)
	                             };
	    }
	}

or

	store.DatabaseCommands.PutIndex("AnotherIndex", new IndexDefinition
	{
	    Map = @"from author in docs.Authors
	            select new
	            {
	                Name = author.Name,
	                Books = author.BookIds.Select(x => LoadDocument(x).Id)
	            }"
	});

{NOTE Indexes will be updated automatically when related documents will change. /}

{NOTE Using `LoadDocument` adds a loaded document to tracking list. This may cause very expensive calculations to occur especially when multiple documents are tracking the same document. /}