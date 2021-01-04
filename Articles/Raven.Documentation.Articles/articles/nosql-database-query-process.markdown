# NoSQL Database Query Process
<small>by Richard Norman</small>

<div class="article-img figure text-center">
  <img src="images/nosql-database-query-process.jpg" alt="NoSQL database enables a private database for each user. Automatic Indexing and Aggregated MapReduce make complex computations fast using NoSQL for Ecommerce" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<h3 id="understanding-how-ravendb-queries-data" class="margin-top">Understanding how RavenDB queries data</h3>

Once upon a time a query was just a simple Select statement. Whether it was a stored procedure, a view or a function it always ended up in one single big SQL statement that was executed by a user upon request.

Everything always happened at the point of select.

Unfortunately, the modern world brought mind bending complexity and a gigantic increase in data volumes. The big SQL statement got a lot bigger and a lot slower. Big Data was always going to require a new approach if it was going to work. Nobody thought it was going to involve time travel.

Time travel?

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <img src="images/nosql-database-query-process/1.jpg" class="img-responsive m-0-auto" alt="RavenDB - The Query Pipeline"/>
</div>

RavenDB offers a highly structured and advanced way of [querying data](https://ravendb.net/docs/article-page/4.2/csharp/indexes/querying/basics). Rather than cramming everything into a single bit of SQL and running it repeatedly it takes a much more scalable and manageable approach that works through a series of layers.

Each layer is radically different in the way that it treats data and as such different layers are better for different types of data. Rather than querying in one big go it processes data at different times and in different ways.

The consequence is that data is added to, joined, processed and aggregated as it passes through these levels. RavenDB might start the select with a simple document but by the time it has fully traversed these layers it will have been processed and enriched to perfection.

This allows RavenDB developers to give the user a fully featured result set in a fraction of the time taken before. Even at massive scale, if it is done right, it should be instantaneous. How does it do this? Well, not all the layers execute at the same time. Chances are that if you run a query on a RavenDB datastore most of the processing has already been done, in the past. Two out of the four layers that make up the selection process execute at the point of save. It's processing data in the past before it's needed by the user.

All the heavy lifting gets done in advance. By the time the user wishes to view their data most of the processing has already been done. Its like running queries in the Tardis. Obviously, this is a much more complicated mechanism than a traditional SQL statement. Get ready to throw that SQL manual out the window and forget everything that was learned in Database school. It's going to get a bit weird.

Your select queries are about to enter the fourth dimension.

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <img src="images/nosql-database-query-process/2.jpg" class="img-responsive m-0-auto" alt="Denormalization"/>
</div>

### Layer One ➡ Denormalization

For the RavenDB purists this is not technically a layer, it's a bit like a pre-layer.

RavenDB is a Document datastore. It is not limited by the constraints of flat tables and rows but has the capability to deal with relations and hierarchies inside a single document. What this allows is the ability to save a document inside another document and effectively denormalize the data. There is a master version in the original document that the user can change but each new transactional document will contain a copy of the related document at the point when the transactional document was created.

Take the scenario of a utility company. Every month a new Invoice is raised for billed electricity and sent to a customers Location. The Location document contains an address.

Each new Invoice will contain a serialised copy of the original Location document, as it was when the Invoice was created.

By denormalizing the documents there is no need for a join. In SQL a foreign key relationship would need to be created and the Location joined with the Invoice at the point of select. When denormalized the Location object is already in the Invoice document and is simply part of document selection. That's why it is kind of a pre-layer in that no processing in necessary because all the data required in now in a single document and is simply selected.

In our Invoice / Location scenario this is useful functionality. If the user changes the Location address, there is no need to go back and change all the addresses for all previous invoices. The electricity was provided to an address and it is correct that the old address is still used in the Invoice for that address. Nothing has changed on the historical Invoices, but all subsequent Invoices will use the new address.

What happens if all the denormalized copies need to be changed? RavenDB provides a mechanism called patching. Patching replicates changes to the original denormalized document through all documents that use that object. If a user changes the address on the Location the patch operation will change any document that used the old data. This all happens asynchronously and on a background thread. A user does not have to stick around until its finished.

Due to this requirement for a manual update to denormalized documents means that not all data is suitable for Denormalization. If the data in question changes frequently the cost of this manual update may outweigh the benefits of Denormalization in the first place.

#### Scenarios that work well for Denormalization

1. Static / Semi-static data that never changes or is changed by an administrative update (Countries)
2. Transactional data that is fixed at a point in time and needs to be retained as is (our example of an Invoice at point of sale)
3. Any data that is referenced by lots of documents in the system (Author)

#### Documentation

The RavenDB documentation about Denormalization can be found [here](https://ravendb.net/docs/article-page/4.2/Csharp/client-api/how-to/handle-document-relationships#denormalization).

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <img src="images/nosql-database-query-process/3.jpg" class="img-responsive m-0-auto" alt="Indexing"/>
</div>

### Layer Two ➡ Indexing

Indexing is the most important part of the RavenDB selection pipeline. It is where a lot of the aggregation, joining and processing of data will happen. It's usually the best place to do any heavy lifting.

For those from an SQL background it is important to understand that an [Index](https://ravendb.net/docs/article-page/4.2/csharp/indexes/creating-and-deploying) in RavenDB is absolutely nothing like an Index in SQL. A RavenDB Index is still used to search data, but its primary purpose is to process and cache information.

Essentially what is happening under the hood with a RavenDB Index is Event Sourcing. When a document is saved it fires an event. Indexes are listening for these events and when they are detected the Index will retrieve the latest data from the save and execute its processing routine.

The critical thing to appreciate is that this process happens asynchronously at the point of save. A user saves a document and control is returned. The Index processing will kick off almost immediately afterwards. At no stage is the user waiting for this because it all executes on a background thread.

What this means is that a lot of data processing is done in dead time. Once the user has saved a document they will go and do something else. While that is happening the background threads are happily crunching data. By the time the user returns to see the result it has already been cached in the Index and the resulting search is immediate. The user is always accessing pre-processed information from an Index.

It is kind of like processing information in the past, hence the time travel analogy.

The Indexes can be complicated, and it is by far the most involved and difficult part of RavenDB development. Indexes utilise Map/Reduce to pull a series of Maps and LoadDocument calls to the raw underlying documents. The final Reduce will crunch everything into a potentially cached result that can also be directly queried.

It is important to note that all queries must go through an Index and if one is not specified it will build it on the fly. That is how critical the process of Indexing is to RavenDB.

This Indexing process is really the big unique feature of RavenDB. Having this extra stage done in advance of the query process makes a massive difference to selection performance. Instead of that processing happening as part of the query it is all ready to go before the query executes.

One word of warning is that Indexes can go stale. If a save occurs and a query is run immediately afterwards then the Index may not have had enough time to fully process. This is the reason why RavenDB utilises Eventual Consistency and a developer must always be aware of avoiding this outcome. It also makes it difficult to make business logic decisions on the results of an Index because the decision may be made on stale out of date data. It is possible to wait for an Index if it is stale, but this may cause the user delay in execution under load.

It is also worth being aware that not all data processing is suitable for Indexing. Using a LoadDocument on data that has a lot of relations may mean that a significant number of Indexes will be reprocessed on the saving of that data. It can be a pitfall of the way RavenDB works.

#### Scenarios that work well for Indexing

1. Anything that involves aggregation or calculation (Counts and Sums)
2. Parent objects with only a few children (User-Created Type)
3. Anything that changes but doesn't change very often (Contact Details)

#### Documentation

There is extensive documentation on RavenDB Indexing that can be found [here](https://ravendb.net/docs/article-page/4.2/csharp/indexes/what-are-indexes).

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <img src="images/nosql-database-query-process/4.jpg" class="img-responsive m-0-auto" alt="Projections"/>
</div>

### Layer 3 ➡ Projections

The third stage of the selection pipeline in RavenDB is a Projection. Unlike the first two stages that execute at the point of save a Projection will execute when the query is run.
In practical terms, the Projection is a place where all the processing that can't easily be done in an Index or through Denormalization resides. If it can be done through Denormalization or Indexing, then it is usually best to it there. For data that changes regularly it is best to pull this data into the result set at the point when the query executes, and this is done in the Projection.

Queries in RavenDB are usually simple Where clauses. The query is effectively wrapped by a Projection and then sent to an Index for processing. The Projection runs alongside the query and decorates the results with this extra data.

It is crucial to understand that all of this is happening on the RavenDB server under a single server call. There is only one round trip to get this data and everything executes on the server.

It is an incredibly efficient way of doing things and saves the numerous round trips to the server that is common within SQL.

It could be argued that the Projection is the safest place to do the joining of data. It is not uncommon that a lot of the join work is put into the Projection when the query is first written by the programmer, only to find later that some of it can be refactored out into an Index or Denormalized.

If in doubt, stick it in the Projection. It may cause problems in the Index.

Once the Projection has executed the result set is ready to be consumed and is sent back to the client.

#### Scenarios that work well for the Projection

1. Highly volatile data that is regularly updated (User State)
2. Common data that is related to a lot of other Documents (Product Category)
3. If its not clear where the data goes its usually safer to stick it in the Projection

#### Documentation

The RavenDB documentation about Projections can be found [here](https://ravendb.net/docs/article-page/4.2/Csharp/indexes/querying/projections).

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <img src="images/nosql-database-query-process/5.jpg" class="img-responsive m-0-auto" alt="Lazy Queries"/>
</div>

### Layer 4 ➡ Lazy Queries

The three layers up until now will create a perfectly usable result from RavenDB without the need for any Laziness. Most operations do not require Lazy functionality and will execute in a single call and provide a result.

To some extent Laziness can be considered a post-layer, much like Denormalization is a pre-layer. All the work is being done by the Index and the Projection and Laziness is not really concerned with execution.

Sometimes there are circumstances where its very difficult to fit all the joins, aggregation and general processing into a single big Index and Projection. The data may not have a natural relationship, or the processing may be doing something completely different.

A simple cure to this problem is to execute two separate queries but this would require two round trips to the server and RavenDB is all about doing everything in one go. This is where Lazy queries come in.

By marking a query as being Lazy it delays execution. This then allows another Lazy query to be created and both Lazy queries sent to the server in one go to be executed in a single call. Lazy queries are effectively a way of batching RavenDB calls to the server. They are not used very often but are very useful fallback in cases where complexity or distance in relationships make it necessary.

#### Scenarios that work well for Lazy Queries

1. Radically different data that cannot be easily joined in an Index or Projection (Dashboards)
2. Results that require further joins with external data (Quoted Price)
3. The absolute last resort when it is not clear how to fit it all into an Index or Projection

#### Documentation

The RavenDB documentation about Lazy Queries can be found [here](https://ravendb.net/docs/article-page/4.2/csharp/client-api/session/querying/how-to-perform-queries-lazily).

### Conclusion

The NoSQL database query pipeline is a far more complicated process than it used to be in SQL. The benefits to user experience, efficiency on the server and richness of the result mean that it is well worth making the effort.

Workload is spread across the layers meaning that the query does very little. Significant parts of the process are being done in advance and this means that users will receive data almost immediately, irrespective of the amount of data being processed.

By executing in that dead time just after a save RavenDB creates an illusion that its ultra-quick. It may have taken the same amount of time as an SQL statement but because a lot of that effort is now being done in advance the user only has to wait for the final Projection and the actual selection of documents.

It is also more efficient for the server. If it is a low write / high read system, then the benefit of being able to cache a result in an Index means that queries don't have to repeat work as they do in SQL. The Index runs once, and each subsequent query is simply reading cached data from the Index.

What this means is that potentially huge operations can look instant. Dashboards refresh immediately, lists filter and page instantaneously and users appreciate the experience as it does not waste their time. Every query should return in milliseconds.

Be aware of the staleness in the Indexing. Most Indexes process almost instantaneously but it is very easy to query something immediately after a save and find that its not there. Likewise, in a high write / low read scenario it may not be suitable. It may be the case that lots of writes cause lots of Indexes to process but no-one is looking at the results. It can be an efficiency saving in reverse.

Saying that, RavenDB is a spectacular platform for building rich, rapid and highly complex platforms that users will enjoy using. It just takes a bit of extra thought and some time travel.



