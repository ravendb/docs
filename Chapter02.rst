Chapter 2 - Grokking Document Databases
***************************************

.. contents:: In this chapter...
  :depth: 3

In the previous chapter, we have spoken at length about a lot of different options for No SQL data stores. But even 
though we touched on document databases, we haven't really discussed them in detail. In essence, document databases
stores documents (duh!). A document is usually represented as JSON (sometimes it can be XML). 

.. note::

  I am going to assume that you are familiar with RDBMS, and compare a document database behavior directly to the
  behavior of RDBMS.

The following JSON document represent an order::

  // Listing 2.1 - A sample order document 
  { 
    "Date": "2010-10-05",
    "Customer": { 
      "Name": "Dorothy Givens",
      "Id": "customers/2941"
    },
    "Items": [
      { 
        "SKU": "products/4910",
        "Name": "Water Bucket",
        "Quantity": 1,
        "Price": { 
          "Amount": 1.29
          "Currency": "USD"
        }
      },
      { 
        "SKU": "products/6573",
        "Name": "Beach Ball",
        "Quantity": 1,
        "Price": { 
          "Amount": 2.19
          "Currency": "USD"
        }
      }      
    ]
  }
  
Documents in a document database don't have to follow any schema and can have any form that they wish to be. This make 
them an excellent choice when you want to use them for sparse models (models where most of the properties are usually
empty) or for dynamic models (customized data model, user generated data, etc).

In addition to that, documents are *not* flat. Take a look at the document shown in listing 2.1, we represent a lot of 
data in a single document here. And we represent that internally. Unlike RDBMS, a document is not just a a set of keys
and values, it can contain nested values, lists and arbitrarily complex data structures. This make it much easier to 
work with documents compared to working with RDBMS, because the complexity of your objects doesn't translate into a 
large number of calls to the database to load the entire object, as it would be in RDBMS.

In order to build the document showing in listing 2.1 in a RDBMS system, we would probably have to query at least 3 
tables, and it is pretty common to have to touch more than five tables to get all the needed information for a single
logical entity. With document databases, all that information is already present in the document, and there is no need
to do anything special. You just need to load the document, and the data is there.

