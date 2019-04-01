# Installation: Embedded

RavenDB makes it very easy to be embedded within your application. The first step is to reference the `Raven.Database.dll`, either via nuget (package name: `RavenDB.Database`) or by taking the files from the zip distribution package.

After referencing the `Raven.Database.dll` from your project, all that is left to do is initializing:

{CODE embedded_1@Server\Installation\Embedded.cs /}

## HTTP access

By default you don't have an external access to RavenDB, so you cannot use the **Studio** to look at what the database is doing, or to use the REST API. Other features relying on being able to communicate over HTTP (like replication) will be disabled too.

RavenDB can be run in an embedded mode with HTTP enabled. To do that, you will just need to set another flag when initializing the embedded document store:

{CODE embedded_2@Server\Installation\Embedded.cs /}

Note that you may want to call `NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(port)` to ensure that you can open the HTTP server without requiring administrator privileges.

Once you initialized the document store, you can access the Studio directly, execute replication scenarios, etc.

## Configuration

Many configuration options are available for tuning RavenDB and fitting it to your needs. See the [Configuration options](https://ravendb.net/docs/server/administration/configuration) page for the complete info.
