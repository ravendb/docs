<p><small class="series-name">Yet Another Bug Tracker: Article #1</small></p>
<h1>NoSQL Data Model Through the DDD Prism</h1>
<small>by <a href="https://alex-klaus.com" target="_blank" rel="nofollow">Alex Klaus</a></small>

<div class="article-img figure text-center">
  <img src="images/nosql-data-model-through-ddd-prism.jpg" alt="Practical modelling of the same database for a traditional SQL and a NoSQL. Comparison of the two approaches and their alignment with the DDD (Domain Driven Design)." class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<p>The <a href="https://github.com/ravendb/samples-yabt" target="_blank" rel="nofollow">Yet Another Bug Tracker</a> (YABT) we're building in this series is a simplified version of a bug-tracking tool that implements some basic functionality. In spite of its simplicity, we leverage the <a href="https://en.wikipedia.org/wiki/Domain-driven_design" target="_blank" rel="nofollow">Domain-driven Design</a> (DDD) to describe independent problem areas as <a href="https://martinfowler.com/bliki/BoundedContext.html" target="_blank" rel="nofollow">Bounded Context</a>, emphasize a common language (<a href="https://martinfowler.com/bliki/UbiquitousLanguage.html" target="_blank" rel="nofollow">Ubiquitous Language</a>) to talk about these problems, and also adopt technical concepts and patterns, like rich models, aggregates, value objects, etc.</p>

But this article is not about the *DDD*, so for more information refer to the gurus:

<ul>
    <li class="margin-top-xs">Vaughn Vernon, "<a href="https://www.amazon.com/Domain-Driven-Design-Distilled-Vaughn-Vernon/dp/0134434420" target="_blank" rel="nofollow">Domain-Driven Design Distilled</a>" and "<a href="https://www.amazon.com/gp/product/B00BCLEBN8" target="_blank" rel="nofollow">Implementing Domain-Driven Design</a>"</li>
    <li class="margin-top-xs">Eric Evans, "<a href="https://www.amazon.com/exec/obidos/ASIN/0321125215" target="_blank" rel="nofollow">Domain-driven design</a>"</li>
    <li class="margin-top-xs">Martin Fowler, his blog at <a href="https://martinfowler.com/tags/domain%20driven%20design.html" target="_blank" rel="nofollow">martinfowler.com</a></li>
</ul>

<p>Luckily for the <em>YABT</em>, the bug-tracking domain is well-known to any dev and well-covered by <a href="https://vaughnvernon.co" target="_blank" rel="nofollow">Vaughn Vernon</a> in his books that is a good source for deeper understanding of our design decisions.</p>

### 1. Bounded context and Domain entities
<hr>

The core model of a bug-tracking system would consist of a *Project*, *Backlog Item*, its *Comments* and of course *Sprint*. Each project has a *Team* linked to *Users* of the system that may bring other entities like subscriptions, payments, permissions, etc.

While the core entities are related to the Scrum processes, `Users` normally would stay outside of the *bounded context* as they are tightly coupled with permissions and tenants that have little to do with Scrum. So it leads us to two bounded contexts:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt1/1.png" class="img-responsive m-0-auto" alt="Bounded contexts"/>
</div>

There is a good practice:

> Implement one database per *Bounded Context* to avoid a monolith architecture.

So, to make the *YABT* simpler, we cut off auxiliary functionality (like user registration, subscriptions, etc.) and implement only the main bounded context. This way we have settled on the following entities:

<ul>
    <li class="margin-top-xs">
        <em>Backlog Item</em> – basic properties of user stories, bugs, features, etc.
    </li>
    <li class="margin-top-xs">
        <em>Custom Field</em> – to spice things up we're adding <em>Backlog Items</em> custom properties managed by the user.
    </li>
    <li class="margin-top-xs">
        <em>Comment</em> – discussions on individual <em>Backlog Items</em>.
    </li>
    <li class="margin-top-xs">
        <em>Sprint</em>.
    </li>
    <li class="margin-top-xs">
        <em>Project</em> – for grouping <em>Backlog Items</em>, <em>Sprints</em> and <em>Users</em>, and providing a context for the user's session.
    </li>
    <li class="margin-top-xs">
        <em>User</em> – represents team members for <em>Projects</em>, registered users with rudimentary user management.
    </li>
</ul>

