# Cloudflare Workers
---

{NOTE: What are Cloudflare Workers?}

[Cloudflare Workers](https://developers.cloudflare.com/workers/) is a serverless platform that allows you to deploy workloads closer to where your users are with 200+ metro data centers in its CDN network.

Learn more about [how Workers works](https://developers.cloudflare.com/workers/learning/how-workers-works/).

Cloudflare Workers run on the V8 Runtime. The RavenDB Node.js client SDK provides support to query RavenDB resources in RavenDB Cloud or in your own cloud infrastructure.

### New to Cloudflare Workers?

For a walkthrough and demo of getting started with Cloudflare Workers, see [TBD](#).

{NOTE/}

In this guide, you will learn how deploy a Cloudflare Worker using the [RavenDB Cloudflare Worker template][template] that is connected to your RavenDB database. This guide assumes you are familiar with Node.js development techniques and the basics of Cloudflare Workers.

* [Before We Get Started](#before-we-get-started)
* [Create a Cloudflare RavenDB Worker project](#create-a-cloudflare-ravendb-worker-project)
* [Configure Cloudflare Worker](#configure-cloudflare-worker)
* [Test the Worker Locally](#test-the-worker-locally)
* [Configuring the Production Environment](#configuring-the-production-environment)

{PANEL: Before We Get Started}

You will need the following before continuing:

- A free or paid [Cloudflare account](https://cloudflare.com)
- A [RavenDB Cloud](https://cloud.ravendb.net) account or self-hosted client certificate
- [Git](https://git-scm.org)
- [Node.js](https://nodejs.com) 16+

{PANEL/}

{PANEL: Create a Cloudflare RavenDB Worker project}

To get started, you can use npm and Cloudflare's `create-cloudflare` package to create a new Worker using [the RavenDB template][template]:

`npm init cloudflare my-project https://github.com/ravendb/template-cloudflare-worker`

This will generate all the required code and run `npm install` for you to set up a new project on your computer. You can then push to GitHub or any other source provider from there.

{PANEL/}

{PANEL: Configure Cloudflare Worker}

If you try to run the template, it _may_ work since by default it is using the Live Test instance of RavenDB which is unauthenticated. However, there are more steps to configure the template for production using client certificate authentication.

Client certificate authentication is handled through [Cloudflare mTLS authentication for Workers][cf-mtls-worker]. You will need to upload your certificate to your Cloudflare account so that it can be accessed and bound to your Worker.

### Obtain RavenDB certificate

First, download your RavenDB client certificate package you will use to authenticate. Follow the guides for either Cloud or for self-hosting.

Once extracted to a folder, you'll need the paths to the `.pem` and `.key` files for the next step.

{WARNING: Do not copy certificate files to the project}

For Cloudflare Workers, you do not store your certificate files in your project directory. **Certificates are password-equivalents.** Take care not to accidentally commit them to source control. Keep them outside the project directory for this next step.

{WARNING/}

### Upload certificate using wrangler

You will use Cloudflare's `wrangler` CLI to upload your `.pem` and `.key` files as an mTLS certificate. You only need to do this once (and each time the certificate needs to be renewed).

{INFO: wrangler global vs. local usage}

This guide will use `npx` to execute wrangler to ensure the commands work across platforms. You can also choose to install `wrangler` globally using `npm i wrangler -g` to use without `npx`, but you will need to keep it updated. Read more about [Installing and updating Wrangler][cf-wrangler]

{INFO/}

In the project directory, run:

`npx wrangler mtls-certificate upload --cert path/to/db.pem --key path/to/db.pem --name db-cert`

Once uploaded, list the certificates:

`npx wrangler mtls-certificate list`

This will output a list and you can copy the Certificate ID for the cert you just uploaded.

### Setup mTLS binding in wrangler

Cloudflare Edge workers use the `wrangler.toml` file for configuration. You will need to add a "binding" so that the certificate is made available and used by the Worker at runtime.

Edit your `wrangler.toml` file to update the following:

{CODE-BLOCK:git}
mtls_certificates = [
  { binding = "DB_CERT", certificate_id = "<CERTIFICATE_ID>" } 
]
{CODE-BLOCK/}

It is important to maintain the `DB_CERT` name here as it is expected by the template. Replace `<CERTIFICATE_ID>` with the Certificate ID you copied from the previous step.

For a deeper dive on what this is doing, you can [read more][cf-mtls-worker] about how mTLS bindings work in Cloudflare Workers.

### Update DB connection settings

In the `wrangler.toml` file you will also see some environment variables:

- `DB_URLS` -- These are the node URLs for your RavenDB instance (Cloud or self-hosted)
- `DB_NAME` -- This is the default database to connect to

The defaults are under `[vars]` and overriden in `[env.production.vars]`. These settings will overwrite whatever values you use in the Cloudflare dashboard on deployment.

{NOTE: Make sure the DB exists}

For brand new projects, the database you connect to may not exist yet. Follow the [create database procedure][docs-create-db] to create a new database otherwise you will encounter a `DatabaseDoesNotExist` exception on startup.

{NOTE/}

{PANEL/}

{PANEL: Test the Worker locally}

Once the certificate binding is added, you will be able to start the Worker locally and test the certificate authentication.

`npm start`

This will launch `wrangler` in development mode. It may require you to sign in to your Cloudflare account before continuing.

Press "l" to enter non-local mode.

{NOTE: }
The `env.DB_CERT` binding will not be available in local mode, this is a known issue with wrangler2.
{NOTE/}

You should see the following message in the console:

> A bound cert was found and will be used for RavenDB requests.

Once started, the Worker will be running on a localhost address through the Miniflare tunnel infrastructure.

Open the local address in your browser (e.g. http://localhost:7071) and you should see a message like this:

`Successfully connected to Node A`

This means you have successfully connected to your RavenDB database.

{PANEL/}

{PANEL: Using RavenDB inside the Worker}

The RavenDB Cloudflare template uses the itty-router package to provide basic routing and middleware support.

Each routing handler is passed a `request` and `env` argument. A document session is opened per-request and accessible through `env.db`.

### Example: Load user on route

{CODE-BLOCK:javascript}
router.get("/users/:id", async (request: IRequest, env: Env) => {
    const user = await env.db.load("users/" + request.params.id);

    return new Response(JSON.stringify({ user }), { status: 200 });
});
{CODE-BLOCK/}

{PANEL/}

{PANEL: Configuring the Production environment}

Once the local worker has been verified, you can deploy it to your Cloudflare Worker account for production use.

`npm run deploy`

If your Worker account is not yet set up, Wrangler will walk you through the steps.

{PANEL/}

[template]: https://github.com/ravendb/template-cloudflare-worker
[cf-mtls-worker]: https://developers.cloudflare.com/workers/runtime-apis/mtls
[cf-wrangler]: https://developers.cloudflare.com/workers/wrangler/install-and-update/
[docs-create-db]: /docs/article-page/latest/csharp/studio/database/create-new-database/general-flow
