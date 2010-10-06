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
    "Email": "
  }

The Aggregate Root for an Order will contain Order Lines, but an Order Line will not contain a Product. Instead, it 
contains a *denormalized reference* to the product. The product is another aggregate, obviously. And here we have a 
tension between competing needs. On the one hand, we want to be able to 


!!!!!!!!!!!!NOT DONE

The Raven Client API will not try to resolve such associations. This is intentional and by design. Instead, the expected usage is to hold the value of the associated document key and explicitly load the association if it is needed.
The reasoning behind this is simple: we want to make it just a tad harder to reference data in other documents. It is very common when using an OR/M to do something like: orderLine.Product.Name, which will load the Product entity. That makes sense when you are living in a relational world, but Raven is not relational. This deliberate omission from the Raven Client API is intended to remind users that they should model their Aggregates and Entities in a format that follows the recommended practice for Raven.
  

* Implications of document only storage:

  * Docs are independent
  * There are no joins
  * Queries only via indexes
* Denromalization & updates
* Documents and Indexes 
* BASE - Basically AVailable, Soft State, Eventually Consistent

.. rubric:: Footnotes
  
.. [#denormalization] Yes, that does means that we are effectively denoramlize the data. RavenDB includes several 
  mechanisms to deal with this issue, but in practice, it turns out to be a fairly minor concern. We will discuss
  this issue at more length later in this chapter.
   