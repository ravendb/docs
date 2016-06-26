# Backward compatibility

When it comes to client server compatibility in RavenDB there are two valid rules. Either:

 * The server version is within the same Major and Minor range as the client version

**OR**

 * The server version is greater than the client version

Some examples:

 * `3.0` client will work with `3.5` server since server is greater than client
 * `3.5` client will work with `3.5` server since client and server are in the same Major and Minor range
 * `3.5.3785` client will work with `3.5.3528` server since client and server are in the same Major and Minor range
 * `3.5` client will **NOT** work with `3.0` server since client is greater than server
 * `3.0` client will **NOT** work with `2.5` server since client is greater than server

## Upgrading

To properly upgrade your applications and server, we advise you to upgrade the server first, then the clients. This way your applications will keep working as before, and you can update them one-by-one if needed.

## Related articles

- [Server : Upgrading to a new version](../../server/installation/upgrading-to-new-version)