# Settings : ETL (Collection Specific Replication)

This is where you setup [ETL](../../../server/scaling-out/replication/etl):   
You can choose `Skip index replication for all destinations` to configure it globally instead of per destination.
![Figure 1. Settings. ETL. First Screen.](images/settings_etl-1.png)

{PANEL:Adding an ETL destination}

Adding an ETL destination is quite similar to [replication setup](./replication), with a few key differences:   
- There is no `Client failover behavior` and `Conflict resolution` in ETL   
- A collection (to replicate from) must be chosen

Simple ETL can be configured by choosing the source collection and providing the destination Url and Database.

![Figure 2. Settings. ETL. Add Destionation.](images/settings_etl-2.png)
![Figure 3. Settings. ETL. Choose Collection.](images/settings_etl-3.png)
{PANEL/}

{PANEL:Defining Transformers}

You can easily define a [transformer](../../../transformers/what-are-transformers) for each collection-based replication. 
Choose the collection you want replicated and add your own script to manipulate the data.

![Figure 4. Settings. ETL. Define Transformers.](images/settings_etl-4.png)

{PANEL/}

{PANEL:Advanced Options}
Some advanced options you can use are:   
- `Force replicate all indexes and transformers`   
- `Skip Index Replication` - for the current destination   
- `Transitive Replication` - marks what document types should be replicated:   
	- Changed only - locally   
	- Changed and replicated - from other sources   

{WARNING: Warning: Failover not supported in ETL}
An important consideration with filtered replication is that because the data is filtered, a destination 
that is using filtered replication isn't a viable fallback target, and it will not be considered as such by the client. 
If you want failover, you need to have multiple replicas, some with the full data set and some with the filtered data.
{WARNING/}

![Figure 5. Settings. ETL. Advanced Options.](images/settings_etl-5.png)

{PANEL/}

## Related articles

- [Server: Scaling-Out: Replication: ETL](../../../server/scaling-out/replication/etl)
- [Transformers: What are transformers?](../../../transformers/what-are-transformers)
- [Server: Scaling-Out: Replication: How Replication Works](../../../server/scaling-out/replication/how-replication-works)
- [Studio : Settings : Replication](../../../studio/overview/settings/replication)