### 2. Relational DB model
<hr>

Before jumping to the database design, let's step back and talk about how the database could look like if we were using a conventional SQL database. This step is not required for designing a DB, and here it's shown purely for academic purposes.

<p>Wiring up the entities listed above we get a <a href="https://en.wikipedia.org/wiki/Database_normalization" target="_blank" rel="nofollow">normalised</a> relational model:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt1/2.png" class="img-responsive m-0-auto" alt="Normalised relational model"/>
</div>

<p>See an interactive version at <a href="https://dbdiagram.io/d/5f014b310425da461f04411f" target="_blank" rel="nofollow">dbdiagram.io</a>.</p>

The main entities are highlighted in green and their fields are self-explanatory. However, some of the relationships between the tables need to be explained.

<ul>
    <li class="margin-top-xs"><code>[ProjectUsers]</code> reflects roles of the users in projects.</li>
    <li class="margin-top-xs"><code>[RelatedBacklogItems]</code> stores relationships between <em>Backlog Items</em> when some are "<em>blocked</em>" by others or "<em>related</em>" to others. Meanwhile a parent-child relation between items is maintained in <code>[ParentBacklogItemID]</code> field of <code>[BacklogItem]</code>.</li>
    <li class="margin-top-xs"><code>[BacklogItemModificationHistory]</code> answers who? when? did what? on a specific backlog item. <em>Backlog Item</em> attributes like <code>date of creation</code> and <code>last modification date</code> will be resolved from this table.</li>
    <li class="margin-top-xs"><code>[BacklogItemCommentMentionedUsers]</code> stores references to the users in the comments (like "<em>@HomerSimpson please test the functionality</em>"), so we could query <em>Backlog Items</em> requiring attention of the current user (<em>Homer Simpson</em> in this case).</li>
</ul>

Note that the ER diagram shown above is simplified and a few things were taken out:

<ul>
    <li class="margin-top-xs">Secondary fields in the tables. Even a basic set of fields for <code>[BacklogItem]</code> would be much richer to support different item types: <em>features</em>, <em>user stories</em> and <em>bugs</em>.</li>
    <li class="margin-top-xs">No reference tables for the supported types of <em>Backlog Items</em> (already mentioned <em>features</em>, <em>user stories</em> and <em>bugs</em>), user roles in the projects (e.g. <em>admin</em>, <em>ordinary member</em>), types of relationship between <em>Backlog Items</em> (e.g. <em>blocker</em>, <em>related</em>), etc.</li>
</ul>

Clearly that the final ER diagram for a such simple database would be much more cluttered, but you get the picture.

### 3. NoSQL model
<hr>

#### 3.1. Need in DB structure

Do we need a DB structure/model when it comes to *NoSQL*? Well it depends. You may get away without a model if you are a data scientist and dumping terabytes of data for future analysis. However, an enterprise developer needs to know how to present the data (on the API or UI) and how to query the data. And it's the case for the *YABT*.

Of course, in NoSQL a structure can't be implemented just at the database level. JSON records are flexible and there are no data integrity checks. Therefore, NoSQL relies on rules and constrains implemented in the domain logic. It leads to enforcing a good practice:

> Prevent direct access to the DB by the consumers. Always shield the DB with a domain layer as a way of enforcing the domain rules and constrains.

Overexposing the database is a common felony in the SQL realm.

#### 3.2. Constructing aggregates

<p>Above, we have prepared a foundation with a strategic design called <em>Bounded context</em>. Now it's time for tactical design to define details of the domain model. We need <a href="https://martinfowler.com/bliki/DDD_Aggregate.html" target="_blank" rel="nofollow">DDD aggregates</a> that will reflect our denormalised database.</p>

The key properties of *aggregates*:

<ul>
    <li class="margin-top-xs">Aggregates are the basic element of transfer of data storage – you request to load or save whole aggregates.</li>
    <li class="margin-top-xs">Any references from outside the aggregate should only go to the aggregate root.</li>
</ul>

<p>Properly structured <em>Aggregates</em> reduce <code>JOIN</code>s in queries that deteriorate the performance and add fragility for <a href="https://en.wikipedia.org/wiki/Eventual_consistency" target="_blank" rel="nofollow">eventually consistent</a> references.</p>

Jumping to the final diagram, we get this:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/yabt1/3.png" class="img-responsive m-0-auto" alt="Final diagram"/>
</div>

