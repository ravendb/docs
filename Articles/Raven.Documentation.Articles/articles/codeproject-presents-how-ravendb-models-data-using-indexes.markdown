# How RavenDB Models Data using Indexes <br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![How RavenDB Models Data using Indexes](images/codeproject-presents-how-ravendb-models-data-using-indexes.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

RavenDB Indexes are known for their [high performance](https://ravendb.net/why-ravendb/high-performance). But that is just the tip of the iceberg. Relational databases may use indexes to speed up how your database accesses your data by creating a fast lane to your tables, but when it comes to data modeling, relational indexes don't do much.

In RavenDB, indexes make use of MapReduce computations to deliver aggregated data. Your indexes impact your data and the documents outputted. This makes RavenDB indexes a significant factor in your data modeling.

Why restrict your use of indexes to aggregate data when you can use those aggregations to predict future data your database will likely query?

You can [save index results](https://ravendb.net/docs/article-page/4.2/All/studio/database/indexes/create-map-reduce-index) as physical documents in an *artificial* collection. Not only can your indexes generate new domain entities, but you can also create *other* indexes using those same stored documents.

You can even use indexes to create a fully event-sourced system where documents are aggregated and then stored as artificial documents, enabling higher-order indexes to serve a read-only aggregation layer.

Deploy all of it with code!

In this article, RavenDB Rockstar Kamran Ayub codes away to <a href="https://www.codeproject.com/Articles/1348454/Data-Modeling-with-Indexes-in-RavenDB" target="_blank" rel="nofollow">take you on a guided tour</a> of modeling your data with RavenDB Indexes.
