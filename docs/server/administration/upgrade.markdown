# Upgrade to a new version

Upgrading RavenDB instance to a new version is very simple. In order to do so, you:

- Shutdown the RavenDB server. (This is depends on your [deployment strategy](../../deployment/index.markdown). For a service - shutdown the service. For a IIS site - shutdown the IIS site.)
- Replace the binaries (including any bundles if you have them, their version has to match the server version).
{NOTE Make sure to not delete your actual data which is the folders like `data` or `tenants`, or overwrite your configuration files like `Raven.Server.exe.config` or `web.config`. /}
- Start the server again.

On a live system, this process typically takes 30 seconds, which is fast sufficient in most cases.

However, if you want zero downtime, you can setup a failover server which will handle requests until the primery server is started up. The steps to do so are:

- When you setup the primary server, you have to make sure to that it has the Raven.Bundles.Replication assembly in the `plugins` folder.
- Setup a second server, with the Raven.Bundles.Replication assembly in the `plugins` folder.
- Setup a replication between the primery server to the second. (Modify Raven/Replication/Destinations document to point to the secondary server).
- Wait until the second node will get all the docs from the primery node (but a minimum of 10 minutes, to make sure that any clients had the chance to get updated with the new replication infomration).
- Shutdown the first. You're now silently failover by the clients to the secondary server.
- Replace the binraies of the first and start it again.
- Shutdown and update the secondary server.

Licensing:

- If you have the secondary node running only for the during of the actual update, you don't need a license.
- If you have the scondary node running constantly (hot backup), you do need a license.