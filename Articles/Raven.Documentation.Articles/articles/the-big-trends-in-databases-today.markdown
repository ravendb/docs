# The Big Trends in Databases Today <br/><small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![The Big Trends in Databases Today](images/the-big-trends-in-databases-today.jpg)

{SOCIAL-MEDIA-LIKE/}

<p class="lead" style="font-size: 24px">As data evolves, so do the needs today's businesses have for their databases.</p>

The first trend is the need for proper edge processing. The Internet of Things and Mobile revolution has enabled billions of devices to pick up new forms of data in places never seen before. Data is being picked up at the bottom of the ocean, it is being picked up at the top of the skies, there are edge devices inside people's bodies that are tracking information - all for the benefit of humanity.

**It's an exciting time to be working in software**

The challenge is to process this data quickly, move it up the hierarchy by scaling all the challenges that come up when moving anything over a third-party network, and maintaining data integrity by keeping ACID *and* enabling your database to hold onto every bit of information even as parts of the network go down and your edge points have to keep working even as they wait to be reconnected to your database cluster.

The second trend is the cloud. Over the last few years, the top question our clients would ask was, "What do you offer on the cloud?".
Today, it's the opposite. They assume we have something on the cloud, and want to know what we offer on-premises.

Over 90% of new [databases are purchased on the cloud](https://ravendb.net/buy). Still, 10% of all information needs to be kept in-house at locally secured locations. Today's need is for a database that can be operated over any cloud, any operating system, any device, and over any local server.

**New Challenges Take Priority with New Trends in Demand**

<br/>
[![Managed RavenDB Cloud Hosting](images/ravendb-cloud.png)](https://cloud.ravendb.net/)
<br/><br/>

The complexity, size and scale of data that we are managing today is growing at exponential levels.

Take something simple like a standard invoice. On it appears the customer name, address, product, price, shipping instructions. Then you need more sophisticated data like the tax rate, payment method, discounts, special offers like awarding points to a card for such a purchase.

Over the last 20 years, over 1 billion people throughout the world have gone <a href="https://www.thetimes.co.uk/article/half-the-world-now-middle-class-as-living-standards-rise-in-east-v38l3m20s" target="_blank">from poverty to middle class consumers</a>. Every item for sale now has a huge audience, requiring lots of digital paperwork which needs a fast database to process it all and report the information to the right departments.

Simply the cost of storing and fetching all the data can take a significant amount of time and effort from the development team. Today's database needs to be fast, and simple - minimizing the developer's work as much as possible.

**Get all the data your database was set up to track**

RavenDB enables both push and pull replication, letting you move data seamlessly up and down your database hierarchy. The first Document Database to be Fully Transactional, RavenDB, is fault tolerant, continuing to work offline if one of the nodes in your data cluster is cut off from the rest of your cluster due to network partition. When the wire is restored, all the data your cluster missed, the node has recorded and will transmit to your cluster.

That means you get all the data your database was set up to track, and with ACID guarantees, giving you the highest standard in data integrity. ACID for over a decade, we have been able to ramp up performance within the [fully transactional framework](https://ravendb.net/features/high-availability), giving you everything you need to turn tomorrow's data into today's results.