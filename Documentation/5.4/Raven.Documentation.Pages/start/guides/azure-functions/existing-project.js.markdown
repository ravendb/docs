# Guides: Add RavenDB to an Existing Azure Functions Project (Node.js)
---

{NOTE: }

* Microsoft **Azure Functions** is a serverless platform that supports multiple 
  languages and frameworks that let you deploy workloads that scale without managing 
  any infrastructure.  

* Learn more about [how Microsoft Azure Functions work][az-funcs].

* In this guide, you will learn how to connect to RavenDB from your existing Node.js Azure Functions.  
  We assume you are familiar with Node.js development techniques and the basics of Azure Function apps.  

* In this page:
    * [Before We Get Started](../../../start/guides/azure-functions/existing-project#before-we-get-started)  
    * [Installing the RavenDB Client SDK](../../../start/guides/azure-functions/existing-project#installing-the-ravendb-client-sdk)
    * [Initializing the Document Store](../../../start/guides/azure-functions/existing-project#initializing-the-document-store)
    * [Adding Support for App Settings](../../../start/guides/azure-functions/existing-project#adding-support-for-app-settings)
    * [Configuring Support for Certificates](../../../start/guides/azure-functions/existing-project#configuring-support-for-certificates)
    * [Configuring Azure](../../../start/guides/azure-functions/existing-project#configuring-azure)

{NOTE/}

{PANEL: Before We Get Started}

You will need the following before continuing:  

- A [RavenDB Cloud][cloud-signup] account or self-hosted client certificate  
- [Azure Function Core Tools][az-core-tools] 4.x+  
- [Node.js][nodejs] 18+  

{NOTE: Starting from scratch?}
For a brand new Azure Functions app, we recommend using the [RavenDB Azure Functions Node.js template](../../../start/guides/azure-functions/overview) 
which is set up with PEM certificate support.  
You can also reference the template to see how the integration is set up.  
{NOTE/}

{PANEL/}

{PANEL: Installing the RavenDB Client SDK}

Get started by installing the [ravendb][npm-ravendb-client] npm package in your project which provides the Node.js client SDK.  

Using npm:

{CODE-BLOCK:bash}
npm install ravendb
{CODE-BLOCK/}

{PANEL/}

{PANEL: Initializing the Document Store}

Import the `DocumentStore` from `ravendb` package to create a new instance with the required 
configuration and initialize your connection to RavenDB by calling the `initialize` method.  
You can then export a function to initialize a document session to use in your Azure functions.  

Example `db.js` Node module:

{CODE-BLOCK:javascript}
import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore(
  ["https://a.free.mycompany.ravendb.cloud"],
  "demo",
  // authOptions
};

var initialized = false;

function initialize() {
    if (initialized) return;
    documentStore.initialize();
    initialized = true;
}

export function openAsyncSession() {
    if (!initialized) {
        initialize();
    }

    return documentStore.openAsyncSession();
}
{CODE-BLOCK/}

For more on what options are available, see [Creating a Document Store][docs-creating-document-store].  

{NOTE: Warm vs. Cold Starts}
In Azure Functions, the instance will be shared across function invocations if the Function is warmed up, 
otherwise it will be constructed each time the function warms up. For more, see [Deployment Considerations][deployment-considerations].  
{NOTE/}

You can set options manually but it's more likely you'll want to configure support for app settings.  

{PANEL/}

{PANEL: Adding Support for App Settings}

You will need a way to pass options to the `DocumentStore` on your local machine and when deployed to Azure.  

Node.js Azure Functions support a `local.settings.json` file which you can use to add additional settings locally. For example:  

{CODE-BLOCK:json}
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "",
    "FUNCTIONS_WORKER_RUNTIME": "node",
    "DB_URLS": "https://a.free.company.ravendb.cloud",
    "DB_NAME": "demo",
    "DB_CERT_PATH": "../certs/company.client.certificate.pfx"
  }
}
{CODE-BLOCK/}

You can then load environment variables through `process.env`:  

{CODE-BLOCK:javascript}
import { readFileSync } from "fs";
import { DocumentStore } from "ravendb";

var documentStore;
var initialized = false;

function initialize() {
    if (initialized) return;

    const authOptions = {
      type: "pfx",
      // Read .pfx file using fs.readFileSync
      certificate: readFileSync(process.env.DB_CERT_PATH)
    };

    documentStore = new DocumentStore(
      process.env.DB_URLS.split(","), // Split by "," separator
      process.env.DB_NAME,
      authOptions
    };
    documentStore.initialize();

    initialized = true;
}

export function openAsyncSession() {
    if (!initialized) {
        initialize();
    }

    return documentStore.openAsyncSession();
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Configuring Support for Certificates}

RavenDB uses client certificate authentication (mutual TLS) to secure your database connection.  
The Node.js client SDK supports `.pfx` files or `.pem` files which is passed to the `authOptions.certificate` option.  
There are multiple ways to load a certificate:  

- Load from .pfx files  
- Load from PEM-encoded certificate  
- Load from Azure Key Vault  

### Load from .pfx Files

You can load PFX files with or without a password by providing the certificate buffer using `authOptions.certificate`:  

{CODE-BLOCK:javascript}
const authOptions = {
    type: "pfx",
    // Read .pfx file using fs.readFileSync
    certificate: readFileSync("../cert/company.client.certificate.pfx"),
    // Optionally provide the password
    password: "<CERT_PASSWORD>"
};

documentStore = new DocumentStore(
    ["https://a.free.company.ravendb.cloud"],
    "demo",
    authOptions
};
documentStore.initialize();
{CODE-BLOCK/}

If the `.pfx` file requires a password, provide it using `password` option.  
However, keep in mind that using an absolute physical file path or a password 
requires manual steps for every developer working on a project to configure.  

{WARNING: Avoid uploading or deploying .pfx files}
PFX files can be compromised, especially if they are not password-protected.  
Using a physical file also makes it hard to manage and rotate when they expire.  
They are only recommended for ease-of-use on your local machine.  
For production, it is better to use the Certificate Store method or Azure Key Vault.  
{WARNING/}

### Load from PEM-encoded certificate

For Node.js-based Azure Functions, it's recommended to use a PEM-encoded certificate that 
can be provided through Azure app settings without deploying any files.  

Unlike a `.pfx` file, a PEM-encoded certificate is plain-text encoded:  

{CODE-BLOCK:plain}
-----BEGIN CERTIFICATE-----
MIIFCzCCAvO...
-----END CERTIFICATE-----
-----BEGIN RSA PRIVATE KEY-----
MIIJKAIBAAK...
-----END RSA PRIVATE KEY-----
{CODE-BLOCK/}

To pass a PEM-encoded certificate, you can read an environment variable like `DB_CERT_PEM` and 
set `authOptions` using the `pem` certificate type:  

{CODE-BLOCK:javascript}
const authOptions = {
    type: "pem",
    certificate: process.env.DB_CERT_PEM
};

documentStore = new DocumentStore(
    ["https://a.free.company.ravendb.cloud"],
    "demo",
    authOptions
};
documentStore.initialize();
{CODE-BLOCK/}

{WARNING: Normalizing PEM certificates}
Be aware that the Azure portal removes line endings and you will need to manually normalize 
the value for PEM parsing to succeed. If you are setting the value in the `local.settings.json` 
file, you will need to format the value for JSON using [a stringify tool][tool-stringify].  
{WARNING/}

Here is how the starter template adds support for loading certificates using a `DB_CERT_PEM` environment variable:  

{CODE-BLOCK:javascript}
import { EOL } from "os";
import { readFile } from "fs/promises";
import { DocumentStore } from "ravendb";

let store;
let initialized = false;

export async function initializeDb({
  urls,
  databaseName,
  dbCertPassword,
  dbCertPath,
  dbCertPem,
  customize,
}) {
  if (initialized) return;

  let authOptions = undefined;

  if (dbCertPath) {
    authOptions = await getAuthOptionsFromCertificatePath(
      dbCertPath,
      dbCertPassword
    );
  } else if (dbCertPem) {
    authOptions = getAuthOptionsFromCertPem(dbCertPem);
  }

  store = new DocumentStore(urls, databaseName, authOptions);

  if (customize) {
    customize(store.conventions);
  }

  store.initialize();

  initialized = true;

  return store;
}

async function getAuthOptionsFromCertificatePath(
  dbCertPath,
  dbCertPassword
) {
  return {
    certificate: await readFile(dbCertPath),
    password: dbCertPassword,
    type: "pfx",
  };
}

function getAuthOptionsFromCertPem(dbCertPem) {
  let certificate = dbCertPem;
  const isMissingLineEndings = !dbCertPem.includes(EOL);

  if (isMissingLineEndings) {
    // Typically when pasting values into Azure env vars
    certificate = normalizePEM(certificate);
  }

  return {
    certificate,
    type: "pem",
  };
}

function normalizePEM(pem: string): string {
  return pem.replace(PEM_REGEX, (match, certSection, certSectionBody) => {
    const normalizedCertSectionBody = certSectionBody.replace(/\s/g, EOL);
    return `-----BEGIN ${certSection}-----${EOL}${normalizedCertSectionBody.trim()}${EOL}-----END ${certSection}-----${EOL}`;
  });
}

const PEM_REGEX =
  /-----BEGIN ([A-Z\s]+)-----(\s?[A-Za-z0-9+\/=\s]+?\s?)-----END \1-----/gm;

export function openDbSession(opts) {
  if (!initialized)
    throw new Error(
      "DocumentStore is not initialized yet. Must `initializeDb()` before calling `openDbSession()`."
    );
  return store.openSession(opts);
}

{CODE-BLOCK/}

This supports using `.pfx` files or a PEM-encoded certificate, if provided.  
It normalizes the PEM value if it does not contain line endings.

### Load from Azure Key Vault

[Azure Key Vault][ms-az-key-vault] is a paid service that allows you to store, retrieve, and rotate 
encrypted secrets including X.509 Certificates. This is recommended for more robust certificate handling.  

Using the [SecretsClient][ms-az-key-vault-secrets-client], you can load secrets from Key Vault.  
However, you will need to use the [CertificateClient][ms-az-key-vault-cert-client] to retrieve a certificate from the vault.  

For more, see the [sample code for using CertificateClient][ms-az-key-vault-sample-get].  

{PANEL/}

{PANEL: Configuring Azure}

You will need to configure certificate authentication in Azure. Depending on the method you choose above, the steps vary.  

### Specifying Path to Certificate

If you are deploying a physical `.pfx` file, you can specify the `DB_CERT_PATH` and `DB_PASSWORD` app settings.  

### Specifying PEM Certificate

If you are loading a PEM-encoded certificate, follow the steps below to make your `.pem` certificate available to your Azure Functions:  

![.NET update Azure app settings](images/js-azure-app-settings.jpg ".NET update Azure app settings")

1. Find the `.pem` certificate provided by RavenDB client certificate package  
1. Copy its full contents  
1. Go to your Azure Functions dashboard in the Portal  
1. Click the Application Settings menu  
1. Modify or add the app setting for `DB_CERT_PEM` and paste the contents of your `.pem` file  

These values will override `local.settings.json` once deployed on Azure.  

{PANEL/}

{PANEL: Next Steps}

- Learn more about [how to use the RavenDB Node.js client SDK][docs-nodejs]  
- Reference the [Node.js Azure Function starter template][gh-ravendb-template] to see the code  
- [Troubleshoot][troubleshooting] issues with RavenDB and Azure Functions  
- [Deployment Considerations][deployment-considerations] for RavenDB and Azure Functions  

{PANEL/}

[troubleshooting]: ../../../start/guides/azure-functions/troubleshooting
[deployment-considerations]: ../../../start/guides/azure-functions/deployment
[cloud-signup]: https://cloud.ravendb.net?utm_source=ravendb_docs&utm_medium=web&utm_campaign=howto_template_azurefns_nodejs_existing&utm_content=cloud_signup
[nodejs]: https://nodejs.org
[npm-ravendb-client]: https://npmjs.com/package/ravendb
[docs-nodejs]: ../../../client-api/session/what-is-a-session-and-how-does-it-work
[docs-creating-document-store]: ../../../client-api/creating-document-store
[gh-ravendb-template]: https://github.com/ravendb/templates/tree/main/azure-functions/node-http
[az-funcs]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-get-started
[az-core-tools]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local
[ms-az-key-vault]: https://learn.microsoft.com/en-us/azure/key-vault/
[ms-az-key-vault-secrets-client]: https://learn.microsoft.com/en-us/javascript/api/overview/azure/keyvault-secrets-readme?view=azure-node-latest
[ms-az-key-vault-cert-client]: https://learn.microsoft.com/en-us/javascript/api/overview/azure/keyvault-certificates-readme?view=azure-node-latest
[ms-az-key-vault-sample-get]: https://github.com/Azure/azure-sdk-for-js/blob/30c703fa2179831d330201bdb0fff5ac6c0a8b57/sdk/keyvault/keyvault-certificates/samples/v4/javascript/helloWorld.js
[tool-stringify]: https://onlinestringtools.com/json-stringify-string

## Related Articles

### Azure

- [Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-get-started)  
- [Core Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local)  
- [Deploy](https://learn.microsoft.com/en-us/azure/azure-functions/functions-deployment-technologies)  

### RavenDB

- [Get Started](../../../start/getting-started)  
- [Client Certificates](../../../client-api/setting-up-authentication-and-authorization)  
- [Session](../../../client-api/session/what-is-a-session-and-how-does-it-work)  
- [Cloud Signup](https://cloud.ravendb.net?utm_source=ravendb_docs&utm_medium=web&utm_campaign=howto_template_azurefns_dotnet&utm_content=cloud_signup)  
- [Azure Functions: Overview](../../../start/guides/azure-functions/overview)  
