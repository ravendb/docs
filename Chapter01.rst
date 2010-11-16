Chapter 1 - NoSQL? What is that?
*********************************************

.. contents:: In this chapter...
  :depth: 3

In the beginning there was the data. And the Programmer put it in memory, and it was so. And on the second day,
it was discovered that this data should be persisted, and then there was a file. And on the third day, the customer
wanted searching, and that was the first database.

This book is about RavenDB, a document database written in .NET. But I don't think that I can accurately talk about 
RavenDB without first discussing some of the history of data storage in the IT field for the last half a century or 
so.

At first, data was simply stored in files, with each application having their own proprietary format. That quickly
became a problem, since it was soon discovered that users have a lot of interesting requirements, such as being able
to retrieve the data, search it, read reports about it, etc.

I distinctly remember learning how to do file IO by writing my own PhoneBook application and doing binary read/writes
from the disk. And probably the hardest part was having to write the search routine. I ended up having to do a 
sequential scan over the entire file for each search, and having to write custom code for each and every search 
permutation that was required. Not surprisingly, developers facing the same solution at the dawn of computing quickly
sought ways to avoid having to do this over & over again.

The first steps toward what we consider a database today were the ISAM (Indexed Sequential Access Method) files. Which
are simply a way to store data in files with indexing. The problem with those cropped up when you wanted to do a bit 
more than just accessing the data, in particular, aggregations. That was the point when data storage grew out of files
and into data management libraries and systems. The next step was Edgar Codd's paper: "A Relational Model of Data for 
Large Shared Data Banks".

And from that point on, an absolute majority of the industry has been focused almost exclusively on relational 
databases. For a very long time, data storage *was* putting things in a database.

Until very recently, in fact...

How we got this NoSQL thing?
============================

Probably the worst thing about relational databases is that they are so *good* in what they are doing. Good enough to 
conquer the entire market on data storage and hold it for decades.

Wait! That is a *bad* thing? How?

It is a bad thing because relational databases are appropriate for a wide range of tasks, but not for *every* task. 
Yet it is exactly that which caused them to be used in contexts where they are not appropriate. In the last month 
alone (March 2010), my strong recommendation for two different clients was that they need to switch to a non 
relational data store because it would greatly simplify the work that they need to do.

That met with some (justified) resistance, predictably. Most people equate data storage with RDBMS, there *is* no other
way to store data. Since you are reading this book, you are probably already aware that there are other options out 
there for data storage. But you may not be certain why you might want to work with a No SQL database. Before we can 
answer that question, we have to revisit some of our knowledge about RDBMS first. We have to understand what it is that
made RDBMS the de facto standard for data storage for so long, and why there is such a fuss around the alternatives for
RDBMS.

Relational Databases have the following properties:

* ACID (Atomic, Consistent, Isolated, Durable)
* Relational (based on relation algebra and the work by Edgar Codd)
* Table / Row based
* Rich querying capabilities
* Foreign keys
* Schema

Just about any of the No SQL approaches give up on some of those properties, sometimes, a NoSQL solution will gives 
up on *all* of those properties. Considering how useful an RDBMS is, and how flexible it turned out to be for so long.
Why should we give all of those advantages?

The most common reason to want to move from a RDBMS is running into the RDBMS limitations. In short, `RDBMS doesn't scale
<http://adamblog.heroku.com/past/2009/7/6/sql_databases_dont_scale/>`_. 
Actually, let me phrase that a little more strongly, RDBMS systems *cannot* be made to scale [1]_.

The problem is inherit into the basic requirements of the relational database system, it *must* be consistent,  to 
handle things like foreign keys, maintain relations over the entire data set, etc. The problem, however, is that trying to 
scale a relational database over a set of machines. At that point, you run head on into the `CAP theorem 
<http://www.julianbrowne.com/article/viewer/brewers-cap-theorem>`_, which state that you can have only two of the 
Consistency, Availability and Partition Tolerance triad. Hence, if consistency is your absolute requirement, you 
need to give up on either availability or partition tolerance.

In most high scaling environments, it is not feasible to give up on either option, you *have* to give up on consistency. 
But RDBMS will not allow that so relational databases are out. That leave you with two basic choices:

