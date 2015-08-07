## What host should I use? IIS or Windows Service?


### IIS: Best to be used when the RavenDD is publicly accessible on the interwebs

- Pros: 
    - you get the benefits of the IIS management tools, monitoring and tracking abilities,
    - designed for small-to-large requests/hits,
    - heaps of security tests and much better support by security products (DDOS protection, IDS systems etc.),
    - it is battle hardened and experienced.

- Cons:
    - you might need to add the IIS specific configuration (such as increasing maximum request time for bulk inserts), for certain scenarios
    - lots of config options means you might mess something up,
    - recycle times (unless settings changed),
    - generally, a bit of work is required to setup.

### Windows Service: Fine if in a private network.

- Pros:
    - easy to setup - just part of the normal install process,
    - available if IIS installation is not an option.
- Cons: 
    - not tested for large requests or battle hardened for security.

There are no real performance considerations between running in the IIS mode or running in the Service mode. 
Both options are supported and the choice is mostly about what is easier for your ops team to support.


## Where to put RavenDB data?

- RavenDB data should be be in the fastest drive on the machine.
- It should put it in the root drive, specifically because that make it more visible and avoid issues such as admin deleting the IIS folder thinking there is nothing in there.
- Take a look at [data settings](../configuration/configuration-options#data-settings) section of configuration options article to properly configure paths.