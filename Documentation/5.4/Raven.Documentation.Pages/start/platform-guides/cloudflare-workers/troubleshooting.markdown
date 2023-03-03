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

[docs-create-db]: /docs/article-page/latest/csharp/studio/database/create-new-database/general-flow
