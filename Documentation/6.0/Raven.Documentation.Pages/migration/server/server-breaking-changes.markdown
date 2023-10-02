# Migration: Server Breaking Changes

The following features, that were available in former RavenDB versions, 
are unavailable under RavenDB `6.x` or incompatible with their previous versions.  

* **License Keys** 
  License keys for versions lower than `6.0` are **not supported** by RavenDB `6.0`.  
  If you own a valid license key for RavenDB `5.x` or lower, please upgrade it using 
  the quick online interface [described here](../../start/licensing/replace-license#upgrade-a-license-key-for-ravendb-6.x).  
* **RavenDB for Docker**  
  RavenDB now applies an improved security model, and uses a **dedicated user** rather than `root`.  
  Read more about this change [here](../../migration/server/docker).  
* **Unsupported sharding features**  
  RavenDB 6.0 introduces [sharding](../../sharding/overview). Server and Client features 
  that are currently unavailable under a sharded database (but remain available in regular 
  databases) are listed [here](../../sharding/unsupported).  
* [Graph Queries](https://ravendb.net/docs/article-page/5.4/csharp/indexes/querying/graph/graph-queries-overview)  
  Graph queries support, available in RavenDB versions `4.2` to `5.x`, is removed from 
  RavenDB `6.x` server and client API.  
* **ETL**  
  SQL ETL no longer tolerates errors on `Load`: load errors are thrown immediately, to distinguish 
  partial load errors that are used in SQL ETL from, for example, commit errors that may happen 
  during load. (Prior to this change ETL would just advance instead of retrying.)  
* `DateOnly` and `TimeOnly` are now supported for every new auto index.  
