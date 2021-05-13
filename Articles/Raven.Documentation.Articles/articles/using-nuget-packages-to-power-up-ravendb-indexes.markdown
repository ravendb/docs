# Using NuGet Packages to Power Up RavenDB Indexes
<small>by Kamran Ayub</small>

<div class="article-img figure text-center">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes.jpg" alt="Leverage the power of NuGet packages within RavenDB indexes to offload work like image EXIF indexing or ML.NET analysis" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}<br/>

In RavenDB 5.1, you can now use third-party NuGet packages and load binary data (or "attachments") within your indexes without any additional code deployment necessary.

In this article, I’ll showcase how to use these two features together to index EXIF metadata on images and run ML.NET sentiment analysis offline on product reviews.

### What Makes RavenDB Indexes So Powerful?

In traditional NoSQL or RDMS databases indexing is usually an afterthought until queries slow down. In RavenDB, you cannot query without an index which makes your app fast by default. Traditional indexes are just lookup tables. In MongoDB, the aggregation pipeline can perform Map-Reduce operations but pipelines are executed by the client code at a point in time.

In contrast, RavenDB indexes can perform complex operations that combine the benefits of MongoDB’s aggregation pipeline with the power of RDMS-style queries. Since indexes are built in parallel in the background, queries respond in milliseconds even under high load. This makes RavenDB [one of the fastest NoSQL databases on the market](https://ravendb.net/why-ravendb/high-performance).

For a deeper dive, learn [what role indexes play in RavenDB compared to MongoDB and PostgreSQL](https://ravendb.net/articles/nosql-document-database-indexing).

## Indexing Image EXIF Data Using Attachments

Imagine a photo gallery web app. If you want to show the date a photo was taken, you need to read its EXIF metadata. EXIF data is stored in the image binary and follows a specific structure.

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image15.png"  class="img-responsive m-0-auto" alt="OneDrive photo with metadata" />
</div>

In a sophisticated app, the EXIF data can be read when the user uploads the image, asynchronously, and then saved back to the database for lookup later. This is fine but it also requires application code, not to mention services to handle the queuing infrastructure.

Why go to all that trouble for simple use cases? We can tackle this without any custom code in RavenDB. Follow along yourself in the [Live Test Playground](http://live-test.ravendb.net/studio/index.html) where anyone on the Internet can play with RavenDB and make databases.

To upload a binary file, you can “attach” it to a document just like you would an email. In the Studio interface, I’ll add a photo document with a few fields like the photo title and description:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image3.png"  class="img-responsive m-0-auto" alt="Photo document structure" />
</div>

Once the document has been created, I can attach my photo. When I lived abroad in France, I took a photo of a garden in front of a chateau in Aix-en-Provence:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image8.png"  class="img-responsive m-0-auto" alt="Photo of a chateau in Aix-en-Provence, France. Photo Credit: Kamran Ayub" />
</div>

I’ll attach that image using the sidebar in the Studio:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image10.png"  class="img-responsive m-0-auto" alt="Studio: add attachment button" />
</div>

For storing binary files in MongoDB, you have to decide between using BinData or using GridFS. In RavenDB, you have one choice. There’s no limit to attachment file size and they are stored separately from the document so they do not affect query performance.

Now, I’ll create an index named `Photos/WithExifAttributes`:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image1.png"  class="img-responsive m-0-auto" alt="Studio: new index editor" />
</div>

RavenDB indexes are defined C# or JavaScript and can be queried using [Raven Query Language](https://ravendb.net/docs/article-page/5.1/csharp/indexes/querying/what-is-rql) (RQL) which supports invoking C# or JavaScript functions respectively. Since I showcase NuGet in these examples, I’m using C# indexes.

This will be a [Map index](https://ravendb.net/docs/article-page/5.1/csharp/studio/database/indexes/create-map-index), meaning we load data from a document and “select” fields that we want to query on. These fields are used for filtering in “where” clauses when querying.

Using the new [Attachment Indexing helpers](https://ravendb.net/docs/article-page/5.1/csharp/document-extensions/attachments/indexing) to load the list of attachments and retrieve the reference to the photo, I’ll select its file name and size to query on:

<pre>
  <code class="language-csharp">
  from photo in docs.Photos
  let attachment = LoadAttachment(photo, AttachmentsFor(Photo)[0].Name)
  select new {
    photo.Title,
    attachment.Name,
    attachment.Size
  }
  </code>
</pre>

This is madness! Can we just stop for a moment and acknowledge that it’s so cool we can _load the photo while indexing and have full access to it?_

I’ll issue a query and filter by the photo title:

<pre>
  <code class="language-sql">
  from index 'Photos/WithExitAttributes'
  where Title = 'Chateau in France'
  </code>
</pre>

The Studio interface can show “raw” index entries which reveal the photo filename and size:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image7.png"  class="img-responsive m-0-auto" alt="Studio: show index cache entries" />
</div>

The query took **0ms** since _indexes are built in the background_ enabling RavenDB to respond to queries instantly.

Now I’ll kick it up a notch. In the index definition, I will add a NuGet package under “Additional Assemblies” for the <a href="https://www.nuget.org/packages/MetadataExtractor/" target="_blank" rel="nofollow">MetadataExtractor</a> package which supports reading EXIF metadata:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image11.png"  class="img-responsive m-0-auto" alt="Studio: add MetadataExtractor Nuget package" />
</div>

I added some usings needed for running the MetadataExtractor. When writing any amount of complex indexing code, you can leverage the [Additional Sources feature](https://ravendb.net/docs/article-page/5.1/csharp/indexes/extending-indexes) to upload source code files like C# or JavaScript and call functions directly from the Map/Reduce operations. These files have complete access to any NuGet packages you’ve added. That way you can use tooling to make sure your code works before trying to use it within an index and it encapsulates any complex logic to keep your index simpler. I won’t be doing that in this demo just for clarity on how the index is working.

Now I can reference APIs from the MetadataExtractor namespaces in my index code. The steps roughly are to:

1. Get the attachment data as a `Stream`
2. Read the image metadata into a “directory structure”
3. Open the EXIF Sub IFD directory which holds some useful metadata
4. Get the date the photo was taken

<p>EXIF metadata is a little intimidating to understand but some <a href="http://gvsoft.no-ip.org/exif/exif-explanation.html" target="_blank" rel="nofollow">explanations</a> are available. Different metadata properties are available in different “directories.”</p>

I’ll walk through the index code step-by-step:

<pre>
  <code class="language-csharp">
  from photo in docs.Photos
  let attachment = LoadAttachment(photo, AttachmentsFor(photo)[0].Name)
  let directories = new DynamicArray(
    ImageMetadataReader.ReadMetadata(attachment.GetContentAsStream()))
  let ifdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault()
  </code>
</pre>

<p>The first step is to load the attachment data using <code>attachment.GetContentAsStream()</code> which I pass to the <a href="https://github.com/drewnoakes/metadata-extractor-dotnet#usage" target="_blank" rel="nofollow">ImageMetadataReader.ReadMetadata</a> static utility. This will return an enumeration of “directories” as MetadataExtractor calls them (it’s a tree structure).</p>

The `new DynamicArray` expression is a class RavenDB uses that wraps an enumerable so that you can safely perform dynamic LINQ operations. The `OfType<ExifSubIfdDirectory>` LINQ expression retrieves the first metadata directory matching the EXIF Sub IFD directory type.

Next, I get the date the photo was taken as `DateTaken`:

<pre>
  <code class="language-csharp">
  from photo in docs.Photos
  let attachment = LoadAttachment(photo, AttachmentsFor(photo)[0].Name)
  let directories = new DynamicArray(
    ImageMetadataReader.ReadMetadata(attachment.GetContentAsStream()))
  let ifdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault()

  let dateTime = DirectoryExtensions.GetDateTime(ifdDirectory, ExifDirectoryBase.TagDateTimeOriginal)
  select new {
    DateTaken = dateTime,
    photo.Title,
    attachment.Name,
    attachment.Size
  }
  </code>
</pre>

You’ll notice I am using LINQ’s `FirstOrDefault()` method which can return a `null` value for `ifdDirectory`. Indexes in RavenDB are resilient to errors and what happens behind the scenes is some magic that will [add null propagation](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/12-working-with-indexes#error-handling-in-indexing) when accessing any values that could be null. This avoids any `NullReferenceException` issues that could cause indexing to fail. I wish I had a null propagation fairy in my regular .NET code!

<p>I use the <code>DirectoryExtensions.GetDateTime</code> static method to retrieve the photo’s “original date” field. Images can <a href="https://github.com/drewnoakes/metadata-extractor/wiki/SampleOutput" target="_blank" rel="nofollow">contain a lot of different date-time fields</a> and it is not consistent between file formats. For this photo, the <code>TagDateTimeOriginal</code> field holds the timestamp the photo was taken so I am using that.</p>

I can now query photos by date! RavenDB supports date range queries when filtering by a date field so I can use the filter expression:

<pre><code style="background:transparent;">where DateTaken between '2015-01-01' and '2015-03-01`</code></pre>

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image16.png"  class="img-responsive m-0-auto" alt="Studio: query for a photo between dates" />
</div>

This will bring back my photo that was taken during that date range.

Just to summarize everything we managed to do without any application code:

-   Upload a photo to the database and associate it with a document
-   Create an index to query the EXIF image data with the following steps:
    -   Load the photo during indexing
    -   Load a third-party NuGet package assembly
    -   Read the image metadata using the API
    -   Add the EXIF metadata as additional fields in the index
-   Query documents by date taken

All of that and the queries are instantaneous.

I can tell you right now: MongoDB can’t touch that. 🔥

### User Review Sentiment Analysis with ML.NET

Let’s change gears now and examine how we could leverage some offline machine learning to index the sentiment of user reviews. “Sentiment analysis” is classifying text as positive, neutral, or negative. Sounds simple but it’s complicated stuff!

In a sentiment analysis machine learning program, the rough steps would be to:

1. Obtain a source dataset of values
2. Train the AI model on the dataset
3. Use the trained model to perform sentiment analysis against new data

<p>Steps 1 and 2 can take a while to perform if the dataset is large as it runs a learning algorithm on it. Step 3 is much faster as it can use the trained model to analyze a limited dataset, like a single product review. It is beyond my ability to explain this in detail but luckily ML.NET has some great documentation to reference <a href="https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/text-classification-tf" target="_blank" rel="nofollow">on using trained offline models with TensorFlow</a>.</p>

<p>I’ll be using the NuGet package <a href="https://www.nuget.org/packages/SentimentAnalyzer/" target="_blank" rel="nofollow">SentimentAnalyzer</a> to run an offline analysis with a pre-trained model included within the package. The benefit of this is that we aren’t incurring the cost of training every time the indexing operation runs.</p>

I’ll start by creating two documents representing simple user reviews with positive and negative sentiments:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image9.png"  class="img-responsive m-0-auto" alt="Studio: two documents for positive/negative reviews" />
</div>

In an index named `Reviews/BySentiment`, I’ll select the `Author` and `Body` fields from the document:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image17.png"  class="img-responsive m-0-auto" alt="Studio: create index for reviews by sentiment" />
</div>

Now I’ll add a NuGet package with the Additional Sources feature to bring in the package and import the `SentimentAnalyzer` namespace:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image2.png"  class="img-responsive m-0-auto" alt="Studio: add SentimentAnalyzer package" />
</div>

I’ll call `Sentiments.Predict` and pass the review text which returns a `Prediction` boolean where `true` is “positive” and `false` is “negative” sentiment. I’ll select that value out into the `Sentiment` field:

<pre>
  <code class="language-csharp">
  from review in docs.Reviews
  select new {
    review.Body,
    review.Author,
    Sentiment = Sentiments.Predict(
  	  review.Body).Prediction ? "Positive" : "Negative"
  }
  </code>
</pre>

What this enables me to do is query for “positive” or “negative” sounding reviews:

<pre>
  <code class="language-sql">
  from index 'Reviews/BySentiment'
  where Sentiment == 'Positive'
  </code>
</pre>

The query returns the expected positive-sounding review:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image14.png"  class="img-responsive m-0-auto" alt="Studio: query returning positive review" />
</div>

That’s it! In a real-world application, we would likely create our own training model to use on our dataset that makes the most sense for our domain (movies, products, books, etc.).

###NuGet Packages from Custom Sources

I’ve shown how you can pull in third-party NuGet packages but you aren’t limited to the official Microsoft package source. You can use any package source when adding a NuGet package such as your company’s MyGet feed:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image23.png"  class="img-responsive m-0-auto" alt="Studio: custom package source" />
</div>

### How Does Running Additional Code Affect Performance?

I am sure you may be curious to know what performance implication this has on the indexing process (remember, it doesn’t impact the _query_ performance).

This is additional code that is running so it will certainly incur overhead during the indexing process. For this article, I used the Free tier on [RavenDB Cloud](https://cloud.ravendb.net) which is 2 vCPUs and 512MB of RAM. This is TINY when you think about what a production database server might require but I want to show you how fast RavenDB can be given these hardware constraints.

<p>I’ll use <a href="https://github.com/nas5w/imdb-data" target="_blank" rel="nofollow">this sample IMDB dataset</a> that has 50,000 movie reviews and create the same kind of sentiment analysis index to compare the indexing performance on a larger sample size.</p>

The first version will _not_ be using any NuGet packages which will be our baseline. The Map operation just returns the text from each review:

<pre>
  <code class="language-csharp">
  from review in docs
  select new {
    review.Text
  }
  </code>
</pre>

In RavenDB, you can view indexing performance for every index in a [_lot_ of detail](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/12-working-with-indexes#indexing-and-querying-performance). Indexing happens in batches. Think of putting a bunch of documents into a bucket and sending each bucket of documents down the “indexing assembly line.” There’s the time to process each bucket and then the total time to process all the buckets.

In this case, we have 49 batches of 1024 documents each and the total time to rebuild the index is about 2.75s (about 11-12k docs/s):

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image21.png"  class="img-responsive m-0-auto" alt="Studio: indexing performance" />
</div>

Each batch takes about 80ms but you’ll notice in the screenshot that when RavenDB commits to disk (the purple + green bars), sometimes it can take longer, about 500ms for those operations. This index is executed on a very low end machine, so RavenDB needs to process the data in small batches to avoid using too much memory. On bigger machines, RavenDB will be able to use much larger batches and index the documents more efficiently.

Now I’ll use SentimentAnalyzer to calculate the sentiment of each movie review alongside the text:

<pre>
  <code class="language-csharp">
  from review in docs
  select new {
    review.Text,
    Sentiment = Sentiments.Predict(
      review.Text).Prediction ? "Positive" : "Negative"
  }
  </code>
</pre>

This time, it takes a total of 16 seconds to build the index or about 5 times as long. Each batch varies between 500ms to 1s depending on whether it needed to commit to disk:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image18.png"  class="img-responsive m-0-auto" alt="Studio: indexing performance" />
</div>

5X longer sounds a lot worse than it is. Executing any additional code in an index, especially ML.NET analysis, will incur overhead. But this isn’t necessarily “bad.” RavenDB will still respond instantly to queries, but the results [will be stale](https://ravendb.net/docs/article-page/5.1/csharp/indexes/indexing-basics#stale-indexes) until the index is rebuilt. Incremental builds of the index will be much faster.

For example, if I clone a review document to cause the index to update, the LINQ step takes ~1ms for a single document:

<div class="margin-top-sm margin-bottom-sm">
  <img src="images/using-nuget-packages-to-power-up-ravendb-indexes/image13.png"  class="img-responsive m-0-auto" alt="Studio: indexing rebuild performance" />
</div>

<p>The <em>entire rebuild</em> took 700ms to run where the bulk of the time was spent writing and merging to disk. This is important to understand because having a lot of CPU power is great (for the analysis speed) but it’s just as important to use fast storage for your RavenDB database when you are looking for the best performance. The Free tier I’m using for this demo is probably using a <a href="https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/InstanceStorage.html#instance-store-volumes" target="_blank" rel="nofollow">low-end HDD</a> from AWS EC2.</p>

When it comes to indexing overhead, there’s a trade-off between performance and ease of querying. The major advantage we get is that we run the analysis computation on the database server instead of within our application, letting us trade that longer index build time for faster queries on the client-side.

### Conclusion

Play with these new features yourself within the [Live Test Playground](http://live-test.ravendb.net/) or follow the [step-by-step tutorials](https://demo.ravendb.net/) to get started with RavenDB.

[NuGet package support](https://ravendb.net/docs/article-page/5.1/csharp/indexes/additional-assemblies) and [attachment indexing](https://ravendb.net/docs/article-page/5.1/csharp/document-extensions/attachments/indexing) enable new use cases that other databases aren’t able to match. Learn about other powerful features released in RavenDB 5.1 like [Replication and Document Compression](https://ravendb.net/why-ravendb/whats-new).
