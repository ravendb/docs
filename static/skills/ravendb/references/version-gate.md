# Version & license gate

A feature marked `(7.0+)`, `(client 7.2.2+)` or `(license)` needs three things to be true before you write code
against it. Each is one cheap check, and doing them up front costs far less than a runtime failure.

| Gate | Check |
|---|---|
| Server version | `GET /build/version` → `{"BuildVersion":…,"ProductVersion":"<major.minor>","FullVersion":"<full>"}`. Read it once and remember it. |
| Client library version | The project's manifest, not the docs: `package.json` (`ravendb`), `.csproj`/`Directory.Packages.props` (`RavenDB.Client`), `requirements.txt`/`pyproject.toml` (`ravendb`). A new server with an old client still fails — `store.ai` needs client 7.2+, vector search 7.0+. To settle whether a specific type exists without compiling first, look in the installed package: `strings ~/.nuget/packages/ravendb.client/<v>/lib/<tfm>/Raven.Client.dll \| grep <TypeName>`, `grep -r <name> node_modules/ravendb/dist/`, or `grep -r` the installed Python package. |
| License | `GET /license/status` → `Type` plus a boolean per feature (`HasAiAgent`, `HasEmbeddingsGeneration`, `HasGenAi`, `HasConcurrentDataSubscriptions`, `HasTimeSeriesRollupsAndRetention`, …). No client wrapper — hit the endpoint directly. |

The two version marks fail differently: a server gap is a 404 or a missing endpoint, a client gap is a name that does
not exist or an obsolete-API warning. An API can also move on the client alone while every supported server handles it.

If any gate fails, say which one and what it would take to pass it, then ask. Do not silently substitute a different
approach, and do not write code that needs a feature the deployment cannot run. `install.md` has the wording for both
cases, plus the free Developer license flow.

Feature support can also depend on the search engine backing the index that serves a query, not on the server version:
one database can hold both Corax and Lucene indexes. `GET /databases/<db>/indexes/stats` reports `SearchEngineType`
per index. See the engine note in `rql-fulltext.md`.