* Use an RDBMS, but instead of having a single instance across multiple nodes, treat each instance as an independent 
  data store. This approach is called sharding, and we will discuss how it applies to RavenDB in :ref:`Chapter 7 
  <Chapter07>`. The problem with this approach with RDMBS is that you lose a lot of the capabilities that RDBMS 
  brings to the table (you can't join between nodes, for example).
* Use a No SQL solution.

What it boils down to is that when you bring the need to scale to multiple machines, the drawbacks of using a RDBMS 
( TODO: provide a full list) out weight the benefits that it usually brings to the table. Since we have to do a lot
of work already with sharded SQL databases, it is worth turning out attention to the NoSQL alternatives, and what 
we might want to choose them. This book is about RavenDB, a Document Database, but I want to give you at least some
background on each of the common NoSQL databases types, before starting to talk about RavenDB specifically.


NoSql Data stores
===================

I am going to briefly touch on each NoSQL data store, from the developer perspective (what kind of API and interfaces 
the data store have), and from the scaling perspective, to see how we can scale our solution. This isn't a book about
NoSQL solutions in general, but it is important to understand who are the other players in the fields when it comes 
the time to evaluate options your data storage strategy. 

Almost all data stores need to handle things like:

* Concurrency
* Queries
* Transactions
* Schema
* Replication
* Scaling

One thing that should be made clear up front is the major difference between performance and scalability, the two are 
often at odds and usually increasing one would decrease the other. For performance, we ask: How can we execute the same
set of requests, over the same set of data with:

* shorter time?
* fewer resources usage (for example, less memory)?

Note that here, too, there is usually a tradeoff between resource usage and processing time. In general, you can cut 
the processing time by consuming more resources (for example, by adding a cache). Conversely, you can reduce resource 
usage by increasing the processing time (compute as needed, instead of precomputing results).

For scaling, we ask: How can we meet our SLA when:

* we get a *lot* more data?
* we get a *lot* more requests?

With relational databases, the answer is usually, you don't scale. The No SQL alternatives are generally quite simple to 
scale, however.

.. sidebar:: Data access strategy follows the data access pattern

  One of the most common problems that I find when reviewing a project is that the first step (or one of them) was to build
  the Entity Relations Diagram, thereby sinking a large time/effort commitment into it before the project really
  starts and real world usage tells us what sort of data we actually need and what is the data access pattern of the
  application.
  
  One of the major problems with this approach is that it simply doesn't work with NoSQL solutions. An
  RDBMS allows very flexible querying, so you can sometimes get away with this approach (although it is generally
  discouraged when using RDBMS as well), but NoSQL solutions often require you to query / access the data only in pre
  defined manner (for example, key/value stores allows only access by key). This means that the structure of your data is
  usually going to be dictated by the way that you are going to access it. This is usually a surprise for people coming from 
  the RDBMS world, since it is the inverse of how you usually model data in RDBMS.
  
  We will discuss modeling techniques for a document database in :ref:`Chapter 2 <Chapter02>`.

Key/Value Stores
----------------
The simplest No SQL databases are the Key/Value stores. They are simplest only in terms of their API, because the 
actual implementation may be quite complex. But let us focus on the API that is exposed to by most key/value stores 
first. Most of the Key/Value stores expose some variation on the following API::

	void Put(string key, byte[] data);
	byte[] Get(string key);
	void Remove(string key);

There are many variations, but that is the basis for everything else. A key value store allows you to store values by 
key, as simple as that. The value itself is just a blob, as far as the data store is concerned, it just stores it, 
it doesn't actually care about the content. In other words, we don't have a data stored defined schema, but a client 
defined semantics for understanding what the values are. The benefits of using this approach is that it is very simple 
to build a key value store, and that it is very easy to scale it. It also tend to have great performance, because the 
access pattern in key value store can be heavily optimized.

In general, most key/value operations can be performed using O(1), regardless of how many machines there are in the 
data stores and regardless of how much data is stored.

Concurrency
^^^^^^^^^^^^
In Key/Value Store, concurrency is only applicable on a single key, and it is usually offered as either optimistic 
writes or as eventually consistent. In highly scalable systems, optimistic writes are often not possible, because of 
the cost of verifying that the value haven't changed (assuming the value may have replicated to other machines), there 
for, we usually see either a key master (one machine own a key) or the eventual consistency model, which is discussed 
below.

Queries
^^^^^^^
There really isn't any way to perform a query in a key value store, except by the key. Some key/value stores allow 
range queries on the key, but that is rare. Most of the time, queries on key/value stores are implemented by the user, 
using a manually maintained secondary index. 

Transactions 
^^^^^^^^^^^^^
While it is possible to offer transaction guarantees in a key value store, those are usually only offer in the context
of a single key put. It is possible to offer those on multiple keys, but that really doesn't work when you start 
thinking about a distributed key/value store, where different keys may reside on different machines. Because of that, 
it is typically best to think about key/value stores as allowing transaction on a single key put on a single machine.

Please note that transactions do *not* imply ACID. In a distributed key/value store, the only way to ensure that is if 
a key can reside on a single machine. However, we usually do not want that, we want each key to live on multiple 
machines, to avoid data loss / data unavailability if a node goes down for some reason. We discuss this model (also 
call eventual consistent key/value store) below.

Schema
^^^^^^^
Key/value stores have the following schema Key is a string, Value is a blob. Which is probably not a very useful schema 
for your purposes. Beyond that, the client is the one that determines how to deal the data. The key/value store just 
stores it.

Scaling Up
^^^^^^^^^^
In Key Value stores, there are two major options for scaling, the simplest one would be to shard the entire key space. 
That means that keys starting in A go to one server, while keys starting with B go to another server, and so on. In 
this system, a key is only stored on a single server. That drastically simplify things like transactions guarantees, 
but it expose the system for data loss if a single server goes down. At this point, we introduce replication, which 
gives us safety from data loss, but also force us to give up on ACID guarantees.

Replication
^^^^^^^^^^^^
In key value stores, the replication can be done by the store itself or by the client (writing to multiple servers). 
Replication also introduce the problem of divergent versions. In other words, two servers in the same cluster think 
that the value of key "ABC" are two different things. Resolving that is a complex issue, the common approaches are to 
decide that it can't happen (Scalaris) and reject updates where we can't ensure non conflict or to accept all updates
and ask the client to resolve them for us at a later date (Amazon Dynamo, Rhino DHT).

Eventually consistent key/value stores
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
A system which decides that divergent versions of the same key should be avoided will reject updates if such a scenario 
may happen. Following the CAP theorem, it means that we give up Partition Tolerance. The problem is that in most cases,
you really can't assume that your network won't be partitioned. If that happen (and it happens quite frequently) and 
you choose the reject divergent updates mode, you can no longer accept writes, rendering you unavailable. 

To avoid this problem, there is a different model, of allowing divergent writes and let the client resolve the conflict 
when the partition is resolves and the conflict is detected. We discuss exactly this problem in detail in 
:ref:`Chapter 8, Replication <Chapter08>`.

Common Usages
^^^^^^^^^^^^^^
Key/Value stores shine when you need to access the data by key. User related data, such as the session or shopping cart 
information are ideal, because we always know what the user id is. Another common usage is to store pre-compute data 
based on the primary key. For example, we may want to store all the information about a product (including related 
products, reviews, etc) in a key/value store based on the product SKU. That allows us to query all the relevant data 
about a product in an O(1) manner. Because key based queries are practically free, by structuring our data access 
along keys, we can get significant performance benefit by structuring our applications to fit that need. It turns out 
that there is quite a lot that you can do with just key/value store. Amazon's shopping cart runs on a key value store 
(Amazon Dynamo), so I think you can surmise that this is a highly scalable technique.

:ref:`Amazon Dynamo Paper
<http://s3.amazonaws.com/AllThingsDistributed/sosp/amazon-dynamo-sosp2007.pdf>` is one of the best resources on the
topic that one can ask for.
:ref:`Rhino DHT <http://github.com/hibernating-rhinos/rhino-dht>` is a scalable, redundant,
zero config, key value store on the .NET platform.


Just remember, if you need to do things more complex than just access a bucket of bits using a key, you probably need 
to look at something else, and the logical next step in the chain in the Document Database.

Document Databases
------------------

A document database is, at its core, a key/value store where the value is in a known format. A document db requires 
that the data will be store in a format that the database can understand. The format can be XML, JSON (JavaScript 
Object Notation), Binary JSON (BSON), or just about anything, as long as the database can understand the document 
internal structure. In practice, most document databases uses JSON (or BSON) or XML.

Why is this such a big thing? Because when the database can understand the format of the data that you send it, it 
can now do server side operations on that data. In most document databases, that means that we can now allow queries on
the document data. The known format also means that it is much easier to write tooling for the database, since it is 
possible to show, display and edit the data.

I am going to use RavenDB as the example for this post. Documents in RavenDB use the JSON format, and each document 
contains both the actual data and additional metadata information about the document that is external to the document 
itself. Here is an example of a document::

  { 
    "name": "ayende", 
    "email": "ayende@ayende.com", 
    "projects": [ 
        "rhino mocks",

        "nhibernate", 
        "rhino service bus", 
        "raven db", 
        "rhino persistent hash table", 
    
   "rhino distributed hash table", 
        "rhino etl", 
        "rhino security", 
        "rampaging rhinos" 
    ]

  }

We can *put* this document in the database, under the key "ayende". We can also *get* the document back by using
the key "ayende". A document database is schema free, you don't have to define your schema ahead of time and adhere 
to that. This allows us to store arbitrarily complex data. If I want to store trees, or collections, or dictionaries, 
that is quite easy. In fact, it is so natural that you don't really think about it.

It does not, however, support relations. Each document is standalone. It can refer to other documents by store their 
key, but there is nothing to enforce relational integrity.

The major benefit of using a document database comes from the fact that while it has all the benefits of a key/value 
store, you aren't limited to just querying by key. By storing information in a form that the database
can understand, we can ask the server to do things for us, such as querying. The following HTTP request will find all 
documents where the name equals to `ayende`::
  
  GET /indexes/dynamic?query=name:ayende
  
Because the document database understand the format of the data, it can answer queries like that. Being able to 
perform queries is just one advantage of the database being able to understand the data, it also allows:

* Projecting the document data into another form.
* Running aggregations over a set of documents.
* Doing partial updates (*patching* a document)

From my point of view, though. The major benefit is that you are dealing with documents. There is little or
no impedance mismatch between objects and documents. That means that storing data in the document database is usually
significantly easier than when using an RDBMS for most non trivial scenarios. It is usually quite painful to design a
good physical data model for an RDBMS, because the way the data is laid out in the database and the way that we think
about it in our application are drastically different. Moreover, RDBMS has this little thing called Schemas. And
modifying a schema can be a painful thing indeed, especially if you have to do it on production an on
multiple nodes.

The schema less nature of a document database means that we don't have to worry about the shape of the
data we are using, we can just serialize things into and out of the database. It helps that the commonly used format
(JSON) is both human readable and easily managed by tools.

A document database doesn't support relations, which means that each document is independent. That makes it much easier
to shard the database than it would be in a relational database, because we don't need to either store all 
relations on the same shard or support distributed joins.  

I like to think about document databases as a natural candidate for Domain Driven Design applications. When using a
relational database, we are instructed to think in terms of Aggregates and always go through an aggregate.
The problem with that is that it tends to produce very bad performance in many instances, as we need to traverse 
the aggregate associations, or specialized knowledge in each context. With a document database, aggregates are quite 
natural, and highly performant, they are just the same document, after all.

Standard modeling technique for a document database is to think in terms of aggregates, in fact. We discuss this 
in depth in the :ref:`next chapter <Chapter02>`.

Concurrency
^^^^^^^^^^^^
In most document stores, concurrency is only applicable on a single document,
and it is usually offered as optimistic writes. For document databases that also have replication support, we have to
deal with the same potential conflicts that arise when using eventual consistency key/value store, and we resolve them
in much the same way. But letting the client decide how to merge all the conflicting versions. We discuss this in more
detail in :ref:`Chapter 8, Replication <Chapter08>`.


Queries
^^^^^^^
There really isn't any way to perform a query in a key value store, except by the key. Some key/value stores allow 
range queries on the key, but that is rare. Most of the time, queries on key/value  stores are implemented by the user, 
using a manually maintained secondary index.


Transactions 
^^^^^^^^^^^^^
Most document databases will offer you transaction support for the a single document.
RavenDB supports multi document (and multi node) transactions, but even so, it isn't recommended for common use,
because of the potential for issues when using distributed transactions.

Schema
^^^^^^^
Document databases doesn't have a schema per-se, you can store any sort of document inside them. The only limitation 
is that the document must be in a format that the database understands (usually JSON).
Note, however, that for while document databases allows arbitrary schema for documents, for practical purposes, 
indexes (or views) in document database does allow you to threat some part of the data in a more formal way.
We discuss indexes in detail in :ref:`Chapter 4 - RavenDB Indexes <Chapter04>`.

Scaling Up
^^^^^^^^^^
The common approach for scaling a document store is using sharding. Since each
document is independent, document databases lends themselves easily to sharding. Usually sharding is combined with
replication support to handle  fail over in case of node failure, but that is about as complex as it gets. We discuss
sharding strategies for RavenDB in :ref:`Chapter 7 - Scaling RavenDB <Chapter07>`.

Common
Usages
^^^^^^^^^^^^^^
Document databases are usually used to store entities (more accurately, aggregates). There is
very little effort involved in turning an object graph to a document, and vice versa. And aggregates plays very well
with both document databases and Domain Driven Design principles.Examples for the type of data that would be stored in
a document database include blog posts and discussion threads, product catalogs, orders and similar entities.

Graph
Databases
---------------

Think about a graph database as a document database, with a special type of documents,
relations. An common example would be a social network, such as the one shown in figure 1.1. 

.. figure:: _static/GraphDb.png 
  :alt: An example of nodes in a graph database
  
  Figure 1.1 - An example of nodes in a graph database

There are four documents and three relations in this example. Relations in a graph database are more than
just a pointer. A relation can be unidirectional or bidirectional, but more importantly, a relation is typed,
I may be associated to you in several ways, you may be a client, family or my alter ego. And the relation itself can 
carry information. In the case of the relation document in figure 1.1 above, we simply record the type 
of the association and the degree of closeness.

And that is about it, mostly. Once you think about graph databases as document databases
with a special document type, you are pretty much done. Except that graph database has one additional quality that make
them very useful. 
They allow you to perform graph operations. The most basic graph operation is *traversal*. For example, let us say that
I want to know who of my friends is in town so I can go and have a drink. That is pretty easy to do, right? 
But what about indirect friends? Using a graph database, I can define the following query::

  new GraphDatabaseQuery
  {
     SourceNode = ayende,
     MaxDepth = 3,
     RelationsToFollow = new[]{"As Known As", "Family", "Friend", "Romantic", "Ex"},
     Where = node => node.Location == ayende.Location,      
     SearchOrder = SearchOrder.BreadthFirst
  }.Execute();
  
I can execute more complex queries, filtering on the relation properties,
considering weights, etc. Graph databases are commonly used to solve network problems. In fact, most social networking
sites use some form of a graph database  to do things like "You might know...". 

Because graph databases are intentionally design to make sure that graph traversal is cheap, they also provide 
other operations that tend to be very expensive without it. For example, Shortest Path between two nodes. That turn 
out to be frequently useful when you want to do things like: Who can recommend me to this company's CTO so they would 
hire me.

One problem with scaling graph databases is that it is *very* hard to find an independent sub graph, which means that 
it is very hard to shard graph databases. There are several effort currently in the academy to solve this problem, 
but I am not aware of any reliable solution as of yet.

Column family databases (BigTable)
----------------------------------
Column family databases are probably most known because of Goggle's BigTable implementation. The are very similar on 
the surface to relational database, but they are actually quite different beast. Some of the difference is 
storing data by rows (relational) vs. storing data by columns (column family databases). But a lot of the difference 
is conceptual in nature. You can't apply the same sort of solutions that you used in a relational form to a column 
database.

That is because column databases are not relational, for that matter, they don't even have what a RDBMS advocate would
recognize as tables. The following concepts are critical to understand how column databases work:

* Column family
* Super columns
* Column

Columns and super columns in a column database are spare, meaning that they take exactly 0 bytes if they don't have a 
value in them. Column families are the nearest thing that we have for a table, since they are about the only 
thing that you need to define up front. Unlike a table, however, the only thing that you define in a column family is 
the name and the key sort options (there is no fixed schema).

Column family databases are probably the best proof of leaky abstractions. Just about everything in CFDB (as I'll call 
them from now on) is based around the idea of exposing the actual physical model to the users so they can make 
efficient use of that.

* Column families - A column family is how the data is stored on the disk. All the data in a single column family will 
  sit in the same file (actually, set of files, but that is close enough).  A column family can contain super columns or columns.
* Super columns - A super column can be thought of as a dictionary, it is a column that contains other columns (but not
  other super columns).
* Column - A column is a tuple of name, value and timestamp (I'll ignore the timestamp and treat it as a key/value pair
  from now on).

It is important to understand that when schema design in a CFDB is of outmost importance, if you don't build your 
schema right, you literally can't get the data out. CFDB usually offer one of two forms of queries, by key or by 
key range. This make sense, since a CFDB is meant to be distributed, and the key determine where the actual physical
data would be located. This is because the data is stored based on the sort order of the column family, and you 
have no real way of changing the sorting (except choosing between ascending or descending).

The sort order, unlike in a relational database, isn't affected by the columns values, but by the column *names*.

Let assume that in the Users column family, in the row with the key ``@ayende``, we have the column named
``name`` set to "Ayende Rahien" and the column named ``location`` set to "Israel". The CFDB will physically sort them
like this in the Users column family file::

  @ayende/location = "Israel"
  @ayende/name = "Ayende Rahien"
  
This is because the column *name* ``location`` is lower than the column name ``name``. If we had a super column 
involved, for example, in the Friends column family, and the user "@ayende" had two friends, they would be physically 
stored like this in the Friends column family file::

  @ayende/friends/arava= 945
  @ayende/friends/rose = 14

This property is quite important to understanding how things work in a CFDB. Let us imagine the twitter model, as our 
example. We need to store: users and tweets. We define three column families:

* Users - sorted by UTF8
* Tweets - sorted by Sequential Guid
* UsersTweets - super column family, sorted by Sequential Guid

Let us create the user (a note about the notation:
I am using named parameters to denote column's name & value here. The key parameter is the row key, and the column
family is Users)::

  cfdb.Users.Insert(key: "@ayende", name: "Ayende Rahien", location: "Israel", profession: "Wizard");

You can see a visualization of how this row looks like in figure 1.2. Note that this doesn't look at all
like how we would typically visualize a row in a relational database.

.. figure:: _static/ColumnFamilyDb1.png
  :alt: A representation of a row in a Column Family Database
  
  Figure 1.2 - A representation of a row in a Column Family Database
  
  
.. figure:: _static/ColumnFamilyDb2.png
  :alt: A representation of two tweets in a Column Family Database
  
  Figure 1.3 - A representation of two tweets in a Column Family Database

Now let us create a tweet::

 
  var firstTweetKey = "Tweets/" + SequentialGuid.Create();
  cfdb.Tweets.Insert(key: firstTweetKey, application: "TweekDeck", text: "Err, is this on?", private: true);

  var secondTweetKey = "Tweets/" + SequentialGuid.Create();
 
  cfdb.Tweets.Insert(key: secondTweetKey, app: "Twhirl", version: "1.2", text: "Well, I guess this is my mandatory hello world", public: true);

Those value are visualized in figure 1.3. There are several things to notice in the figure:

* The actual key value doesn't matter, but it *does* matter that it is sequential, because that will allow us to sort 
  of it later. 
* Both rows have different data columns on them, because we don't have a schema for the column family. 
* We don't have any way to associate a user to a tweet.

That last bears some talking about. In a relational database, we would define a column called UserId, and that would 
give us the ability to link back to the user. Moreover, a relational database will allow us to query the tweets 
by the user id, letting us get the user's tweets. A CFDB doesn't give us this option, there is no way to query by 
column value. For that matter, there is no way to query by column (which is a familiar trick if you are using something
like Lucene).

Instead, the only thing that a CFDB gives us is a query by key. In order to answer that question, we need to create a 
secondary index, which is where the UsersTweets column family comes into play::

  cfdb.UsersTweets.Insert(key: "@ayende",  timeline: { SequentialGuid.Create(): firstTweetKey } );
  cfdb.UsersTweets.Insert(key: "@ayende",  timeline: { SequentialGuid.Create(): secondTweetKey } );


Figure 1.4 visualize how it looks like in the database. We insert into the UsersTweets column family, to the row with
the key: "@ayende", to the super column ``timeline`` two columns, the name of each column is a sequential guid, which
means that we can sort by it. What this actually does is create a single row with a single super column, holding two
columns, where each column name is a guid, and the value of each column is the key of a 
row in the Tweets table.

.. figure:: _static/ColumnFamilyDb3.png
  :alt: A representation of secondary index, connecting users & tweets, in a Column Family Database
  
  Figure 1.4 - A representation of secondary index, connecting users & tweets, in a Column Family Database
  
.. note::
  Couldn't we create a super column in the Users' column family to store the relationship?

  
  We could, except that a column family can contain either columns or super columns, it cannot contain both.
  
In order to get tweets for a user, we need to execute::

  var tweetIds = 
       cfdb.UsersTweets.Get("@ayende")
      .FetchSuperColumnValues("timeline")
               .Take(25)
               .OrderByDescending()
               .Select(x=>x.Value);
  var tweets = cfdb.Tweets.Get(tweetIds);

.. note::
  
  There isn't such an API for .NET (at least, not that I am aware of), I created this sample to show a point, not to 
  demonstrate real API.

In essence, we execute two queries, the first on the UsersTweets column family, requesting the columns & values in 
the ``timeline`` super column in the row keyed "@ayende", we then execute another query against the Tweets column 
family to get the actual tweets.

This sort of behavior is pretty common in NosQL data stores. It is called *secondary index*, a way to quickly access 
the data by key based on another entity/row/document value. This is one example of how the need to query for tweets
by user has affected the data that we store. If we didn't create this secondary index, we would have no possible way 
to answer a question such as "show me the last 25 tweets from @ayende". 

Because the data is sorted by the column name, and because we choose to sort in descending order, we get the last 25 
tweets for this user. What would happen if I wanted to show the last 25 tweets overall (for the public timeline)? 
Well, that is actually very easy, all I need to do is to query the Tweets column family for tweets, ordering them by 
descending key order.

Why is column family database so limiting?
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

You might have noticed how many times I noted differences between RDBMS and a CFDB. I think that it is the CFDB that 
is the hardest to understand at first, since it is so close on the surface to the relational model. 
But it seems to suffer from so many limitations. No joins, no real querying capability (except by primary key), 
nothing like the richness that we get from a relational database. Hell, Sqlite or Access gives me more than that. 
Why is it so *limited*?

The answer is quite simple. A CFDB is designed to run on a large number of machines, and store huge amount of 
information. You literally cannot store that amount of data in a relational database, and even multi-machine relational 
databases, such as Oracle RAC will fall over and die very rapidly on the size of data and queries that a typical CFDB 
is handling easily. Remember that CFDB is really all about removing abstractions. CFDB is what happens when you 
take a relational database, strip everything away that make it hard to run in on a cluster and see what happens.

The reason that CFDB don't provide joins is that joins require you to be able to scan the entire data set. That requires 
either someplace that has a view of the whole database (resulting in a bottleneck and a single point of failure) or 
actually executing a query over all machines in the cluster. Since that number can be pretty high, we want to avoid 
that. CFDB don't provide a way to query by column or value because that would necessitate either an  index of the 
entire data set (or just in a single column family) which is again, not practical, or running the query on all 
machines, which is not possible. By limiting queries to just by key, CFDB ensure that they know exactly what node a 
query can run on. It means that each query is running on a small set of data, making them much cheaper.

It requires a drastically different mode of thinking, and while I don't have practical experience with CFDB, I would 
imagine that migrations using them are... unpleasant affairs, but they are one of the ways to get really high 
scalability out of your data storage. 

How to select a data storage solution?
=======================================
So far I have shown you the major players in the NoSQL fields. Each of them has its own weaknesses and strengths. 
A question that I get a lot is: 

  I want to use NoSql-Technology-X for Xyz and...
  
I usually cringe when I hear this sort of question, because almost invariably, it falls into one of two pitfalls:

* Trying to import a relational mind set into a NoSQL data store.
* Trying to use a single data store for all things, including things that it really isn't suitable for.

Selecting a data storage strategy isn't a one time decision. In a single application, you may use a Key/Value store 
to hold session information, graph database to serve social queries and a document database to hold your entities. 
I view the "we use a single data store" mentality in the same way that I view people who want to write all their code 
in a single file. You certainly *can* do that, but that is going to be... awkward.

I try to break down things based on the expected data access patterns expected from each section in the application. 
If in the product catalog am always dealing with queries by the product SKU, and speed is of the essence, it make 
a lot of sense to use a key/value store. But that doesn't means that orders should be stored there, for order I need 
a lot more flexibility, so I put them in a document database, etc. 

Multiple data stores in a single application?
----------------------------------------------
The logical conclusion of this approach is that a single application may have several different data stores. While I 
wouldn't go out of my way to try to use any data store technology that exists out there in a project, I wouldn't 
balk from using the best data store technology for the application purposes. The idea is to choose the best match for 
what we need to do, not to just use whatever is already there whatever it fits our purposes or not.

That said, be aware that it only make sense to introduce a new data store technology to a project if the benefit of 
having multiple data stores outweigh the cost. If I need to support user defined fields, I would gravitate very quickly 
to a document database, rather than try to implement that on top of a RDBMS.

.. warning:: Don't forget about the RDBMS

  Despite the name, NosQL actually stands for Not Only SQL. The main point is that the problem isn't with the RDBMS as 
  a technology, the problem is that for many people, data storage *is* RDBMS. When choosing a data storage technology
  I always take care to include RDBMS in the mix as well. RDBMS is an incredibly powerful tool and should not be 
  discarded just because there are younger and sexier contenders in the ring.

When is NoSQL a poor choice?
-----------------------------
After spending so long extolling the benefits of the various NoSQL solutions, I would like to point out at least one 
scenario where I haven't seen a good NosQL solution for the RDBMS: Reporting.
One of the great things about RDBMS is that given the information that it already have, it is very easy to massage the 
data into a lot of interesting forms. That is especially important when you are trying to do things like give the
user the ability to analyze the data on their own, such as by providing the user with a report tool that allows them 
to query, aggregate and manipulate the data to their heart's content.

While it is certainly possible to produce reports on top of a NoSQL store, you wouldn't be able to come close to the 
level of flexibility that a RDMBS will offer. That is one of the major benefits of the RDBMS, its flexibility.
The NoSQL solutions will tend to outperform the RDBMS solution (as long as you stay in the appropriate niche for each 
NoSQL solution) and they certainly have better scalability story than the RDBMS, but for user driven reports, the 
RDBMS is still my tool of choice.

And when scaling is not an issue?
----------------------------------
The application data is one of the most precious assets that we have. And for a long time, there wasn't any question 
about where we are going to put this data. The RDBMS was the only game in town. The initial drive away from the RDBMS 
was indeed driven by the need to scale. But that was just the original impetuous to start developing the NoSQL 
solutions. Once those solutions came into being and matured, it isn't just the "we need web-scale" players that 
benefited. 

Proven & Mature NoSQL solutions aren't applicable just at high end of scaling. NoSQL solutions provide a lot of 
benefits even for applications that will never need to scale higher than a single machine. Document databases 
drastically simplify things like user defined fields, or working with Aggregates. The performance of a NoSQL solution 
can often exceed a comparable RDBMS solution, because the NoSQL solution will usually focus on a very small subset of 
the feature set that RDMBS has. 

Summary
========
In this chapter, we have gone over the reasons for the NoSQL movement, born out of the need to handle ever increasing 
data, users and complexity. We have explored the various NoSQL options and discussed their benefits and disadvantages 
as well as what scenarios they are suitable for. We looked at how to select an appropriate data store for specific
purposes and finally discussed how the emergence of robust NoSQL solutions has improved our options even when we aren't
required to scale, because we have more data storage models to select from when it comes the time to design our 
application.

In the next chapter, we will leave the general topic of NoSQL and begin to focus specifically on document databases, 
the topic of this book. So turn the page to the next chapter, and let us explore...


.. [1] To be rather more exact, I should say that when I am talking about scaling, I am talking about scaling a 
   database instance across a large number of machines. It is certainly possible to scale RDBMS solutions, but the 
   typical approach is by breaking the data store to independent nodes (sharding), which means that things like cross
   node joins are no longer possible.  
   
   Another RDBMS scaling solution is a set of servers that acts as a single logical database instance, such as Oracle 
   RAC. The problem with this approach that the number of machines that can take part in such a system  in limited
   (usually to low single digits), making it impractical for high scaling requirements.