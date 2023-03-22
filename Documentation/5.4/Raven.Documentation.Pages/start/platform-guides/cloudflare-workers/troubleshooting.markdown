# Troubleshooting
---


### `DatabaseDoesNotExist` error

The instance you're connecting to doesn't have a database yet (specified through `DB_NAME`).

Follow the instructions to [create a new database][docs-create-db] in the Studio.

### Cannot find `DB_CERT.fetch` function

Wrangler must be run in _non-local mode_ to populate the certificate binding environment variable.

When Wrangler boots up, press the `l` key to enter non-local mode, which will restart Wrangler and show a message like this:

`Shutting down local server.`

You may need to authenticate Wrangler again to your Cloudflare account.

### Cannot Connect to RavenDB When Deployed

If you have IP restrictions enabled for your RavenDB cluster, be sure to allow the [Cloudflare IP ranges][cf-ips].

Be sure to also verify your `wrangler.toml` in case you are using [different deployment environments][cf-worker-env].

[docs-create-db]: /docs/article-page/latest/csharp/studio/database/create-new-database/general-flow
[cf-ips]: https://cloudflare.com/ips
[cf-worker-env]: https://developers.cloudflare.com/workers/platform/environments/ 
