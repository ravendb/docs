# Testing RavenDB code — .NET

Routes and the language-neutral discipline (staleness, what to test, CI): `../testing.md`.

## Route 1 — test driver

```csharp
// NuGet: RavenDB.TestDriver
public class DealTests : RavenTestDriver
{
    [Fact]
    public void Won_deals_are_queryable()
    {
        using var store = GetDocumentStore();      // database named after the test method
        new Deals_ByStatus().Execute(store);
        using (var s = store.OpenSession()) { s.Store(new Deal { Status = "won" }); s.SaveChanges(); }

        WaitForIndexing(store);

        using var session = store.OpenSession();
        Assert.Single(session.Query<Deal, Deals_ByStatus>().Where(d => d.Status == "won"));
    }
}
```

- Database per call to `GetDocumentStore()` — no cross-test leakage, no cleanup code.
- Override `PreConfigureDatabase` / `SetupDatabase` to seed or configure revisions once per database.
- `WaitForIndexing(store)` before any assertion that queries. `WaitForUserToContinueTheTest(store)` opens Studio against the live test database — the fastest way to see why an assertion failed.
- `ConfigureServer(new TestServerOptions { ... })` must be called before the first `GetDocumentStore()`; after that the server is up and it throws.
- The driver pulls a server via `RavenDB.Embedded` and starts it as a real process, not an in-memory fake.

### The bundled server pins an exact runtime patch, and a lower one on the machine will not do

`RavenDB.Embedded`'s `RavenDBServer/Raven.Server.runtimeconfig.json` names the `Microsoft.NETCore.App` *and* `Microsoft.AspNetCore.App` version it requires; framework roll-forward only moves up, so a machine one patch behind fails at first use with `Could not find a matching runtime for '<version>+'`. Read the required version out of that file, then either install a matching ASP.NET Core runtime — `dotnet-install.sh --runtime aspnetcore --version <v> --install-dir <dir>` needs no root — and point the driver at it, or use a CI image that already has it:

```csharp
RavenTestDriver.ConfigureServer(new TestServerOptions { DotNetPath = "<dir>/dotnet" });
```

`FrameworkVersion` on the same options object becomes `--fx-version`, which covers only `Microsoft.NETCore.App` — it cannot satisfy the ASP.NET Core half, so it is not the fix for a too-old runtime.

## Route 2 — one server, database per test

`CreateDatabaseOperation` / `DeleteDatabasesOperation` from `operations.md`, one uniquely named database per test, hard-deleted on teardown. Take this route when the machine cannot get a matching runtime, or when the suite must run against the same server the app uses.

## Route 3 — embedded server, driven directly

When the test needs to own server configuration or survive across many tests:

```csharp
EmbeddedServer.Instance.StartServer(new ServerOptions { DataDirectory = tempDir });
using var store = EmbeddedServer.Instance.GetDocumentStore("Tests");
```

Same trade-off as Route 1 — real server process, matching runtime required — but you control lifetime and settings. `EmbeddedServer.Instance.OpenStudioInBrowser()` for a look inside.
