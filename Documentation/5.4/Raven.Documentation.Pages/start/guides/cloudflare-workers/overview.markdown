# Cloudflare Workers
---

{NOTE: What are Cloudflare Workers?}

[Cloudflare Workers](https://developers.cloudflare.com/workers/) is a serverless platform that allows you to deploy workloads closer to where your users are with 200+ metro data centers in its CDN network.

Learn more about [how Workers works](https://developers.cloudflare.com/workers/learning/how-workers-works/).

Cloudflare Workers run on the V8 Runtime. The RavenDB Node.js client SDK provides support to query RavenDB resources in RavenDB Cloud or in your own cloud infrastructure.

<!-- ### New to Cloudflare Workers?

For a walkthrough and demo of getting started with Cloudflare Workers, see [TBD](#). -->

{NOTE/}

In this guide, you will learn how deploy a Cloudflare Worker using the [RavenDB Cloudflare Worker template][template] that is connected to your RavenDB database. This guide assumes you are familiar with Node.js development techniques and the basics of Cloudflare Workers.

* [Before We Get Started](#before-we-get-started)
* [Create a Cloudflare RavenDB Worker project](#create-a-cloudflare-ravendb-worker-project)
* [Updating Database Connection Settings](#updating-database-connection-settings)
* [Connecting to an Authenticated RavenDB Cluster](#connecting-to-an-authenticated-ravendb-cluster)
* [Deploying to Production](#deploying-to-production)
* [Using RavenDB in the Worker](#using-ravendb-in-the-worker)

{PANEL: Before We Get Started}

You will need the following before continuing:

- A [RavenDB Cloud][cloud-signup] account or self-hosted client certificate
- A free or paid [Cloudflare account](https://cloudflare.com)
- [Git](https://git-scm.org)
- [Node.js](https://nodejs.com) 16+ with npm

{PANEL/}

{PANEL: Create a Cloudflare RavenDB Worker project}

There are two primary ways to get started:

- Using the Cloudflare Deploy with Workers wizard
- Using npm to initialize an empty template

### Using Deploy with Workers wizard

<!-- TODO: Embed YT video how-to -->

Using [Deploy with Workers][deploy-with-workers] step-by-step wizard is the simplest method but requires a GitHub account and authorized access, which may not be applicable in all situations. For example, this method will not work with GitLab or GitHub Enterprise.

![Screenshot of Deploy with Cloudflare Wizard][image-template-deploy-cloudflare]

The wizard will guide you through deploying a Worker and hooking up a new repo with continuous deployment through GitHub actions. It will also automatically set up your repository secrets.

{WARNING: Forking the Template}
The deployment wizard will fork the GitHub repository into your GitHub user account (not an organization). You will want to manually rename the repository and [unmark it as a "Template"][gh-template-repo] in the repository settings before cloning it.
{WARNING/}

### Using npm to initialize an empty template

If you do not want to use the wizard, you can use npm and Cloudflare's `create-cloudflare` package to create a new Worker using [the RavenDB template][template]:

`npm init cloudflare my-project https://github.com/ravendb/template-cloudflare-worker`

This will generate all the required code and run `npm install` for you to set up a new project on your computer. You can then push to GitHub or any other source provider from there.

### Test the template locally

By default, the template is set up to connect to the [RavenDB Live Test cluster][live-test].

You can run the template locally to test the connection:

`npm run dev`

{NOTE: First-Time Wrangler Setup}
If this is the first time you are connecting using the Wrangler CLI, it will open a browser window for you to authenticate using your Cloudflare account. After you sign in, you can return to the terminal.
{NOTE/}

Open the browser by pressing the "B" key (e.g. http://localhost:7071) and you should see a screen like this:

![Successfully connected to RavenDB welcome screen from Cloudflare][image-template-welcome-unauthenticated]

This means you have successfully connected to your RavenDB database.

{PANEL/}

{PANEL: Updating Database Connection Settings}

The `wrangler.toml` file contains configuration for the worker. Here's an example:

{CODE-BLOCK:json}
name = "ravendb-worker"
main = "./src/index.ts"
node_compat = true
compatibility_date = "2022-05-03"

# mtls_certificates = [
#  { binding = "DB_CERT", certificate_id = "<CERTIFICATE_ID>" } 
# ]

# Define top-level environment variables
# under the `[vars]` block using
# the `key = "value"` format
[vars]
DB_URLS = ""
DB_NAME = ""

# Override values for `--env production` usage
[env.production]
name = "ravendb-worker-production"
[env.production.vars]
DB_URLS = ""
DB_NAME = ""
{CODE-BLOCK/}

The `node_compat = true` setting is required for the RavenDB Node.js SDK to operate correctly. If this is `false` or missing, you will experience runtime exceptions.

There are two variables defined that are used by the template:

- `DB_URLS` -- These are the node URLs for your RavenDB instance (Cloud or self-hosted). The values are comma-separated
- `DB_NAME` -- This is the default database to connect to
 
When left blank, the Live Test connection settings are used. The defaults are under `[vars]` and overriden in `[env.production.vars]`.

{NOTE: wrangler.toml overrides on deployment}
You can also define settings within the Cloudflare worker dashboard. The values in the `wrangler.toml` will overwrite those values *on new deployment*. Keep this in mind when deciding where to define the variables!
{NOTE/}

{WARNING: Make sure the named database exists}

For brand new projects, the database you connect to may not exist yet. Follow the [create database procedure][docs-create-db] to create a new database otherwise you will encounter a `DatabaseDoesNotExist` exception on startup.

{WARNING/}

Variables defined here, including the `DB_CERT` mTLS binding, will be exposed as `process.env` variables you can access in the worker at runtime. You'll use the mTLS binding when connecting to an authenticated cluster using your client certificate.

{PANEL/}

{PANEL: Connecting to an Authenticated RavenDB Cluster}

Client certificate authentication is handled through [Cloudflare mTLS authentication for Workers][cf-mtls-worker]. You will need to upload your certificate to your Cloudflare account so that it can be accessed and bound to your Worker.

### Obtain RavenDB certificate

First, download your RavenDB client certificate package you will use to authenticate. Follow the guides for either [Cloud certificates][docs-cloud-certs] or for [self-hosted certificates][docs-on-prem-certs]. It is recommended to [generate a new client certificate][docs-generate-client-certificate] with limited access rights instead of a `ClusterAdmin`-level certificate. This also ensures the Worker is using a dedicated certificate that can be managed separately.

Once extracted to a folder, you'll need the paths to the `.crt` and `.key` files for the next step.

{WARNING: Do not copy certificate files to the project}

For Cloudflare Workers, you do not store your certificate files in your project directory. **Certificates are password-equivalents.** Take care not to accidentally commit them to source control. Keep them outside the project directory for this next step.

{WARNING/}

### Upload certificate using wrangler

You will use Cloudflare's `wrangler` CLI to upload your `.crt` and `.key` files as an mTLS certificate. You only need to do this once (and each time the certificate needs to be renewed).

{INFO: wrangler global vs. local usage}

This guide will use `npx` to execute wrangler to ensure the commands work across platforms. You can also choose to install `wrangler` globally using `npm i wrangler -g` to use without `npx`, but you will need to keep it updated. Read more about [Installing and updating Wrangler][cf-wrangler]

{INFO/}

In the project directory, run:

`npx wrangler mtls-certificate upload --cert path/to/db.crt --key path/to/db.key --name ravendb_cert`

This will display output like:

{CODE-BLOCK:bash}
Uploading mTLS Certificate ravendb_cert...
Success! Uploaded mTLS Certificate ravendb_cert
ID: <CERTIFICATE_ID>
Issuer: CN=...
Expires on ...
{CODE-BLOCK/}

Copy the `<CERTIFICATE_ID>` in the output for the next step.

### Setup mTLS binding in wrangler

Cloudflare Workers use the `wrangler.toml` file for configuration. You will need to add a "binding" so that the certificate is made available and used by the Worker at runtime.

Edit your `wrangler.toml` file to update the following:

{CODE-BLOCK:git}
mtls_certificates = [
  { binding = "DB_CERT", certificate_id = "<CERTIFICATE_ID>" } 
]
{CODE-BLOCK/}

It is important to maintain the `DB_CERT` name here as it is expected by the template. Replace `<CERTIFICATE_ID>` with the Certificate ID you copied from the previous step.

Be sure to also update the `DB_URLS` and `DB_NAME` variables for your cluster.

For a deeper dive on what this is doing, you can [read more][cf-mtls-worker] about how mTLS bindings work in Cloudflare Workers.

### Testing Certificate Authentication Locally

Once the certificate binding is added, you will be able to start the Worker locally and test the certificate authentication.

`npm run dev`

This will launch `wrangler` in development mode. It may require you to sign in to your Cloudflare account before continuing.

{NOTE: }
The `env.DB_CERT` binding will not be available in local mode (`--local`), this is a known issue with Wrangler. The template is configured to start Wrangler in non-local mode.
{NOTE/}

You should see the following message in the console:

> A bound cert was found and will be used for RavenDB requests.

Once started, the Worker will be running on a localhost address.

Open the browser by pressing the "B" key (e.g. http://localhost:7071) and you should see a screen like this:

![Successfully connected to RavenDB welcome screen from Cloudflare][image-template-welcome-authenticated]

If you see a green check and the correct connection details, this means you have successfully connected to your RavenDB database.

{PANEL/}


{PANEL: Deploying to Production}

### Automated Deployment

If you have used the Deploy with Workers wizard, your GitHub repository is already set up for continuous integration and deployment to Cloudflare.

If you have manually initialized the template, once pushed to GitHub you can [enable GitHub action workflows][gh-workflows].

You will also need to [add two repository secrets][gh-repo-secrets]:

- `CF_ACCOUNT_ID` -- Your Cloudflare global account ID
- `CF_API_TOKEN` -- An API token with the "Edit Workers" permission

Once these secrets are added, [trigger a workflow manually][gh-workflows-manual] or push a commit to trigger a deployment.

### Manual Deployment

You can also deploy a Worker manually using:

`npm run deploy`

If your Worker account is not yet set up, Wrangler will walk you through the steps.

### Verifying Production Worker

In your Cloudflare Dashboard, the Worker should be deployed. You can find your Worker URL in the dashboard under "Preview URL" and open it to test the connection is working.

![Preview URL shown in the Cloudflare Worker dashboard][image-cloudflare-worker-preview]

If it is not working, verify the Wrangler settings are being applied.

{PANEL/}

{PANEL: Using RavenDB in the Worker}

The RavenDB Cloudflare template uses the [itty-router package][npm-itty-router] to provide basic routing and middleware support.

Each routing handler is passed a `request` and `env` argument. A document session is opened per-request and accessible through `env.db`.

### Example: Load user on route

{CODE-BLOCK:javascript}
router.get("/users/:id", async (request: IRequest, env: Env) => {
    const user = await env.db.load("users/" + request.params.id);

    return new Response(JSON.stringify({ user }), { status: 200 });
});
{CODE-BLOCK/}

{PANEL/}

[cloud-signup]: https://cloud.ravendb.net?utm_source=ravendb_docs&utm_medium=web&utm_campaign=howto_template_cloudflare_worker&utm_content=cloud_signup
[template]: https://github.com/ravendb/template-cloudflare-worker
[deploy-with-workers]: https://deploy.workers.cloudflare.com/?url=https://github.com/ravendb/template-cloudflare-worker
[live-test]: http://live-test.ravendb.net
[cf-mtls-worker]: https://developers.cloudflare.com/workers/runtime-apis/mtls
[cf-wrangler]: https://developers.cloudflare.com/workers/wrangler/install-and-update/
[docs-create-db]: /docs/article-page/latest/csharp/studio/database/create-new-database/general-flow
[docs-cloud-certs]: /docs/article-page/latest/csharp/cloud/cloud-security
[docs-on-prem-certs]: /docs/article-page/latest/csharp/studio/overview
[docs-generate-client-certificate]: /docs/article-page/latest/csharp/server/security/authentication/certificate-management#generate-client-certificate
[npm-itty-router]: https://npmjs.com/package/itty-router
[gh-repo-secrets]: https://docs.github.com/en/actions/security-guides/encrypted-secrets
[gh-workflows]: https://docs.github.com/en/actions/learn-github-actions/understanding-github-actions
[gh-workflows-manual]: https://docs.github.com/en/actions/managing-workflow-runs/manually-running-a-workflow
[gh-template-repo]: https://docs.github.com/en/repositories/creating-and-managing-repositories/creating-a-template-repository
[image-template-deploy-cloudflare]: images/template-deploy-cloudflare.jpg
[image-template-welcome-unauthenticated]: images/template-welcome-unauthenticated.jpg
[image-template-welcome-authenticated]: images/template-welcome-authenticated.jpg
[image-cloudflare-worker-preview]: images/cloudflare-worker-preview.jpg
