# RavenDB .NET client — core usage

Verified against `Raven.Client` v7.2 source (NuGet package `RavenDB.Client`). Covers store/session lifecycle, CRUD, querying, includes, bulk insert.

## Setup

```csharp
using Raven.Client.Documents;

var store = new DocumentStore
{
    Urls = new[] { "http://127.0.0.1:8080" },
    Database = "ConsultingDeals"
};
store.Initialize();
// on shutdown:
store.Dispose();
```

- One `DocumentStore` per app (it is thread-safe and expensive). `Initialize()` is mandatory; conventions are frozen at that point.
- Conventions live on `store.Conventions` (set before `Initialize()`): `MaxNumberOfRequestsPerSession`, `FindCollectionName`, `IdentityPartsSeparator`, etc. Collection name derives from the class name, pluralized (`Client` → `Clients`).

In an ASP.NET Core app the store is a singleton and the session is scoped to the request:

```csharp
var store = new DocumentStore { Urls = new[] { cfg["Raven:Url"] }, Database = cfg["Raven:Database"] };
store.Initialize();
builder.Services.AddSingleton<IDocumentStore>(store);
builder.Services.AddScoped<IAsyncDocumentSession>(sp => sp.GetRequiredService<IDocumentStore>().OpenAsyncSession());
```

The scoped session is one unit of work per request, disposed with the request. Inject `IDocumentStore` instead when a handler needs several sessions or a bulk insert. Index deployment and database creation belong in the startup path before `app.Run()` (`../structure.md`).

## Secured server (certificate)

```csharp
using System.Security.Cryptography.X509Certificates;   // X509CertificateLoader lives here
using Raven.Client.Documents;

var store = new DocumentStore
{
    Urls = new[] { "https://a.myapp.ravendb.cloud" },
    Database = "ConsultingDeals",
    Certificate = X509CertificateLoader.LoadPkcs12FromFile(
        Environment.GetEnvironmentVariable("RAVENDB_CERT_PATH"),
        Environment.GetEnvironmentVariable("RAVENDB_CERT_PASSWORD"))
};
store.Initialize();
```

- `Certificate` is an `X509Certificate2` and must be set before `Initialize()`. On .NET 9+ use `X509CertificateLoader`; the `new X509Certificate2(path, password)` constructor is obsolete there but still the form on older targets. The loader is **pkcs12-only** — convert a `.pem` download first (`openssl pkcs12 -export -in client.pem -out client.pfx`) — and the file must be readable by the process uid.
- **The client certificate is only half of it: the *server's* certificate must also be trusted.** Against RavenDB Cloud this is invisible because those chain to a public root, but a self-signed or private-CA server fails at `Initialize()` with `Failed to retrieve database topology from all known nodes` → `The remote certificate was rejected by the provided RemoteCertificateValidationCallback`. Trust the CA out of band — `SSL_CERT_FILE=/path/ca.crt` needs no code — or register `Raven.Client.Http.RequestExecutor.RemoteCertificateValidationCallback`, a public static event.
- URL must be `https://`: a certificate against an `http://` url fails client-side before any request is sent — `InvalidOperationException: The url <url> is using HTTP, but a certificate is specified, which require us to use HTTPS` at `Initialize()`.
- On Linux/containers load from a file or a mounted secret; `X509KeyStorageFlags.MachineKeySet` matters only on Windows service accounts. Never commit the `.pfx` or its password.

## Session lifecycle

```csharp
using (var session = store.OpenSession())
{
    var client = new Client { Name = "Acme" };
    session.Store(client);                 // id auto: "clients/1-A"
    session.Store(client, "clients/42");   // explicit id
    session.Store(client, "clients/");     // "/" suffix: server assigns the id tail
    session.Store(client, "clients|");     // "|" suffix: cluster-wide identity number
    session.SaveChanges();                 // one batch request; nothing hits the server before this
}
```

Async mirror: `store.OpenAsyncSession()`, `session.StoreAsync(entity[, id])`, `session.SaveChangesAsync()`, `session.LoadAsync<T>(id)`, and `ToListAsync()`/`CountAsync()`/`FirstOrDefaultAsync()` from `Raven.Client.Documents` on queries. Prefer async in app code.

- A session is a unit of work with an identity map. It holds every document it received in full (`Load`, an include, a query without a projection), keyed by id, with a copy of the JSON as it arrived. `SaveChanges()` diffs the tracked documents against those copies and sends the differences, plus anything stored or deleted, in one batch; nothing else reaches the server. Not thread-safe.
- Not in the map, so not tracked: projections (`Select`), streams, aggregations, facets. Mutating one and calling `SaveChanges()` writes nothing and raises nothing; `GetDocumentId` and `GetMetadataFor` cannot see it either. To change documents without loading them in full, patch (`operations.md`).
- Limit: 30 requests per session (`store.Conventions.MaxNumberOfRequestsPerSession`); exceeding throws. Open a new session instead of raising it.
- `session.Delete(entity)` or `session.Delete("clients/1-A")`, then `SaveChanges()`.
- `Load<T>(id)` returns `null` for a missing id; loading the same id twice in one session hits the session cache (no second request).
- Load many: `session.Load<Client>(new[] { "clients/1-A", "clients/2-A" })` returns `Dictionary<string, Client>` (missing ids map to null).
- A `public string Id` property on the entity is populated by the client after `Store()`; it is not stored inside the document body.
- To read the id of a document you **queried** rather than stored, use `session.Advanced.GetDocumentId(entity)`. It works on any tracked entity, including one whose class has no `Id` property. Projections are not tracked, so project `id()` in the query instead.

## Metadata

```csharp
var metadata = session.Advanced.GetMetadataFor(client);   // tracked entity only
var collection = metadata[Constants.Documents.Metadata.Collection];   // "@collection"
metadata["Custom-Key"] = "v";   // mutate + SaveChanges() persists
```

