# How to Spin up a RavenDB Cloud Database
<small>by <a href="mailto:ayende@hibernatingrhinos.com">Oren Eini</a></small>

![How to Spin up a RavenDB Cloud Database](images/codeproject-presents-ravendb-cloud-step-by-step.jpg)

{SOCIAL-MEDIA-LIKE/}

<br/>
It takes more time to read about how to spin up a RavenDB Cloud Database than to spin it up. Judah Gabriel Himango does a great job of <a href="https://www.codeproject.com/Articles/5164620/Getting-Started-with-RavenDB-Cloud-Database" target="_blank" rel="nofollow">walking you through it</a>.

See how Judah spins up a free instance of RavenDB Cloud. He secures it with a certificate and then shows you the code to connect from a C# web app.

Judah also guides you to start up a 3-node cluster in RavenDB Cloud where you'll be able to see how databases in the group are automatically synced to replicate to all the other nodes. Changes in one node automatically replicate to the other nodes throughout your cluster to keep your database resilient to failure.

Since RavenDB reads and writes to any of the nodes in your cluster, latency is minimal and performance is pushed to the max.

Judah goes over in detail:

* Setting up a free RavenDB [DBaaS Instance](https://ravendb.net/articles/how-ravendb-cloud-dbaas-bolsters-your-price-predictability),
* Connecting your application to the database,
* Querying the Database,
* Working with multiple nodes in a database cluster,
* Creating and Querying in your RavenDB database cluster.

See the article at <a href="https://www.codeproject.com/Articles/5164620/Getting-Started-with-RavenDB-Cloud-Database" target="_blank" rel="nofollow">codeproject.com</a>.
