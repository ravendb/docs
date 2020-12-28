# Resolving the 3 Hidden Challenges of Holiday Traffic to Your Database

![3 Hidden Challenges for Your Database on Black Friday](images/black-friday-cyber-monday-nosql-database-ravendb.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

It's not just Black Friday, Christmas, or Cyber Monday. 9 out of the top 10 busiest online shopping days take place from Thanksgiving to the first week in January. At any moment, your online business can receive a huge wave of traffic and customers. Having in place a solid data architecture can guarantee that your best day will become a cause for celebration, and not concern. 

The Dow Jones is at record highs. Unemployment sits at 15 year lows. Personal income levels have risen to all-time highs and household debt as a % of the economy has fallen to historical lows. That can mean only one thing:

It's time to shop!

This holiday season promises to be the busiest on record. Ten years into the revolution in Smartphones, a brand-new audience has been introduced and matured into the retail equation.  

The winners will reap a windfall unprecedented in the annals of commerce. The numbers are staggering:

154 million shoppers went online last year, spending over $5.5 billion over Black Friday weekend. For 2017 users will spend an estimated $40,000 *per second* and $2.4 million *per minute* in online commerce on Cyber Monday alone. 

![3 Hidden Challenges for Your Database on Black Friday](images/not-just-spike-in-traffic.jpg)

<br/>

Here is another surprise: Black Friday is not the busiest shopping day of the year, it's the night before Christmas. With same day delivery exploding, last minute shoppers could flood your application when you least expect. 

## New Prosperity Creates New Challenges

Your database is at the heart of every application. It processes your traffic coming by taking their orders and giving them the relevant feedback. If turnaround time is quick, the user is happy and will stay on your app to make a purchase. If it takes too long, the app may as well be broken. Long pauses, time outs, freezes, and crashes chase away your users and can destroy your brand name. 

Major companies like Target and Macy's got killed on social media by not being able to handle or process holiday traffic in a timely manner. 

What does your database need to be ready for this holiday season, and the next one as well? What are the hidden challenges you need to be prepared to face?

{SOCIAL-MEDIA-FOLLOW/}

## Challenge #1: The real spike is not in traffic, but in conversions

Black Friday got its name because retailers typically lose money all year long. They stay in the red until the last week in November. Sales are so explosive on this one day that retailers swing into the black in a matter of hours. For OLTP databases, this poses the first big challenge, **conversions**. 

Throughout the year, you can enjoy spikes to your traffic. This means more reads as your database fetches data and serves it to an added number of users. Conversion rates are usually unaffected. For the holidays, people are looking to buy. Along with a surge in traffic of 100-300% you may see an increase in conversions from 3% of your traffic to 7.5%, an increase in 250% on higher volume. 

An update or a write to your database can cost you 4 times as much as a read. Planning for increased traffic is only part of the plan. Writes impact every future query, often invalidating much of your cached info. To keep performance optimal, you need to take this into account. 

**Solution**: A NoSQL database can perform much faster than its relational counterpart. RavenDB is special in that it is a NoSQL solution, processing tens of millions of transactions within minutes, and is also a fully transactional database. You can serve huge amounts of traffic while maintaining the integrity of your data just like you would in an SQL database.

## Challenge #2: Concurrency – You need more service reps to handle longer lines

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

If your special holiday discounts work out, you should enjoy a season where hundreds of satisfied shoppers are all hitting the buy button at the same time. As long as your concurrency issues are managed, tis the season to be jolly. 

<img class="floating-right" alt="3 Hidden Challenges for Your Database on Black Friday" src="images/thanksgiving-turkey.jpg" />

The real challenge is that you have targeted concurrency. You may offer 10% on a specific item, or category of items. That puts additional load on one area of your database, which can create a long line of users trying to process their order. It is estimated that 1 in 4 shopping carts are abandoned due to performance issues. 

**Solution**: The advantage of NoSQL solutions is that they scale out, and not up. You can expand your network of servers to multiple points of sale to widen your reach. RavenDB lets you add nodes in a matter of minutes, replicating your entire database within hours. It's like instantly opening up 10 extra cash registers to serve a rapidly growing line of people. They like that. 


## Challenge #3: High Availability is critical. Replication and backup in real time are obligatory

If $40,000 is being made every second on Cyber Monday, then each moment counts. The costs of your database stalling, or hemorrhaging data while it is coming in an unprecedented velocity can be critical. What happens if you get overloaded on Black Friday? What happens if your server goes down? Can you recover?

The consequences to a database failure, even if just for a minute, are big. If mega companies like Target or Dell couldn't handle their traffic, how can you?

**Solution**: A distributed database that replicates in real time and has high-availability puts you a step ahead of even the largest of enterprises using a relational solution. A NoSQL database contains a cluster of nodes, all replicating to each other in real time. Each node has your entire database. If one goes down, the others keep moving. With RavenDB, if you have network partition, where the nodes cannot talk to one another, they keep working. Once they are up, they implement protocols where the most recent state of your data is updated and then replicated to all the nodes. 

You can always have more than one database up and running, and serving your customers. 

We cover all the bases so this season will be your biggest ever!

<div class="bottom-line">
<p>
    <a href="https://ravendb.net/"><strong>RavenDB 4.0</strong></a> is an open source NoSQL document database that specializes in online transaction processing (OLTP). RavenDB is fully transactional (ACID), and compatible with legacy SQL RMDBs. You can have the best of SQL while enjoying high performance, a distributed data cluster, flexibility, and scalability with low overhead that comes with a top of the line NoSQL solution. RavenDB is an easy to use all-in-one database, striving to minimize your need for third party applications, tools, or support.</p

<p>RavenDB has a built-in storage engine, <em>Voron</em>, that operates at speeds up to 1,000,000 writes per second on a single node. You can build high-performance, low-latency applications quickly and efficiently. <a href="https://ravendb.net/downloads#server/dev"><strong>Grab RavenDB 4.0 for free</strong></a>, and get 3 cores, our state of the art GUI, and a 6 gigabyte RAM database with up to a 3-server cluster up and running for your next project.</p>
</div>