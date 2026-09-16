# `ravendb` — agent skill

RavenDB knowledge for coding agents: install a server, model documents, write client code (Node.js, .NET, Python), write RQL and indexes, use vector search and AI agents.

## Install

```bash
npx skills add ravendb/skills --skill ravendb
```

Or copy this directory into the agent's skills path — `~/.claude/skills/` or `.claude/skills/` (Claude Code), `~/.cursor/skills/` or `.agents/skills/` (Cursor).

## Shape

`SKILL.md` is a router, not a manual: the agent reads it, picks the ONE topic file its task needs, and opens only that. Nothing but the front-matter description is in context until a RavenDB task shows up.

```
SKILL.md                    router — language table, topic table, version/license gate
references/
  install.md                find a server first, docker, database, version + licensing
  install-native.md         download, extract, service, per-OS commands
  license.md                free key request form, activation call, verification
  windows.md                PowerShell probes, rvn windows-service, WSL2, firewall
  structure.md              store singleton, startup order, session per unit of work
  modeling.md               collections, ids, embed vs reference, metadata
  http-api.md               docs / queries / bulk_docs over raw HTTP
  ai-agents.md              agent configs + conversation endpoints (7.1+)
  testing.md                route choice, staleness flakes, what to test, CI
  diagnostics.md            wrong results, broken index, slow server, exceptions, logs
  samples.md                official sample apps and what each one showcases
  rql-*.md                  cheatsheet, reference, functions, fulltext, facets,
                            advanced (suggest/MLT/vector), spatial, timeseries
  nodejs/ dotnet/ python/   client.md · indexes.md · operations.md · testing.md ·
                            ai-agents.md
```

Server baseline is **6.2+**. Version- and license-gated features carry an inline `(7.0+)` / `(license)` mark in the file that documents them, and the router requires three checks before using one: server version (`GET /build/version`), the client version pinned in the project, and `GET /license/status`.

## Maintaining it

Content is verified against the real client repos and a tri-language sample app, never written from recall — the same rule the skill imposes on its readers. When a client API changes, fix the language file; when RQL changes, fix the `rql-*.md` file. Adding a topic means adding a row to the router table in `SKILL.md`, otherwise the agent never finds it.
