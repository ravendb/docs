# ACID 2020: The Next Generation of Data Consistency
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/acid-cluster-distributed-nonrelational-database.jpg" alt="ACID 2020: The Next Generation of Data Consistency" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<br/>

For decades, ACID and the relational database went hand in hand. As the pace and scope of data broke the mold of relational solutions, a new way had to be developed.

Nonrelational databases historically worked for things like a CMS or newsfeed, but for operations like financial transactions, its inability to insure data consistency made it useless to those who needed it most.

Until RavenDB came up with a solution by becoming <em>the pioneer database</em> to offer ACID in a nonrelational context. In 2010, RavenDB offered <em>ACID consistency across multiple documents</em>.

In doing so, large enterprises were able to attach an instance of RavenDB to <em>millions of points of sale systems</em> and enjoy the scale that is possible with NoSQL while holding on to the data safety they needed.

That was almost 10 years ago. In 2010, there were 1.8 billion internet users, mostly accessing the internet from their desktops. Times have changed.

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

The explosion of smartphones and other mobile devices have tripled the number of internet users to over 4 billion by 2020.

Traffic has increased over 100-fold. Unstructured data accounts for 98% of all information being passed along in the world. The need for databases to handle this tsunami wave of terabytes with nonrelational solutions has only intensified.

Include over 20 billion connected “things”, all moving data back and forth and you have a lot of data that needs to be processed in a nonrelational way.

## The Challenge to the Old ACID Model

A database was considered ACID if it could be ACID by the individual node. The challenge comes in when waves of traffic hit your applications and multiple users try to do the same thing on different nodes.

Say 2 people want to book a plane ticket from London to Singapore. They both log onto a travel site that supports a 5-node data cluster with replication ability.

The user in London books his flight at the same time another user in Liverpool books hers. Two different nodes in the data cluster write the transaction.

What happens when you replicate to the cluster? One write will be committed to the database, and the other will be discarded in its entirety, likely frustrating a customer.

Imagine what happens when they reserve a night at a hotel or front row seats to a play.

## ACID 2020: Clusterwide Consistency with Distributed ACID

To solve this problem, RavenDB has expanded its ACID feature from being ACID over multiple documents to being ACID <em>over your entire cluster</em>, enabling <em>distributed ACID</em>, the next step in data consistency.

{RAW}
{{INTRO_VIDEO}}
{RAW/}

The idea is simple: You can take a set of changes and send them to the entire cluster. Your set of changes will be applied, in atomic fashion, to all the nodes in the cluster only if they have been accepted by a majority of nodes. If not, then the entire transaction will be rejected and the user will be notified that the transaction didn’t go through.

This resolves the concurrency issue.

Only when a plane ticket order has been committed throughout the entire cluster will it be considered completed. Even use of inventory. If customer A purchases the last blood pressure machine, only when the majority of the cluster agrees that inventory is now zero will it be committed to the database and the user get a confirmation of purchase. If another user makes the same buy at the same time and it doesn’t replicate to the majority of nodes, he will get a notification that there are no more in stock.

Only when the entire cluster confirms that your write was committed to the majority of nodes, guaranteeing that it won't be overruled, do you get the confirmation that it was processed. No longer will you get the call from the travel agent telling you that due to "technical difficulties," you were bumped from your 5AM flight and got up in the middle of the night for nothing.

This is <em>the next generation of ACID</em>, a fully transactional solution that works in a NoSQL distributed data system to accommodate <em>todays speed and scale of Big Data</em>.

<div class="bottom-line">
    <p>
        <a href="https://ravendb.net/"><strong>RavenDB</strong></a> is the industry's premiere NoSQL ACID Document Database. Easy to install, quick to learn, and fast to secure, RavenDB is fully transactional across your entire database cluster. RavenDB can be used on-premise or in cloud solutions like Amazon Web Services (AWS) and Microsoft Azure.
    </p>
    <p>
        RavenDB has a built-in storage engine, Voron, that operates at speeds up to 1,000,000 reads and 100,000 writes per second on a single node using simple commodity hardware.
    </p>
    <p>
        Go schemaless and take advantage of our dynamic indexing to stay agile and keep your release cycle efficient. Expand to IoT by fitting RavenDB onto a Raspberry Pi or an ARM Chip. We perform faster on these smaller servers than anyone else. 
    </p>
    <p>
        <a href="https://ravendb.net#play-video"><strong>Watch our Explainer Video</strong></a> or <a href="https://ravendb.net/downloads"><strong>Grab RavenDB 4.0 for free</strong></a> and get 3 cores, our state of the art GUI, and a 6 gigabyte RAM database with up to a 3-server cluster up and running quickly for your next project.
    </p>
</div>
