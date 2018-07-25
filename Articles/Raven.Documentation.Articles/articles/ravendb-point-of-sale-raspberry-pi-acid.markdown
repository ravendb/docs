# Performing Well on Older Machines and Smaller Hardware<br/><small>by <a href="mailto:ayende@ravendb.net">Oren Eini</a></small>

![Performing Well on Older Machines and Smaller Hardware](images/ravendb-point-of-sale-raspberry-pi-acid.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

A client of RavenDB had a challenge. They needed RavenDB on all their point of sale systems.

Challenge did you say? 

That’s what RavenDB does best, operational database work, especially for transactions that need an ACID guarantee while maintaining top performance.

It’s kind of like asking Beyonce to perform a complicated dance routine in front of 80,000 people at MetLife Stadium.

No problem either, right?

What if she was told that she could only dance in an area the size of a walk-in closet?

What if she was told that she would have to partner up in this condensed dance space with Steve Wozniak?

There’s your problem.

Like the Queen Diva, don’t underestimate what’s possible with <em>a spark of innovation</em>.

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

## Working When You Have Little to Work With

When it comes to brick and mortar companies, as long as computer performance doesn’t impact customer service, upgrading point of sale hardware can have the same priority as remodeling the employee bathroom. Even on hardware that is aging, a database can process a transaction faster than the cashier can scan the item.

Upgrading hardware can be costly.

This client is a leading fast food company with over 36,000 restaurants worldwide. That means hundreds of thousands of point-of-sale machines serve <em>billions of customers worldwide</em>. Upgrading can cost the company billions, so they are done sparingly. RavenDB will often find itself operating on machines that haven’t been upgraded for 10 years. They have limited processing power, even more limited memory, but still must serve demands made on state of the art machines.

It’s like asking Steve Wozniak to keep up with Beyonce’s best. You better be one heck of a choreographer.

RavenDB is embedded on <em>over 1.5 million point of sale machines</em>, and it performs exceedingly well while sharing memory with an aging operating system. RavenDB has been fine tuning its memory usage over a decade.  As a result, customers are served as fast as they would be anywhere else, and the company <em>saves a ton on hardware costs</em>.

## Reversing the Reckoning

{RAW}
{{INTRO_VIDEO}}
{RAW/}

RavenDB is self-optimizing, requiring low overhead. Our client may have RavenDB installed inside its 36,000 restaurants, but they don’t need 36,000 Database Administrators to manage it.

RavenDB is built to <em>save you time and money</em> by being flexible enough to support any type of decision made to maximize the bottom line.

One day this client will need to upgrade its hardware. The machines will get so old that they will have to be replaced. That’s going to cost a lot of money.

RavenDB 4.1 is able to fit an entire database on a Raspberry Pi or an ARM chip, the chips that fit inside your Smartphone, tablet, or any other embedded device. The next generation of machines for clients is to enable them to replace large servers with Raspberry Pis, or even mobile devices acting as both clients and servers.

We estimate that replacing current point of sale machines with Raspberry Pis will save them at least 65% of the replacement costs while maintaining their performance and enabling them to increase the services they can offer their customers.

## Serving Your Business

Any company with a large amount of point of sale machines can use RavenDB with the confidence that if they decide to keep these machines on hand for a long time, focusing their resources on more business-critical areas, they can do so while maintaining the number of smiles on their customers faces.

If and when they do decide to replace their systems, they have the chance to <em>increase performance and functionality</em>, while <em>saving money</em> in the process.

<div class="bottom-line">
    <p>
        <a href="http://ravendb.net/"><strong>RavenDB</strong></a> is the industry’s premiere NoSQL ACID Document Database. Easy to install, quick to learn, and fast to secure, RavenDB is fully transactional across your entire database cluster. RavenDB can be used on-premise or in cloud solutions like Amazon Web Services (AWS) and Microsoft Azure.
    </p>
    <p>
        RavenDB has a built-in storage engine, <em>Voron</em>, that operates at speeds up to 1,000,000 reads and 100,000 writes per second on a single node using simple commodity hardware.
    </p>
    <p>
        Go schemaless and take advantage of our dynamic indexing to stay agile and keep your release cycle efficient. Expand to IoT by fitting RavenDB onto a Raspberry Pi or an ARM Chip. We perform faster on these smaller servers than anyone else.
    </p>
    <p>
        <a href="https://ravendb.net#play-video"><strong>Watch our Explainer Video</strong></a> or <a href="https://ravendb.net/downloads"><strong>Grab RavenDB 4.0 for free</strong></a> and get 3 cores, our state of the art GUI, and a 6 gigabyte RAM database with up to a 3-server cluster up and running quickly for your next project.
    </p>
</div>
