<div class="series-top-nav"><small class="series-name">Yet Another Bug Tracker: Article #4</small>
<a href="https://ravendb.net/news/use-cases/yabt-series"><small class="margin-top">Read more articles in this series ›</small></a></div>
<h1>Entity Relationships in NoSQL (one-to-many, many-to-many)</h1>
<small>by <a href="https://alex-klaus.com" target="_blank" rel="nofollow">Alex Klaus</a></small>

![The power of dynamic fields for indexing dictionaries and nested collections in RavenDB](images/entity-relationships-in-nosql.jpg)

{SOCIAL-MEDIA-LIKE/}

We know how to maintain a relationship between two tables in SQL. Normalise. Add a reference field with a foreign key. When querying data from both tables – `JOIN` the two.

<p>It makes writing normalised records to the DB straightforward and optimises storage space, but it comes with a higher cost of querying the data. All the <code>JOIN</code>s and <code>GROUP BY</code>s on a normalised DB are hammering the disk (on random seeks, table scans, etc.) and RAM with the CPU (on finding matching rows, especially in <a href="https://en.wikipedia.org/wiki/Hash_join" target="_blank" rel="nofollow">hash join</a>).</p>

<p>Considering that querying data is the most often operation against the DB, denormalisation (and NoSQL in general) can be a more attractive option to speed up the queries. Though, it may require more analysis before creating entities and <a href="https://martinfowler.com/bliki/DDD_Aggregate.html" target="_blank" rel="nofollow">aggregates</a>. The recommended steps are:</p>

<ol>
    <li class="margin-top-xs">Learn the domain and document all workflows.</li>
    <li class="margin-top-xs">Understand every access pattern: read/write patterns; query dimensions and aggregations.</li>
    <li class="margin-top-xs">Design the Data Model, which is tuned to the discovered access patterns.</li>
    <li class="margin-top-xs">Review -> Repeat -> Review, as it's unlikely to get it right on the first attempt.</li>
</ol>

Designing aggregates is a huge topic and a previous article of the *YABT* series "[NoSQL Data Model through the DDD prism](https://ravendb.net/articles/nosql-data-model-through-ddd-prism)" might be a good start. But here we focus on organising relationships between aggregates.

As a starting point, let's take an "*one-to-many*" relationship between 3 entities of the *YABT* project: `Backlog Item`, `Comment` and `User`. The traditional relational diagram for them would look like this:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt4/1.png" class="img-responsive m-0-auto" alt="Relational diagram of Backlog Item, Comment and User"/>
</div>

What are the options for these relationships in NoSQL in general and *RavenDB* in particular?

### 1. Embedded collection
<hr style="border-color:rgba(34,37,43,.15);">

Perhaps, the simplest approach would be keeping the referred entities as an embedded collection on the main one.

A classic example would be a collection of addresses, phones or emails for a user:

<pre>
    <code class="language-json" style="background:transparent;">
    {
        "fullName": "Homer Simpson",
        "addresses" : [
            { "street": "742 Evergreen Terrace", "city": "Springfield", "state": "Nevada", "country": "USA" },
            { "street": "430 Spalding Way", "city": "Springfield", "state": "Nevada", "country": "USA" }
        ],
        "emails" : [
            "chunkylover53@aol.com",
            "homer@gmail.com"
        ],
    }
    </code>
</pre>

##### When to use?

<ol>
    <li class="margin-top-xs">
        Items from the embedded collection are not referred anywhere independently from the main collection.<br/>
        E.g. all other entities and aggregates would refer to the users rather than to just their addresses.
    </li>
    <li class="margin-top-xs">
        The embedded collection is not queried independently from the main one.<br/>
        E.g. we won't query phones only without mentioning users.
    </li>
    <li class="margin-top-xs">
        The embedded collection doesn't grow without bound.<br/>
        E.g. there is going to be just a handful of addresses per user.<br/>
        Perhaps, the number of embedded records should remain as less than a hundred for most of the cases. Of course, a hundred is an arbitrary number, but the idea is to avoid performance deterioration on the aggregate caused by heavy nested collections.
    </li>
</ol>

##### How it's used in YABT

This approach would work like a charm for `BacklogItem` and `BacklogItemComment`. Assuming that most of the tickets have a handful of comments and the comments aren't referred or queried outside of the parent entity.

So a `BacklogItem` record with nested comments would look like:

<pre>
    <code class="language-json" style="background:transparent;">
    {
        "title": "Malfunction at the Springfield Nuclear Power Plant",
        "comments": [
            {
                "timestamp": "2020-01-01T10:15:23",
                "message": "Homer, what have you done?"
            },
            {
                "timestamp": "2020-01-01T11:05:10",
                "message": "Nothing, just left my donuts on the big red button."
            }
        ]
    }
    </code>
