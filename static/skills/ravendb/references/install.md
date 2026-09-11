# Getting a RavenDB server running

## Find a server before installing one

**Never install RavenDB unasked.** A machine that already runs one — a container, a service, a colleague's shared server — must be reused, and the user is the only authority on which server their app should talk to. Probe, then ask. On Windows, run the equivalents in `windows.md`; the commands below are POSIX.

```bash
for u in http://localhost:8080 http://localhost:80; do
  printf '%s ' "$u"; curl -s -o /dev/null -w '%{http_code}\n' --max-time 3 "$u/setup/alive"   # 204 → RavenDB here
done
docker ps --format '{{.Names}}\t{{.Image}}\t{{.Ports}}' | grep -i raven                       # running container + its published port
docker ps -a --format '{{.Names}}\t{{.Image}}\t{{.Status}}' | grep -i raven                   # stopped one: `docker start <name>` beats a fresh install
pgrep -af 'Raven.Server' | head                                                               # native process; Windows: Get-Service RavenDB
ss -tlnp | grep -E ':(80|8080|8081|8090|9090|38888|38889)\b'                                  # macOS: lsof -i :8080
```

**A port list is not a discovery protocol.** A server on a port nobody guessed is invisible to every probe above, so enumerate what is actually listening (`ss -tln`, no filter) and curl `/setup/alive` on each candidate — 8090 in particular, since it is the remap this file itself recommends. A native server on an odd port is found this way or not at all.

A native process and a container often both look like "the server on 8080" while only one holds the port. **`docker port <name>` is the reliable tie-breaker**: the process column of `ss -tlnp` is blank for sockets owned by another user unless you are root, so it usually resolves nothing. And a containerised server appears in the host's `pgrep -af Raven.Server` output like a native one — check process ownership or `docker top` before telling the user they have a native install.

Also read what the project already declares — `appsettings*.json`, `.env`, `docker-compose*.yml` often name a URL and database that must be used verbatim, including a non-default port.

Then, in one interaction:

- **Something answered** — report each URL found, with its `GET /build/version`, `GET /databases` and `GET /license/status` so the user sees version, database names, each database's declared `Environment`, and licence, then ask which server and database to use and wait for the answer. The licence belongs in that report: a licensed server that is shared and an unlicensed one that is yours lead to opposite recommendations. Reuse and plain-dev-install are not the only options either — say so when the task needs a *different kind* of server than the ones running (secured, isolated, a specific version). **Finding exactly one server is not an answer to that question** — one server plus one plausible database still gets asked, because the user may want an isolated instance, a different port, or a database you would never have guessed. A running server on a non-standard port is normal; do not assume 8080.
- **A RavenDB process or container exists but answers nothing** — offer to start it (`docker start <name>`, `systemctl start ravendb`) before offering to install. Report why it looked unreachable (wrong port, wizard mode, dead container).
- **Nothing found** — say so and ask whether to install a local dev server, naming the method you would use (Docker if the daemon is up, otherwise native) and the port. Install only after the user agrees. If they have a server elsewhere — a cloud instance, a team dev box — ask for its URL, database, and certificate instead. For a throwaway experiment, offer `http://live-test.ravendb.net` as the no-install option, and say plainly that it is a free **shared, public, periodically wiped** server — fine for a spike, never for real or private data.

Never overwrite or reconfigure a server that is already running, and never repurpose a database that already holds data without asking. **Permission to use a database is not permission to change its record**: enabling revisions or expiration, or creating a data subscription, changes behaviour for every client of that database and can consume a licensed slot — ask for those the same way you would ask before creating one.

**A server whose `GET /databases` lists databases you did not create is a shared server.** Creating a database on it, changing server settings, or registering a license there affects other people's work — say what you found and get a yes first. "It was the only server running" is not consent.

## Installing a dev server

Target: **one unsecured single-node server on `http://localhost:8080`**, Docker if the daemon is up, native otherwise (`install-native.md`). Unsecured means no certificate and no auth — keep it on localhost, never on a routable address. Two host ports get published, **8080 and 38888** (the TCP/cluster port), and both must be free — another RavenDB container already publishing 38888 is the common case. The HTTP port can be remapped freely (`-p 8090:8080`) — report the URL you settled on. **The TCP port cannot, unless the server is told about it.** A client that needs a raw TCP connection (data subscriptions, cluster traffic) asks `GET /info/tcp`, and the server answers with its own bind port — it knows nothing about the Docker publish mapping. So `-p 38889:38888` alone leaves clients dialling 38888 on the host, where they reach whichever *other* RavenDB holds that port, and the failure reads as a missing database rather than a wrong server. Publish the same number the server binds, or move both together — the same port number in all three places, and a number you checked is free rather than the one written here:

```bash
-p <free>:<free> -e RAVEN_ServerUrl_Tcp=tcp://0.0.0.0:<free>
# add -e RAVEN_PublicServerUrl_Tcp=tcp://<host clients dial>:<free> when that hostname is not the bind address
```

Published ports are fixed when a container is created, so this is not an in-place repair: an existing container has to be `docker rm -f`'d and re-run with the same named volumes, which keeps the data.

