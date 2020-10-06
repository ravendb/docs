<h1 class="padding-bottom-sm">A Grown-Up Database for Startups</h1>

![A grown-up database for startups needs minimal supervision to do its task. Developers can put more time and resources bringing their application to production.](images/a-grown-up-database-for-startups.jpg)

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size:24px;">11 ways a database for startups maximizes the value it is designed to deliver.</p>

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">A grown-up database, in my eyes, is one that needs a minimal amount of supervision to do its task. If you need a babysitter at all times to care, monitor, and feed a database, that isn't much of a grown-up.</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Oren Eini</span>
</div>

A startup is like an apple tree. It is merely a seed, something no bigger than your fingernail. It is fragile and needs a lot of help to break through the ground, grow strong, and consistently produce.

The entire enterprise of growing the tree is for one purpose: make apples.

The biggest adversary is the weeds, especially when the tree is still struggling. Weeds sap the nutrients from the tree, making it harder to grow and requiring more resources to expand.

The supporting cast members of your application can be hungry weeds or fertile soil. They can perform their specific role at little to no cost in time or money, or they can demand resources be taken away from your product, terrorizing deadlines, and broadening release cycles.

This is why using the right [open source NoSQL Database](https://ravendb.net/why-ravendb/ease-of-use) is vital to any business run by people with the audacity to dream big and the courage to back it up with hard work.

The right database is designed to make the process of managing data so self-guiding and straightforward that you have more time to get your application into production and your business off the ground.

<h3 class="margin-top-sm">11 Ways a Grown-up Database for Startups proves itself</h3>

Here are 11 Ways a Grown-up Database for Startups proves itself by minimizing diversion from your app while maximizing the value it is designed to deliver:

<h4 class="margin-top-sm">1. The Database must be Schemaless</h4>
<hr style="border-color:rgba(34,37,43,.15);">

Right when you know the least about how your application will work in the real-world, relational databases and many document databases demand you label your data.

A schema is like the cement inside the foundation of a building. It's not so easy to change. The possibility that you might have to change your data model after the first release is real, and it shouldn't cost you.

Once you launch and you can take in more and exciting pieces of information, each new release should not be stalled by having to make changes to your data foundation and test every angle of it.

Using a [schemaless database](https://ravendb.net/articles/save-time-and-money-with-non-relational-database-data-compression), this isn't necessary.

Just state your labels in a document and press start. If you have to add labels, you don't need to restructure anything. You can do it on the fly, reducing your time to market and minimizing developer time between releases.

One of our startup clients said that it took them six months to get to market instead of the twelve months they anticipated had they needed a schema.

<h4 class="margin-top-sm">2. You Need Automatic Indexes</h4>
<hr style="border-color:rgba(34,37,43,.15);">

Indexes can take a long time to develop. Automatic indexing is where your database takes this task off your shoulders and does it for you. For every query, the right NoSQL solution will create its own index. If the index is already there, it will use it. If an already existing index is almost optimal, the database will update the index before executing the query.

<h4 class="margin-top-sm">3. ACID Transactions are a Bare Minimum</h4>
<hr style="border-color:rgba(34,37,43,.15);">

The most fragile part of a startup, aside from the 1.0 version of your project, is your reputation. The worst type of disaster is when it isn't your fault.

The right database must deliver on the promise that when it says your information is available and accurate, it's available and accurate. ACID transactions guarantee this.

It's not just about being ACID. You need to be ACID over multiple documents and throughout your cluster. Today's applications are distributed, and the option to expand that cover of consistency throughout your entire network must be open to you without plugins, addons, or complicated documentation.

ACID data consistency cannot cost you or your customers in performance. A good open source NoSQL database for startups must have at least a decade of experience in being ACID. That gives the developers time to create enough optimizations to keep your data moving at the speed of business.

<div class="margin-top margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="Managed Cloud Hosting"/></a>
</div>

<h4 class="margin-top-sm">4. All the Moving Parts are in One Place</h4>
<hr style="border-color:rgba(34,37,43,.15);">

Full-Text Search. MapReduce Aggregations. A fully loaded UI. Automatic Caching. All of these can cost you in complexity if you have to add them on through external programs. Diverting time on these issues shouldn't be necessary. The right database system has all of this in the box the moment you open it up.

You also want it to be multi-model. One database should be able to handle documents, graph API, and time series. It would help if you didn't have to learn more than one system to enable multiple ways to process different types of information.

<h4 class="margin-top-sm">5. Master-Master Nodes Mean a Real Distributed Network</h4>
<hr style="border-color:rgba(34,37,43,.15)">

What's the point of having multiple nodes for your database when you can only write to a single point? It's essential to keep latency to a minimum by giving all users, from any location, the ability to make the most out of the nearest node.

One of our clients is a gaming company that handles users playing live games from diverse locations like Manchester, England, Beijing, China, and Reno, Nevada. Master-Master reads and writes are essential for making sure that all three players see the same scores, updates, and any other state changes all at the same time. I'd give some meat to the claim by speaking about the cluster's ability to automatically choose the most suitable node for a task.

<h4 class="margin-top-sm">6. Document Data Ingestion to Eliminate Impedance Mismatch</h4>
<hr style="border-color:rgba(34,37,43,.15)">

Programming object-oriented applications using relational databases is like creating a giant cube and trying to fit it into a round hole. For every query, there is lots of work to get the data flowing. It can take weeks, even months, to match your application queries to a relational database.

For every new version, which will include further queries, there is even more work.

When you are on the cloud, querying tables, then putting them together, then querying the entire data set will cost you more time and money than if your data were in the form of document JSON objects.

Document Databases are faster because you don't need to create and merge tables. Queries are cleaner, saving you both time and computing costs when on the cloud.

<h4 class="margin-top-sm">7. Documents Compression to Save on Memory and Cloud Costs</h4>
<hr style="border-color:rgba(34,37,43,.15)">

The one challenge a document database poses is what to do when you have to state your data labels in every single document. That can be a waste of memory. On the cloud, it becomes a waste of money.

The right database solution for startups is one that compresses as many data labels as possible. It learns from the querying history and usage to develop even more compression solutions on the fly. Not only does a good database compress the keys, but also the values. If 500 people have a document that says they became members on January 5, 2021, you can compress that recurring information also.

Our solution saves clients 50% on cloud storage costs and 10% in overall cloud database expenses.

Use the WEB feature-block that explains it: [ravendb.net/features/extensions/documents-compression](https://ravendb.net/features/extensions/documents-compression)

<h4 class="margin-top-sm">8. Shared Instances to Make Getting Started Cheaper than Ordering a Sushi Dinner</h4>
<hr style="border-color:rgba(34,37,43,.15)">

Just because you won't be making the bulk of your revenues in the initial stages of your business doesn't mean you should be paying the majority of your expenses.

Shared instances enable you to put your database on the cloud by using minimal resources. If you don't see yourself using the entire machine Amazon or Microsoft is offering, why pay for it? Shared instances are when you co-space your server with other clients, reducing your total expenses per instance to as little as $30.

If you have one node where your roommate is hogging too much memory, the right data solution will be able to transfer background tasks and workload to other nodes with quieter roommates until things calm down.

Once you decide you are ready for a full server or more, you can provision the resources, and they will be prepared before you finish your sushi.

<h4 class="margin-top-sm">9. Effective Error Handling</h4>
<hr style="border-color:rgba(34,37,43,.15)">

Error handling can be the most significant time expense when prepping your product to market. Your database must be a partner in resolving issues as efficiently and thoroughly at every step in the debugging process.

That means error messages must explain everything in full detail. Documentation must be easy to understand and include what you need to know right away. There should even be links that enable you to email directly to the developers of the database the scenario that caused the error, so you don't need to waste 20 minutes on the phone trying to reproduce it.

<h4 class="margin-top-sm">10. Generous community license package</h4>
<hr style="border-color:rgba(34,37,43,.15)">

Throughout your development process, you should have everything you need to play with your database to see how it fits into your system. A community license should include all the main features the database offers and enable you to get used to them while you are still in development.

It would help to have the UI, a multi-node data cluster, and built-in features like MapReduce and Auto Indexing so you can feel the value of the system from the moment you start using it.

<div>
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner2.jpg" class="img-responsive m-0-auto" alt="Schedule a FREE Demo of RavenDB"/></a>
</div>

<h4 class="margin-top-sm">11. Everything Just Works</h4>
<hr style="border-color:rgba(34,37,43,.15)">

From getting started, to everyday use, to securing your data while in development *and* when you release your product into production, and in meeting the unique challenges of your project, the right database needs the attitude that *everything should just work*.

Everyday use should be straightforward and simple. There should be tons of resources to learn the system, but it should be a system that is easy to use and does as much as possible to enable you to put more of your mind and money to change the world with your next big idea.

Managing the intricate details of every nook and cranny of your database system should be the problem of your database and not your development team.