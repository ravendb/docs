<h1>Data Subscriptions Feature in RavenDB NoSQL Database</h1>
<small>by <a href="mailto:mor@ravendb.net">Mor Hilai</a></small>

<div class="article-img figure text-center">
  <img src="images/data-subscriptions-feature.jpg" alt="Data Subscriptions Article Image" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size: 24px;">Auto data subscriptions allow effortless data transfer with RavenDB subscriptions.</p>

Any database can fetch data in response to a query, and this basic ability is enough to support any complex application you can think of. But at RavenDB, we don't want to just *enable* you to build your application, we want to make it *easy* to build your application, to maintain it, and to make it more robust. Using most databases, you'll have to reinvent the wheel over and over again. At RavenDB we want to give you the wheels, the engine, and the cup holder right out of the box. In addition to our powerful querying capabilities, we offer *Data Subscriptions*.

### The Data Subscriptions Benefits

[Data subscriptions](https://ravendb.net/docs/article-page/5.1/csharp/studio/database/tasks/ongoing-tasks/subscription-task) allow you to automate the transfer of data from the server to the client. Each subscription is a push service that will continuously send batches of documents to a designated client called the `subscription worker`. The worker can process each batch one at a time and send an acknowledgement to the server to receive the next batch. Subscriptions are highly available: if the server fails, another server in the same cluster can begin to serve the subscription. Even in a one-server cluster, the server will pick up where it left off when it comes back online. So while a query receives just one response, a subscription is a channel of communication that stays open indefinitely, and also guarantees that the data arrives at its destination.

A subscription retrieves data that matches a certain query, which in itself offers a lot of options. For example, a RavenDB query can retrieve not just a specified document, but can also dynamically fetch related documents. Subscriptions can also filter and process data according to a script. So when you're building a data pipeline, it doesn't have to do all its work from scratch, you can outsource a lot of the work to RavenDB and build a simpler pipeline from a more convenient starting point.

The subscription is triggered each time documents that meet certain criteria are updated. So rather than configuring your application to check the server for updates, which would require a lot of unnecessary traffic, the process happens automatically as new data enters the database.

### Easier to Run Data Operations on the Client Side

RavenDB keeps close track of all changes made to documents. The subscription feature uses this to ensure that the client receives data in the correct order even if there are interruptions in communication. The server also remembers which batches the client acknowledged, so that the subscription can always continue from the last point it reached, rather than starting over each time.

This feature makes it way easier to run operations on the client side. Rather than hacking together your own failure resistant push service, in a few minutes RavenDB allows you to set up the back end, so you can just focus on your application.