## Querying

```csharp
var page = session.Query<Client>()
    .Where(x => x.Status == "active" && x.SignedAt >= cutoff)
    .Search(x => x.Notes, "renewal risk")     // full-text; terms OR-ed within the field
    .OrderBy(x => x.Name)
    .Skip(20).Take(10)
    .ToList();

int total = session.Query<Client>().Count(x => x.Status == "active");
```

- `session.Query<T>()` is a dynamic query (server creates/reuses an auto index); `session.Query<T, TIndexCreator>()` or `session.Query<T>("Deals/ByClient")` targets a static index — see `indexes.md`.
- That bare string overload is an **index** name, not a collection name: `session.Query<Deal>("Deals")` throws `IndexDoesNotExistException: Could not find index Deals`. To name a collection, pass the argument by name: `session.Query<Deal>(collectionName: "Deals")`.
- **Declare a small class for the entity; do not query `dynamic` or `object`.** `Query<dynamic>()` breaks LINQ two ways: a lambda over it fails to compile with `CS1963: An expression tree may not contain a dynamic operation`, and the string-named operators return a plain `IOrderedQueryable<dynamic>`, which has lost `Statistics` and every other Raven extension. A three-property class costs less than working around either.
- `string.Contains` does not translate: `.Where(x => x.Name.Contains("acme"))` throws `NotSupportedException: Could not understand expression`. Use `.Search(x => x.Name, "acme")` for an analyzed field, or `StartsWith`/`EndsWith`.
- Searching **several fields at once** chains `Search` calls and passes the combining option on the later ones: `.Search(x => x.Name, q).Search(x => x.Email, q, options: SearchOptions.Or)`. The enum is `Raven.Client.Documents.SearchOptions` (`Or`/`And`/`Not`/`Guess`, default `Guess`) — not under `.Queries`. `OrElse()` exists only on `Advanced.DocumentQuery`, not on `IRavenQueryable<T>`, so it will not compile here.
- Projections: `.Select(x => new { x.Name, x.Status })` or `.ProjectInto<ClientSummary>()` (matches fields by name; server-side, only projected fields travel).
- Sorting: LINQ `.OrderBy(x => x.Amount)` carries the property type and sorts numerically. The **string-named** form on `Advanced.DocumentQuery` does not: `.OrderByDescending("Amount")` sorts as a string and puts `99952` above `243889170`. Pass the ordering type, from `Raven.Client.Documents.Session`: `.OrderByDescending("Amount", OrderingType.Double)`, and `Long` / `AlphaNumeric` likewise. This returns wrong rows with no error.
- Query stats: `.Statistics(out QueryStatistics stats)` before the conditions (`stats.TotalResults` is a `long`, `stats.IsStale`). It is defined on `IRavenQueryable<T>`, so keep the query in a `var` and keep `T` a real class. Anything that drops it back to a plain `IQueryable<T>` / `IOrderedQueryable<T>` loses the method, and the compiler only says `CS1061: does not contain a definition for 'Statistics'`.
- Raw RQL: `session.Advanced.RawQuery<T>("from Clients where Status = $s").AddParameter("s", "active")` (`AsyncRawQuery<T>` on an async session) — the escape hatch when the query is RQL text, e.g. copied from Studio. When the RQL projects, `T` is a class with the projected properties or `Dictionary<string, object>`; each row also carries an `@metadata` object, so `Dictionary<string, string>` fails to deserialize.
- String-based alternative: `session.Advanced.DocumentQuery<T>(indexName, collectionName, isMapReduce)` with `WhereEquals`/`AndAlso`/`OpenSubclause` etc. — use it when field names are only known at runtime; otherwise stay in LINQ.
- Large result sets: `session.Advanced.Stream(query)` (results not tracked).

## Include — avoid N+1

```csharp
var client = session.Include<Client>(x => x.DealIds).Load("clients/1-A");
var deal = session.Load<Deal>(client.DealIds[0]);   // already in session, no extra request
```

Query-side: `session.Query<Deal>().Include(x => x.ClientId)`.

Include paths are resolved on what the query returns. On a projected query the include is silently dropped unless the field it walks is in the projection: `select DealName, AccountId include AccountId` includes the accounts, `select DealName include AccountId` includes nothing and every later load is a request.

## Bulk insert

```csharp
using (var bulk = store.BulkInsert())
{
    foreach (var row in rows)
        bulk.Store(row);          // or bulk.Store(entity, id); async: await bulk.StoreAsync(...)
}
```

- Streams to the server in batches; orders of magnitude faster than session `Store` loops for seeding. Disposing flushes the remaining batch.
- Ids ending in `|` are rejected in bulk insert. Metadata via `bulk.Store(entity, id, metadata)`.
- An entity whose `Id` is initialized to `""` throws `BulkInsertInvalidOperationException: Document id must have a non empty value` — bulk insert treats a non-null id as explicit. Leave the property `null` (`string?`) and let the client assign it.

## Staleness

Queries run on indexes that update asynchronously; a query right after `SaveChanges()` may return stale results. Details and wait patterns: `indexes.md`. Test-suite discipline: `../testing.md`; the driver and per-test database call forms: `testing.md`.

## Gotchas

- Nothing persists without `SaveChanges()` (or disposing the bulk insert).
- Field names in the database are the C# property names (PascalCase by default) — RQL/DocumentQuery strings must match exactly.
- The session tracks entities by reference: mutate a loaded entity and `SaveChanges()` persists it; no second `Store` call needed.
- `session.Advanced.NumberOfRequests` shows the current count against the 30-request cap.
- Dispose sessions (`using`); an undisposed session leaks tracked entities, not connections — but treat it as a bug.
