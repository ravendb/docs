# Backward compatibility

When it comes to client server compatibility in RavenDB there are two valid rules. Either:

 * The server version is within the same Major and Minor range as the client version

**OR**

 * The server version is greater than the client version

Some examples:

 * `4.0` client will work with `4.5` server since server is greater than client
 * `4.5` client will work with `4.5` server since client and server are in the same Major and Minor range
 * `4.0.2` client will work with `4.0.1` server since client and server are in the same Major and Minor range
 * `4.5` client will **NOT** work with `4.0` server since client is greater than server

## Upgrading

To properly upgrade your applications and server, we advise you to upgrade the server first, then the clients. This way your applications will keep working as before, and you can update them one-by-one if needed.

## Related articles

- [Server : Upgrading to a new version](../../server/installation/upgrading-to-new-version)
