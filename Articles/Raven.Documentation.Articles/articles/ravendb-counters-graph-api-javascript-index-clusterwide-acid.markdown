# Code Project: RavenDB NoSQL Database Counters, Graph API, JavaScript Index, Clusterwide ACID
<small>by Kamran Ayub</small>

![RavenDB 4.2 NoSQL database comes with some fantastic new features: Graph Queries, Clusterwide Transactions, Distributed Counters, JavaScript Indexes, and Revert Revisions.](images/ravendb-counters-graph-api-javascript-index-clusterwide-acid.jpg)

{SOCIAL-MEDIA-LIKE/}


<div class="margin-top-xs margin-bottom-xs">
    <a href="https://www.codeproject.com/Articles/4553133/Whats-New-in-RavenDB-4-2" target="_blank" rel="nofollow">
        <img src="images/codeproject.jpg" class="img-responsive m-0-auto" alt="Codeproject Article"/>
    </a>
</div>

The release of RavenDB 4.2 comes with some fantastic new features that enable you to do a lot more with your NoSQL Database. RavenDB's latest version comes with Graph Queries, Clusterwide Transactions, Distributed Counters, JavaScript Indexes, and Revert Revisions.

Our new [Graph Query API](https://ravendb.net/features/querying/graph-api) allows you to perform *petabyte-scale* aggregation. You have the power to work with enormous datasets that contain many relationships among data points. This is great for health care systems looking to correlate symptoms with patient data or among groups of patients in specific areas. It is also suitable for insurance companies looking to detect fraud, which causes increased company expenses and higher premiums.

New to 4.2 are [cluster-wide ACID transactions](https://ravendb.net/docs/article-page/4.2/csharp/server/clustering/cluster-transactions). Along with enjoying ACID data integrity over multiple documents, you are also now covered throughout your entire cluster.

[Distributed counters](https://ravendb.net/features/extensions/distributed-counters) make massive-scale "incrementing counter" scenarios possible with minimal overhead because you don't have to write the full document to disk for every request. RavenDB also resolves distributed concurrent updates automatically. This is critical if you have your own "like" button for local content or a shopping cart with products you want your users to give stars to or share with their friends.

JavaScript indexes are an alternative way to perform complex modelling logic versus C#'s LINQ syntax. JavaScript indexes support referencing external scripts like Lodash or Moment.js to bring in additional global functions.

You can also revert documents to previous revisions without downtime by taking your database offline and annoying your users. RavenDB utilizes its existing <a href="https://www.codeproject.com/Articles/4553133/Whats-New-in-RavenDB-4-2" target="_blank" rel="nofollow">revisions feature</a> so you can restore to a snapshot in time while maintaining read-access to the database.

<div class="margin-top-xs margin-bottom-xs">
    <a href="https://ravendb.net/live-demo"><img src="images/live-demo-banner.jpg" class="img-responsive m-0-auto" alt="Schedule a FREE Demo of RavenDB"/></a>
</div>

