<h1>A NoSQL Database With Much More Than Just an Index</h1>
<small>by <a href="mailto:mor@ravendb.net">Mor Hilai</a></small>

<div class="article-img figure text-center">
  <img src="images/nosql-database-with-much-more-than-just-an-index.jpg" alt="Sneleentaxi enables people to find the nearest taxi driver at the right price. They use NoSQL Database RavenDB in a distributed database cluster to make it happen." class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size:24px;">RavenDB's new Additional Assemblies feature grants you more flexibility and creativity than ever before.</p>

When you're searching your data, a database is only as fast and useful as its indexes. To search for an item without an index, you'd need to scan the entire database. An index contains all the addresses where a given item can be found. This can make queries much faster, and the larger the database, the bigger the difference that indexes make.

In RavenDB we perfected the speed and efficiency of our indexes early on, and then we began to push the boundaries of what an index is and what it can do. In most databases, indexes are created manually. In RavenDB, a query that can't find an appropriate index to serve it automatically triggers the creation of a new index that serves that query and others like it. We've kitted our indexes with flexible and intuitive syntax for full-text searching, indexing data as graphs, indexing geospatial and time series data, and performing all kinds of processing and projections. In parallel, we have developed a powerful query language with many features of its own. Starting in version 4.0, indexes could be extended with code from additional source files.

In RavenDB 5.1 we've added a new feature that grants you more flexibility and creativity than ever before: [additional assemblies](https://ravendb.net/docs/article-page/5.1/csharp/indexes/additional-assemblies). You can now integrate any .NET code library into the index and use its methods and classes in the index syntax. Accessing any library from NuGet is now just a matter of typing its name.

### Indexes: Additional Assemblies

Indexes are no longer limited to their basic query fetching role, they are a platform that other functions can be built on top of. They can now perform almost any role in a larger system or data pipeline. Besides NuGet, it is just as easy to use the namespaces of .NET runtime, as well as any .dll you have locally.

Here are some examples. A very common kind of index is one that breaks up text into indexed terms. But with so much of our text contained on Word, PDFs, spreadsheets, and so on, it's enough work trying to synchronize these different file types without having to transcribe them to pure text. Using one publicly available library from NuGet (`DocumentsFormat.OpenXml`) and about 50 lines of code, an index can take any Word or Excel document and extract all of its text. This text will now be available to Full Text Search. See Oren Eini's [blog post](https://ravendb.net/articles/ravendb-5-1-features-searching-in-office-documents) for a step by step demonstration.

### Save you a lot of time

This application can definitely save a lot of time messing around with sharepoint, but let's look at an example that gives a better sense of the possibilities this feature offers. What about an index that uses machine learning? Maybe we'd like an index that can 'look' at an image and categorize it by its contents? For example, the index might categorize photos by whether they contain cats, dogs, or people. Sounds like science fiction? This too is possible with 50 lines of code and the publicly available NuGet package `Microsoft.ML`. See Oren's other [blog post](https://ravendb.net/articles/using-machine-learning-with-ravendb) for a demonstration.

Why worry about building multiple separate systems and manually moving your data from one to another? RavenDB indexes provide a highly optimized platform where you can combine all your logic with an unlimited array of imported technologies. Once you've defined your index, all you'll have to do is upload your data and [let RavenDB take care of the rest](https://ravendb.net/features/querying/full-text-search).