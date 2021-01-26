<h1>NoSQL Distributed Database as a Game-Changer for a Taxi Booking Platform</h1>
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/sneleentaxi-uses-ravendb-nosql-database-to-take-taxi-riders-on-a-joyride.jpg" alt="Sneleentaxi enables people to find the nearest taxi driver at the right price. They use NoSQL Database RavenDB in a distributed database cluster to make it happen." class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size:24px;">Sneleentaxi booking platform chooses RavenDB for its two-sided machine learning marketplace system.</p>

On the one side, the customer enters their pickup point and destination.

On the other side, an army of drivers stands ready. Once someone declares where they want to go, all drivers nearest to the customer will be notified of the fare and the price through a real-time reverse auction.

Drivers compete to take you places.

2020 was the real test for their application.

<p>People used their service to get to hospitals, testing centers, pharmacies, and clinics. During lockdowns, people lacked the mobility to flag down a taxi, so <a href="https://sneleentaxi.nl/" target="_blank" rel="nofollow">sneleentaxi</a> helped the most affordable driver come straight to their door.</p>

### Requirements for a NoSQL Database

Their development team are .NET coders who love C#, making RavenDB the perfect fit.

Based in the Netherlands, their growth has expanded that they need to be in as many points as possible, entering the world of distributed databases. An emerging microservices setup gives them the flexibility to scale up quickly with ease.

[NoSQL Database](https://ravendb.net/docs/article-page/5.1/csharp) RavenDB, with its multi-model ability to offer document, key-value, graph, and time-series models, is playing a core role in that expansion.

Cluster behavior is vital as they migrate from an internal API to an open API. As they expand from a monolith database architecture to a distributed setup, RavenDB ETL process makes migrating data easy, further enabling their cluster expansion.

As a startup rapidly maturing to scaleup, everything is continuously rewritten.

They worked with MongoDB, but the essential thing was seamless integration with .NET and C# and breakneck performance to meet real-time demands.

RavenDB was the only one to meet their standards. As the .NET ecosystem evolves, RavenDB adapts to it all.

### Database for Machine Learning

<p>Using machine learning, <a href="https://sneleentaxi.nl/" target="_blank" rel="nofollow">sneleentaxi</a>'s platform calculates best prices for riders and drivers and locates the drivers operating closest to a rider's location and destination. To participate in the platform, each driver must enter the zip codes they cover.</p>

The system is built to match the right driver to the correct route to minimize the rider's wait time.

Indexes are vital as every hour riders and drivers at varied points throughout the country request different destinations, requiring the platform to continually calculate multiple routes in real-time from possible drivers for a specific ride.

RavenDB automatic indexes are a game-changer for their performance requirements.

### The Future of sneleentaxi

<div class="flex-vertical text-center margin-top-sm margin-bottom-sm" style="align-items:center">
    <a href="https://sneleentaxi.nl" target="_blank" rel="nofollow"><img src="images/sneleentaxi.png" class="img-responsive m-0-auto" alt="ServerFlex"/></a>
</div>

<div class="f-s-quote margin-top-sm margin-bottom-sm">
    <span class="quote-content">Scaling to a distributed architecture with RavenDB saves a lot of valuable time.</span>
    <span class="quote-author margin-top-xs margin-bottom-xs">Areeb Laloe - CTO, sneleentaxi</span>
</div>

Their production database currently holds millions of documents. A transaction is completed every 3 minutes. Their service is the cheapest in the Netherlands right now, so other platforms can get the least expensive ride with them.

Their primary focus is for other apps to talk to their platform, enabling other ride offering services to use their driver network. They are also integrating with Holland's MaaS or Mobility as a Service.

MaaS gives its users lots of options for transportation. They have opportunities to book a train, bus, taxi, electric bike, even a scooter.

<p>Working with MaaS and other platforms makes <a href="https://sneleentaxi.nl/" target="_blank" rel="nofollow">sneleentaxi</a> available to a much larger audience, increasing traffic substantially.</p>

Large enterprises are also excited to work with them, priming the company to enjoy that upward trajectory at the end of the hockey stick.

This will require a distributed system with lots of nodes. With even more orders pouring in every minute, downtime will sting harder, so availability and effective failover management come into play.

This is where RavenDB stands out.

RavenDB is *always* a [distributed database](https://ravendb.net/why-ravendb/high-availability), even when there is only one node. Most databases will distinguish between being a part of a monolith system and a distributed one. One database is a separate world from several database nodes.

But it doesn't have to be.

Even in a monolith system, RavenDB operates as a single node inside a distributed system of one point. Expanding to a distributed network is simple as you merely need to expand additional nodes without further reconfiguration.

*It just works.*

<p>As the world opens up in 2021 to renewed commerce and excitement, <a href="https://sneleentaxi.nl/" target="_blank" rel="nofollow">sneleentaxi</a> is poised to see rapid growth in customers, infrastructure, and traffic.</p>

RavenDB is proud to join the ride.

**See for yourself! Schedule a live one-on-one [demo of RavenDB 5.1](https://ravendb.net/live-demo/) today.**

<div>
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner2.jpg" class="img-responsive m-0-auto" alt="Schedule a FREE Demo of RavenDB"/></a>
</div>