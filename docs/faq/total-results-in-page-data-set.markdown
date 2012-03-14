#Finding total results when using paging

RavenDB make it very easy to work with paging (in fact, with large data sets, it makes it mandatory), which bring the question, how can I find the total number of results for a particular query?

The API supports this explicitly:

    RavenQueryStatistics stats;
    var results =  session.Query<Customer>()
       .Statistics(out stats)
       .Where(x => x.Region == "Asia")
       .ToArray()

    var totalResutls = stats.TotalResults;

And here is how you do this using the advanced Lucene API:

    var documentQuery = session.Advanced.LuceneQuery<Customer>()
        .WhereEquals("Region", "Asia")
        .Take(10);

    var pagedCustomers = documentQuery.ToList();
    var totalResults = documentQuery.QueryResult.TotalResults;

The result of the query is just 10 results, and the total results is the total number of matching documents.