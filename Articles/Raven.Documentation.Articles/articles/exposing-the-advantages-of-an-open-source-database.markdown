# An Open Source Database - Exposing the Advantages <br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![Exposing the Advantages of an Open Source Database](images/exposing-the-advantages-of-an-open-source-database.jpg)

{SOCIAL-MEDIA-LIKE/}

"I've never seen it done before. I don't think this is even possible. You know what, show me the code..."

Automatic indexes? Rob Ashton thought it was possible, and I was certain it can't be done. He persisted talking about an impossible feature, and eventually I told him that he is welcome to show me how it is done.

24 hours later he got back to me. It was an email that blew my mind.

He sent me a patch demonstrating how you can get RavenDB to [automatically generate an index](https://ravendb.net/docs/article-page/4.2/csharp/server/configuration/indexing-configuration) if one was needed for a query. This meant that the cost of querying became negligible and that the operational overhead of using RavenDB dropped to the bottom.

RavenDB has never been the same.

## Open Source Database

Offering RavenDB via open source turns our users into developers. Many of my clients have asked for specific changes on RavenDB to suit their specific needs and were able to make those changes themselves.

It's kind of like telling the chef at your favorite restaurant that you would like your burger to come with a special sauce, and then going behind the counter to take some mustard and mayo packets to make it yourself.

We all benefit - especially when a month later that restaurant comes out with their new "Secret Sauce" and it tastes exactly like yours.

Even better, once the code has been contributed, we are in charge of enhancing, supporting, and maintaining it. When our customer who made his own special sauce comes back for a new meal, he relishes in the taste of his own recipe, only now it's new and better.

## Automatic Indexes

Thanks to our open source community, RavenDB became the first and only database to offer automatic indexes.

[Automatic Indexes](https://ravendb.net/features/indexes/auto-indexes) are where your database sets up an index *before* servicing a query. RavenDB will set up an index, use one already there, or update one in existence. The more you use your database, the more efficient your indexes will become as RavenDB learns from your usage patterns to develop better indexes for you.

As a result, developers are spared a key step in the data management process while they save money on the cloud. The faster the database, the less you pay in computing costs on the cloud for each query.

## Keeping Things Simple

One of the big topics we are all talking about is the worrying trend in complexity.

We have gotten to the point where the level of complexity in today's application is explosive and untenable.
<br/>
<br/>
<div class="text-center"><iframe style="display: block; max-width: 100%; margin: auto;" width="560" height="315" src="https://www.youtube.com/embed/17PkUsTVa7g?start=185&end=406;" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe></div>
<br/>
## We Need To Reduce Complexity

It's like the factories of the early 18<sup>th</sup> century well into the industrial revolution. There were so many gears, levers, and assorted parts that just one wrong move could take off a limb or cost a worker's life, and often did. In the original Ford cars of the 1900s, there was so little room for mistakes that if you switched the gears incorrectly, the car would jerk so hard you could sprain your arm.

Today's cars are built to actively prevent you from harm. The 2.0 revolution in autos was the push to create features like collision detection, lane change alert, and seat belts that made autos safe as well as fast.

Today's applications won't cause such physical harm, but the potential for havoc is pretty terrifying. Kubernetes as an example of great technology that has so many spinning wheels that it is very hard to manage and operate.

Like the evolution of the auto, which made a 180 degree turn to keep moving forward, we need to reduce complexity by a lot.

We kept that in mind when we built RavenDB. We made sure that everything you needed for a database is already there. One part for one database that already includes caching, indexing, aggregates, full text search, and an effective GUI that like the car with its new blinkers, will tell you everything you need to know in order to keep your database running, and running efficiently.

We are a part of today's latest trend in technology: Less is More.

<br/>
<div class="text-center">
    <a href="https://ravendb.net/whitepapers/mongodb-ravendb-best-nosql-open-source-document-database"><img src="images/ravendb-vs-mongodb.png" class="img-responsive" style="margin: auto;" alt="RavenDB vs MongoDB Whitepaper"/></a>
</div>
<br/>