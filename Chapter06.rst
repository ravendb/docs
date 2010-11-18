Map / Reduce indexes
********************************

In this chapter:

* What is map / reduce?
* How map / reduce works in RavenDB?
* Creating and querying map / reduce indexes
* Where should we use map / reduce indexes?

.. _MapReduce:

One of the biggest hurdles for NoSQL databases has always been the perception that map/reduce is such a hard topic - this cannot be further from the truth.
Map/reduce is actually a very simple (and elegant) solution to an equally simple problem.

Map/reduce is simply another way to say *group by*. Chances are, you are already familiar with the notion of group by and in fact, I am not aware of anything who has nightmares about group by - but I do know a few people who get the shakes at
the mere mention of map/reduce (just ask `Bob <http://browsertoolkit.com/fault-tolerance.png>`).

It is usually best to demonstrate such concepts using an example, and we will use counting the number of comments 
for each blog as our map/reduce sample. You can see a sample blog post document in listing 5.1::

  // Listing 5.1 - A sample blog post 
  
  { // Document id: posts/1923
    "Name": "Raven's Map/Reduce functionality",
    "BlogId": "blogs/1234",
    "Comments": [
      { 
        "Author": "Martin",
        "Text": "..."
      }
    ]
  }

In order to answer the question of how many comments each blog have, we have to *aggregate* the data from multiple documents. Using 
Linq, we can do so very easily, as shown in listing 5.2::

  // Listing 5.2 - A Linq query to aggregate all the comment count per blog
  
  from post in docs.Posts
  group post by post.BlogId into g
  select new 
    { 
      BlogId = post.BlogId, 
      CommentCount = g.Sum(x=>x.Comments.Length) 
    };
  
You have probably seen similar code scores of times. Unfortunately, this code has a small, almost insignificant problem,
it assumes that it can access all the data. But what happens if the data is too big to fit in memory? Or even too big to
fit on a single machine?

This is where map/reduce comes into play. Map/reduce is merely meant to deal with group by on a massive scale, but the
concept is stll the same old concept. It is just that we need to break the group by into multiple steps that can each
run on a different machine.

Stepping through the map / reduce process
==========================================

The first thing that we need to do is to break the operation in listing 5.2 to distinct operations. Let us look at what
the original code there is doing... We start by grouping all posts on the ``BlogId`` and then we select the ``BlogId`` and the 
``Comment.Length``. 
This suggests that that the only information that we actually need from the post are the ``BlogId`` and the 
``Comment.Length`` properties. So we define a linq query that executes just that part, shown in listing 5.3::

  // Listing 5.3 - Projecting just the required fields from the posts
  
  from post in docs.Posts
  select new 
    { 
      post.BlogId,
      CommentCount = Comments.Length
    }
    
How is this useful? Well now instead of having to deal with full blown post documents, we can deal with a much smaller 
projection. We have minimized the amount of data that we have to work on and if we feed a set of documents through
the linq query in listing 5.3, we are going to get the results we can see in listing 5.4::

  // Listing 5.4 - The results of the query in liting 5.4
  
  { BlogId: "blogs/1234", CommentCount: 4 }
  { BlogId: "blogs/9313", CommentCount: 2 }
  { BlogId: "blogs/1234", CommentCount: 3 }
  { BlogId: "blogs/2394", CommentCount: 1 }
  { BlogId: "blogs/9313", CommentCount: 0 }

The size difference between the results of the query and the original document is pretty big, as you can see. Now we
need to take a look at the second part of this task; grouping the results and performing the actual aggregation. We do that in 
listing 5.5::
  
  // Listing 5.5 - Grouping the results to find the final result
  
  from result in results
  group result by result.BlogId into g
  select new
    {
      BlogId = g.Key,
      CommentCount = g.Sum(x=>x.CommentCount) 
    }

So far, so good. The query in listing 5.4 seems reasonable, it is *very* similar to the one we have seen in listing 5.2,
after all. What we need to do now is to feed the results in listing 5.4 through the query. We can see the result of 
that in listing 5.6::

  { BlogId: "blogs/1234", CommentCount: 7 }
  { BlogId: "blogs/9313", CommentCount: 2 }
  { BlogId: "blogs/2394", CommentCount: 1 }
  
So far, we haven't done anything special. But we have actually done something that might surprise you. We have defined a 
pair of map/reduce functions.

* Listing 5.3 is the map function.
* Listing 5.5 is the reduce function.

I know what you are thinking, I am explaining to you things that you already know, but bear with me - the fat lady 
hasn't sung yet after all. I didn't complicate the query in 5.2 by breaking it apart into two separate queries for
no reason. Let us assume that we have *another* data set, on another machine. This data set is shown in listing 5.7::

  { BlogId: "blogs/1234", CommentCount: 5 }
  { BlogId: "blogs/7269", CommentCount: 2 }
  { BlogId: "blogs/1234", CommentCount: 4 }
  { BlogId: "blogs/9313", CommentCount: 2 }
  
