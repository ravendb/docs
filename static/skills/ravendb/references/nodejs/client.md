# RavenDB Node.js client — core usage

Verified against `ravendb` npm package v7.2.0 source. Covers store/session lifecycle, CRUD, querying, includes, bulk insert.

## Setup

```js
const { DocumentStore } = require("ravendb");

const store = new DocumentStore("http://127.0.0.1:8080", "ConsultingDeals");
store.conventions.findCollectionNameForObjectLiteral = e => e["collection"];
store.initialize();
// on shutdown:
store.dispose();
```

- One `DocumentStore` per app. `initialize()` is mandatory before any use; conventions are frozen at that point.
- `findCollectionNameForObjectLiteral` MUST be set before `initialize()` when storing plain object literals; otherwise all literals land in the `@empty` collection. Class instances don't need it (collection derives from class name).

## Secured server (certificate)

A secured server needs an X.509 client certificate on the store — a third constructor argument, not a convention:

```js
const fs = require("fs");

const store = new DocumentStore("https://a.myapp.ravendb.cloud", "ConsultingDeals", {
    certificate: fs.readFileSync("./client.pfx"),   // pfx: Buffer; pem: the file's text
    type: "pfx",                                    // "pem" | "pfx"
    password: process.env.RAVENDB_CERT_PASSWORD,    // omit if unprotected
    ca: fs.readFileSync("./ca.crt")                 // only for a self-signed CA
});
store.initialize();
```

