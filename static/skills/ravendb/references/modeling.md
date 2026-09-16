# Document modeling

How to shape documents, ids, and relationships in RavenDB — the decisions that make queries cheap.

## Documents and collections

- A document is a JSON object; its `@metadata.@collection` names its collection. Collections are just labels — no schema, no migration step.
- Model around **aggregates**: one document = one unit of change and load. A deal with its line items is one document; a deal and its client are two.
- Prefer plural PascalCase collection names (`Clients`, `Contracts`); field casing is whatever you store — queries use the literal property names.

## Ids

| Form | Meaning |
|---|---|
| `clients/1-A` | Standard server-generated id (`collection/number-node`) |
| `clients/` suffix | Server assigns the tail on store |
| `clients|` suffix | Cluster-wide sequential identity |
| `clients/acme` | Semantic id — pick when the natural key is stable and known |

Ids are strings and globally unique across collections. Semantic ids make cross-references self-documenting and enable direct `load()` without a query.

## References vs embedding

- **Embed** what is owned and always read together (deal → its lines).
- **Reference by id** what lives independently or is shared (deal → `clientId: "clients/1-A"`).
- Follow references with `include`/`load` (one round-trip), never a query per reference. See `rql-cheatsheet.md` (include/load) and your language's `client.md`.
- Denormalize a *stable* display field into the referrer (e.g. `clientName` next to `clientId`) when it saves a lookup on a hot path; skip it for anything that changes often.

## Relationship patterns

- one-to-many, child-owned: array of embedded objects in the parent.
- one-to-many, independent children: each child stores `parentId`; query children by it (auto-index) rather than keeping an id array on the parent that grows unbounded.
- many-to-many: array of ids on the lighter side (`triggerSignals: ["signals/m-and-a"]`); index it with `where 'signals/m-and-a' in triggerSignals`.
- Aggregations over many documents (counts per status, sums per client) belong in a **map-reduce index** (your language's `indexes.md`), not in a document you update on every write.

## Metadata

`@metadata` carries `@id`, `@collection`, `@change-vector`, `@last-modified`; you may add custom keys. `@change-vector` is the concurrency token — send it back on writes to detect conflicting updates.

## Anti-patterns

- One giant document holding a whole collection (unbounded growth, write contention).
- SQL-style row-per-fact splitting that forces multi-document joins for every read.
- Storing derived rankings/scores that a query or map-reduce index can compute. The exception is a derived value that is also a **write-time invariant** — a seat counter checked against a room's capacity, stock against a reorder floor. Indexes are eventually consistent, so an index total cannot gate a write; keep that counter on the document and defend it with optimistic concurrency (`operations.md`). Read-optimization is the thing to avoid duplicating, not enforcement.
