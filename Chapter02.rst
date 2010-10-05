Chapter 2 - Grokking Document Databases
***************************************

.. contents:: In this chapter...
  :depth: 3

In the previous chapter, we have spoken at length about a lot of different options for No SQL data stores. But even 
though we touched on document databases, we haven't really discussed them in detail. In essence, document dataases
stores documents (duh!). A document is usually represented as JSON (sometimes it can be XML). 
The following JSON document represent an order::

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

.. rubric:: Listing 2.1 - A sample order document 
  
Documents in a document database don't have to follow any schema and can have any form that they wish to be. This make 
them an excellent choice when you want to use them for sparse models (models where most of the properties are usually
empty) or for dynamic models (customized data model, user generated data, etc).

In addition to that, documents are *not* flat. Take a look at the document shown in listing 2.1, we represent a lot of 
data in a single document here. And we represent that internally. Unlike RDBMS, a document is not just a a set of keys
and values, it can contain nested values, lists and arbitrarily complex data structures. This make it much easier to 
work with documents copmared to working with RDBMS, because the complexity of your objects doesn't translate into a 
large number of calls to the database to load the entire object, as it would be in RDBMS.

In order to build the document showing in listing 2.1 in a RDBMS system, we would probably have to query at least 3 
tables, and it is pretty common to have to touch more than five tables to get all the needed information for a single
logical entity. With document databases, all that information is already present in the document, and there is no need
to do anything special. You just need to load the document, and the data is there.

The down side here is while you can 
  
Data modeling with document databases
=====================================

* How to model a document database
* Documents and Indexes
* Implications of document only storage:

  * Docs are independent
  * There are no joins
  * Queries only via indexes
 
* BASE - Basically AVailable, Soft State, Eventually Consistent