HTTP-only work (CRUD, queries, bulk insert) succeeds regardless, so a mismatch stays invisible until the first subscription. On a server you do not control — someone else's container, with its mapping off-limits — the answer is a server of your own with matching ports, not a client-side workaround: the TCP address is chosen by the server, and every honest fix is on that side.

## Docker (development, unsecured)

`docker-compose.ravendb.yml`:

```yaml
services:
  ravendb:
    image: ravendb/ravendb:latest
    container_name: ravendb-dev
    ports:
      - "8080:8080"
      - "38888:38888"
    environment:
      - RAVEN_Setup_Mode=None
      - RAVEN_License_Eula_Accepted=true
      - RAVEN_DATABASE=MyApp
    volumes:
      - ravendb-data:/var/lib/ravendb/data
      - ravendb-config:/etc/ravendb

volumes:
  ravendb-data:
  ravendb-config:
```

```bash
docker compose -f docker-compose.ravendb.yml up -d     # stop / down -v to remove data
```

The image defaults to `Setup.Mode=Initial`, which boots into the setup wizard and serves no API — `RAVEN_Setup_Mode=None` is what makes a usable dev server. Any config key works as an env var with a `RAVEN_` prefix and dots as underscores (`RAVEN_Logs_Mode=Information`), or as `RAVEN_ARGS=--Setup.Mode=None`.

`RAVEN_DATABASE` makes the entrypoint create that database once the server is up — no provisioning call needed. It lands a moment *after* `/setup/alive` starts answering, so poll for the database, not just for the server.

The two volumes are what survive `docker rm`: `/var/lib/ravendb/data` holds the databases, `/etc/ravendb` the settings and any certificates. Pre-6.0 images used `/opt/RavenDB/Server/RavenData`; mounting that path on a current image silently persists nothing.

Unsecured access needs no extra flag: the container binds `http://<container-hostname>:8080`, which falls inside the image's default `Security.UnsecuredAccessAllowed=PrivateNetwork`, and the published port is what keeps it on your localhost. `PublicNetwork` is for multi-node compose setups where nodes advertise a `PublicServerUrl` to each other.

Single-shot equivalent:

```bash
docker run -d --name ravendb-dev -p 8080:8080 -p 38888:38888 \
  -e RAVEN_Setup_Mode=None -e RAVEN_License_Eula_Accepted=true -e RAVEN_DATABASE=MyApp \
  -v ravendb-data:/var/lib/ravendb/data -v ravendb-config:/etc/ravendb \
  ravendb/ravendb:latest
```

## Wait for ready, then verify

```bash
for i in $(seq 1 20); do
  [ "$(curl -s -o /dev/null -w '%{http_code}' --max-time 3 http://localhost:8080/setup/alive)" = "204" ] && break
  sleep 3
done
curl -s http://localhost:8080/build/version                                            # {"BuildVersion":…,"ProductVersion":"7.2","FullVersion":"7.2.x",…}
curl -s -o /dev/null -w '%{http_code}\n' http://localhost:8080/databases/MyApp/stats    # 200 once the database exists
```