*Note:* For simplicity, this diagram doesn't show all the fields of the final aggregates.

As you see, the `BacklogItem` aggregate is quite rich with many nested collections:

<ul>
    <li class="margin-top-xs"><code>LinkedBacklogItems</code> represents dependencies to other <em>Backlog Items</em> (parent/child, related, blocked, etc.).</li>
    <li class="margin-top-xs"><code>ModifiedBy</code> has a history of all modifications of the <em>Backlog Item</em>. Whenever a user changes a value of field, links to another item, writes a comment, etc. a record will appear in this collection.</li>
    <li class="margin-top-xs"><code>CustomFields</code> has values of custom fields specified by the user.</li>
    <li class="margin-top-xs"><code>Comments</code> –- as a part of the <em>Backlog Item</em>, they will never be queried or edited independently, or referenced outside of the <em>Backlog Item</em>. <code>Comment</code> has a collection of mentioned users that will play its role on querying <em>Backlog Items</em> requiring attention of the current user.</li>
</ul>

All the 5 aggregates have references, and you may have noticed that in some cases it's just an `ID`, in others it's a bundle of 2+ fields (e.g. `ID` and `Name`). There will be a separate article on managing references between aggregates, but as a rule of thumb:

<ul>
    <li class="margin-top-xs">If a reference is used for filtering only, then an <code>ID</code> is sufficient.</li>
    <li class="margin-top-xs">If a reference is going to be exposed to the consumer along with some fields of the referred entity, then those fields need to be included into the reference. This way we avoid excessive <code>JOIN</code>s in queries (e.g. the comment's author has <code>ID</code> and <code>Name</code> that duplicates the name of the referred <code>User</code> record).</li>
</ul>

#### 3.3. Convenience of using RavenDB

<p>Many NoSQL databases have a very modest limit on the document/record size that could affect the decision of bringing the <em>Comments</em> inside of the <code>BacklogItem</code> aggregate. While usually we should expect less than a dozen of comments with the total size in KB rather than in MB, a production system shouldn't shy away of a couple of megabytes of comments. And it's not a concern for <em>RavenDB</em>. Though, technically the document/record can be <a href="https://ayende.com/blog/156865/ravendb-net-memory-management-and-variable-size-obese-documents" target="_blank" rel="nofollow">up to 2GB</a>, the <a href="https://stackoverflow.com/a/45031998/968003" target="_blank" rel="nofollow">optimal size should stay in megabytes</a> that still is more than enough for a <code>BacklogItem</code> with hundreds of comments (that admittedly would be a rare case).

Another limitation of some NoSQL vendors is inability of querying a subset of fields forcing the developer to fetch whole records. It can pose a performance concern if unnecessary fields are heavy, like for our common request of listing 3-4 main fields of <em>Backlog Items</em> and omitting potentially bulky comments. <em>RavenDB</em> gives a way of <a href="https://ravendb.net/docs/article-page/latest/csharp/client-api/session/querying/how-to-project-query-results">grabbing only necessary fields</a>.</p>

Another common concern that may affect the database model is immature indexes preventing certain filtering, grouping and data aggregation. Well, <em>RavenDB</em> has got your back here and we will consider various scenarios involving indexes in the *YABT* series.

That's it.

<p>Check out the full source code at our repository on GitHub - <a href="https://github.com/ravendb/samples-yabt" target="_blank" rel="nofollow">github.com/ravendb/samples-yabt</a> and let us know what you think. Stay tuned for the next articles in the <em>YABT</em> series.</p>

<a href="https://ravendb.net/news/use-cases/yabt-series"><h4 class="margin-top">Read more articles in this series</h4></a>
<div class="series-nav">
    <a href="https://ravendb.net/articles/building-application-with-net-core-and-ravendb-nosql-database">
        <div class="nav-btn margin-bottom-xs">
            <small>‹ Previous in the series</small>
            <strong class="previous">Building an enterprise application with the .NET Core and RavenDB NoSQL database</strong>
        </div>
    </a>
    <a href="https://ravendb.net/articles/hidden-side-of-document-ids-in-ravendb">
        <div class="nav-btn margin-bottom-xs">
            <small>Next in the series ›</small>
            <strong class="next">Hidden Side of Document IDs in RavenDB</strong>
        </div>
    </a>
</div>