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

The ability of a client API of version `7.0` to connect a server, depends 
on the **server's version** and the **compression algorithm the client uses**.  

* Connecting your client to a server of version `6.0` or higher presents no problem.  
* But if the server is of version `5.4` or earlier, the client must switch back to 
  `Gzip` to succeed to connect the server.  
  {INFO: }
  [See how to switch the algorithm to Gzip](../../migration/client-api/client-breaking-changes#http-compression-algorithm-is-now-zstd-by-default)
  {INFO/}

{PANEL/}
