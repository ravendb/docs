# FAQ: Backward Compatibility

When it comes to client server compatibility in RavenDB, there are two valid rules. 

Either:

 * The server version is within the same Major and Minor range as the client version

**OR**

 * The server version is greater than the client version, but the Major version must match

Some examples:

 * A `4.0` client will work with a `4.5` server since the server is greater than the client
 * A `4.5` client will work with a `4.5` server since both the client and the server are in the same Major and Minor range
 * A `4.0.2` client will work with a `4.0.1` server since both the client and the server are in the same Major and Minor range
 * A `4.5` client will **NOT** work with a `4.0` server since the client is greater than the server
 * A `3.x` client will **NOT** work with a `4.0` server since both the client's and the server's Major version do not match

 <br />

## Upgrading

### Same Major Version

To properly upgrade your applications and server, we advise you to upgrade the server first, then the clients. This way, your applications will keep working as before and you can update them one-by-one if needed.

### Different Major Version

Upgrading to a different Major version necessitates upgrading the server and all clients in lockstep. Please visit [migration guide section](../../migration/client-api/introduction) talking about migration from 3.x.

## Related Articles

### Installation

- [Upgrading to a New Version](../../start/installation/upgrading-to-new-version)
