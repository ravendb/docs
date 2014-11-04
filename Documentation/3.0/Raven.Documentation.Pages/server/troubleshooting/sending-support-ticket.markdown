# Troubleshooting : Sending support ticket

When sending a support ticket, it is good to include as much information about the issue as you can. The following article describes what can be send along with the issue description to let us better understand your problem.

1. Server and client version
2. Web Traffic logs. We recommend using a [FiddlerCap](http://www.telerik.com/fiddler/fiddlercap) recorder which will create a SAZ file that we can analyze further:
    - [Download](http://www.telerik.com/fiddler/fiddlercap) FiddlerCap
    - Close all instances of Internet Explorer (or other browsers).
    - Run the FiddlerCapSetup.exe file.
    - FiddlerCap will start automatically when the installer completes.
    - Inside FiddlerCap press Clear Cookies button and then the Clear Cache button.
    - Inside FiddlerCap, click the **1. Start Capture** button.
    - Record web traffic using your application.
    - To add a screenshot to your capture, press the +Screenshot button inside FiddlerCap.
    - Inside FiddlerCap, click the **2. Stop Capture** button.
    - Click the **3. Save Capture** button. Save the .SAZ file to your desktop.
3. Debug Info Package. RavenDB supports creating a info package (statistics, queries, requests, hardware information, etc.) that can be database scoped or server scoped. [Database scoped package](../../studio/overview/status/gather-debug-info) can be created in: `Studio -> Status -> Gather Debug Info`, for [server scoped package](../../studio/management/gather-debug-info) go to `Studio -> Manage Your Server -> Gather Debug Info`.
4. [Server logs](../../server/troubleshooting/enabling-logging)
5. [Statistics](../../server/administration/statistics)
6. Unit test

## Writing unit tests

The NuGet package has been created for easier RavenDB test creation and can be downloaded [here](http://www.nuget.org/packages/RavenDB.Tests.Helpers/).

The package contains base class (`RavenTestBase`) with various methods useful for test creation:

- `NewDocumentStore` - method that creates a new EmbeddableDocumentStore.
- `NewRemoteDocumentStore` - method that creates a new DocumentStore and starts the Server.
- `WaitForIndexing` - waits for the indexing to complete for all indexes.
- ...and many more

{CODE support_1@Server/Troubleshooting/Support.cs/}
