#Running RavenDB in embedded mode with HTTP enabled

One of the benefits of RavenDB is that it makes it very easy to embed it inside your application. All you need to do is to just initialize it, and you are done:

    var documentStore = new EmbeddableDocumentStore
    {
        DataDirectory = "Data"
    };

But unlike the server mode, you have no external access to Raven, if you want to use the WebUI to look at what the database is doing, you can't, and other features that rely on being able to communicate over HTTP (like replication) aren't possible.

Luckily, you can run RavenDB in an embedded and HTTP mode easily enough, all you need to do is:

    var documentStore = new EmbeddableDocumentStore
    {
        DataDirectory = "Data",
        UseEmbeddedHttpServer = true
    };

Note that you may want to call NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(port) to ensure that you can open the HTTP server without requiring administrator privileges.

Once you initialized the document store, you can browse directly to the WebUI, execute replication scenarios, etc.