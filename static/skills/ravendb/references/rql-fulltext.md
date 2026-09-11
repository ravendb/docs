# RQL — full-text search

Tokenized text search over analyzed fields. Case-insensitive with the default analyzer. Dynamic queries auto-create a full-text auto-index; for heavy or custom-analyzer use, define a static index and query it with `from index '…'`. Version-correct details: ravendb.net/docs.

## search(field, terms [, and|or])
```
from Companies where search(Notes, 'University Sales Japanese')      # terms default to OR
from Companies where search(Notes, 'College German', and)           # require ALL terms
```
- Third arg sets the operator **between the terms** in this one call (`or` default, or `and`).
- The operand is an analyzed field; `search` tokenizes both the field value and the query string. This is different from `=`/`startsWith`, which match the raw (non-analyzed) value.
- The mismatch is silent in both directions. `search()` against a static-index field that was not stored with `Indexing: Search` returns zero rows, and `=` against one that was matches single tokens instead of the whole string. Neither raises. `GET /databases/{db}/indexes?name=<index>` reports the `Indexing` mode per field.

### Combining multiple search() calls
Join separate `search()` calls with an explicit `or`/`and` (raw RQL requires the operator; only the client API defaults to OR):
```
from Companies where search(Address.Country,'France') or search(Name,'Markets')
from Employees where search(Notes,'French') and (exists(Title) and not search(Title,'Manager'))
```

### Exact phrase (quotes inside the term string)
```
where search(Notes,'"raven black"')   # the two words adjacent, in this order
```
Double-quote the terms to match them as a phrase instead of independently. Word order is part of the match — with the same data, `'"raven black"'` and `'raven black'` both hit 52 documents while `'"black raven"'` hits 0. Unquoted terms default to OR, so a bare two-word string is *not* a phrase search.

### Wildcards (inside the term string)
```
where search(Notes,'art*')     # prefix match
where search(Notes,'*logy')    # postfix match — triggers a FULL INDEX SCAN (avoid on hot paths)
where search(Notes,'*mark*')   # contains — also scan-heavy
```

## Relevance scoring
```
order by score()                              # sort best matches first (order by only — score() is NOT valid in select)
```
The score value itself lives in each result's `@metadata` under `@index-score` (project it with `select { Meta: getMetadata(x) }`).

### boost(predicate, factor)
Multiply the relevance contribution of a clause; results auto-order by score.
```
from Companies
where boost(startsWith(Name,'O'), 10)
   or boost(startsWith(Name,'P'), 50)
   or boost(endsWith(Name,'OP'), 90)
```

## Approximate matching — Lucene-backed index only
```
where fuzzy(Name = 'Bob', 0.7)                      # edit-distance match, factor 0.0..1.0 (closer to 1 = stricter)
where proximity(search(Bio,'raven database'), 3)    # the searched terms within 3 words of each other
```

## exact(predicate) — bypass the analyzer
Force case-sensitive, non-analyzed matching:
```
from Employees where exact(FirstName == 'Robert')
from Orders    where exact(Lines.ProductName == 'Teatime Chocolate Biscuits')
```

## lucene(field, 'query') — raw Lucene (Lucene engine only)
```
where lucene(Name, 'Jo* AND -Johnson')
```

## Notes & gotchas
- **Engine matters, and it is decided per index.** `fuzzy()`, `proximity()` and `lucene()` run only on a
  Lucene-backed index. On Corax (the default for new databases since 6.0) each is rejected outright with
  `InvalidQueryException: Method 'Fuzzy' is not supported.` — a hard failure, not degraded behaviour. One database can
  hold indexes of both engines, so check the index that will serve the query: `GET /databases/<db>/indexes/stats`
  reports `SearchEngineType` per index. Analyzer and wildcard behaviour also differs between the two.
- Postfix/contains wildcards force a full scan; prefer prefix (`term*`) or a purpose-built static index.
- `search` ≠ `startsWith`/`=`: use `search` for analyzed text relevance, the others for exact/prefix on raw values.
- For "did you mean" and similar-document retrieval see `rql-advanced.md` (suggestions, more-like-this).