We want to get the answer for *all* blogs, not just the posts on a particular machine (the query in listing 5.2 would
do just fine for *that*). What we are going to do is to run all the data in listing 5.7 through the query in 5.3, giving
us the data in listing 5.8::

  { BlogId: "blogs/1234", CommentCount: 9 }
  { BlogId: "blogs/7269", CommentCount: 2 }
  { BlogId: "blogs/9313", CommentCount: 2 }

The fun part starts now, because the reduce function *can be applied recursively*. What we are going to do now is to
execute the query in listing 5.5 on the data in both listing 5.6 and 5.8 (we are simply going to combine the two datasets
and execute the query on all of the data at once). This gives us the results in listing 5.9::

  { BlogId: "blogs/1234", CommentCount: 16 }  
  { BlogId: "blogs/7269", CommentCount: 2  }  
  { BlogId: "blogs/9313", CommentCount: 4  }  
  { BlogId: "blogs/2394", CommentCount: 1  }  
  
And that is the whole secret of map/reduce, honestly. We were able to take two data sets from two distinct nodes and by
applying the map/reduce algorithm, we were able to derive the final result for an aggregation that spanned machine 
boundaries.

What is map/reduce, again?
===========================

Map/reduce [#google]_ is simply a way to break the concept of ``group by`` into multiple steps. By breaking the ``group by`` 
operation into multiple steps, we can execute a group by operation over a set of machines, allowing us to execute such
operations on data sets which are too big to fit on a single machine. Map/reduce is composed of two steps:

The first step is the map. The map is just a function (or a linq query) which is
executed over a data set. It is the responsibility of the map to filter the data set (Linq where clause) from data
that we don't care about and project the data that we are interested in for the task at hand from the data that was
passed in (the Linq select clause).

The second step in the map/reduce process is the reduce function (or a linq query). This function takes the output of
the map function and *reduces* the values. In practice, the reduce function almost always uses a ``group by`` clause to 
aggregate the incoming dataset based on a common key.

Distributed map/reduce relies on an executer that can execute the map function, and then the reduce function on the 
output of the map function. If multiple nodes are used, the executer merges the reduced data from several nodes and then
executes reduce again on these merged result sets.

Most of the complexity that was attached to map/reduce is because writing the executer is a non trivial task, but 
conceptually, the idea is very simple.

Rules for Map/Reduce operations
================================

RavenDB primarily uses Linq queries to define the map and reduce functions, and linq queries tend to naturally match
the rules for map/reduce functions, but it is important to be aware of what those rules are:

* The reduce function *must* be able to process the map function output as well as its own output.
  This is required because reduce may be applied recursively to its own output. In practice, what this means is that
  the map function outputs the same type as the output of the reduce function. Since the types are the same, it is 
  naturally possible to run the reduce function on its own output (after all, it is also the map function output).
  
  Listing 5.10 shows an example of a map/reduce pair returning the same type::
  
    // Listing 5.10 - Map/reduce pair returning the same type.
    
    // map
    from post in docs.Posts
    select new { post.BlogId, CommentCount = post.Comments.Length }
    
    // reduce
    from result in results
    group result by result.BlogId into g
    select new { BlogId = g.Key, CommentCount = g.Sum(x=>x.CommentCount) }
    
  And listing 5.11 shows an example of an invalid map/reduce pair::
  
    // Listing 5.11 - Map/reduce pair returning different types
    
    // map
    from post in docs.Posts
    select new { post.BlogId, CommentCount = post.Comments.Length }
    
    // reduce
    from result in results
    group result by result.BlogId into g
    select new { BlogId = g.Key, TotalComments = g.Sum(x=>x.CommentCount) }
    
  If we attempt to send the output of the reduce function in listing 5.11 back into the same function, we are going to
  get an error because there is no CommentCount in the output of the reduce function.

* The map and reduce function *must* be pure functions. A pure function is a function that:
  
  * Given the same input will return the same output. i.e. [ ``map(doc) == map(doc)``, for any doc ] 
    What this means is that you cannot rely on any external input, only the input that it was passed in.
    
  * Evaluation of the function will have no side effects.
  
  What this means in practice is that you can't make any external calls from the map/reduce functions. That isn't an 
  onerous requirement, since you usually don't have a way to *make* external calls anyway.
  
As I mentioned, for the most part, we don't really need to pay close attention to those rules, Linq queries tend
to follow them anyway.

Applications of Map/Reduce
============================

As I mentioned, map/reduce is mostly just a glorified way of using group by. But what is interesting is how much this is
useful. One obvious result of map/reduce is running these simple aggregations:

* Count
* Sum
* Distinct
* Average

And many others like that. But you can also use map/reduce to implement joins. We will discuss how to do just that later
in this chapter.

Map/reduce is not applicable however, in scenarios where the dataset alone is not sufficient to perform the operation. 
In the case of a navigation computation, you can't really handle this via map/reduce because you lack key data points
(the start and end points). Trying to compute paths from all points to all other points is probably a losing 
proposition, unless you have a very small graph. 

Another problem occurs when you have a 1:1 mapping between input and output. Oh, Map/Reduce will still work, but the 
resulting output is probably going to be too big to be really useful. It also means that you have a simple parallel 
problem, not a map/reduce sort of problem.

Map/reduce assumes that the reduce step is going to... well *reduce* the data set :-).

