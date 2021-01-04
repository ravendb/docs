# A NoSQL Database That Fits in Your Network With Ease
<small>by <a href="mailto:mor@ravendb.com">Mor Hilai</a></small>  

<div class="article-img figure text-center">
  <img src="images/etl-website.jpg" alt="RavenDB’s Inter-Database Communication" class="img-responsive img-thumbnail">
</div>

{SOCIAL-MEDIA-LIKE/}  

## RavenDB’s Easy Tools for Inter-Database Communication  

RavenDB can be deployed in a wide variety of configurations, ranging from a single server powering a simple website, to sprawling networks of different and loosely connected databases. Building and maintaining such networks can be a huge headache. In this article I’ll introduce you to a few features that can help you set up complex configurations of databases and have them communicating autonomously, efficiently, and flexibly, with minimal setup and maintenance. These are <a href="https://ravendb.net/features" target="_blank">*external replication, pull replication,* and *ETL*</a>.  


Even as the world gets smaller and more integrated, there are still frontier regions where connectivity is not a given, or communication is less direct. In many scenarios databases must be able to function independently when separated, but be ready to trade information quickly once they’re reconnected. Though none of these challenges new, the rise of the internet of things promises to create many more of these ‘frontiers’ in years to come.  


As an example, imagine you have a fleet of cargo ships sailing the world’s seas. Each one has a database for its own cargo manifest. You also have a large central database that holds all the information for the entire fleet, and all the shipments that need to be performed. While out in the vast empty ocean, your ships have no internet connection, so each time one of them docks at port it downloads all the information it needs. This might include things like new cargo it’s taking aboard, current weather data, or updates to its route. It also *uploads* information that the ship’s database collected during the journey, like the needs of the crew or the state of their supplies.  

### Database Replication  

Data is automatically distributed among RavenDB instances that are in the same cluster - we call this *replication*. But there is also *external replication*, which creates a copy of the database on a server outside its original cluster. The replication is a continuous process, creating a live copy. (The replication can also be offset by some amount of time. This allows for replication of a database with a ten minutes delay, allowing a little time to verify the data before sending it).  

But external replication is a ‘push’ service, meaning it is initiated by the source database. Transferring data this way makes sense when the network is reliable, but if the target database is unreachable, RavenDB has nowhere to push to. Your cargo ships can’t communicate with the central database until they’re docked, and try as they might to keep to their schedule, the exact time they’ll be docked is unpredictable. Trying to push data to them is a waste of bandwidth.  

### Pull Replication  


This is why we recently added the *pull replication* feature. Pull replication is initiated by the destination database, so that the transfer of data will occur only when the connection is reestablished. This also allows the destination database to perform its own logic to decide whether and when it needs updates.  

The strength of this feature is its simplicity. In just a minute, a RavenDB instance can be configured as a pull replication *hub*, or the data’s source. When this is done, you export the hub’s configuration, and import it into other RavenDB instances, which are designated pull replication *sinks* - the data’s destination. This process also takes just a minute. From here on forward, you can add as many sinks as you want whenever you want and the hub never needs to be updated. This is in contrast to external replication, which requires the source server to be updated for each new target you add.  

Each hub also has a separate encryption certificate just for pull replication (which can be added manually or generated automatically) adding an extra layer of security. Plus, because the connection is initiated by the destination server, this gives the connection direct access, <a href="https://ravendb.net/news?subcategory=database-security" target="_blank">saving the need to configure firewalls</a> to trust the source.  

### ETL  

So far we’ve discussed simple, direct sharing of a whole database. *ETL* is the familiar process of transferring data between databases and also filtering and transforming it on the way. In RavenDB it’s very easy to define new ETL pipelines. ETL can be configured to transfer data from one collection to another rather than being limited to whole databases. After selecting source and destination collections, all that’s needed is a transform script that filters and modifies the data, written in JavaScript with some dedicated RavenDB syntax. Sort of like external replication, the process occurs each time a document is added to, modified, or deleted from the source collection(s). ETL pipelines can be created between any two RavenDB instances, but also from RavenDB to any of the <a href="https://dzone.com/articles/top-5-sql-databases" target="_blank" rel="nofollow">major SQL databases</a> (including Oracle, SQL Server, PostgreSQL, MySQL)!  

ETL is a push service, and it overwrites data. If the script modifies a document that already exists in the target database, the document will be deleted and replaced with the output from ETL. This is important to remember when using ETL, but it can be an advantage because that means ETL does not create conflicts or require additional traffic to double-check everything.  

ETL grants versatility that simple replication doesn’t. Returning to our ship analogy, suppose a ship has a database that tracks its course, its direction and speed, and the currents and weather that it encounters. Rather than replicating this massive amount of data to the central database, an ETL script could be used to filter out only the highlights, such as an hourly update of the ship’s position, or send weather data only during extreme weather conditions. From the other direction, a central database could be set up like a bucket where you chuck all the vast amount of data about your whole fleet, and ETL will serve as the filter that sends each ship only the data that it needs.  

<hr>

I hope these features have shown you how RavenDB is not just good as a lone database, but is also great at fitting into a larger network environment or data pipeline with minimal fuss. You won’t have to build and test all your plumbing by yourself - RavenDB is a NoSQL database that *just fits*.  
