# Azure Functions (.NET C#)
---

{NOTE: What are Azure Functions?}
Microsoft Azure Functions are a serverless platform that supports multiple languages and frameworks that let you deploy workloads that scale without managing any infrastructure.

Learn more about [how Microsoft Azure Functions work][az-funcs].

### New to Azure Functions?

For a walkthrough and demo of getting started with Azure Functions, see [TBD](#).
{NOTE/}

In this guide, you will learn how to deploy a C# Azure Function using the [RavenDB Azure Functions C# template][template] that is connected to your RavenDB database. This guide assumes you are familiar with .NET development techniques and the basics of Azure Function apps.

On this page:

* [Before We Get Started](#before-we-get-started)
* [Create a Local Azure Function App](#create-a-local-azure-function-app)
* [Configuring Local Connection to RavenDB](#configuring-local-connection-to-ravendb)
* [Creating Function App Resources in Azure](#creating-function-app-resources-in-azure)
* [Deploying to Azure](#deploying-to-azure)
* [Verify the Connection Works](#verify-the-connection-works)
* [Using RavenDB in the Azure Functions App](#using-ravendb-in-the-azure-functions-app)

{PANEL: Before We Get Started}

You will need the following before continuing:

- A [RavenDB Cloud][cloud-signup] account or self-hosted client certificate
- [Azure Function Core Tools][az-core-tools] 4.x+
- [Git](https://git-scm.org)
- [.NET 6.x][download-dotnet]

If you are new to Azure Function local development, see the [Getting started guide][az-funcs] for how to get up and running with your toolchain of choice.

{PANEL/}

{PANEL: Create a Local Azure Function App}

The [RavenDB Azure Function template][template] is a template repository on GitHub which means you can either create a new repository derived from the template or clone and push it to a new repository.

This will set up a local Azure Function app that we will deploy to your Azure account at the end of the guide.

### Creating a New Repository from the Template

1. Open the template in GitHub
1. Click the green "Use this template" button
1. Click "In a new repository"

GitHub will walk you through creating a new repository in your account or organization which you can clone.

### Cloning the Repository

Clone the repository with Git on your machine:

{CODE-BLOCK:bash}
$ git clone <GIT_CLONE_URL> my-project
$ cd my-project
{CODE-BLOCK/}

Replace `<GIT_CLONE_URL>` with the repository you are cloning, either the original template or your newly derived repository.

### Install Dependencies

After cloning the repository locally, restore .NET dependencies with `dotnet`:

`dotnet restore`

By default, the template is configured to connect to the Live Test instance of RavenDB and the Northwind database. Since this is only for testing purposes, next you will configure the app to connect to your existing RavenDB database.

### Starting the Function

You can start the Azure Function locally using:

`func start`

If you are using Visual Studio Code, you can also debug the function with F5 debugging.

{PANEL/}


{PANEL: Configuring Local Connection to RavenDB}

To configure the local version of your Azure Functions app to connect to RavenDB, you will need to update the `appsettings.json` file with the `RavenSettings:Urls` value and `RavenSettings:DatabaseName` value. The default is:

{CODE-BLOCK:json}
{
  "RavenSettings": {
    "Urls": ["http://live-test.ravendb.net"],
    "DatabaseName": "Northwind"
  }
}
{CODE-BLOCK/}

If using an authenticated RavenDB URL, you will need a local client certificate installed. Learn more about [configuring client authentication for RavenDB][docs-client-certs].

### Certificate Path and Password (Windows and Linux)

To specify the path to a `.pfx` file, specify a relative or absolute file path using `RavenSettings:CertFilePath`.

To specify a PFX password, use the .NET User Secrets tool to add a secret locally:

{CODE-BLOCK:bash}
dotnet user-secrets init
dotnet user-secrets set RavenSettings:CertPassword "<CERT_PASSWORD>"
{CODE-BLOCK/}

Replace `<CERT_PASSWORD>` with your PFX password.

Example `appsettings.json`:

{CODE-BLOCK:json}
{
  "RavenSettings": {
    "Urls": ["https://a.free.mycompany.ravendb.cloud"],
    "DatabaseName": "company_db",
    "CertFilePath": "a.free.mycompany.ravendb.cloud.with.password.pfx"
  }
}
{CODE-BLOCK/}

### Certificate Thumbprint (Windows Only)

You can also specify a certificate to use from the `CurrentUser\My` Windows certificate store by setting `RavenSettings:CertThumbprint`.

Example `appsettings.json`:

{CODE-BLOCK:json}
{
  "RavenSettings": {
    "Urls": ["https://a.free.mycompany.ravendb.cloud"],
    "DatabaseName": "company_db",
    "CertThumbprint": "<CERT_THUMBPRINT>"
  }
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Creating Function App Resources in Azure}
At this point, the local Function app is ready to be deployed. Before you can do that, you need to set up the Function App resources in Azure first.

The template includes an ARM deployment option using the **Deploy to Azure** button. This will open the Azure Portal and walkthrough creating a default Function App with the required resources and app settings.

Follow the guide of your choice in the Microsoft docs. Once the app is created, come back here to finish configuring your database connection.

### Upload Your Client Certificate (.pfx)

Once the app is created in the portal, follow these steps to upload the client certificate and make it accessible to your Function.

1. Go to your Azure Functions dashboard in the Portal
1. Click "Certificates"
1. Click the "Bring Your Own Certificate" tab
1. Click "+ Add Certificate" button
1. Upload the RavenDB client certificate (PFX) file
1. Enter the certificate password
1. Once uploaded, click the certificate to view details
1. Copy the "Thumbprint" for the next step

{WARNING: Do not store certificate password}
The Azure portal will only use the certificate password once on upload. You will not need the password in your Functions App, only the public thumbprint. You can safely delete the password from your device once the certificate is uploaded in the Portal so as not to risk it being discovered.
{WARNING/}

### Configure Application Settings

1. Go to your Azure Functions dashboard in the Portal
1. Click the Application Settings menu
1. Modify or add app setting for `WEBSITE_LOAD_CERTIFICATES` to the certificate thumbprint you copied
1. Modify or add app setting for `RavenSettings__CertThumbprint` with the certificate thumbprint you copied
1. Modify or add app setting for `RavenSettings__Urls` with the comma-separated list of RavenDB node URLs to connect to
1. Modify or add an app setting for `RavenSettings__DatabaseName` with the database name to connect to

These values will override `appsettings.json` once deployed on Azure.

{NOTE: Loading multiple certificates}
`WEBSITE_LOAD_CERTIFICATES` makes any specified certificates available in the Windows Certificate Store under the `CurrentUser\My` location. You can use the wildcard value `*` for `WEBSITE_LOAD_CERTIFICATES` to load ALL uploaded certificates for your Function App. However, it's recommended to be specific and use comma-separated thumbprints so that only allowed certificates are made available. This avoids accidentally exposing a certificate to the application that isn't explicitly used.
{NOTE/}

{PANEL/}

{PANEL: Deploying to Azure}
Once the Azure app is set up in the portal, you are ready to deploy your app. There are 3 main ways to deploy your new Azure Function app: GitHub actions, command-line, and an extension.

The template has already been set up to use continuous deployment using GitHub Actions. For the other methods, see [Deploying Azure Function apps][az-deploy].

### Configure GitHub Secrets

The GitHub actions rely on having a secret environment variable `AZURE_FUNCTIONAPP_PUBLISH_PROFILE` in your repository secrets.

1. Go to your Azure Functions dashboard in the Azure Portal
1. Click "Get Publish Profile"
1. Download the publish profile
1. Open it and copy the full XML
1. Go to your [GitHub repository's secrets settings][gh-secrets]
1. Add a new secret: `AZURE_FUNCTIONAPP_PUBLISH_PROFILE`
1. Paste in the value of the publish profile

### Trigger a Deployment

Your repository and GitHub action is now set up. To test the deployment, you can push a commit to the repository.

If you have already committed and pushed, it is likely that the Action failed and you can re-run the job using the new secret variable.

{PANEL/}

{PANEL: Verify the Connection Works}

If the deployment succeeds, the `HttpTrigger` endpoint should now be available at your Function URL.

Once you open the URL in the browser, you should see a message like this:

`Connected successfully to RavenDB - Node A`

This means your Azure Functions app is correctly configured and ready to work with RavenDB.

{PANEL/}

{PANEL: Using RavenDB in the Azure Functions App}

The template sets up a singleton `DocumentStore` and dependency injection for the `IAsyncDocumentStore` per function invocation which you can inject into Function classes.

### Example: Injecting `IAsyncDocumentSession`

Pass the `IAsyncDocumentSession` in a function class constructor to make it available to trigger functions:

{CODE-BLOCK:csharp}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
using System.Threading.Tasks;

namespace Company.FunctionApp
{
  public class HttpTrigger
  {
    private readonly IAsyncDocumentSession session;

    public HttpTrigger(IAsyncDocumentSession session)
    {
      this.session = session;
    }

    [FunctionName("HttpTrigger")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
      // Access `session` within the body of the function
    }
  }
}

{CODE-BLOCK/}

You can also inject an `IDocumentStore` to get a reference to the current store instance.

### Example: Loading a user

{CODE-BLOCK:csharp}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
using System.Threading.Tasks;

namespace Company.FunctionApp
{
  public class HttpTrigger
  {
    private readonly IAsyncDocumentSession session;

    public HttpTrigger(IAsyncDocumentSession session)
    {
      this.session = session;
    }

    [FunctionName("HttpTrigger")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "{id:int}")] int id,
        ILogger log)
    {
      log.LogInformation("C# HTTP trigger function processed a request.");

      var user = await session.Load<object>("users/" + id);

      return new OkObjectResult(user);
    }
  }
}
{CODE-BLOCK/}

Learn more about [how to use the RavenDB .NET client SDK][ravendb-dotnet]

{PANEL/}

[download-dotnet]: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
[az-funcs]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-get-started
[az-core-tools]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local
[az-deploy]: https://foo
[template]: https://github.com/ravendb/template-azure-functions-csharp
[gh-secrets]: https://docs.github.com/en/actions/security-guides/encrypted-secrets
[cloud-signup]: https://cloud.ravendb.net?utm_source=ravendb_docs&utm_medium=web&utm_campaign=howto_template_azurefns_nodejs&utm_content=cloud_signup
[docs-get-started]: /docs/article-page/csharp/start/getting-started
[docs-client-certs]: /docs/article-page/csharp/client-api/setting-up-authentication-and-authorization
[ravendb-dotnet]: /docs/article-page/csharp/client-api/session/what-is-a-session-and-how-does-it-work
