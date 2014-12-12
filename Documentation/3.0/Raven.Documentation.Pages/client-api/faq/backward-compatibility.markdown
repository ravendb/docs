# Backward compatibility

When it comes to backward compatibility, in RavenDB we follow the rule that _older clients are compatibile with newer servers, but do not work with older ones._

This means that **2.5 client will work with 3.0 server**, but not with 2.0 server and **3.0 client will NOT work with 2.5 server**.

## Upgrading

To properly upgrade your applications and server, we advise you to upgrade the server first, then the clients. This way your applications will keep working as before, and you can update them one-by-one if needed.

## Related articles

- [Server : Upgrading to a new version](../../server/installation/upgrading-to-new-version)