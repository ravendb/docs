# Getting Started: Writing your Unit Test using TestDriver

In this section we will explain how to use [RavenDB.TestDriver](https://www.nuget.org/packages/RavenDB.TestDriver/) in order to write unit tests for working with RavenDB.
TestDriver uses an [Embedded](../server/embedded) package with the same set of [prerequisites](../server/embedded#prerequisites) to run the Server.

- [RavenTestDriver](../start/test-driver#raventestdriver)
- [Pre-initializing the store](../start/test-driver#preinitialize)
- [ConfigureServer](../start/test-driver#configureserver)
- [Unit test](../start/test-driver#unittest)
- [Complete example](../start/test-driver#complete-example)
- [CI Servers](../start/test-driver#continuous-integration-servers)

{PANEL:RavenTestDriver}

First, we define a class that derives from Raven's TestDriver.
Lets start with reviewing the TestDriver's methods and properties and later we will get into implementation (a complete code sample of a RavenTestDriver can be found at the [bottom](../start/test-driver##complete-example) of the page).

### Properties and Methods
| Signature | Description |
| ----------| ----- |
| **protected virtual string DatabaseDumpFilePath => null;** | Allows you to override the path to the database dump file that will be loaded when calling ImportDatabase. |
| **protected virtual Stream DatabaseDumpFileStream => null;** |  Allows you to override the stream containing the database dump that will be loaded when calling ImportDatabase.  |
| **public IDocumentStore GetDocumentStore([CallerMemberName] string database = null, TimeSpan? waitForIndexingTimeout = null)** | Gets you an IDocumentStore instance for the requested database. |
| **protected virtual void PreInitialize(IDocumentStore documentStore)** |Allows you to pre-initialize the IDocumentStore. |
| **protected virtual void SetupDatabase(IDocumentStore documentStore)** | Allows you to initialize the database. |
| **protected event EventHandler DriverDisposed;** |An event that is raised when the test driver has been disposed of. |
| **public static void ConfigureServer(TestServerOptions options)** |Allows you to configure your server before running it|
| **public void WaitForIndexing(IDocumentStore store, string database = null, TimeSpan? timeout = null)** | Allows you to wait for indexes to become non-stale. |
| **public void WaitForUserToContinueTheTest(IDocumentStore store)** | Allows you to break the test and launch the Studio to examine the state of the database. |
| **protected virtual void OpenBrowser(string url)** | Allows you to open the browser. |
| **public virtual void Dispose()** | Allows you to dispose of the server. |

{PANEL/}

{PANEL:PreInitialize}

Pre-Initializing the IDocumentStore allows you to mutate the conventions used by the document store.

### Example

{CODE test_driver_PreInitialize@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL:UnitTest}
We'll be using [xunit](https://www.nuget.org/packages/xunit/) for my test framework in the below example.
Note that the test itself is meant to show different capabilities of the test driver and is not meant to be the most efficient.
The example below depends on the `TestDocumentByName` index and `TestDocument` class that can be seen in the [full example](../start/test-driver#complete-example)

### Example

{CODE test_driver_MyFirstTest@Start\RavenDBTestDriver.cs /}

In the test we get an IDocumentStore to our test database. Deploy an index and insert two documents into it. 
We then wait for the indexing to complete and launch the Studio so we can verify that the documents and index are deployed (we can remove this line once the test is working).
At the end of the test we query for TestDocument where their name contains the world 'hello' and assert that we have only one such document.

{PANEL/}

{PANEL: ConfigureServer}
The `ConfigureServer` method allows you to be more in control on your server. 
You can use it with `ServerTestOptions` to change the path to the Raven server binaries or to specify where your RavenDB data is stored, security, etc.

{INFO:ServerTestOptions}

`ServerTestOptions` inherits from [ServerOptions](../server/Embedded#getting-started). In that way you can be more in control of how the embedded server is going to run
with just a minor change. Here you can change your ServerDirectory.

{INFO /}

### Example

{CODE test_driver_ConfigureServer@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL:Complete Example}

{CODE test_full_example@Start\RavenDBTestDriverFull.cs /}

{PANEL/}


{PANEL:Continuous Integration Servers}

Best practice is to use a CI/CD server to help automate the testing and deployment of your new code. 
Popular CI/CD products are [AppVeyor](https://www.appveyor.com/) or [Visual Studio Team Services (aka. VSTS)](https://visualstudio.microsoft.com/team-services/).

{PANEL/}

## Related Articles

- [Running an Embedded Instance](../server/Embedded)

### Troubleshooting

- [Sending Support Ticket](../server/troubleshooting/sending-support-ticket)
