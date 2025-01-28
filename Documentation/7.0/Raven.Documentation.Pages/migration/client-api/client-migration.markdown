# Client Migration
---

{NOTE: }

* In this page:
   * [Client migration to RavenDB `7.x`](../../migration/client-api/client-migration#client-migration-to-ravendb-7.x)

{NOTE/}

---

{PANEL: Client migration to RavenDB `7.x`}

Prior to version `7.0`, our default HTTP compression algorithm was `Gzip`.  
From version `7.0` on, our default HTTP compression algorithm is `Zstd`.  

Version `7.0`'s client API ability to connect to a server depends on the 
**server's version** and on the **compression algorithm the client uses**.  

* Connecting your client to a server of version `6.0` or higher presents no problem.  
* But if you want to connect the client to a server of version `5.4` or earlier, 
  you must switch the client algorithm back to `Gzip` for the connection to succeed.  
  {INFO: }
  [See how to switch the algorithm to Gzip](../../migration/client-api/client-breaking-changes#http-compression-algorithm-is-now-zstd-by-default)
  {INFO/}

{PANEL/}
