<h1>The Best Database For Startups Used by Battle Crate</h1>
<small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![ACID transactions that just work, automatic indexes, and a self-sustaining system to let you focus more on your product makes RavenDB the database for startups.](images/battle-crate-uses-ravendb-to-become-the-aws-of-gaming.jpg)

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size:24px;">Battle Crate Uses RavenDB ACID Database to become the AWS of Gaming</p>

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <a href="https://battlecrate.io" target="_blank" rel="nofollow"><img src="images/battle-crate.png" class="img-responsive m-0-auto" alt="Battle Crate"/></a>
</div>

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">You shouldn't have to be worrying too much about your data, the database should just do the job.</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Alan Doherty, co-Founder of Battle Crate</span>
</div>

Jonathan Madeley and Alan Doherty are avid gamers. Seeing the limits that popular game hosting solutions were hitting, they decided to offer something better.

Battle Crate was born.

Starting their project while still in college, they kept hitting speed bumps using MySQL on the backend. Understanding the nature of their data required something more suitable to the needs of today, they changed gears to a document database.

Once they saw that they could get high performance, ACID data integrity, and when a transaction persisted to the disk, was there safe and sound, Battle Crate chose RavenDB to advance their battle plan.

### A One-Stop Shop for All Your Gaming Needs

Using new cloud technology and powered by [the best document database for startups](https://ravendb.net/articles/ravendb-best-nosql-database-example-for-startups), Battle Crate is poised to shake things up by creating the next generation of game hosting.

For starters, they want to improve the game hosting experience by requiring a less financial commitment to play. Providers can demand a month-long commitment, but Battle Crate allows you to pay per hour with no commitments.

Typically, if a user wants to build a community, they have to buy hosting and make it themselves. Battle Crate allows you to do this out of the box, without the headache.

A good example is mini-games. There are large gaming communities for a game like Minecraft. People will create new games from it, and invite some members of the community to play their specific version of the game. In order to do that you're required to boot up a new server, take in the new people, and start playing. Battle Crate lets you do it all on their platform.

You can build up mini-servers on Battle Crate without plugins or external components. Players save time spinning up their game faster. Battle Crate is rapidly expanding from a simple hosting locale to a one-stop shop for all your gaming needs.

They also want to guarantee a secure high availability for their service. One of their competitors suffered an SQL Injection Hack interrupting service for an entire week.

### ACID Transactions and Distributed Data

<div class="margin-top margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="Managed Cloud Hosting"/></a>
</div>

RavenDB is the database storage for spinning up a crate. All the information for the networking storage, IP addresses are stored there along with all operations on the nodes and communications servers.

The document database is also used to store players, users, and groups. Selling to individuals and communities of thousands, even hundreds of thousands of people make distributed databases ops a must, and RavenDB underwrites all of it, enabling Battle Crate to go after big business.

**ACID transactions** not only maintained the performance necessary, but Alan and Jonathan were also able to sleep at night knowing their data was going to persist securely, and it all just worked. Performance is vital when you have a real-time game played by people in Bristol, England, Tokyo, Japan, and Santa Fe, New Mexico.

The most recent scores and events need to be captured and replicated to the nodes serving all players in all locations. ACID transactions are vital to make sure all players have the same accurate information on their game consoles.

In a live game, a delay is not an option. To that end, JOINS were not feasible as putting together multiple tables to serve a live query couldn't hack it. RavenDB's *includes* feature will grab all other related documents required to fulfill the query. Along with boosting performance by eliminating the need to grab data from multiple tables and put them together, the *includes* feature cuts your initial and subsequent release cycles.

Making changes to your database doesn't require a change in the schema, which can take time. It also doesn't require you to check out all the relationships between tables or to check the schema of your tables either.

### The Database for Startups

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">It's been a six-month development cycle where we have been working 24/7. If we were trying to do this in MySQL or something similar, we would not be as far ahead as we are right now.</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Jonathan Madeley, co-Founder of Battle Crate</span>
</div>

Gaming is an industry where you either get big quick or die slowly. As Battle Crate receives a massive boost in users, they have to scale up and scale out fast. Being able to spin up new instances of RavenDB on-demand in all regions is going to be essential for Battle Crate's success story. RavenDB is a distributed database that can handle the load and the scope of what they need.

It's master-master setup lets anyone write to a single node, with that node replicating throughout the system. Battle Crate, based in the UK, can write locally, but its users can read the info from anywhere. Users can also write new information locally, and send that data to Battle Crate servers at company HQ.

What makes a data management solution the database for startups is the ability to follow the cardinal rule for startups:

> Time is limited. Demands are not.

Developer costs are the primary expense for any hi-tech company at any stage of development. Every moment a developer can spend away from their database and onto a product is enormous. The more work RavenDB takes on, the less time and money diverts to developers tinkering with their database.

RavenDB proved to be so self-sufficient, Alan and Jonathan were able to reduce their time to market from 1 year to six months.

They didn't need to work on the schema because RavenDB is schemaless. They didn't have to take too much time developing indexes because RavenDB has automatic indexes. ACID transactions were a breeze once they realized how fast and reliable they were.

At the stage of their company lifecycle where the goals are product, product, product, using the database whose motto is "it just works" proved invaluable to managing expenses, time, and money.

<div class="margin-top margin-bottom">
    <a href="https://ravendb.net/learn/webinars/little-known-features-in-ravendb-document-oriented-database"><img src="images/little-known-features-in-ravendb-50-document-oriented-database.png" class="img-responsive m-0-auto" alt="Little Known Features in RavenDB 5.0"/></a>
</div>