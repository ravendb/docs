# Using A NoSQL Database for Black Friday: The Gift that Keeps On Giving <br/><small>by <a href="https://www.linkedin.com/in/sabrina-globus-3609ba34/" rel="nofollow">Sabrina Globus</a></small>

![A NoSQL Database on Black Friday: The Gift that Keeps Giving](images/nosql-database-on-black-friday-ravendb-document-database.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

From Thanksgiving to New Year’s, the stakes couldn’t be higher. Relying solely on database technology that came out with disco is a huge risk. Adding a layer of NoSQL, especially a database that is also ACID, maximizes the ability of your database environment to handle large spikes in traffic and to provide the best service to all your users. 

Just 500 milliseconds of web delay can cost you 5% of your projected sales. Performance is the new function. If it takes too long for your database to ferry information back and forth to your user, the user will abandon you. 25% of online sales fall through due to poor performance. That’s just half the cost. Poor performance is a customer service issue. It takes 6 times more money to retain a dissatisfied customer than to get a new one.

## The Limits of the Disco Database

<div class="youtube-frame youtube-frame--right">
    <div class="embed-responsive embed-responsive-16by9">
        <iframe class="embed-responsive-item" width="500" height="315" src="https://www.youtube.com/embed/I_izvAbhExY" frameborder="0" allowfullscreen></iframe>
    </div>
    <div class="caption">Stayin Alive, 1977, the year relational databases were fully developed.</div>
</div>

The challenge for today’s database is to keep your data fresh at all times, available to users anywhere and whenever they want it, and to deliver the information they ask for as fast as you can. 

Since 1974, SQL has performed these functions. For the first 30 years of its use, it has performed these tasks diligently. With the emergence of massive amounts of data coming in from 3.5 billion smartphone users, and another billion on desktops, it’s questionable that a technology which emerged with the Bee Gees can handle the great scale up of the 21st century. 

{SOCIAL-MEDIA-FOLLOW/}

## SQL puts your holiday traffic at risk

<img class="floating-right" alt="A NoSQL Database on Black Friday: The Gift that Keeps Giving" src="images/time-is-money.jpg" />

### A. The Single Server Model is a Killer for Load Balance and Latency

Relational databases, with a sole server, are built with an architecture that limits their performance.  Concurrency can drag down performance if there is too much of it. 

On just a single server, load balance also becomes an issue as too many people are waiting on line to be served. One server in one location makes users further away from that server wait longer, creating latency issues. 

### B. The Single Server Model creates a Huge Risk of Failure

The single server model means you have a single point of failure. Your house of cards is standing on top of the jack of diamonds. If there is just one problem with the server your entire application can go down. All your users are vulnerable to time outs, freezes, reloads, and crashes. 

an <a href="https://itic-corp.com/blog/2016/08/cost-of-hourly-downtime-soars-81-of-enterprises-say-it-exceeds-300k-on-average/" rel="nofollow">ITIC study</a> reported that 98% of organizations say a single hour of downtime costs over $100,000.

### C. Your Options are Limited

The relational model gives you limited options to replicate your data. In SQL, it’s a difficult task that costs you. The tools that Database Admins have to work with are not up to the task to handle complex procedures like sharding, or other types of synthetic data replication. The schema itself restricts flexibility, making it difficult to adapt your data to new business environment’s like the holiday season or game changing technologies. 

*Why do we stay with SQL in commerce?* The best way to maintain the integrity of your data in an active application is with a fully transactional database that only the relational model can deliver. For ecommerce applications, this is vital. Every transaction must be committed to the database in its entirety or else you stand to upset a lot of good people giving you money.

Organizations have historically stayed away from NoSQL solutions because they aren’t transactional, and they put the integrity of your data at risk. 

Until now.

## NoSQL Database Advantages: Inheriting ACID while Advancing Performance

To process huge amounts of requests coming in from a larger audience, you need better performance from your database. NoSql is the next generation of database that’s designed specifically for this purpose. In the age where performance is king, a new technology has assumed the throne. 

NoSQL resolves the issues left unchallenged by the disco database:

### 1. You can reduce latency and balance load with a distributed network.

A NoSQL database like RavenDB  4.0 enables you to set up a cluster of nodes, each consisting of a commodity computer. You can reach high performance while replicating your data throughout a series of machines in real time. You can balance load by splitting your one SQL line up into several smaller NoSQL lines. Reduce latency when the user accesses the node closest to his point of query. 

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

### 2. The right solution lets you scale up for high traffic periods. 

A cluster of 3 or 5 nodes can handle most levels of traffic. But what if you are offering a special 2 for 1 deal to your customers in the next 48 hours? What happens when it is Cyber Monday and you are getting 10 times the normal amount of traffic, and 20 times the level of conversions? The busiest shopping day of the year is December 24. With the widespread use of same day delivery, this day could turn out to be a bigger surprise than you expected. Can you adapt on the fly?

RavenDB allows you to set up extra nodes in a matter of minutes. Depending on how large your database is, it’s just a matter of hours before all your data is replicated to the new nodes. 

You can quickly adapt to spiking traffic, maintaining top performance for your new users. 

### 3. With no need for multiple tables NoSQL reads faster

<img class="floating-left" alt="A NoSQL Database on Black Friday: The Gift that Keeps Giving" src="images/enjoy-the-data.jpg" />

RavenDB 4.0 reaches over 100,000 writes per second and over half a million reads as the premiere NoSQL solution. You feel it the most during peak holiday season when you need that speed more than ever. 

As a fully transactional database, you lose nothing by upgrading from a legacy SQL solution to a NoSQL database. RavenDB is easy to install and secure. You can set up a database cluster, with full security in a matter of minutes. Our query language is a dialect of SQL, incorporating 85% of SQL syntax to make the ramp up to mastering RavenDB fast. With over a decade over experience with NoSQL database, we know where all the major pain points are and how to resolve them. 

## Two Great Options with NoSQL

From this holiday season to the next, traffic will continue to expand. New innovations in applications will enable users to ask for greater amounts of individual and aggregated data in each session. To keep up with your customers, you have two options. 

You can get a [nosql database and set up a cluster right away](https://ravendb.net/buy), erasing the one fail point dilemma. Your database becomes an octopus: if one arm goes down, you still have seven. Your next project can start off with a stand-alone NoSQL solution.

If you have a legacy SQL database, or invested a lot into your current solutions, you can also create a polyglot architecture, attaching RavenDB to your current database, and enjoy the best of both worlds. You can retain the ACIDity of SQL while adding on top of it the speed and flexibility of NoSQL. 

Either way, you have a tremendous opportunity to take the great leap forward in database technology. 

<div class="bottom-line">
<p>
    <a href="https://ravendb.net/"><strong>RavenDB 4.0</strong></a> is an open source NoSQL document database that specializes in online transaction processing (<em>OLTP</em>). It's fully transactional (<em>ACID</em>), and compatible with legacy SQL RMDBs. You can have the best of SQL while enjoying high performance, a distributed data cluster, flexibility, and scalability with low overhead that comes with a top of the line NoSQL solution. RavenDB is an easy to use all-in-one database, striving to minimize your need for third party applications, tools, or support.</p>
<p>RavenDB has a built-in storage engine, <em>Voron</em>, that operates at speeds up to 1,000,000 writes per second on a single node. You can build high-performance, low-latency applications quickly and efficiently. <a href="https://ravendb.net/downloads#server/dev">
</p>

<p><strong>Grab RavenDB 4.0 for free</strong></a>, and get 3 cores, our state of the art GUI, and a 6 gigabyte RAM database with up to a 3-server cluster up and running for your next project.</p>
</div>

## Our Credo

At RavenDB, we want to make every step in the process as simple as possible so you can set up and start using the best NoSQL database on the market with ease. We are so confident in proving thorough solutions to all your database challenges that, while we will always provide top level service for all your support needs, we charge a fraction for it compared to our competitors. Your RavenDB database is designed to be a full-on solution that takes many of the arduous tasks involved with installation, security, operations, and repair, and integrates them into the back end so you don’t have to worry about it at all. 

Where other database companies plow 75% of their revenues into winning new customers, we reinvest the majority of our resources into making the best full-service database to save you time and money on your next application. We take pride in dedicating our energy into living up to our core standard:

It just works. 
