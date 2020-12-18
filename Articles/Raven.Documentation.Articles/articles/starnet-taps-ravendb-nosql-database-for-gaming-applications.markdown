# A NoSQL Database for Online Gaming
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/starnet-taps-ravendb-nosql-database-for-gaming-applications.jpg" alt="A NoSQL Database for Online Gaming" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

StarNet chooses RavenDB NoSQL database to meet the fast-growing gaming industry demands.

<a href="http://www.starnet.co.ba/" target="_blank" rel="nofollow">StarNet</a> specializes in Domain-Driven Design of information systems across all platforms (desktop, web, and mobile).

Their primary domain is sports betting and RNG casino games. The company's objective is to provide a platform to organize, market, promote, manage, support, and operate all types of betting and other operations of betting exchange, interactive casinos, bingos, lotteries, and other interactive games.

For years, they were using a relational database for storage, processing, and reporting.

As mobile devices increased traffic and tax regulations started to change, there was a need to upgrade the platform to a system that was more flexible to the changes taking place.

They needed the ability to support new features for users rapidly.

SQL Server makes schema change hard. Delays in the delivery of features mean a lot of lost customers. If there is a tournament, like the Super Bowl, there is a surge of activity. There is a need to adapt on the flu, usually within hours. This cannot be done with a schema.

A major upset in the NCAA Sweet Sixteen or the final rounds of the U.S. Open can send an unexpected flurry of transactions to the application. Higher traffic also generates new opportunities. A radical new approach to system design was needed.

StarNet switched from a classic three-tier to an eventually consistent microservice architecture based on event sourcing and a Command Query Responsibility Segregation (CQRS) architecture pattern. They had to carefully choose underlying components to support their new DDD (Data-Driven Design) approach.

Crucial areas were messaging, event store, and read model for OLTP and reporting.

## A NoSQL Database for StarNet's Software Architecture

![StarNet's Software Architecture](images/starnet-software-architecture.jpg)

A flexible and fast database establishes a foundational layer of managing game life cycles, where thousands of devices will be triggering betting transactions.

They had to choose a [NoSQL database](https://ravendb.net/why-ravendb) for the read model component.

## Bluffing with MongoDB and Folding on Cassandra

StarNet started evaluating several NoSQL Database products on the market. They gave MongoDB a shot first. After a thorough evaluation, they determined that MongoDB did not support the critical features they needed.

The NoSQL Database they needed had to include:
<ul>
    <li>Atomic multi-document updates (ACID)</li>
    <li class="margin-top-sm">Fast optimistic consistency</li>
    <li class="margin-top-sm">Rich SQL connectivity and transfer</li>
    <li class="margin-top-sm">The ability to tune the database easily for highly concurrent updates</li>
    <li class="margin-top-sm">Ease of database administration</li>
    <li class="margin-top-sm">Excellent support</li>
    <li class="margin-top-sm">Seamless integration into the .NET environment and Docker friendliness</li>
</ul>

MongoDB and Cassandra offers these features but at too high a price. StarNet's evaluation of these databases uncovered that their versions of schema flexibility and adding extra resources to tune these NoSQL databases were too expensive.

They then reviewed RavenDB.

They performed the same tests on RavenDB as they did for MongoDB and Cassandra. StarNet was very pleased with how RavenDB's ground-up approach met its core requirements.

<div class="margin-bottom">
    <a href="https://cloud.ravendb.net"><img src="images/ravendb-cloud.png" class="img-responsive m-0-auto" alt="Managed Cloud Hosting"/></a>
</div>

For example, executing a multi-document update on RavenDB in a single transaction as users scaled was easy and did not cause any performance degradation. StarNet was able to tune the database on rare occasions with [negligible downtime](https://ravendb.net/learn/webinars/when-failure-is-not-an-option-for-your-database). They also had to transfer NoSQL data to an SQL Server for legacy player stats and reporting. That too, was easy to configure.

They used C# to develop their betting application. RavenDB supports C#, Node.js JavaScript, Java, C++, Python, and Go Clients. It optimizes database operations exploiting parallel async constructs of C#, so the performance of the gaming application on RavenDB was fast as well.

StarNet loved the open-source nature of RavenDB. During platform development, they were able to contact the RavenDB community and get help. One person was able to easily migrate their legacy SQL Server schema to RavenDB NoSQL while keeping most of the C# code intact.

NoSQL empowered them to add new features rapidly and release them in the fast-changing betting industry. RavenDB's power and flexibility were already at the heart of some components used by their systems, such as NServiceBus and ServiceStack.

This reduced the overall complexity of the system.

## Doubling Down on RavenDB

Thanks to RavenDB, StarNet now has a platform that is reliable, borderless, secure, and fast for online gaming. Delays and sluggishness are gone, and end-users can now enjoy a far better online experience.

StarNet plans to expand its cluster size. They want to onboard more legacy game applications onto RavenDB. They want to deploy more applications to a RavenDB cluster that is centrally managed and reportable while [scaling out to accommodate thousands of users](https://ravendb.net/articles/from-monolith-to-microservices-scaling-out-your-architecture) using devices of various forms and capabilities. Rapid deployment of new features onto new devices for new games and quick adaption to new government regulations and laws would not have been possible without RavenDB.

<div>
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner.jpg" class="img-responsive m-0-auto" alt="Schedule a FREE Demo of RavenDB"/></a>
</div>

