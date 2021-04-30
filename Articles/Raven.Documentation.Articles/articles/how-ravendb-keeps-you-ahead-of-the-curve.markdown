<h1>How RavenDB 5.1 ACID Document Database Keeps You Ahead of the Curve</h1>
<small>by <a href="mailto:dejan@ravendb.net">Dejan Miličić</a></small>

<div class="article-img figure text-center">
  <img src="images/how-ravendb-keeps-you-ahead-of-the-curve.jpg" alt="Fine tune your data replication, preserve resources with data compression, manage Big Data and IoT with a Time Series model." class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size: 24px;">RavenDB 5.1 New Features</p>

RavenDB 5.1 ACID document database includes the following new features that allow you to expand your data indexing to include Microsoft Office documents, fine tune your data replication, preserve resources with data compression, manage Big Data and IoT with a Time Series model, and more.

### Indexes

Indexes are the main tool for implementing features in RavenDB.

In version 5.1 [indexing capabilities have been significantly expanded](https://ravendb.net/why-ravendb/whats-new). Up to now, you could supply Additional Sources in JavaScript and C# to augment RavenDB with your custom functionalities. Now you can reference Additional Assemblies either directly, or from the NuGet package.

You can offload even more computation to your database. Combined with the ability to index attachments, time-series, and counters - exciting new functionality is now possible.

For example, you can:

- Apply machine learning analysis to image attachments
- Parse MS Office attached documents, index their content, and include them in full-text search
- Use OCR to extract textual content from attached images

### Replication

RavenDB 5.1 expands the replication scenario to automatically and seamlessly share data between servers in real-time with more advanced capabilities.

#### Revamped [Pull Replication](https://ravendb.net/docs/article-page/5.0/csharp/server/ongoing-tasks/pull-replication)

Replication was always initiated by Sink, resulting in machines on the edge receiving information from a central Hub. This feature now allows two-way replication. You can replicate data from Sink into Hub as well.

#### Filtered Replication

Expanding on the idea of more control over things, you are now able to establish replication between two servers, and configure it so they are able to send and receive just a specific subset of data based on document IDs and ID prefixes, defining allowed ranges and subranges.

### Time Series

Time Series is an efficient way of collecting and analyzing [a huge number of data points](https://ravendb.net/docs/article-page/5.1/csharp/document-extensions/timeseries/overview). A large number of systems, including an expanding variety of IoT devices generate a continuous stream of numerical values that RavenDB is able to ingest and store with high efficiency.

Latest additions to Time Series data manipulation capabilities include:

- Gap Filling: extrapolate existing values to fill in gaps
- Scaling: Multiply received values with a specified scaling factor
- ETL: Time Series data is now available for transformation within ETL tasks
- Streaming: Support for streaming Time Series data with queries

### Query Projection Behavior

Query Projections empower you to create the exact shape of data your application needs. RavenDB efficiently combines data from several collections and provides data structures that your application can use right away, without any additional transformations.

With five new Projection Behaviors, you can leverage fine-tuned control over storing index fields so that they can be retrieved from index itself, instead of retrieving them from the document. From now on, your projections will be even faster.

### Document Compression

[Document Compression has been battle-tested and is now ready for your project.](https://ravendb.net/why-ravendb/ravendb-5-features#documentsCompression) RavenDB will analyze documents across multiple collections, detect redundant data, and build customized dictionaries that can reduce your storage allocation between 50% and 80%.