The down side here is while you can embed information inside the document very easily, it is harder to *reference* 
information in other documents. In RDBMS, you can simply join to another table and get the data from the database
that way. But document databases do not have the concept of joins (RavenDB has something similar called *includes*, 
which is discussed in :ref:`Chapter 6 <Chapter06>`, but it isn't really a parallel).

As you can imagine, these two changes leads to drastically different methods of modeling data in a document 
database...
  
Data modeling with document databases
=====================================

While document databases are schema-free data stores, that doesn't mean that you shouldn't take some time to consider 
how to design your documents to ensure that you can access all the data that you need to serve requests efficiently, 
reliably and with as little maintainability cost as possible. The most typical error people make when trying to design
the data model on top of a document database is to try to model it the same way you would on top of a relational 
database. A document database is a non-relational data store. Trying to hammer a relational model on top of it will 
produce sub-optimal results. But you can get fantastic results by taking advantage of the documented oriented nature 
of a document database.

Documents are not flat
^^^^^^^^^^^^^^^^^^^^^^^

Documents, unlike a row in a RDBMS, are not flat. You are not limited to just storing keys and values. Instead, you 
can store complex object graphs as a single document. That includes arrays, dictionaries and trees. Unlike a relational
database, where a row can only contain simple values and more complex data structures need to be stored as relations, 
you don't need to work hard to map your data into a document database.

Take a look at figure 2.1 for an example of a simple blog page.

.. figure::  _static/BlogPost.png

  Figure 2.1 - A simple blog post page
  
In a relational database, we would have to touch no less than 4 tables to show the data in this single page (Posts, 
Comments, Tags and RelatedPosts). But a document database let us store all the data in a single document, as shown 
in listing 2.1::
  
  {// Listing 2.1 - A blog post document can contain complex data
    "Title": "Modeling in Docs DBs",
    "Content": "Modeling data in...",
    "Tags": [
      "Raven",
      "DocDB",
      "Modeling"
    ],
    "Comments": [
      {
        "Content": "Great post...",
        "Author": "John"
      },
      {
        "Content": "Sed ut...",
        "Author": "Nosh"
      }
    ],
    "RelatedPosts": [
      {
        "Id": "posts/1234",
        "Title": "Doc Db Modeling Anti Patterns"
      },
      {
        "Id": "posts/4321",
        "Title": "Common Access Patterns"
      }
    ]
  }

Using a document database in this fashion allows us to get everything that we need to display the page shown above in a 
single request.

Document databases are not relational
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
When starting out with a document database, the most common problems happen when users attempt to use relational concepts.
The major issue with that is, of course, that Raven is non-relational. However, it's actually more than that; there is
a reason why Raven is non-relational.
A document database treats each document as an independent entity. By doing so, it is able to optimize the way 
documents are stored and managed. Moreover, one of the sweet spots that we see for a document database is for 
storing large amounts of data (too much data to store on a single machine).

Document databases sharding are very simple, since each document is isolated and independent, it is very easy to simply
split the data across the various sharded nodes. Doing so is very since, since there is no need to store a group of 
related documents together. Each document is independent and can be stored on any shard in the system. Another aspect 
of the non-relational nature of document databases is that documents are expected to be meaningful on their own. You 
can certainly store references to other documents, but if you need to refer to another document to understand what the
current document means, you are probably using document databases incorrectly.

With a document database, you are encouraged to include all of the information you need in a single document. Take a 
look at the post example in listing 2.1. In a relational database, we would have a link table for RelatedPosts, which 
would contain just the ids of the linked posts. If we wanted to get the titles of the related posts, we would need to
join to the Posts table again. You can do that in a document database, but that isn't the recommended approach. 
Instead, as shown in the example above, you should include all of the details that you need inside the document [#denormalization]_. Using 
this approach, you can display the page with just a single request, leading to much better overall performance.

Documents are Aggregates
^^^^^^^^^^^^^^^^^^^^^^^^^

When thinking about using a document database to persist entities, we need to consider the two previous points. The 
suggested approach is to follow the `Aggregate pattern from the Domain Driven Design book 
<http://domaindrivendesign.org/node/88>`. 
An Aggregate Root contains several entities and value types and controls all access to the objects contained in its 
boundaries. External references may only refer to the Aggregate Root, butnever to one of its child entities / value 
objects. When you apply this sort of thinking to a document database, there is a natural and easy to follow correlation
between an Aggregate Root (in DDD terms) and a document in a document database. An Aggregate Root, and all the objects 
that it holds, is a document.

This also neatly resolves a common problem with Aggregates when using relational databases: traversing the path through
the Aggregate to the object we need for a specific operation is very expensive in terms of number of database calls. 
Using a document database, loading the entire Aggregate is just a single call and hydrating a document to the full 
Aggregate Root object graph is a very cheap operation.

Changes to the Aggregate are also easier to control, when using RDMBS, it can be hard to ensure that concurrent 
requests won't violate business rules. The problem is that two separate requests may touch two different parts of 
the Aggregate. And while each request is valid on its own, together they result in an invalid state. This has led to 
the usage of `coarse grained locks <http://martinfowler.com/eaaCatalog/coarseGrainedLock.html>`, which are hard to 
implement when using RDBMS. Since a document database treats the entire Aggregate as a single document, the problem
simply doesn't exist. You can utilize the database concurrency support to determine if the Aggregate or any of
its children has changed. And if that happened, you can simply refresh the modified Aggregate and retry the 
transaction.

Relations and Associations
^^^^^^^^^^^^^^^^^^^^^^^^^^^

Aggregate Roots may contain all of their children, but even Aggregates do not live in isolation. Let us look at the 
document in listing 2.2::
  
   // listing 2.2 - The Order aggregate refers to other aggregates
   
   { // Order document - id: orders/95128
    "Customer": {
      "Id": "customers/84822",
      "Name": "John Doe"
    },
    "OrderLines": [
      "Product": {
        "Id": "products/1724",
        "Name": "Milk"
      },
      "Quantity": 3,
      "Price": { 
        "Amount": 1.2,
        "Currency": "USD"
      }
    ]
  }
  
  { // Product document - id: products/1724
    "Name": "Milk",
    "Price": { 
        "Amount": 1.2,
        "Currency": "USD"
     },
    "OrganicFood": true,
    "GoodForYou": true
  }
  
  { // Customer document - id: customers/84822
    "Name": "John Doe",
    "Email": "john.doe@example.org",
    "LastLogin": "2010-10-05T15:40:19"
  }

The Aggregate Root for an Order will contain Order Lines, but an Order Line will not contain a Product. Instead, it 
contains a *denormalized reference* to the product. The product is another aggregate, obviously. And here we have a 
tension between competing needs. On the one hand, we want to be able to process the order document without having to
reference another document (since this results in *much* better overall perfromance). But on the other hand, in order
to do so, we have to duplicate the product (and customer, for that matter) information inside the order documnt.
We will discuss this problem in the next section.

.. note:: What to denormalize?

  While I think that denormalizing smoe data to the referering document, you should carefully consider what *sort* of
  data you are going to denormalize. For example, in the customer case, we denormalized the customer name. That is a 
  good choice, because a name is going to change rarely.
  But the LastLogin property is going to change all the time. In this case, we don't really care about the customer 
  login time, but even if we did, we still wouldn't be able to denormalize the LastLogin property.
  Like in most cases, the answer to "What to denormalized?" is it depends!
  
  It depends on:
  
  * How often the value changes?
  * How important is the value to the referring document?
  
  Luckily, in practice it turns out that it is rare that you would want to have access to a rapidly changing from 
  another docuemnt. BUt if you do, it might be a good idea to relax the "documents are indepndent" rule.

In a relational database, we can usually rely on *Lazy Loading* to help us, but most document databases client API
will *not* support lazy loading. This is intentional, explicit, and by design decision. Instead of relying on lazy 
loadin, the expected usage is to hold the associated document key as well as the information from it to process the 
current document. If you really need the full associated document, you need to explicitly load it [#includes]_. 

The reasoning behind this is simple: we want to make it just a tad harder to reference data in other documents. It is 
very common when using an Object Relational Mapper to do something like: ``orderLine.Product.Name``, which will lazily 
load the Product entity. That makes sense when you are living in a relational world, but a document database is not 
relational.

Denormalization isn't scary
============================

Data modeling in relational database is usaully focused on discovering what data we need to keep, and normalizing it so
each piece of data will live in only a single location. Normalization in RDBMS had such a major role because storage was
*expensive*. It helps to remember that when a lot of those techniques were develop, in 1981, a megabyte of persistentn 
storage cost U$460. At the time of this writing you can get a 1 *terrabyte* HD for 63$, putting the price of a 
*gigabyte* of persistent storage at 6 cents USD!

It made sense to try to optimize this with normalization. In essence, normalization is compressing the data, by taking 
the repeated patterns and substituting them with a marker. There is also another issue, when normalization came out, 
the applications being being were far different than the type of applications we build today. In terms of number of 
users, time that you had to process a single request, concurrent requests, amount of data that you had to deal with, 
etc. Under those circumstances, it actually made sense to trade off read speed for storage. In today’s world? I don’t 
think that it hold as much.

The other major benefit of normalization, which took extra emphasis when the reduction in storage became less important
as HD sizes grew, is that when you state a fact only once, you can modify it only once. The corollary to that is that 
when you need to modify this data, you can do so in only one location. Except… there is a large set of scenarios where you
*don’t* want to do that. 

Let us take invoices as a good example. In the case of an invoicing system , if you changed the product 
name from “Thingamajig” to “Foomiester”, that is going to be mighty confusing for the user when I look at that invoice and 
have see an invoice for a product that they never baught. What about the name of the customer? Think about the scenarios 
in which someone changes their name (marriage is most common one, probably). If a woman orders a book under her maiden name,
then changes her name after she married, what is supposed to show on the order when it is displayed? If it is the new name,
that person didn’t exist at the time of the order.

Another very important consideration is to consider costs. In the vast majority of systems, the number of reads far exceeds
the number of writes. But normalization is a technique that trades off write speed for read speed (you have to write the
data only once, but you have to join the data on every read). At the time the technique was introduced, it made a lot of 
sense, but today... I don't think so.

So we have ruled out the space saving as not really important, and the only thing that is left is the cost of actually 
ensuring that when we update the data, we update it in all locations. As I mentioned previously, there is a large set
of scenarios where you actually *don't* want to update the data, you want to keep the information as it was at the time 
the document was created. Not suprisingly, this tends to show up a lot when you are dealing with data that represent actual
documents (orders, invoices, loan contract, etc).

And when you *do* want to update the data, you can do so when you write to the master source. That is a bit annoying, 
because you have to keep track of where you denormalized the data, but it isn't hard, and the end result is that you 
are doing some additional amount of work on writes (rare) but significantly reduces the amount of work that you do for
reads (common).

That is a good tradeoff, in my eyes.

Indexes bring order to schema free world
=========================================

Document databases allow you to store data without requiring any schema. That is great, except that in practice, there
isn't much that you can do if I just hand you a document. You can display it, and allow the user to edit it, but that
is about it. In practice, our documents usually have the same structure. An order will always have OrderLines, for 
example. And even though two different order documents may have slightly different schema, they will tend to look fairly
similar to one another.

Some document databases (RavendB and CouchDB, for example) have the notion of indexes (CouchDB calls them Views), which 
allow us to bring some order back to our database. An index defines how to transform a document from the basic anything
goes form to a predictable, known, format. The advantages in that are huge. After all, there is a reason why relational
databases requires you to have a schema. When you have a known data format, there are a lot of things that you can do with
it. 

In particular, you can search that data *really* fast. Moreover, you can pull the data directly from the index, skipping
the schema free nature of documents in favor of the predictable nature of the index format. What happens in practice is 
that document databases generally use indexes to allow you to define how you want to query the docuents.

There is another aspcet to it, however. Remember the notion that documents are independent? That is great when you are 
thinking about a single document, but one of the major features that a user expects from a database is to be able to 
query on aggregation of documents (how many posts in Ayende's blog, for example).

In document databases, aggregations are handled using map/reduce indexes.

.. note:: Don't Panic!
  
  Yes, I know that map/reduce sounds scary. But map/reduce is really just another way to call ``group by``. That is all
  what map/reduce is, when you get down to it. We will discuss map/reduce inexes in detail in :ref:`Chapter 5 <Chapter05>`,
  don't worry, you'll pick it up very quickly.

All aggregations inside a document database is done using map/reduce. Some databases (such as MongoDB) allows you to
run those map/reduce queries on the fly. Others (RavenDB, CouchDB) requires you to define a map/reduce index and then 
query the index. We will discuss the differences between the two in detail in :ref:`Chapter 4 <Chapter04>`.


Summary
========

In this chapter we have explored what exactly a document database *is*, not only in the sense of what sort of data is stored
inside a document database, but how we work with it.
Documents can be arbitrarily complex, which allows us to hold an entire Aggregate Root inside a single document. And because
documents are independent, tehy should not require referencing another document in order to process requests regarding that 
document. Therefor, we model documents in order to include denoramlized references to other documents.
Those denormalized reference copy the document id as well as whatever properties that are important to the referring document.

We can handle denormalized updates in one of two ways:

* Keep the old data - useful for invoices, orders, etc. Where the document referent a point in time.
* Update all copies of the data - useful when the data represent the current value.

RavenDB includes explicit support to make handle denromalized updates, whch we discuss in TODO.

Finally, we discussed the role of indexes in a document database, and introduced the dreaded map/reduce indexes. Indexes are
used to give the database a way to extract a schema out of a set of documents.

And now, enough with discussing high level concepts, we are going to go ahead and start working with RavenDB directly and 
discover why it is the best document database [#bias]_ that you have seen.

.. rubric:: Footnotes
  
.. [#denormalization] Yes, that does means that we are effectively denoramlize the data. RavenDB includes several 
  mechanisms to deal with this issue, but in practice, it turns out to be a fairly minor concern. We will discuss
  this issue at more length later in this chapter.

.. [#includes] Note, however, that RavenDB specifically incldues a feature to make such operations more efficent.
  We discuss this in :ref:`chapter 6 <includes>`. The featuer is called *includes*. 
  
.. [#bias] In my obviously unbiased opinion :-).