</pre>

Of course, it wouldn't be a good solution if the comments didn't meet the three conditions described above or simply tended to have bulky messages with KBs or MBs of text. Currently, backlog items in the *YABT* have 3 comments on average with length around 150 symbols. We consider it small enough and keep comments as an embedded collection in `BacklogItem`.

### 2. Reference by ID
<hr style="border-color:rgba(34,37,43,.15);">

The traditional SQL approach of referring records would still work. You can have the main entity/collection with an `ID` value of the referred record that's stored in a separate entity/collection.

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt4/2.png" class="img-responsive m-0-auto" alt="Reference to ID"/>
</div>

Well, what about all the benefits of denormalisation? Speaking for NoSQL in general, there are none and on querying you'll run expensive `JOIN`s to bring properties from the referred entity (some NoSQL servers support such operations on the server side, others would require an application level `JOIN`).

However, *RavenDB* has a trick up its sleeves (or wings) – storing often used fields of the referred entity right in the index, so you won't need to `JOIN` the other collection for fetching those fields. Check out [the official docs](https://ravendb.net/docs/article-page/latest/Csharp/indexes/storing-data-in-index).

##### When to use?

<ol>
    <li class="margin-top-xs">
        The collection is referenced in more than one place.<br/>
        E.g. <em>YABT</em> users are referenced in the backlog items, comments, project, etc.
    </li>
    <li class="margin-top-xs">
        The referenced records get often updated. In this case, denormalisation would cause too much of a burden to find and update all the instances of the modified record.<br/>
        <p>Of course, it all depends on what fields would be duplicated in a denormalised database. For <code>User</code> entity it can be just a form of the name (e.g. <code>NameWithInitials</code>) in addition to the <code>ID</code>. Whether updating the user names is an often operation that's up for debate. If it's the case, then <code>User</code> would be a good candidate for <code>ID</code> references.</p>
    </li>
</ol>

##### How it could be used in YABT

For our bug-tracker, we often want to show user names along with other records. For example, a list of backlog items might have 3 columns for referenced users. Hmm... 3 references would be a stretch but bear with me.

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt4/3.png" class="img-responsive m-0-auto" alt="Backlog Items"/>
</div>

In this case running a triple `JOIN` on `User` entity would be excruciating. Fortunately, an option to [store data in RavenDB indexes](https://ravendb.net/docs/article-page/latest/Csharp/indexes/storing-data-in-index) comes in handy.

For an index like this:

<pre>
    <code class="language-csharp" style="background:transparent;">
    public class BacklogItems_ForList : AbstractIndexCreationTask&lt;BacklogItem&gt;
    {
        public BacklogItems_ForLists()
        {
            Map = tickets => 
                    from ticket in tickets
                    let assignee   = LoadDocument&lt;User&gt;(ticket.AssigneeId)
                    let modifiedBy = LoadDocument&lt;User&gt;(ticket.ModifiedById)
                    let createdBy  = LoadDocument&lt;User&gt;(ticket.CreatedById)
                    select new
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        AssigneeName = assignee.NameWithInitials,
                        CreatedByName = createdBy.NameWithInitials,
                        ModifiedByName = modifiedBy.NameWithInitials,
                    };
            Stores.Add(x => x.AssigneeName, FieldStorage.Yes);   // Stores names of 'Assignee'
            Stores.Add(x => x.CreatedByName, FieldStorage.Yes);  // Stores names of 'CreatedBy'
            Stores.Add(x => x.ModifiedByName, FieldStorage.Yes); // Stores names of 'ModifiedBy'
        }
    }
    </code>
</pre>

All the name changes of the users will be picked up by *RavenDB* and stored in the index. That let us querying a backlog list with no `JOIN`s, as we can refer the index fields along with the main entity:

<pre>
    <code class="language-csharp" style="background:transparent;">
    from b in session.Query&lt;BacklogItems_ForList&gt;
    select new { b.Id, b.Title, b.AssigneeName, b.CreatedByName, b.ModifiedByName }
    </code>
</pre>

##### Costs of storing in index

The index size gets slightly bigger, but the main downside is that the index gets rebuilt on each change in the `User` collection that would consume extra provisioned throughput in the cloud.

### 3. Duplicating often used fields
<hr style="border-color:rgba(34,37,43,.15);">

The read/write ratio plays a significant role in forming denormalised entities and aggregates. When reads overweight writes, you may consider another approach – duplicating some fields of the referred entity in other aggregates.

Let's have another look at the Backlog list. This time a more realistic view that shows names of assigned users.

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt4/4.png" class="img-responsive m-0-auto" alt="Backlog Items"/>
</div>

