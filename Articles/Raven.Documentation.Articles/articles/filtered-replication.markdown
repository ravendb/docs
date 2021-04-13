<h1>Filtered Replication</h1>
<small>by <a href="mailto:ayende@ayende.com">Oren Eini</a></small>

<div class="article-img figure text-center">
  <img src="images/filtered-replication.jpg" alt="Filtered replication enables running the database in high availability mode, or creating a mesh network of independent, collaborating databases." class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}

<p class="lead margin-top-sm" style="font-size: 24px;">Filtered Replication enables you to deliver better applications by letting RavenDB shoulder the task of not only holding your data but securely disseminating it between the cloud and the edge.</p>

RavenDB is a [distributed database](https://ravendb.net/why-ravendb/high-availability). That can mean a lot of things so let me pinpoint exactly what I'm talking about. RavenDB allows you to run a database cluster using nodes that can be geo distributed with a multi-master topology. Let me explain further...

A RavenDB database can reside on multiple computers which can be located on the same data center or scattered throughout the globe. Multi-master topology means that there is no single leader among those nodes. A write or a read can be served by any of the nodes in the database without having to route it to a single location.

Originally, this feature was meant to allow you to run RavenDB in a high availability mode. You could continue to operate RavenDB even in partial failure scenarios because each node could act independently and they will all coordinate together to achieve the overall objective, keeping your data.

A multi-master, geo distributed, replication topology also has additional uses. You can also create a mesh network of independent, collaborating databases.

Consider the case of a system running on the edge, like a Point of Sales machine that is used to record orders and track inventory. RavenDB is running locally inside the store. Replication is set up from the edge to a centralized location, which aggregates the data across all the stores in the chain. This is a novel use of a feature in a way that we haven't really considered when we designed RavenDB's replication features.

### Replicate data to or from an untrusted party

In RavenDB 5.1, we have decided to focus on enabling more of these sorts of scenarios. In particular, we consider a situation where there is limited amount of trust between the replication participants. In distributed computing terms, this is called a Byzantine system.

Why would I want to replicate data to or from an untrusted party? Consider the standard web application, this is a Byzantine system. The client using the application may be malicious, as a long history of exploiting vulnerabilities in application will show.

In the context of database systems, we need to address accepting data from outside parties and sharing *some* data to them. Using the same chain store example, if the same organization is running all the stores, we can assume that all the participants are trusted. What if we are using the franchise model, though?

In this case, we certainly want to get the data from the franchise, and we want to send *some* data back to them. However, they shouldn't see anything related to *other* franchises. There can be a significant advantage for a franchise owner if they can see (or better yet, *modify*) data belonging to other franchise owners. While both the chain and the franchises are cooperating, they should also be careful about how much trust is given to each party.

### Filtered Replication on the cloud

RavenDB's [Filtered Replication](https://ravendb.net/features/replication/filtered-replication) is meant to be used for such a scenario. In this model, we have Hooper's (the candy store from Sesame Street), which has a franchise in every kid's imagination. Hooper's has a RavenDB cluster in the cloud and each physical store has a RavenDB server in a closet for managing the work inside the store.

In the cloud, we have data that we want to share we all the stores, such as the menu, recipes, price lists, etc. From the store side, we want to push the orders to the cloud since that is how commission is computed.

We define a Replication Hub on the cloud side, like so:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/filtered-replication-1.png" class="img-responsive m-0-auto" alt="Graph"/>
</div>

Note that this is a two-way hub which allows to push data to the stores as well as receive data from them. We also enable Replication Filtering to allow only some data to replicate through. You can see that we defined the access restrictions for Bert when using this Replication Hub:

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/filtered-replication-2.png" class="img-responsive m-0-auto" alt="Graph"/>
</div>

The Replication Hub endpoint is secured using X509 Certificates. We use the certificates both for strong authentication as well as the encrypt for all the data over the wire.

On the other side, we have Bert. He can configure his system to act as a Replication Sink, the other side of the Replication Hub defined by the cloud cluster of Hooper's Store.

<div class="margin-top-sm margin-bottom-sm">
    <img src="images/filtered-replication-3.png" class="img-responsive m-0-auto" alt="Graph"/>
</div>

Bert can also define additional filtering options to limit what documents will be sent and received from the central cluster.

The idea is that in this case, both Hooper's and Bert can define a fine-grained set of permissions that would detail what they are willing to accept and send to the other side. Hooper's is also able to define different set of permissions for each one of their franchises.

### Everything Will Just Work

This is all done in a [cryptographically secure](https://ravendb.net/docs/article-page/5.1/csharp/server/security/encryption/secret-key-management) manner, with strong authentication and encryption of the data to prevent any eavesdropping.

It also means that there is very little that is required from the Replication Sink. You don't need to mess about in the network configuration, define NAT traversals, firewall rules, etc. All you need is to have access to the Internet, and everything Will Just Work.

RavenDB enables a local first style of work. Any computation that is required to run the store can be done by Bert locally, without any dependency on the outside world. At the same time, Bert's RavenDB instance is taking care of syncing all the required data to the cloud behind the scenes, as well as continuously pulling updates.

I focused primarily on the bidirectional replication scenario in this article, but you can limit your scope further to specify replication that will send data only from the cloud to the edge or just accept data from the edge and aggregate it.

Using RavenDB, your application doesn't need to handle the fairly complex topic of managing on its own. It can focus on the business logic and shell out all such concerns to RavenDB. The fact that you can filter the data on both ends means that you have a good way to control exactly what is going on.