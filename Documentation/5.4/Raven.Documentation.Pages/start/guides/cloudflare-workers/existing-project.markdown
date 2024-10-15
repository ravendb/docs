# Add RavenDB to an Existing Cloudflare Worker (TypeScript)
---

{NOTE: }

* [Cloudflare Workers](https://developers.cloudflare.com/workers/) is a serverless platform that allows 
  you to deploy workloads closer to where your users are with 200+ metro data centers in its CDN network.  

* Learn more about [how Workers works](https://developers.cloudflare.com/workers/learning/how-workers-works/).  

* Cloudflare Workers run on the V8 Runtime.  
  The RavenDB Node.js client SDK provides support to query RavenDB resources in RavenDB Cloud or in your 
  own cloud infrastructure.  

* In this guide, you will learn how connect to RavenDB from an existing Cloudflare Worker.  
  We assume you are familiar with Node.js development techniques and the basics of Cloudflare Workers.  

* In this page:  
    * [Before We Get Started](../../../start/guides/cloudflare-workers/existing-project#before-we-get-started)
    * [Installing the RavenDB Client SDK](../../../start/guides/cloudflare-workers/existing-project#installing-the-ravendb-client-sdk)
    * [Initializing the Document Store](../../../start/guides/cloudflare-workers/existing-project#initializing-the-document-store)
    * [Updating Database Connection Settings](../../../start/guides/cloudflare-workers/existing-project#updating-database-connection-settings)
    * [Configuring Support for Certificates](../../../start/guides/cloudflare-workers/existing-project#configuring-support-for-certificates)
    * [Configuring Cloudflare](../../../start/guides/cloudflare-workers/existing-project#configuring-cloudflare)

{NOTE/}

{PANEL: Before We Get Started}

You will need the following before continuing:

- A [RavenDB Cloud][cloud-signup] account or self-hosted client certificate  
- A free or paid [Cloudflare account](https://cloudflare.com)  
- [Node.js](https://nodejs.com) 16+ with npm  

{PANEL/}

{PANEL: Installing the RavenDB Client SDK}

Get started by installing the [ravendb][npm-ravendb-client] npm package in your project which provides the Node.js client SDK.  

Using npm:  

{CODE-BLOCK:bash}
npm install ravendb
{CODE-BLOCK/}

{NOTE: }
Support for Cloudflare Workers was added in `5.2.8+`.
{NOTE/}

{PANEL/}

{PANEL: Initializing the Document Store}

Import the `DocumentStore` from `ravendb` package to create a new instance with the required configuration 
and initialize your connection to RavenDB by calling the `initialize` method.   
You can then export a function to initialize a document session to use in your Cloudflare Worker.  

Example `db.ts` TypeScript module:

{CODE-BLOCK:javascript}
import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore(
  ["https://a.free.mycompany.ravendb.cloud"],
  "demo",
  // authOptions
);

let initialized = false;

function initialize() {
    if (initialized) return;
    documentStore.initialize();
    initialized = true;
}

export function openAsyncSession() {
    if (!initialized) {
        initialize();
    }

    return documentStore.openSession();
}
{CODE-BLOCK/}

For more on what options are available, see [Creating a Document Store][docs-what-is-document-store].  

You can set options manually but it's more likely you'll want to set config variables in Wrangler 
or in Cloudflare to customize the document store initialization.  

{PANEL/}

{PANEL: Updating Database Connection Settings}

### Enable Node Compatibility

Update your `wrangler.toml` file to enable Node.js compatibility by setting `node_compat` to `true`:  

{CODE-BLOCK:json}
name = "ravendb-worker"
main = "./src/index.ts"
node_compat = true
compatibility_date = "2022-05-03"
{CODE-BLOCK/}

This setting is required for the RavenDB Node.js SDK to operate correctly. If this is `false` or missing, 
you will experience runtime exceptions.  

### Define environment variables

You will also want to set environment variables to pass database information such as the URLs and database name:  

{CODE-BLOCK:json}
# Define top-level environment variables
# under the `[vars]` block using
# the `key = "value"` format
[vars]
DB_URLS = "https://a.free.company.ravendb.cloud"
DB_NAME = "dev"

# Override values for `--env production` usage
[env.production.vars]
DB_URLS = "https://a.free.company.ravendb.cloud,https://b.free.company.ravendb.cloud"
DB_NAME = "prod"
{CODE-BLOCK/}

There are two variables defined above:  

- `DB_URLS` -- These are the node URLs for your RavenDB instance (Cloud or self-hosted). The values are comma-separated.  
- `DB_NAME` -- This is the default database to connect to.  
 
The defaults are under `[vars]` and overriden in `[env.production.vars]`.  

{NOTE: wrangler.toml overrides on deployment}
You can also define settings within the Cloudflare worker dashboard. The values in the `wrangler.toml` will 
overwrite those values *on new deployment*. Keep this in mind when deciding where to define the variables!  
{NOTE/}

Variables defined here will be exposed on the `env` variable passed to the root `fetch` function of the Worker.  

For example, a barebones `index.ts` ES module could look like:  

{CODE-BLOCK:javascript}
import { DocumentStore } from "ravendb";

let documentStore: DocumentStore;
let initialized = false;

function initialize({ urls, databaseName }) {
    if (initialized) return;
    documentStore = new DocumentStore(
      urls,
      databaseName
    );
    documentStore.initialize();
    initialized = true;
}

function openAsyncSession() {
    if (!initialized) {
        throw new Error("DocumentStore has not been initialized");
    }

    return documentStore.openSession();
}

export default {
    fetch(req, env, ctx) {
        const { DB_URLS, DB_NAME } = env;

        initialize({ 
            urls: DB_URLS.split(","), 
            databaseName: DB_NAME
        });

        ctx.db = openAsyncSession();

        // Handle request
        // ...
        return handleRequest(req, env, ctx);
    }
}
{CODE-BLOCK/}

This creates a session-per-request and avoids re-initializing the document store.  
The request handler can then pass the document session to middleware or route handlers as needed.  
The [gh-ravendb-template][gh-ravendb-template] uses the [itty-router][npm-itty-router] package to make this easier.  

{NOTE: Cloudflare Worker requests are isolated}
Cloudflare Worker invocations do not incur cold start cost like other serverless platforms.  
However, requests are isolated and modules are not shared between requests. This means that 
document store initialization is incurred every request, however the overhead is minimal.  

For document caching during a session or across requests, using Cloudflare's KV natively is not yet 
supported by the RavenDB Node.js client SDK but could be implemented manually through application logic.  
{NOTE/}

{PANEL/}

{PANEL: Configuring Support for Certificates}

Client certificate authentication is handled through [Cloudflare mTLS authentication for Workers][cf-mtls-worker].  
You will need to upload your certificate to your Cloudflare account so that it can be accessed and bound to your Worker.  

### Obtain RavenDB certificate

First, download your RavenDB client certificate package you will use to authenticate.  
Follow the guides for either [Cloud certificates][docs-cloud-certs] or for [self-hosted certificates][docs-on-prem-certs].  
It is recommended to [generate a new client certificate][docs-generate-client-certificate] 
with limited access rights instead of a `ClusterAdmin`-level certificate.  
This also ensures the Worker is using a dedicated certificate that can be managed separately.  

Once extracted to a folder, you'll need the paths to the `.crt` and `.key` files for the next step.  

{WARNING: Do not copy certificate files to the project}

For Cloudflare Workers, you do not store your certificate files in your project directory.  
**Certificates are password-equivalents.** Take care not to accidentally commit them to source control.  
Keep them outside the project directory for this next step.  

{WARNING/}

### Upload certificate using wrangler

You will use Cloudflare's `wrangler` CLI to upload your `.crt` and `.key` files as an mTLS certificate.  
You only need to do this once (and each time the certificate needs to be renewed).  

{INFO: wrangler global vs. local usage}

This guide will use `npx` to execute wrangler to ensure the commands work across platforms.  
You can also choose to install `wrangler` globally using `npm i wrangler -g` to use without `npx`, 
but you will need to keep it updated. Read more about [Installing and updating Wrangler][cf-wrangler]  

{INFO/}

In the project directory, run:  

{CODE-BLOCK:bash}
npx wrangler mtls-certificate upload --cert path/to/db.crt --key path/to/db.key --name ravendb_cert
{CODE-BLOCK/}

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

You will need to add a mTLS "binding" so that the certificate is made available and used by the Worker at runtime.  

Edit your `wrangler.toml` file to update the following:  

{CODE-BLOCK:git}
mtls_certificates = [
  { binding = "DB_CERT", certificate_id = "<CERTIFICATE_ID>" } 
]
{CODE-BLOCK/}

Replace `<CERTIFICATE_ID>` with the Certificate ID you copied from the previous step.  

Be sure to also update the `DB_URLS` and `DB_NAME` variables for your cluster.  

For a deeper dive on what this is doing, you can [read more][cf-mtls-worker] about how mTLS bindings work in Cloudflare Workers.  

### Set custom fetcher for DocumentStore

Once the certificate binding is added, Cloudflare will create a `DB_CERT` object exposed through `env`.  
You can then bind the provided `env.DB_CERT.fetch` custom fetch function to the `DocumentStore` using 
the `DocumentConventions.customFetch` option.  

An updated example `index.ts` ES module:  

{CODE-BLOCK:javascript}
import { DocumentStore } from "ravendb";

let documentStore: DocumentStore;
let initialized = false;

function initialize({ urls, databaseName, mtlsBinding }) {
    if (initialized) return;
    documentStore = new DocumentStore(
      urls,
      databaseName
    );

    // Bind custom mTLS binding `fetch()` function and ensure `this` remains bound to
    // original context
    documentStore.conventions.customFetch = mtlsBinding.fetch.bind(mtlsBinding);

    documentStore.initialize();
    initialized = true;
}

function openAsyncSession() {
    if (!initialized) {
        throw new Error("DocumentStore has not been initialized");
    }

    return documentStore.openSession();
}

export default {
    fetch(req, env, ctx) {
        const { DB_URLS, DB_NAME, DB_CERT } = env;

        initialize({ 
            urls: DB_URLS.split(","), 
            databaseName: DB_NAME,
            mtlsBinding: DB_CERT
        });

        ctx.db = openAsyncSession();

        // Handle request
        // ...
        return handleRequest(req, env, ctx);
    }
}
{CODE-BLOCK/}

The `DB_CERT` variable is exposed on `env` and has a single `fetch` function that is automatically 
bound to the client certificate at runtime. Note that passing the function requires binding the `this` 
context to the original scope, otherwise you will run into closure-related exceptions.  

{NOTE: }
The `env.DB_CERT` binding will not be available in local mode (`--local`), this is a known issue with Wrangler.  
{NOTE/}

### Add TypeScript Declaration for DB_CERT

If you are using TypeScript, `env.DB_CERT` will not be typed by default. You can create a `globals.d.ts` file 
in your project and add the following type declarations:  

{CODE-BLOCK:javascript}
declare global {
  namespace NodeJS {
    interface ProcessEnv {
      DB_CERT?: { fetch: typeof fetch }
    }
  }
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Configuring Cloudflare}

There is no extra configuration necessary if you are providing all the connection information in your `wrangler.toml` file. 
However, you may want to override variables set in the `[env.production.vars]` through the Cloudflare Worker dashboard.  

Navigate to your Worker **Settings > Variables > Environment Variables** and add variables to override, like 
`DB_NAME` and `DB_URLS`. You will also see the `DB_CERT` binding listed if the mTLS binding was successfully uploaded.  

![Cloudflare Worker environment variables and settings](images/cf-env-vars.jpg "Cloudflare Worker environment variables and settings")

{PANEL/}

{PANEL: Next Steps}

- Learn more about [how to use the RavenDB Node.js client SDK][docs-nodejs]  
- Reference the [Cloudflare Worker starter template][gh-ravendb-template] to see the code  
- [Troubleshoot][troubleshooting] issues with RavenDB and Cloudflare Workers  

{PANEL/}

[troubleshooting]: ../../../start/guides/cloudflare-workers/troubleshooting
[cloud-signup]: https://cloud.ravendb.net?utm_source=ravendb_docs&utm_medium=web&utm_campaign=howto_template_cloudflare_worker&utm_content=cloud_signup
[gh-ravendb-template]: https://github.com/ravendb/template-cloudflare-worker
[deploy-with-workers]: https://deploy.workers.cloudflare.com/?url=https://github.com/ravendb/template-cloudflare-worker
[live-test]: http://live-test.ravendb.net
[cf-mtls-worker]: https://developers.cloudflare.com/workers/runtime-apis/mtls
[cf-wrangler]: https://developers.cloudflare.com/workers/wrangler/install-and-update/
[docs-nodejs]: ../../../../nodejs/client-api/session/what-is-a-session-and-how-does-it-work
[docs-what-is-document-store]: ../../../..//nodejs/client-api/what-is-a-document-store
[docs-create-db]: ../../../studio/database/create-new-database/general-flow
[docs-cloud-certs]: ../../../cloud/cloud-security
[docs-on-prem-certs]: ../../../studio/overview
[docs-generate-client-certificate]: ../../../server/security/authentication/certificate-management#generate-client-certificate
[npm-ravendb-client]: https://npmjs.com/package/ravendb
[npm-itty-router]: https://npmjs.com/package/itty-router

## Related Articles

### RavenDB

- [Cloudflare Workers: Overview](../../../start/guides/cloudflare-workers/overview)  
- [Cloud Signup](https://cloud.ravendb.net?utm_source=ravendb_docs&utm_medium=web&utm_campaign=howto_template_cloudflare_worker&utm_content=cloud_signup)  
- [Live Test](http://live-test.ravendb.net)  
- [Create Database](../../../studio/database/create-new-database/general-flow)  
- [Cloud Certificate](../../../cloud/cloud-security)  
- [On-prem Certificate](../../../studio/overview)  
- [Generate Client Certificate](../../../server/security/authentication/certificate-management#generate-client-certificate)  

### CloudFlare

- [Template](https://github.com/ravendb/template-cloudflare-worker)  
- [Deploy With Workers](https://deploy.workers.cloudflare.com/?url=https://github.com/ravendb/template-cloudflare-worker)  
- [mTLS Worker](https://developers.cloudflare.com/workers/runtime-apis/mtls)  
- [Wrangler](https://developers.cloudflare.com/workers/wrangler/install-and-update/)  