##### When to use?
<ol>
    <li class="margin-top-xs">
        <em>Read patterns</em> for one collection require a small non-varying set of fields from another collection.<br/>
        <p>As per screenshot above, the backlog view in <em>YABT</em> shows names of assigned users along with the backlog item fields. It's one of the most common requests in the app. The user reference is standard – <code>ID</code> and <code>Name</code> (in a chosen form). We don't expect this field set to vary when querying the backlog (e.g. not bringing other fields like emails to some views).</p>
    </li>
    <li class="margin-top-xs">
        The referenced records DON'T get updated often.<br/>
        <p>Analysis of <em>access patterns</em> for <em>YABT</em> showed that changing user's name is a seldom operation.</p>
    </li>
    <li class="margin-top-xs">
        A delay on updating the duplicated values is acceptable.<br/>
        <p><a href="https://en.wikipedia.org/wiki/Eventual_consistency" target="_blank" rel="nofollow">Eventual consistency</a> helps to reduce operational costs of the database. Hence, it's greatly encouraged. In case of <em>YABT</em>, it's not an issue if propagating a name change takes a couple of seconds.</p>
    </li>
</ol>

In addition, there can be a combination of smaller contributing factors swinging you towards data duplication vs referencing users by ID:

<ul>
    <li class="margin-top-xs">A requirement to bring the field set from another entity two more than one index. It would amplify the downsides of referencing by ID.</li>
    <li class="margin-top-xs">Often requests to the items by ID that can't leverage stored fields in the index.</li>
</ul>

##### How it's used in YABT

To optimise the query for a list of backlog items, we duplicate user's names in the `BacklogItem` aggregate. This way all the data in the backlog view is coming from the `BacklogItem` collection.

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt4/5.png" class="img-responsive m-0-auto" alt="Duplicating fields"/>
</div>

It's a clear win on the read side. But what about keeping the duplicated values in sync with the corresponding `User` records?

##### Propagating the change to other collections

Now to the interesting part. A user has changed the name (a rare event, but we still account for that) and it's a dev's responsibility to apply the change to all the backlog items where the user is the assignee, so the data becomes eventually consistent. The implementation would heavily depend on the type of NoSQL server you're dealing with.

In RavenDB, you would

<ol>
    <li class="margin-top-xs">Persist the changes in the <code>User</code> record (e.g. by calling <code>IAsyncDocumentSession.SaveChangesAsync()</code>).</li>
    <li class="margin-top-xs">Run a <a href="https://ravendb.net/docs/article-page/latest/csharp/client-api/operations/patching/set-based">Set Based Operation</a> on the <code>BacklogItem</code> collection updating the <code>Assignee</code> fields where applicable.</li>
</ol>

The key features of *Set Based Operation* include:

<ul>
    <li class="margin-top-xs">allows you to run a logic what changes records entirely on the <em>RavenDB</em> server-side;</li>
    <li class="margin-top-xs">waiting till completion of the operation is optional (and if waiting, you <a href="https://ravendb.net/docs/article-page/latest/csharp/client-api/operations/patching/set-based#process-patch-results-details">get some handy stats</a> at the end);</li>
    <li class="margin-top-xs">can wait till the index catches up when the patching query is based on an index.</li>
</ul>

In our example it will be updated by a static index:

<pre>
    <code class="language-csharp" style="background:transparent;">
    public class BacklogItems_ForList : AbstractIndexCreationTask&lt;BacklogItem&gt;
    {
        public BacklogItems_ForList()
        {
            Map = tickets =>
                select new
                {
                    ...
                    AssignedUserId = ticket.Assignee.Id,   // the ID of the assigned user
                    ...
                };
        }
    }
    </code>
</pre>

So to update the references to

<pre>
    <code class="language-csharp" style="background:transparent;">
    var newUserReference = new { Id: "users/1-A", Name: "H. Simpson" };
    </code>
</pre>

the operation will look like

<pre>
    <code class="language-csharp" style="background:transparent;">
    var operation = store
        .Operations
        .Send(new PatchByQueryOperation(new IndexQuery
        {
            Query = @"from index 'BacklogItems/ForList' as i
                    where i.AssignedUserId == $userId
                    update
                    {
                    i.Assignee = $userRef;
                    }",
            QueryParameters = new Parameters
            {
                { "userId", newUserReference.Id },
                { "userRef", newUserReference },
            }
        })
    );
    </code>
</pre>

##### Downsides

