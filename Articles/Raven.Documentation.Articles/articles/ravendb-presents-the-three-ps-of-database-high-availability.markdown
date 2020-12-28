# RavenDB Presents the Three Ps of Database High Availability
<small>How RavenDB Proliferates, Preserves, and Perpetuates your Data</small>

![RavenDB Presents The Three Ps of Database High Availability](images/ravendb-presents-the-three-ps-of-database-high-availability-main.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>

One of the most vital necessities of an OLTP database is constant availability. Whether it's an employee, customer, or developer accessing your application from Singapore, San Francisco, or Scandinavia, your data has to be whole, fresh, and available to them within milliseconds. The information has to appear on the screen the moment the *submit* button is pressed. 

Using RavenDB, we take a multi-pronged approach to making sure your data never sleeps. Even if an asteroid were to hit the Pacific Ocean, knocking out half the machines on earth, it's up to us to make sure your data is still accessible.

## Proliferating: A Replicating Cluster
<img class="floating-left" alt="RavenDB Presents The Three Ps of Database High Availability" src="images/ravendb-presents-the-three-ps-of-database-high-availability-pic1.jpg" />
The first step is to host your data at multiple locations. RavenDB enables you to host your database on a series of machines, called nodes, that connect to each other to form a network, called a cluster. Nodes can be physical servers, simple PCs being employed as servers, or even a Raspberry Pi device – a server the size of the palm of your hand that literally puts your database at your fingertips.

Once you designate which machines will become nodes, your database will be hosted on them. Every time an update is made to the data, other nodes will be updated to reflect the same state of data. The update will replicate to the nodes you decide. 

At all times your database will be available in multiple locations. 

To reduce latency, your nodes can be spread out to different offices nationwide, or worldwide. Clients in Asia can access the node in China while developers in Silicon Valley can use the node in San Francisco. 

{SOCIAL-MEDIA-FOLLOW/}

## Preserving: Auto Recovery

What happens when a node goes down? Less nodes serving the same number of requests can lead to resource exhaustion, causing load problems and destroying performance. 

A replicating cluster maintains data availability, but what about maintaining data performance? How can we make sure requests will still be served at the same speed, even if a node, or a couple of nodes go down?

Auto recovery is the process of instantly replacing nodes to make sure your cluster is always at 100%. 

Say you have a cluster of five nodes. You decide to host 3 databases on the cluster, one for sales, one for customer service, and another for expenses. Each of these databases are hosted on 3 nodes in the cluster. If the sales database is on nodes 1, 3, and 5, and node 5 goes down, node 1 can replicate the sales database to node 2, which is hosting the customer service and orders database. At all times each database will be available in three distinct places.

We maintain a “*chain of command*” among nodes to keep them working with one another. For every cluster, there is one leader, either 3, 5, or 7 voters who decide on the next leader if the lead node goes down, and the rest of the nodes are watchers, taking orders from the leader. This is done so all database decisions will be made by the lead node. If there is a disagreement among the nodes, an odd number of voters means there will not be a tie in any decision. 

Even communication within your cluster will always be fluid, keeping your data fresh at all times. 

What happens when all the voting nodes are down? The chain of command is part of RavenDB, so a protocol is always followed making sure your database is always assigning leaders and voters, keeping your distributed database moving. 

Performance is unaffected, and the user doesn't realize that anything has changed. This is ideal for hosting multiple applications using different databases. Each database can be housed on some, but not all the nodes in a cluster, giving you auto recovery when a node needs to be repaired.

{RAW}
{{WHITEPAPER_BANNER}}
{RAW/}

## Perpetuating: Offsite Replica

We have contingencies even if your entire cluster goes down. What happens when the “perfect storm” knocks out all your nodes at the same time?

RavenDB will have an offsite replica made. An offsite replica is a node that lies outside the cluster. Users have no connection with it at all, but the cluster will communicate with this node. Once an update is made, a node within the cluster will update the node outside the cluster. An active node is kept up to date, and can be used if the cluster is compromised. 

But what if there is an error? What if someone applies an update that impacts huge amounts of documents in your data, and that update was made incorrectly. It will be a big task to undo the damage. If a human error is made in an update, that inaccurate state of the data will be replicated to all the nodes in the cluster. 
Is there a way to get back the data before the damage was done?

You can configure your offsite replica to be updated with a time delay. For example, if you set the delay to 6 hours, then when an update is made at 3PM, the cluster will pass on to the offsite node these updates at 9PM. The state of data in the replica will represent the way the database looked 6 hours ago. 

If an error happened at 4PM, you can apply the offsite replica to the nodes in the cluster, and work from there.  

## Added Layer: Automatic Backups
<img class="floating-right" alt="RavenDB Presents The Three Ps of Database High Availability" src="images/ravendb-presents-the-three-ps-of-database-high-availability-pic2.jpg" />
We can take this one step further by turning your database into a time machine. 

You can tell your RavenDB that every X hours, one node in the cluster will backup your data into a file. Your file can be saved locally, to a network location, or to the cloud. Your database becomes a file.

The first time the node copies your database to the file, it will be a full backup. For each subsequent time, it will be an incremental backup, sending just the updates from the last time you created the file.  

Backups allow you to have long term storage, and point in time recovery. You can save your files based on the time you made the backup. Say you want to see the state of the data for end of year 2016? Simply call the file *backup_2016_31_12*. You need to audit something last week? Go to the file that was backed up on Friday. Was there a data disaster? A human error that changed 70% of the information stored? You can recall the file to right before, and restore everything. 



<div class="bottom-line">
<p>
<a href="https://ravendb.net/"><strong>RavenDB</strong></a> is the industry's premiere NoSQL ACID Document Database. Easy to install, quick to learn, and fast to secure, RavenDB is fully transactional across your entire database. RavenDB can be used on-premise or in cloud solutions like Amazon Web Services (AWS) and Microsoft Azure.
</p>

<p>
RavenDB has a built-in storage engine, <em>Voron</em>, that operates at speeds up to 1,000,000 reads and 100,000 writes per second on a single node using simple commodity hardware. With RavenDB, you can <a href="https://ravendb.net/features"><strong>build high-performance</strong></a>, low-latency applications quickly and efficiently.
</p>

<p>
Go schemaless and take advantage of our dynamic indexing to stay agile and keep your release cycle efficient. Avoid speed bumps in your development process with RavenDB's high powered diagnostics. It reduces your need for tech support by detecting issues before they become problems.
</p>

<p>
With our SQL like query language, you can enjoy the computing power of NoSql with the functionality and ease of SQL for an all-in-one next generation solution. RavenDB also gives you a distributed data cluster, flexibility, and rapid scalability with low overhead.
</p>

<p>
Try us out! <a href="https://ravendb.net/download"><strong>Grab RavenDB 4.0 for free</strong></a>, and get 3 cores, our state of the art GUI, and a 6 gigabyte RAM database with up to a 3-server cluster up and running quickly for your next project.
</p>

</div>