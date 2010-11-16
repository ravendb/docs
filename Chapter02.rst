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

* Implications of document only storage:

  * Docs are independent
  * There are no joins
  * Queries only via indexes
* Documents and Indexes 
* BASE - Basically AVailable, Soft State, Eventually Consistent