Never ready → `docker logs ravendb-dev` (native: the server's console output). The usual causes are a port already bound, a missing `RAVEN_Setup_Mode=None`, or a volume the container user cannot write.

## Create a database explicitly

```bash
curl -s -X PUT http://localhost:8080/admin/databases \
  -H 'Content-Type: application/json' -d '{"DatabaseName":"MyApp"}'
```

The body is a database record; `DatabaseName` is the only required field. Success returns `{"RaftCommandIndex":n,"Name":"MyApp","Topology":{…}}`. A name that already exists returns **409** with a `ConcurrencyException` — treat that as success so provisioning stays idempotent. Deleting takes `DELETE /admin/databases` with `{"DatabaseNames":["MyApp"],"HardDelete":true}`.

Doing this from application startup instead of curl (the right place for it) — see `structure.md` and your language's `operations.md`. Writing and reading documents over plain HTTP, for a smoke test with no client library: `http-api.md`.

## Version — pin it down once

Two versions decide what compiles and what runs; check both before writing code, not after a 404 or a missing method.

```bash
curl -s http://localhost:8080/build/version
# {"BuildVersion":…,"ProductVersion":"<major.minor>","FullVersion":"<full>","CommitHash":"..."}
```

The client library version is in the project, not on the server — `package.json` (`ravendb`), `.csproj` or `Directory.Packages.props` (`RavenDB.Client`), `requirements.txt`/`pyproject.toml` (`ravendb`). A `7.2` server does not give an app pinned to `6.2` the 7.2 client APIs, and a current client talking to an older server gets a 404 on endpoints that server never had.

State both versions to the user when they differ from what the task assumes, and when a feature you need is marked `(7.0+)`/`(7.1+)` above either one, say which side is short and what upgrading it involves — a package bump, or a server upgrade. Never write code against a version the deployment does not have.

## Licensing — check it, and say something

Check every server you connect to, whether you installed it or found it:

```bash
curl -s http://localhost:8080/license/status     # 6.2 / 7.0: /admin/license/status
```

`Type` is `None` on an unlicensed server, and a `Has*` boolean per feature says what is actually enabled (`HasAiAgent`, `HasEmbeddingsGeneration`, `HasGenAi`, `HasEncryption`, `HasRavenEtl`, …). Read this before relying on any feature marked `(license)`, rather than inferring from the version.

The same response also carries **numeric `Max*` caps, and exceeding one is rejected with `LicenseLimitException` and HTTP 402** — not a feature that is off, but a number that is too big. Revisions to keep and their age, subscriptions per database, custom analyzers and sorters, cluster size and core/memory allowances all arrive as `Max*` numbers in that response. **Read the cap you are about to touch out of the response** before configuring revisions, subscriptions, analyzers or sorters — the unlicensed values are small and vary by version, and a boolean sweep of `Has*` will not warn you.

**Cap enforcement does not start with the server.** For roughly the first minute after startup an over-cap configuration is accepted with a 200, persisted into the database record, and honoured afterwards — the same call rejected with 402 a minute later. A provisioning path that runs as the container comes up is exactly where this lands, so reading the cap first is the only defence; a 200 is not evidence you were inside the limit.

A third category hides between the booleans and the numbers: `Can*` flags. `CanSetupDefaultRevisionsConfiguration` is `false` unlicensed, which refuses a database-wide **default** revisions configuration at any size ("Your license doesn't allow the creation of a default configuration for revisions") while per-collection configuration inside the caps is allowed. Sweeping only `Has*` and `Max*` misses it, and it blocks the obvious way to turn on revisions for a whole app.

A cap on retention is a cap on the **configured number**, not on the history you end up with: `MinimumRevisionsToKeep` is a floor protecting recent revisions from pruning, and with an age limit also set, pruning is driven by age. Three writes under a cap of `2` legitimately keep three revisions — do not tell a user that an unlicensed server stores only two versions of a document.

**When `Type` is `None`, or the license is expired, warn the user before building on it** — do not silently proceed and do not silently install a license either. State the limits that bite, reading the actual numbers out of the status response — cores, cores per node, memory, cluster size — and that every add-on feature is off — which means AI agents, embeddings generation, encryption, ETL, and cluster features fail at runtime, not at startup. Then ask whether they want you to obtain a **free Developer license** now, and continue only on their answer.

If they say yes, `license.md` has the rest: who requests the key, the activation call, and the verification step.

If they decline, note which features their plan can no longer use, and keep the design inside what an unlicensed server can do.

## Connecting from the app

The URL and database the user confirmed — `http://localhost:8080` and `MyApp` for the server above, whatever they named otherwise — and no certificate on an unsecured server. One `DocumentStore` per process — construction, index deployment, and seeding all belong in one startup path (`structure.md`), with the concrete calls in your language's `client.md`.

## Secured / production

**A license is required by the setup *wizard*, not by security itself.** An unlicensed server runs fully secured — HTTPS plus client-certificate authentication — when configured directly, which is the route to take for a local secured dev server:

```bash
-e RAVEN_Setup_Mode=None -e RAVEN_License_Eula_Accepted=true \
-e RAVEN_ServerUrl=https://0.0.0.0:<port> -e RAVEN_PublicServerUrl=https://<host clients dial>:<port> \
-e RAVEN_Security_Certificate_Path=/certs/server.pfx \
-e RAVEN_Security_Certificate_Password=<password> \
-e RAVEN_Security_WellKnownIssuers_Admin="$(openssl x509 -in ca.crt -outform DER | base64 -w0)"
```

- The server certificate needs a SAN matching the host clients dial (`DNS:localhost`, `IP:127.0.0.1` for a local one), and the certificate's own identity authenticates as cluster admin — which is what breaks the bootstrap chicken-and-egg of needing an admin certificate to register the first certificate.
- `Security.WellKnownIssuers.Admin` takes **base64 of the issuer's DER bytes**, not a thumbprint; a thumbprint kills the server at boot with `Unable to parse the provided 'Security.WellKnownIssuers.Admin' value` → `ASN1 corrupted data`. Client certificates signed by that issuer are then admitted; anything else gets `403 InvalidAuth` naming the unknown certificate.
- Once a certificate is configured, `Security.UnsecuredAccessAllowed` stops applying — every request is certificate-gated.
- `RAVEN_DATABASE` does **not** work on a secured server: the image's entrypoint fails (`./cert-utils.sh: No such file or directory`) and the container exits 1. Create the database with `PUT /admin/databases` presenting your client certificate.
- `rvn` cannot generate certificates; it only consumes them (`put-client-certificate`, `create-setup-package -m own-certificate`). Generate them with `openssl`, and note `put-client-certificate` accepts no password, so a pfx handed to it must be password-less.

The wizard remains the route for a public-facing server: start with only `RAVEN_License_Eula_Accepted=true`, open the server URL, and follow it. It issues a Let's Encrypt certificate (RavenDB provides a free `*.development.run` domain) or takes your own, and hands back a client-certificate bundle. **That path needs a license key before it can complete.** Details: <https://ravendb.net/docs> → Start / Installation.

Client-side wiring for either: the "Secured server" section of your language's `client.md`.