- `type: "pem"` expects one PEM holding **both** the certificate and its private key (that is what RavenDB's `.pem` download is); `type: "pfx"` expects the raw bytes as a `Buffer`.
- Also settable as `store.authOptions = {...}` before `initialize()`.
- URL must be `https://` when a certificate is used; the mismatch is rejected by the client at initialization, not by the server. Never commit the certificate or its password — read both from the environment.

## Session lifecycle

```js
const session = store.openSession();

await session.store({ collection: "Clients", name: "Acme" });   // id auto: "clients/1-A" — the prefix is lower-cased
await session.store(doc, "clients/42");                          // explicit id
await session.store(doc, "clients/");                            // "/" suffix: server assigns the id tail
await session.store(doc, "clients|");                            // "|" suffix: cluster-wide identity number

await session.saveChanges();   // one batch request; nothing hits the server before this
session.dispose();
```

- A session is a unit of work with an identity map. It holds every document it received in full (`load`, an include, a query without `selectFields`), keyed by id, with a copy of the JSON as it arrived. `saveChanges()` diffs the tracked documents against those copies and sends the differences, plus anything stored or deleted, in one batch; nothing else reaches the server.
- Not in the map, so not tracked: projections (`selectFields`), streams, aggregations, facets. Mutating one and calling `saveChanges()` writes nothing and raises nothing (`session.advanced.hasChanges()` stays false); `getDocumentId` and `getMetadataFor` cannot see it either. To change documents without loading them in full, patch (`operations.md`).
- Limit: 30 requests per session (`store.conventions.maxNumberOfRequestsPerSession`); exceeding throws. Open a new session instead of raising it.
- `session.delete(entity)` or `session.delete("clients/1-A")`, then `saveChanges()`.
- Loading a missing id returns `null`. Loading the same id twice in one session hits the cache (no second request).
- Load many: `await session.load(["clients/1-A", "clients/2-A"])` returns `{ id: entity|null }` object.

## Metadata / collections

```js
await session.store({ name: "x", "@metadata": { "@expires": iso } }); // metadata on store
const md = session.advanced.getMetadataFor(entity);  // tracked entity only
md["@collection"];               // collection name
md["customKey"] = "v";           // mutate + saveChanges() persists
```

## Querying

```js
const results = await session.query({ collection: "Clients" })
    .whereEquals("status", "active")
    .whereIn("region", ["EU", "US"])
    .whereBetween("signedAt", new Date(2025, 0, 1), new Date())
    .orderBy("name")        // orderByDescending(...) for descending
    .skip(20).take(10)
    .all();

const n = await session.query({ collection: "Clients" }).count();
const one = await query.first();          // also firstOrNull / single / singleOrNull
```

- `query({ indexName: "Clients/ByName" })` targets a static index; `query({ collection })` is a dynamic query (auto index).
- **Sorting on a number or a date needs the ordering type as a second argument.** `orderBy`/`orderByDescending` name the field as a string, and a string field name sorts as a **string** unless you say otherwise: `.orderByDescending("amount")` puts `99952` above `243889170`. Pass `"Double"`, `"Long"` or `"AlphaNumeric"` whenever the field is not text: `.orderByDescending("amount", "Double")`. This returns wrong rows with no error.
- **That second argument is a case-sensitive exact match and an unrecognised value is discarded in silence.** `"double"`, `"DOUBLE"` and `"banana"` all behave exactly like passing nothing, so the sort falls back to lexical and you get the same wrong rows you were trying to avoid. Only `"Long"`, `"Double"`, `"AlphaNumeric"` and `"String"` count. Read a number back off the top row and check it is the largest one, because the call site looks correct either way.
- Query stats: `.statistics(s => stats = s)` anywhere in the chain, then `stats.totalResults` (how many the query matched overall, regardless of `skip`/`take`) and `stats.isStale`. There is no other way to get the total without running a second query.
- Default operator between `where*` clauses is AND; `.usingDefaultOperator("OR")` must come before any condition. Explicit: `.andAlso()` / `.orElse()` / `.not()` / `.openSubclause()...closeSubclause()`.
- Full-text: `.search("description", "term1 term2")` (terms OR-ed within the field).
- Projection: `.selectFields("name")` returns values; `.selectFields(["name", "age"])` returns objects with just those fields.
- `.distinct()`, `.whereStartsWith`, `.whereGreaterThan(OrEqual)`, `.whereLessThan(OrEqual)`, `.whereExists`, `.containsAny`/`.containsAll` all exist.
- Field names in queries are the literal JSON property names — the client does no case conversion by default, so use exactly what you stored.
- Raw RQL: `session.advanced.rawQuery("from Clients where status = $s").addParameter("s", "active")` — the escape hatch when the query is RQL text, e.g. copied from Studio.
- Lazy: `.lazily()` / `.countLazily()` defer; batch executes on first `.getValue()`.
- Large result sets: `await session.advanced.stream(query)` returns a `DocumentResultStream` — a Node readable you consume with `for await`; results are not tracked. `streamInto(query, writable)` pipes straight into a `Writable` instead.

## include() — avoid N+1

```js
const client = await session.include("dealIds").load("clients/1-A");
const deal = await session.load(client.dealIds[0]);  // already cached, no extra request
```

Query-side: `session.query({ collection: "Deals" }).include("clientId")`.

Include paths are resolved on what the query returns. On a projected query the include is silently dropped unless the field it walks is in the projection: `select DealName, AccountId include AccountId` includes the accounts, `select DealName include AccountId` includes nothing and every later load is a request.

## Bulk insert

```js
const bulk = store.bulkInsert();
for (const row of rows) {
    await bulk.store({ collection: "Deals", ...row });      // needs findCollectionNameForObjectLiteral (see Setup) or this lands in @empty
}
await bulk.finish();   // mandatory; flushes remaining batch
```

- Streams to the server in batches; orders of magnitude faster than session `store` loops for seeding.
- Ids ending in `|` are rejected in bulk insert. `bulk.store(entity, metadataDictionary)` sets metadata.
- `findCollectionNameForObjectLiteral` applies here too.

## Staleness

Queries run on indexes that update asynchronously; a query right after `saveChanges()` may return stale results. Details and wait patterns: `indexes.md`. Test-suite discipline: `../testing.md`; the driver and per-test database call forms: `testing.md`.

## Gotchas

- Forgetting `store.initialize()` → errors on first operation; forgetting `findCollectionNameForObjectLiteral` → documents in `@empty`.
- Nothing persists without `saveChanges()` (or `bulk.finish()`).
- `id` property on the entity is populated by the client after `store()`; it is not stored inside the document body. It is also populated on entities returned by a query, so `results[0].id` is the document id; `session.advanced.getDocumentId(entity)` does the same. Projections are not tracked and carry no `id`, so project `id()` in the query instead.
- 30-requests-per-session cap; `session.advanced.numberOfRequests` shows the current count.
- Dates round-trip as `Date` for class entities; when comparing in queries pass `Date` objects, not strings, unless you stored ISO strings.
