Advanced RavenDB indexing
**************************

We have learned how to work with RavenDB and how to query it, but we still have only a very rough understanding about how everything actualy works.
The terms ``indexing`` and ``indexes`` were thrown around a lot, but we haven't yet talked about what it actually *means*. This chapter will go into
all the details about RavenDB indexes, when you want to pay particular attention to them and what sort of features they expose.

What is an index?
==================

The first thing to approach, however, is to understand exactly what an index is. RavenDB doesn't allow unindexed queries, so all the queries that you make
using RavenDB always use an index. That statments sounds strange, on the face of it, isn't it? So far, we have seen neither hide nor hair of any indexes, but
we have certainly been able to query. The code in listing 5.1 certainly *seems* to work::
  
  //listing 5.1 - querying RavenDB
  
  var ayendeBlog = session.Query<Blog>()
                     .Where(blog => blog.Title == "Ayende @ Rahien")
                     .First();

How, then, does this works? When you make a query to RavenDB, the RavenDB query optimizer will find the appropriate index for the query. But what happens when
there *isn't* any matching index?
RavenDB will create a temporary index for us, just for this query. We discussed how and what RavenDB does this in the prevous chapter.
But we still don't have a good idea what an index *is*, right? Listing 5.2 shows the index that RavenDB generates on the server::

  //listing 5.2 - the auto generated index created by RavenDB
  
  from blogItem in docs.Blogs
  select new { blogItem.Title }
  
That looks like a Linq query, and not any sort of index that *I* have seen before, so what is going on? Well, the answer is that what you see is the *index 
definition function``, which is what RavenDB uses to extract the information to be indexed from the documents. Let us assume that the server contains the 
documents in listing 5.3::

  // listing 5.3 - sample documents
  
  { // blogs/1234
    "Title": "Ayende @ Rahien",
    "Author": "...",
    "StartedAt": "..."
  }
  
  { // blogs/1235
    "Title": "Raven's Flight",
    "Author": "...",
    "StartedAt": "..."
  }

The output of the indexing function in listing 5.2 over the documents in listing 5.3 is showing in listing 5.4::

  // listing 5.4 - the ouput of the indexing function over the sample documents

  { "Title": "Ayende @ Rahien", "__document_id": "blogs/1234" }
  { "Title": "Raven's Flight", "__document_id": "blogs/1235" }
  
Those values are then stored inside a persistent index, which gives us the ability to perfom low cost queries over the values
stored in the index. 

.. question::
  
  Where did the ``__document_id`` in listing 5.4 came from? It doesn't appear in the indexing function in listing 5.2.
  
  That value is inserted by RavenDB to all the results of the indexing function, this is one of a few values that is automatically
  inserted by RavenDB (another is the ``__reduce_key`` value, which serves the same function, but for Map/Reduce indexex).
  
After RavenDB ensures that an index exists, it can query the index. In chapter 4, we discussed the way RavenDB builds indexes on the 
background and the notion of staleness. Because RavenDB doesn't have to wait for the indexing process to complete, it is able to produce
answers without having to wait even if there are concurrent indexing tasks running.

All of that together brings us to the reason why RavenDB queries are so fast. All the queries are running on precomputed indexes, and those 
queries never have to wait.

The index storage format is a Lucene index, which is discussed in greater detail in Chapter TODO.

Index optimizations
====================

We mentioned that if a query is made when the query optimizer cannot find an applicable index, that index will be created. That index is temporary,
but it will hang around for a while, just in case additional queries that requires it are needed. Indeed, if enough queries using that temporary index
are made, the index will be promoted into a persistent index.

In general, it is better to have fewer indexes that index more fields than more indexes, where each index fewer fields. The dominating factor in indexing 
performance is I/O, and bigger indexes can utilize the disk I/O better than more indexes that each compete for disk I/O. For generated indexes, the query 
optimizer will aggregate indexes together, but for manually created indexes, you should be aware that you should strive for bigger indexes than fine 
grained indexes.

For the most part, the generated indexes serve just fine, but there are several advanced options that are available when you write your own indexes. For
the rest of the chapter, we will discuss those in detail.

Collation
===========

RavenDB support sorting in a culture sensitive manner, but you have to explicitly tell it about that. The index defintion in listing 5.5 shows how we can
sort the shopping cart by the customer name using Swedish sorting rules::
  
  // listing 5.5 - index definition sorting carts by customer name using Swedish sorting rules
  
  public class ShoppingCarts_ByCustomerName_InSwedish : AbstractIndexCreationTask<ShoppingCart>
  {
    public Products_ByCountInShoppingCart()
    {
        Map = carts => from cart in carts
                       select new { cart.Customer.Name };
        Analyzers.Add( x => x.Customer.Name, typeof(Raven.Database.Indexing.Collation.Cultures.SvCollationAnalyzer));
    }
  }

Querying the ShoppingCarts_ByCustomerName_InSwedish index now will return results sorted by the customer name using the Swedish sorting rules. The same approach
is available for most languages. All you need is to select change the two letter language code prefix for the CollationAnalyzer.

Exact matches
==============

By default, RavenDB uses case insensitive match to compare values. There are certain values where case sensitivity matters, and you want to capture the value 
exactly as it is. You can do that by specifying that the value is ``NotAnalyzed``, which will cause RavenDB to make an exact (and case sensitive) match to it.
You can see how to set this option in listing 5.6::

  public class ShoppingCarts_ByCustomerName_NotAnalyzed: AbstractIndexCreationTask<ShoppingCart>
  {
    public Products_ByCountInShoppingCart()
    {
        Map = carts => from cart in carts
                       select new { cart.Customer.Name };
        Indexes.Add( x => x.Customer.Name, FieldInexing.NotAnalyzed);
    }
  }

Full text search
================

As mentioned previously, RavenDB will default to case insensitive match to compare values, but often we want to query on more than just the exact value, we want
to query using Full Text Search. So the value "The Green Fox jumped over the Grey Hill" would be matched by "fox" and "hill". In order to do that, we need to set
the value to be ``Analyzed``, which will enable full text searching on the value. Listing 5.7 shows how this can be done::

    public class ShoppingCarts_ByCustomerName_Analyzed: AbstractIndexCreationTask<ShoppingCart>
  {
    public Products_ByCountInShoppingCart()
    {
        Map = carts => from cart in carts
                       select new { cart.Customer.Name };
        Indexes.Add( x => x.Customer.Name, FieldInexing.Analyzed);
    }
  }



* Hierarchies
* Spatial
* WhereEntityIs
* Suggestions