If you need fresh results, map/reduce isn't applicable either, it is an inherently a batch operation, not an online one.
Trying to invoke map/reduce operation for a user request is going to be very expensive, and not something that you 
really want to do. 

If you data size is small enough to fit on a single machine, it is probably going to be faster to process it as a single
reduce(map(data)) operation, than go through the entire map/reduce process (which require synchronization). 

And now that we have discussed *what* map/reduce is, exactly, let us see how RavenDB uses that and how you can utilize
map/reduce within RavenDB.

How map/reduce works in RavenDB
================================

RavenDB uses map/reduce to allow you to perform aggregations over multiple documents. One thing that it is important to
note from the start is that RavenDB doesn't apply distributed map/reduce, but runs all the map/reduce operations locally.
This raises the question, if we are going to use map/reduce on a single machine only, why bother, can't we just execute
the process as a single Linq query with a ``group by`` clause?

Theoretically, we could do that, but while RavenDB doesn't use distributed map/reduce, it does have a use for map/reduce
and that is avoiding unnecessary computation and I/O. Because a map/reduce process is commutative, it means that we can 
efficently cache and partition work as needed. When a document that is indexed by a Map/Reduce index is changed, we run
the map function only on that document, and then reduce the document along with the reduce results of all the other 
documents that share the same reduce key (the item the Linq query groups on).

Listing 5.12 shows a reduce function::

    //Listing 5.12 - A sample reduce function
    
    // reduce
    from result in results
    group result by result.BlogId into g
    select new { BlogId = g.Key, CommentCount = g.Sum(x=>x.CommentCount) }
    
The reduce key in listing 5.12 is the *value* of ``result.BlogId``. RavenDB will use that to optimize the values it will
pass to the reduce function (the actual group by is usually done by RavenDB, and not by the linq query). This results in a
much cheaper cost of indexing for map/reduce indexes, compared to running a single query with a group by on all 
documents with the same reduce key. 

.. note:: RavenDB doesn't implement re-reduce (yet)

  This is an implementation detail that should only concern you if you are interested in reducing a very large number of
  results on the same reduce key. That is because RavenDB currently implements reduce as a single operation, and will
  pass all the documents with the same reduce key to a single reduce function.
  
  This may cause performance issues if you have *very* large numbers of results with the same reduce key, where very 
  large is in the tens or hundreds of thousands of results for each reduce key. Fixing this limitation is already on 
  the roadmap.
 
We are almost done with the theory, I promise. We just have to deal with one tiny detail before we can start looking at
some real code.

How RavendB stores the results of map/reduce indexes?
======================================================

In the :ref:`previous chapter <Chapter05>` we discussed how RavenDB deals with the results of simple indexes (containing
only a map function). Map/reduce indexes actually produce two different data points. The first is the output from the map
function; internally these values are called *mapped results* inside RavenDB and are never exposed externally,
but they are what allows RavenDB to perform partial index updates.

The second output is the output from the reduce function. This is the externally visible output from a map/reduce index.
And like simple indexes, that data is also stored inside a Lucene index. Storing the data in Lucene allows efficent and
full featured querying capabilties (as well as all the other goodies, like full text searching).

Unlike simple indexes (where the assumption is that most of the time you would like to search on the index, but get the
actual document), map/reduce indexes don't just serve as an index, but actually store the data that we are going to 
get as a result of a query.

For example, if I query the index that we defined in listing 5.3 and listing 5.4 (and whose output is shown in listing 
5.9) for the result for the ``blogs/9313`` blog, we will get::

  { BlogId: "blogs/9313", CommentCount: 4  }
  
This value is stored in the index itself, and it is loaded directly from there. This means that you don't touch
any documents when you query a map/reduce index. All the work is being handled by RavenDB in the background. And like
simple indexes, it is possible to query a map/reduce and get a stale result. We handle this in exactly the same way we
handle stale index with simple indexes.

And now, after much ado, let us get to coding and write our first map/reduce index.

Creating our first map/reduce index
====================================