<ul>
    <li class="margin-top-xs">
        RavenDB doesn't do concurrency checks during the operation so if during the run a backlog item has changed the assignee, then the its ID and name might be overwritten.<br/>
        <p>It can be mitigated by checking additional conditions before modifying a record (e.g. a timestamp of last modification).</p>
    </li>
    <li class="margin-top-xs">
        The logic inside the operation can leverage <em>JavaScript</em> and be very powerful that also might lead to a run-time error. If you don't wait on completion of the operation, then you don't get notified about the error (until you check RavenDB server logs).<br/>
        <p>The mitigation strategy would include:</p>
        <ul>
            <li class="margin-top-xs">wrapping the fragile logic in a <code>try/catch</code> if you can handle errors gracefully;</li>
            <li class="margin-top-xs">add a good test coverage to boost your confidence in the code;</li>
            <li class="margin-top-xs">if the above is not enough, check the <em>Data subscriptions</em> mentioned below.</li>
        </ul>
    </li>
</ul>

### 4. Many-to-many relationship (array of references)
<hr style="border-color:rgba(34,37,43,.15);">

Once you get a handle on *one-to-many* relationships in NoSQL, stepping up to *many-to-many* is trivial. The trick is in keeping an array of references on either side.

Consider our example with `BacklogItem` and `User` collections for maintaining a list of users who modified backlog items. There are two options:

<ol>
    <li class="margin-top-xs"><code>User</code> has an array of backlog items modified by the user;</li>
    <li class="margin-top-xs"><code>BacklogItem</code> has an array of users who've ever modified it.</li>
</ol>

Location of the array is determined by the most common direction of querying. For *YABT*, the second option suits better, as we more often present users in the context of backlog items than the other way around. This way the `BacklogItem` would have an array of user IDs like this:

<pre>
    <code class="language-json" style="background:transparent;">
    {
        "ModifiedBy": [
            "users/1-A",
            "users/3-A"
        ],
    }
    </code>
</pre>

and an index

<pre>
    <code class="language-csharp" style="background:transparent;">
    public class BacklogItems_ForList : AbstractIndexCreationTask&lt;BacklogItem&gt;
    {
        public BacklogItems_ForList()
        {
            Map = tickets =>
                select new
                {
                    ...
                    ModifiedBy = ticket.ModifiedBy,
                    ...
                };
        }
    }
    </code>
</pre>

So we can query backlog items modified by a `userId`:

<pre>
    <code class="language-csharp" style="background:transparent;">
    from b in session.Query&lt;BacklogItems_ForList&gt;
    where b.ModifiedBy.Contains(userId)
    select new { b.Id, b.Title }
    </code>
</pre>

Of course, there are many factors affecting the implementation. For example, *YABT* needs to sort backlog items by the date of modifications, so an array is not enough and we opted for a `{ ID: DateTime }` dictionary. This case is described in a previous article -
[Power of Dynamic fields for indexing dictionaries and nested collections in RavenDB](https://ravendb.net/articles/dynamic-fields-for-indexing).

### Data subscriptions (special case)
<hr style="border-color:rgba(34,37,43,.15);">

Perhaps this one falls out of the scope of managing relationships, but it's remotely related to the topic. If you have

<ul>
    <li class="margin-top-xs">a complex logic that needs to be executed on an event (e.g. on updating a record) and</li>
    <li class="margin-top-xs">it can't be described in a server-side JavaScript (needs to be processed on the client-side),</li>
</ul>

then [Data subscriptions](https://ravendb.net/docs/article-page/latest/Csharp/client-api/data-subscriptions/what-are-data-subscriptions) is your tool. They provide a way of queueing messages and processing them asynchronously on the client-side.

Examples of applying *Data subscriptions* vary a lot, e.g. a system distributed in many regions may require a [cluster-wide transaction](https://ravendb.net/docs/article-page/latest/csharp/server/clustering/cluster-transactions) to apply a change.

But it's a different story. If interested, check out this video – [Using RavenDB as a queuing infrastructure](https://ravendb.net/learn/webinars/using-ravendb-as-a-queuing-infrastructure).

### That's it.

<p>Check out the full source code at our repository on GitHub – <a href="https://github.com/ravendb/samples-yabt" target="_blank" rel="nofollow">github.com/ravendb/samples-yabt</a> that contains practical implementations of all the approaches discussed here. And let us know what do you think. Stay tuned for the next articles in the <em>YABT</em> series.</p>

<a href="https://ravendb.net/news/use-cases/yabt-series"><h4 class="margin-top">Read more articles in this series</h4></a>
<div class="series-nav">
    <a href="https://ravendb.net/articles/dynamic-fields-for-indexing">
        <div class="nav-btn margin-bottom-xs">
            <small>‹ Previous in the series</small>
            <strong class="previous">Dynamic Fields for Indexing</strong>
        </div>
    </a>
    <div class="nav-btn disabled margin-bottom-xs">
        <small>Next in the series ›</small>
        <strong class="next">Coming soon</strong>
    </div>
</div>