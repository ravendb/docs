# How A NoSQL MapReduce Can Boost Your Database Performance
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a>, CEO RavenDB</small>

<div class="article-img figure text-center">
  <img src="images/how-nosql-mapreduce-can-boost-your-database-performance.jpg" alt="How A NoSQL MapReduce Can Boost Your Database Performance" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

Aggregations are a crucial part of any business. Totals like monthly sales, daily users, or even number of comments made in the morning hours can uncover a new opportunity, identify where to cut costs, or direct your resources towards an overall increase in productivity.

<img class="floating-right margin-left img-responsive" alt="How A NoSQL MapReduce Can Boost Your Database Performance" src="images/abandoned-shopping-carts.jpg" />

MapReduce queries add flexibility and speed to aggregating your data, creating a whole new world of potential. 

Aggregation queries on significant data sizes can take hours. If you are aggregating every sale, or even each comment made on your social media assets, it can take days. A database administrator may have to commandeer the server for an entire weekend just to crunch a handful of numbers. This can put on hold other important operations.

In a worst-case scenario, your server can be so tied up with an aggregation, all of your users suffer a slowdown in web performance. Even in the best of cases, you can end up with data that lags real time reality by days or even weeks.

## Legacy Aggregating from the Days of Disco

<div class="youtube-frame youtube-frame--left">
    <div class="embed-responsive embed-responsive-16by9">
        <iframe class="embed-responsive-item" width="420" height="315" src="https://www.youtube.com/embed/-ihs-vT9T3Q" frameborder="0" allowfullscreen></iframe>
    </div>
    <div class="caption">
        Relational databases have been aggregating roughly the same way since SQL first came out alongside the Bee Gees in the mid-1970s
    </div>
</div>


If you wanted to know how much money you generated from orders made from European customers, your SQL database would have to go through a number of steps. 

1. Join tables like sales orders, payments, customer ID, address, country/region, and so on. 

2. Go over every single row and aggregate by region.

Bottlenecks appear in the form of the time involved for each query, and the demands made on the server while the database is processing. The bottleneck tightens when you have to perform the same steps every time you want to run the query. 

If you want to know your daily sales figures by region, you have to perform this aggregation every day. If you want it by month, that’s 30 times the amount of data to crunch, likely over a weekend. If the 31st falls on a Tuesday, and the only possible way to dedicate so much resources to this task is to start it late Friday night, your finance team may have to wait until Monday the 6th to get their information. 

For a relational database, MapReduce queries are not an option.

{SOCIAL-MEDIA-FOLLOW/}

## What is MapReduce in RavenDB?

<img class="floating-left margin-right img-responsive" alt="How A NoSQL MapReduce Can Boost Your Database Performance" src="images/results-wait-for-you.jpg" />

The developers at RavenDB came up with a solution for fast and easy aggregation for singular or polymorphic data. You only have to aggregate once. RavenDB MapReduce will keep your results live as new data comes in.
The first time you perform an aggregation, a MapReduce index will be created. It will go through all the documents, sort by region, and give you your totals. 

Once the data has been sorted, the MapReduce index will stay in place. RavenDB will keep tallying the results, and storing it on your server. With every new transaction RavenDB will update your totals. 

For example, say you want monthly sales by region for 2018. The first query will be on January 31. Assuming this is the first time you are making such a query, you will have to go over all the data. That sets the index. On February 1, for every sale that is entered into the database, the sum totals for sales, and region will automatically update. 

Come the end of February, the answers are waiting for you. Your database does not need to comb millions of documents, just the new totals that are already sitting on your server, giftwrapped. You can also call weekly sales data, daily, even by the hour. Knowing when are the high times, you know when to deploy more staff, when to release articles and social media posts, when to offer sweet discounts.

There is no extra cost in time, money, or resources to run these aggregation queries as much as you like.

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

## MapReduce Applications with Better Database Performance

The flexibility of RavenDB MapReduce gives you a treasure chest of new options:

1. You can run your aggregations in real time. If South Korea stuns Germany in the World Cup and your CFO needs to make a snap decision on an ad campaign offering a 30% discount to all South Koreans, he needs to know sales from South Korea over the past year to see the ROI on this snap decision. 

    RavenDB MapReduce will have these figures in your database ready for retrieval before the penalty kick heard round the world was even shot. 

2. You save the time it took to make the first query every time you make it again.

3. You don’t have to worry about choking off your server to perform aggregations. Resources will not be siphoned off from other areas of your business. Nobody suffers from delayed performance.

Whether using RavenDB as your primary database, or in conjunction with a legacy relational option, MapReduce queries give your team the information it needs the moment it needs it  -- allowing you to optimize your next big decision. 

<div class="bottom-line">
<p>
    <a href="https://ravendb.net/"><strong>RavenDB 4.0</strong></a> is an open source NoSQL document database that specializes in online transaction processing (OLTP). It's fully transactional (ACID), and compatible with legacy SQL RMDBs. You can have the best of SQL while enjoying high performance, a distributed data cluster, flexibility, and scalability with low overhead that comes with a top of the line NoSQL solution. RavenDB is an easy to use all-in-one database, striving to minimize your need for third party applications, tools, or support.</p>
<p>RavenDB has a built-in storage engine, <em>Voron</em>, that operates at speeds up to 1,000,000 writes per second on a single node. You can build high-performance, low-latency applications quickly and efficiently. <a href="https://ravendb.net/downloads#server/dev">
</p>

<p><strong>Grab RavenDB 4.0 for free</strong></a>, and get 3 cores, our state of the art GUI, and a 6 gigabyte RAM database with up to a 3-server cluster up and running for your next project.</p>
</div>

## Our Credo

At RavenDB, we want to make every step in the process as simple as possible so you can set up and start using the best NoSQL database on the market with ease. We are so confident in proving thorough solutions to all your database challenges that, while we will always provide top level service for all your support needs, we charge a fraction for it compared to our competitors. Your RavenDB database is designed to be a full-on solution that takes many of the arduous tasks involved with installation, security, operations, and repair, and integrates them into the back end so you don’t have to worry about it at all. 

Where other database companies plow 75% of their revenues into winning new customers, we reinvest the majority of our resources into making the best full-service database to save you time and money on your next application. We take pride in dedicating our energy into living up to our core standard:

It just works. 
