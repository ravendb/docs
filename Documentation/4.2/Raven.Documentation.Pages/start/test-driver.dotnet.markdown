# Getting Started: Writing your Unit Test using TestDriver
---

{NOTE: }

* In this section, we explain how to use [RavenDB.TestDriver](https://www.nuget.org/packages/RavenDB.TestDriver/) to write RavenDB unit tests.  

* TestDriver uses an [Embedded Server](../server/embedded) package with the same set of [prerequisites](../server/embedded#prerequisites) as embedded servers to run the tests.  

* In this page: 
 - [RavenTestDriver](../start/test-driver#raventestdriver)
 - [Pre-initializing the store](../start/test-driver#preinitialize)
 - [ConfigureServer](../start/test-driver#configureserver)
 - [Unit test](../start/test-driver#unittest)
 - [Complete example](../start/test-driver#complete-example)
 - [CI Servers](../start/test-driver#continuous-integration-servers)

{NOTE/}

{PANEL:RavenTestDriver}

First, we define a class that derives from Raven's TestDriver.
Let's start with reviewing the TestDriver's methods and properties and later we will get into implementation (a complete code sample of a RavenTestDriver can be found at the [bottom](../start/test-driver##complete-example) of the page).

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

We use [xunit](https://www.nuget.org/packages/xunit/) for the test framework in the below example.  

{NOTE: }
Note that the test itself is meant to show different capabilities of the test driver and is not meant to be the most efficient.  
{NOTE/}

The example below depends on the `TestDocumentByName` index and `TestDocument` class that can be seen in the [full example](../start/test-driver#complete-example)

### Example

In the test, we get an `IDocumentStore` object to our test database, deploy an index, and insert two documents into the document store.  

We then use `WaitForUserToContinueTheTest(store)` which launches the Studio so we can verify that the documents 
and index are deployed (we can remove this line after the test succeeds).  

At the end of the test, we use `session.Query` to query for "TestDocument" where the name contains the word 'hello', 
and we assert that we have only one such document.

{CODE test_driver_MyFirstTest@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL: ConfigureServer}

The `ConfigureServer` method allows you to be more in control of your server.  
You can use it with `TestServerOptions` to change the path to the Raven server binaries, specify data storage path, adjust .NET framework versions, etc.

* `ConfigureServer` can only be set once per test run.  
  It needs to be set before `DocumentStore` is called.  
  See an [example](../start/test-driver#complete-example) below.  

* If it is called twice, or within the `DocumentStore` scope, you will get the following error message:
  `System.InvalidOperationException : Cannot configure server after it was started. Please call 'ConfigureServer' method before any 'GetDocumentStore' is called.`  

{INFO:TestServerOptions}

Defining TestServerOptions allows you to be more in control of 
how the embedded server is going to run with just a minor [definition change](../start/test-driver#example-2).

* To see the complete list of `TestServerOptions`, which inherits from embedded servers, go to embedded [ServerOptions](../server/Embedded#getting-started).  
* It's important to be sure that the correct [.NET FrameworkVersion](../server/Embedded#net-frameworkversion) is set.

{INFO /}

#### Example

{CODE test_driver_ConfigureServer@Start\RavenDBTestDriver.cs /}

{PANEL/}

{PANEL:Complete Example}

This is a full unit test using [Xunit](https://www.nuget.org/packages/xunit/).

In the test, we get an `IDocumentStore` object to our test database, deploy an index, and insert two documents into the document store.  

We then use `WaitForUserToContinueTheTest(store)` which launches the Studio so we can verify that the documents 
and index are deployed (we can remove this line after the test succeeds).  

At the end of the test, we use `session.Query` to query for "TestDocument" where the name contains the word 'hello', 
and we assert that we have only one such document.

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