Using our shopping cart example, we want to find out how many items of each product were sold. As a reminder listing 
5.13 shows the format of a shopping cart::

  //listing 5.13 - a shopping cart document
  
  { // shoppingcarts/1342
    "Products": [
      { "Id": "products/31", "Quantity":3 },
      { "Id": "products/25", "Quantity":1 },
    ]
  }
  
Before we start writing the map/reduce index, I usually find it useful to write the full linq query to do the same 
calculation. That tends to make it easier to write the index later on. The linq query is shown in listing 5.14::

  // listing 5.14 - a linq query to calculate the count of products across all shopping carts
  
  from shoppingCart from docs.ShoppingCarts
  from product in shoppingCart.Products
  group product by product.Id into g
  select new { ProductId = g.Key, Count = g.Sum(x=>x.Count) }
  
The next step is to break the query in listing 5.14 to multiple steps, and create an index out of it. We will use the 
``AbstractIndexCreationTask`` to do that as shown in listing 5.15::

  // listing 5.15 - The products count index
  
  public class Products_ByCountInShoppingCart : AbstractIndexCreationTask<ShoppingCart, ProductByCountProjection>
  {
    public Products_ByCountInShoppingCart()
    {
        Map = carts => from cart in carts
                       from product in cart.Products
                       select new { ProductId = product.Id, Count = product.Count };
        Reduce = results => from result in results
                            group result by result.ProductId into g
                            select new { ProductId = g.Key, Count = g.Sum(x=>x.Count) };
    }
  }

The Map part in the index will extract a count for each product from all the shopping cart, exactly as in the blog example that
we have examined previously. The only interesting part is that we dig deeper into the shopping cart, and project the values from
one of its collections. And the Reduce part will aggregate the results by the product id into the final answer.

You might have noticed that we have added a new twist to the ``AbstractIndexCreationTask``, in the form of an additional 
generic parameter. The second parameter ``ProductByCountProjection`` is the output of the Map function and is both the 
input and output of the Reduce function.

Querying map/reduce indexes
============================

Just like standard indexes, we can query a map / reduce index using the session API. Listing 5.16 shows loading the sold count
for a particular product::
  
  // listing 5.16 - querying a map / reduce index
  var results = session.Query<ProductByCountProjection, Products_ByCountInShoppingCart>()
                  .Where(x => x.ProductId == "products/31")
                  .ToList();

The first parameter of the ``Query`` method is the type of the results, while the second parameter indicates which index we 
should query. Unlike standard indexes (also called simple indexes or map-only indexes), the result of a map / reduce function
is always a projection and never the original document.

We usually use the same type for the results that we use when creating the index using the ``AbstractIndexCreationTask`` class. Now that we 
know how to create and query indexes, we can move on to a important topic, _where_ should we use those?

Where should we use map / reduce indexes?
==========================================

Map / reduce indexes are very useful in aggregating data, but they shouldn't be confused with a full blown reporting solution.
While you can certainly use map / reduce indexes to build _some_ reports, in many cases, a report requires more than a map / reduce
index can provide (for example, map / reduce indexes cannot support arbitrary grouping).

Map / reduce indexes are useful when we want to look at the data in a single format. One common usage is as a part of a homepage or 
dashboard views. A major advantage of the map / reduce indexes in RavenDB is that (like standard indexes), they are pre-computed, 
which means that querying them is a very cheap operation.

That makes them ideal for aggregating large amount of data that will be viewed often.

Summary
========

In this chapter, we have learned what map / reduce is; a way to break the calculation of data into discrete units that can be processed
independently (and even on separate machines). Afterward, we continued to discover how map / reduce is implemented inside RavenDB
and how best to take advantage of that.

We finished with a sample of creating and querying a map / reduce index, which allowed us to calculate how many items were sold for
each product. Because of the way map / reduce works with RavenDB, querying the index is very cheap, and we can use this as part of 
the product page, to show, for example, how popular a particular product is.

Finally, we discussed where do we want to use map / reduce index. The obvious answer is that we want to use them whenever we have a 
reason to use aggregation, but we have to be aware that unlike group by queries in a relational database, map / reduce queries in 
RavenDB doesn't allow arbitrary grouping (which rules them out for use as part of a generic reporting service). On the other hand, they
do provide very fast responses for fixed queries, such as the ones typically used in a dashboard / homepage scenarios. Their low cost of
querying make it efficient to use them even in the high traffic locations of your applications.

In the next chapter, we will discuss Live Projections, Includes and other advanced indexing options. In the chapter after that, we will
go over various querying scenarios and see how we can solve them with RavenDB.

.. rubric:: Footnotes

.. [#google] Map/reduce is an old concept, most functional languages uses the notion of map and reduce constant. In many
  such languages, those functions usually serve where loops would be used in procedural languages. Google is the one
  responsible for taking those concepts and popularizing them with regards to distributing work across a set of worker nodes.
