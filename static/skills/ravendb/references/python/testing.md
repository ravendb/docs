# Testing RavenDB code — Python

Routes and the language-neutral discipline (staleness, what to test, CI): `../testing.md`.

## Route 1 — test driver

```python
# pip install ravendb-test-driver
from ravendb_test_driver import RavenTestDriver

class TestDeals(TestCase):
    def setUp(self):
        self.driver = RavenTestDriver()

    def tearDown(self):
        self.driver.close()

    def test_won_deals(self):
        with self.driver.get_document_store() as store:
            with store.open_session() as session:
                session.store({"Status": "won", "collection": "Deals"})
                session.save_changes()
            RavenTestDriver.wait_for_indexing(store)
```

- Database per call to `get_document_store()` — no cross-test leakage, no cleanup code.
- Override `setup_database` to seed or configure revisions once per database.
- `wait_for_indexing(store)` before any assertion that queries. It and `configure_server` are `@staticmethod`s on `RavenTestDriver`; calling them on the instance works but implies per-instance state that does not exist.
- `configure_server(options)` must be called before the first `get_document_store()`; after that the server is up and it throws.
- The driver pulls a server via `ravendb-embedded` and starts it as a real process, not an in-memory fake.

### The bundled server pins an exact runtime patch, and a lower one on the machine will not do

The embedded package's `RavenDBServer/Raven.Server.runtimeconfig.json` names the `Microsoft.NETCore.App` *and* `Microsoft.AspNetCore.App` version it requires; framework roll-forward only moves up, so a machine one patch behind fails at first use with `Could not find a matching runtime for '<version>+'`. Read the required version out of that file, install a matching ASP.NET Core runtime — `dotnet-install.sh --runtime aspnetcore --version <v> --install-dir <dir>` needs no root — and point the driver at it:

```python
from ravendb_embedded import ServerOptions
from ravendb_test_driver import RavenTestDriver

options = ServerOptions()
options.dot_net_path = "<dir>/dotnet"
RavenTestDriver.configure_server(options)
```

`framework_version` on the same options object becomes `--fx-version`, which covers only `Microsoft.NETCore.App` — it cannot satisfy the ASP.NET Core half, so it is not the fix for a too-old runtime.

When no matching runtime can be installed at all, the driver still gives database-per-test against a server you already run — no embedded boot, nothing needing .NET:

```python
RavenTestDriver.configure_external_server(url, certificate_pem_path=None, trust_store_path=None)
```

Equivalent env vars are `RAVENDB_TEST_SERVER_URL` / `_CERT` / `_CA`. Call it before the first `get_document_store()`.

## Route 2 — one server, database per test

`configure_external_server` (above) is the easiest form of this route: the driver still names and hard-deletes a database per test, it just skips the embedded boot. Hand-rolled instead — `CreateDatabaseOperation` / `DeleteDatabaseOperation` from `operations.md`, one uniquely named database per test, hard-deleted on teardown — when the suite must control the names or the database record.

Either way the store you get is already initialized, so store-level conventions cannot be changed on it; anything that must be set before `initialize()` belongs to a store you construct yourself.

## Route 3 — embedded server, driven directly

When the test needs to own server configuration or survive across many tests:

```python
from ravendb_embedded import EmbeddedServer, ServerOptions

options = ServerOptions()
options.data_directory = temp_dir
options.server_url = "http://127.0.0.1:38712"   # omit and the server picks a free port
server = EmbeddedServer()          # not enforced as a singleton: hold this instance, a second one has no server
server.start_server(options)
store = server.get_document_store("Tests")      # creates the database too
try:
    server.close()
except RuntimeError:
    pass                            # 7.2.5.post1: the server process is already down by now
```

Same trade-off as Route 1 — real server process, matching runtime required — but you control lifetime and settings. `open_studio_in_browser()` for a look inside.

- `get_document_store(name)` **creates the database** unless you pass `DatabaseOptions` with `skip_creating_database = True` via `get_document_store_from_options`, so a following `CreateDatabaseOperation` is redundant.
- `close()` in `ravendb-embedded` 7.2.5.post1 kills the server process first and then raises `RuntimeError: dictionary changed size during iteration` while closing the stores it tracks — the shutdown succeeded, the bookkeeping did not. Catch it rather than reading it as a failed teardown.
- There is no public accessor for the server's pid; asserting the process is gone means checking the port or the process table.
