# Official sample code — what to read for what

Use these when you need to see a feature **wired into a real application** — project layout, startup, background tasks, how the pieces talk. For syntax, stay in this pack; a sample shows shape, not exact signatures, and each is pinned to whatever version it was written against. Everything listed is a public `github.com/ravendb/…` repo.

## Full applications, by the feature you care about

| Repo | Stack | Showcases |
|---|---|---|
| `samples-library` | .NET 10 + Aspire, Svelte | The clearest starting point. `include` for related documents in one request, document **refresh** used as a timeout mechanism, Azure Queue **ETL** driven off `@refresh`, and **vector search** over similar books |
| `samples-hr` | .NET 10 + Aspire, React | **AI agents** with tool calling and conversation management, **vector search** via `vector.search(embedding.text(...))`, **time series** used for rate limiting, attachments for signature images, counters for token accounting, expiration for session cleanup |
| `samples-fit` | .NET 10 + Aspire, React | The densest feature set: a parent **AI agent with four sub-agents** plus streaming, **GenAI tasks** producing structured output deduped via `@ai-hashes`, **time series with rollups** (raw/hourly/daily/monthly, query picks the tier), **subscriptions** fanning events to followers, RabbitMQ **Queue ETL**, **OLAP ETL** to Parquet, **Changes API** for a push-driven ticker |
| `samples-verity` | .NET 10 + Aspire, Svelte | Governance and document lifecycle: **GenAI tasks** over documents larger than the model's context window, **AI agents**, **remote attachments** in Azure/S3, **revisions** as an audit trail, **archival** and **expiration**, hub/sink replication, subscriptions driving a live CLI |
| `samples-brain-slop` | .NET 10 + Aspire, Next.js | Small, focused **AI agent** app — free-form text interpreted into actions, plus GenAI for chat titles |
| `samples-properties` | .NET 8 + ASP.NET Core, React, Telegram bot | The only sample with **spatial indexing** in an application, plus a **GenAI ETL** that turns an uploaded photo into a maintenance description, **AI agents** reached through a Telegram bot, **subscriptions** for push notifications, and time series for utility usage |
| `samples-crypto-app` | .NET + Node.js Azure Functions, TS frontend | **Time series** end to end, with the *same* ingestion implemented in both the .NET and Node.js clients — the best side-by-side of the two APIs. Live at crypto.samples.ravendb.net, with database exports in the repo |

`ravendb/docs` → `samples/` holds the maintained gallery page for each of these; check it before assuming a sample is current.

**A sample's pinned version is not evidence of what is current.** Client versions in these repos are bumped in batches, and the upgrade PR is often still open — `samples-properties`, `samples-fit` and `samples-hugin` all have a 7.2.5 upgrade in flight. Take API shape from a sample, take the version gate from `install.md`.

## Node.js / TypeScript

| Repo | What it gives you |
|---|---|
| `samples-nextjs`, `samples-sveltekit` | The smallest honest examples of the Node client in a framework — a single store module (`src/db/store.ts` / `src/lib/store.ts`) plus CRUD and query routes. Both point at `live-test.ravendb.net` by default |
| `template-cloudflare-worker` | `npm init cloudflare my-project https://github.com/ravendb/template-cloudflare-worker`. Read it before writing any Worker/edge code — the runtime has no Node `https` agent, so client-certificate auth uses a Cloudflare mTLS binding, not `authOptions` |

## The definitive API examples: client repos

When this pack does not cover a call, the client's own repository beats the docs for exactness — it is what actually ships.

| Where | Why |
|---|---|
| `ravendb-nodejs-client` → `README.md` | ~85 KB, effectively the Node client manual, with a worked example per area |
| `ravendb-nodejs-client` → `test/` | Grouped by area (`Documents/`, `ServerWide/`, `Issues/`). Search here for any method you are unsure about; the tests are executable proof of the signature |
| `ravendb-python-client` → `ravendb/tests/` | The **only** Python examples of the AI surface: `ai_agent_tests/`, `gen_ai_tests/`, `embeddings_generation_tests/`. Also `session_tests/`, `operations_tests/` |
| `ravendb/ravendb` → `test/` | The server's own test suite — the last word on behavior when a sample and the docs disagree |

There is **no Python sample application**. For a Python app, take structure from `samples-library` and API usage from the Python client's tests.

## Learning material and playgrounds

| Resource | Use it for |
|---|---|
| demo.ravendb.net (source: `ravendb/demo`) | Runnable per-feature demos against your own database, each with a code walkthrough. Multiple languages, Northwind data |
| live-test.ravendb.net | A free shared 7.2 server — no install, create a database and go. **Public and periodically wiped**: never put real or private data there |
| `ravendb/bootcamp` | Four self-directed units, .NET, Northwind. Written for RavenDB 4.0, so the concepts hold and the API details do not |
| `ravendb/book` | Source of *Inside RavenDB* — the reference for **why** the engine behaves as it does (storage, clustering, indexing internals). Also 4.0-era |
| `ravendb/awesome-ravendb` | Community integrations: DI, health checks, Serilog sinks, ASP.NET Identity |
| `samples-hugin` | RavenDB on **ARM / a Raspberry Pi**, serving full-text search over Stack Exchange dumps offline — the pointer for embedded or constrained deployments and for a large full-text dataset. Node client, pinned `^5.4.2` on `main` |
| `samples-jvm`, `samples-php-laravel` | Only if the task is Java or PHP — outside this pack's three languages |
