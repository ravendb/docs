# Upgrade to a new version

Upgrading RavenDB instance to a new version is very simple. In order to do so, you:

- Shutdown the RavenDB server. (This is depends on your [deployment strategy](../../deployment/index.markdown). For a service - shutdown the service. For a IIS site - shutdown the IIS site.)
- Replace the binaries.
{NOTE Make sure to not delete your actual data which is the folders like `data`, `tenants`, `plugins` etc, or overwrite your configuration files like `Raven.Server.exe.config` or `web.config`. /}
- Start the server again.

On a live system, this process typically takes 30 seconds, which is fast sufficient in most of the cases.

However, if you want zero downtime, you can setup a failover server which will handle requests until the primery server is started up. The steps to do so are:

- Setup a secons server.
- Setup a replication between the primery server to the second.
- Wait until the second node will get all the docs from the primery node.
- Shutdown the first. You're now silently failover to the secondary server.
- Replace the binraies of the first and start it again.
- Shutdown and update the secondary server.