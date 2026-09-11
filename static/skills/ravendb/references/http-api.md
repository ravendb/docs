# Raw HTTP API

Talking to RavenDB with plain HTTP — for apps or scripts that don't use a client library. Base: `http://<host>:<port>/databases/<db>`.

Prefer the official client for application code (your language's `client.md`: `nodejs/`, `dotnet/`, `python/`); raw HTTP fits load scripts, proxies, and pass-through endpoints.

## Documents

| Call | Shape |
|---|---|
| `GET /docs?id=clients/1-A` | `{ "Results": [ { ...doc, "@metadata": {...} } ] }`; 404 when missing |
| `GET /docs?id=a&id=b` | Multiple ids in one round-trip |
| `PUT /docs?id=clients/1-A` | Body = the document JSON; creates or overwrites. Include `"@metadata": {"@collection": "Clients"}` — without it the write still returns 201 but the document lands in no named collection, so `from Clients` and `/collections/docs?name=Clients` never see it |
| `DELETE /docs?id=clients/1-A` | Deletes |

## Queries (RQL)

```
POST /queries
{ "Query": "from Clients where region = $r limit 25",
  "QueryParameters": { "r": "EU" },
  "Start": 0, "PageSize": 25 }

Send the body from a file (`curl --data-binary @body.json`) rather than inlining it in a shell argument. RQL string literals need quotes inside a JSON string inside a shell quote, and hand-escaping that nest is where `\x27` appears and the server answers `Invalid escape char, numeric value is 120`. Parameters keep literals out of the query text entirely.
```

Response: `{ "Results": [...], "TotalResults": n, "IsStale": bool, "IndexName": "..." }`. Always pass values via `QueryParameters`, never string-interpolate. RQL syntax: `rql-cheatsheet.md`.

## Batch writes

```
POST /bulk_docs
{ "Commands": [
    { "Type": "PUT", "Id": "clients/1-A", "ChangeVector": null, "Document": { ..., "@metadata": { "@collection": "Clients" } } },
    { "Type": "DELETE", "Id": "clients/2-A", "ChangeVector": null } ] }
```

One transaction. `ChangeVector: null` = unconditional write; pass a document's current change vector to fail on concurrent modification. Suits seeding in batches (a few hundred commands per call); for very large loads the client library's bulk insert streams instead.

## Database & health

| Call | Purpose |
|---|---|
| `GET /databases/<db>/stats` | Exists + document/index counts (non-2xx → database missing) |
| `GET /databases/<db>/indexes` | Static + auto index definitions. A single index is `?name=<n>`, a **query string, not a path segment**: `/indexes/<n>` returns `RouteNotFoundException`. The response carries each field's `Indexing` mode, which is what tells you whether `=` or `search()` will match |
| `DELETE /databases/<db>/indexes?name=<n>` | Delete an index, including an `Auto/…` one. The path is **not** symmetric with the `PUT` below: there is no `admin/indexes` DELETE handler, and asking for one returns `RouteNotFoundException` |
| `PUT /databases/<db>/admin/indexes` | Define a static index: `{"Indexes":[{"Name":"<n>","Maps":["from d in docs.<Collection> select new { … }"],"Fields":{"<field>":{"Indexing":"Search","Storage":"Yes"}}}]}`. The non-admin `/indexes` path rejects a C# map with `UnauthorizedAccessException`, so use `admin`. A field the map computes needs `"Storage":"Yes"` to be projectable at all — without it the projection returns `null` — and it comes back as a **string** (`"113.598"`), so cast before arithmetic |
| `PUT /admin/databases` (server root) | Create database: `{ "DatabaseName": "<db>" }`; an existing name returns 409 `ConcurrencyException` |
| `GET /setup/alive` (server root) | Server responds (204); the readiness probe — see `install.md` |

## Notes

- Unsecured servers take no auth headers; secured servers require the client certificate — plain `http.request` won't do.
- Responses use PascalCase envelope keys (`Results`, `TotalResults`); your document fields keep their stored casing.
