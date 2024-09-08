# Getting Started: Writing your Unit Test using TestDriver
---

{NOTE: }

* In this section, we explain how to use [ravendb-test-driver](https://central.sonatype.com/search?q=g:net.ravendb%20%20a:ravendb-test-driver&core=gav&smo=true) 
  to write RavenDB unit tests.  

* TestDriver uses an [Embedded Server](../server/embedded) package with the same set of 
  [prerequisites](../server/embedded#prerequisites) as embedded servers to run the tests.  

* In this page: 
   - [RavenTestDriver](../start/test-driver#raventestdriver)
   - [Pre-initializing the store](../start/test-driver#preinitialize)
   - [ConfigureServer](../start/test-driver#configureserver)
   - [Unit test](../start/test-driver#unittest)
   - [Complete example](../start/test-driver#complete-example)

{NOTE/}

{PANEL: RavenTestDriver}

First, please make sure that the Raven TestDriver is added to your project dependencies.  

```
<dependency>
    <groupId>net.ravendb</groupId>
    <artifactId>ravendb-test-driver</artifactId>
    <version>4.1.3</version>
    <scope>test</scope>
</dependency>
```

Then, define a class that derives from the Raven TestDriver.

We can start by reviewing TestDriver methods, and then get into implementation 
(find a complete Raven TestDriver code sample [below](../start/test-driver#complete-example)).  

---

### Methods

| Signature | Description |
| ----------| ----- |
| **protected String getDatabaseDumpFilePath()** | Allows you to override the path to the database dump file that will be loaded when calling importDatabase. |
| **protected InputStream getDatabaseDumpFileStream()** |  Allows you to override the stream containing the database dump that will be loaded when calling importDatabase.  |
| **public IDocumentStore getDocumentStore()** | Gets you an IDocumentStore instance. |
| **public IDocumentStore getDocumentStore(String database)** | Gets you an IDocumentStore instance for the requested database. |
| **public IDocumentStore getDocumentStore(GetDocumentStoreOptions options)** | Gets you an IDocumentStore instance. |
| **public IDocumentStore getDocumentStore(GetDocumentStoreOptions options, String database)** | Gets you an IDocumentStore instance for the requested database. |
| **protected void preInitialize(IDocumentStore documentStore)** |Allows you to pre-initialize the IDocumentStore. |
| **protected void setupDatabase(IDocumentStore documentStore)** | Allows you to initialize the database. |
| **protected Consumer<RavenTestDriver> onDriverClosed** | An event that is raised when the test driver has been closed. |
| **public static void configureServer(ServerOptions options)** |Allows you to configure your server before running it|
| **public static void waitForIndexing(IDocumentStore store)** | Allows you to wait for indexes to become non-stale. |
| **public static void waitForIndexing(IDocumentStore store, String database)** | Allows you to wait for indexes to become non-stale. |
| **public static void waitForIndexing(IDocumentStore store, String database, Duration timeout)** | Allows you to wait for indexes to become non-stale. |
| **protected void waitForUserToContinueTheTest(IDocumentStore store)** | Allows you to break the test and launch the Studio to examine the state of the database. |
| **protected void openBrowser(String url)** | Allows you to open the browser. |
| **public void close()** | Allows you to dispose of the server. |

{PANEL/}

{PANEL: PreInitialize}

Pre-Initializing the IDocumentStore allows you to mutate the conventions used by the document store.

### Example

{CODE:java test_driver_PreInitialize@Start\RavenDBTestDriver.java /}

{PANEL/}

{PANEL: UnitTest}

We'll be using [JUnit](https://junit.org/) for my test framework in the below example.
Note that the test itself is meant to show different capabilities of the test driver and is not meant to be the most efficient.
The example below depends on the `TestDocumentByName` index and `TestDocument` class that can be seen in the [full example](../start/test-driver#complete-example)

### Example

{CODE:java test_driver_MyFirstTest@Start\RavenDBTestDriver.java /}

In the test we get an IDocumentStore to our test database, deploy an index and insert two documents into it.  
We then wait for the indexing to complete, and launch Studio so we can verify that the documents and index 
are deployed (we can remove this line once the test is working).  
Finally we query for a TestDocument whose name contains the word 'hello' and assert that we have only one 
such document.  

{PANEL/}

{PANEL: ConfigureServer}

Before RavenDB server can be started, TestDriver extracts binaries to `targetServerLocation` (Default: `.`). Optionally before doing this, target directory can be cleaned up (when `cleanTargetServerLocation` option is turned on (Default: false)). 

The `configureServer` method allows you to be more in control on your server. 
You can use it with `ServerOptions` to change the target path where Raven server binaries are extracted to or to specify where your RavenDB data is stored, security, etc.

{INFO: ServerOptions}

`ServerOptions` gives you control of how the embedded server is going to run
with just a minor change. Here you can change your targetServerLocation.

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **targetServerLocation** | string | The temporary path used by TestDriver to extract server binary files (.dll) |
| **logsPath** | string | Path to server logs files |
| **dataDirectory**  | string | Path where server stores data files |
| **cleanTargetServerLocation** | boolean | Should we remove all files from targetServerLocation before extracting server binaries? |

{INFO /}

### Example

{CODE:java test_driver_ConfigureServer@Start\RavenDBTestDriver.java /}

{PANEL/}

{PANEL:Complete Example}

{CODE:java test_full_example@Start\RavenDBTestDriverFull.java /}

{PANEL/}


## Related Articles

- [Running an Embedded Instance](../server/Embedded)

### Troubleshooting

- [Collect information for support](../server/troubleshooting/collect-info)
