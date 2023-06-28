# Add RavenDB to an Existing Azure Functions Project (.NET C#)
---

{NOTE: What are Azure Functions?}
Microsoft Azure Functions are a serverless platform that supports multiple languages and frameworks that let you deploy workloads that scale without managing any infrastructure.

Learn more about [how Microsoft Azure Functions work][az-funcs].

<!-- ### New to Azure Functions?

For a walkthrough and demo of getting started with Azure Functions, see [TBD](#). -->
{NOTE/}

In this guide, you will learn how to connect to RavenDB from your existing C# Azure Functions. This guide assumes you are familiar with .NET development techniques and the basics of Azure Function apps.

On this page:

* [Before We Get Started](#before-we-get-started)
* [Installing the RavenDB Client SDK](#installing-the-ravendb-client-sdk)
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

For a brand new Azure Functions app, we recommend using the [RavenDB Azure Functions .NET template](overview) which is set up with dependency injection and X.509 certificate support. You can also reference the template to see how the integration is set up.

{PANEL/}

{PANEL: Installing the RavenDB Client SDK}

The .NET SDK is provided through the RavenDB.Client Nuget package and will need to be added to your solution or project:

{CODE-BLOCK:bash}
dotnet add package RavenDB.Client
{CODE-BLOCK/}

{PANEL/}


{PANEL: Initializing the Document Store}

Import the `DocumentStore` from `RavenDB.Documents` namespace to create a new instance with the required configuration and initialize your connection to RavenDB by calling the `Initialize` method.

Rather than creating an instance within Azure Function methods, it's recommended to use .NET Core dependency injection. 

The easiest way is to use the community Nuget package RavenDB.DependencyInjection:

{CODE-BLOCK:bash}
dotnet add package RavenDB.DependencyInjection
{CODE-BLOCK/}

The pattern to set up dependency injection to inject an `IDocumentStore` with Azure Functions differs depending on whether your C# functions are running [in-process][az-func-di-ip] or [out-of-process][az-func-di-oop]. Follow whichever guide pertains to your environment.

The resulting service configuration will look like this:

{CODE-BLOCK:csharp}
services.AddRavenDbDocumentStore();
{CODE-BLOCK/}

You can pass an options builder inline function to customize the options before they get passed down to the underlying `DocumentStore` with an overload:

{CODE-BLOCK:csharp}
services.AddRavenDbDocumentStore(options => {
    // ...
    // Customize `options`
    // ...

    options.Conventions.UseOptimisticConcurrency = true;
});
{CODE-BLOCK/}

{NOTE: Warm vs. Cold Starts}
In Azure Functions, the instance will be shared across function invocations if the Function is warmed up, otherwise it will be constructed each time the function warms up. For more, see [Deployment Considerations](deployment-considerations)
{NOTE/}

{PANEL/}

{PANEL: Adding Support for App Settings}
You will need a way to pass options to the `DocumentStore` on your local machine and when deployed to Azure.

The RavenDB.DependencyInjection package supports reading settings from `appsettings.json` for ASP.NET applications but Azure Function apps require some manually setup. To support Azure app settings, you will also need to add support to override those settings through environment variables using XXX Nuget package.

Within your `FunctionsStartup` class, override the `ConfigureAppConfiguration` method to customize how configuration is read:

{CODE-BLOCK:csharp}
public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
  {
    FunctionsHostBuilderContext context = builder.GetContext();

    builder.ConfigurationBuilder
        // Add support for appsettings.json
        .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
        // Add support for appsettings.ENV.json
        .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
        // Allow environment variables to override
        .AddEnvironmentVariables();
  }
{CODE-BLOCK/}

### Using JSON settings

An example `appsettings.json` file may look like this:

{CODE-BLOCK:json}
{
    "RavenSettings": {
        "Urls": ["https://a.free.company.ravendb.cloud"],
        "DatabaseName": "demo"
    }
}
{CODE-BLOCK/}

### Using environment variables

Environment variables follow the .NET conventions with `__` being the dot-notation separator (e.g. `RavenSettings__DatabaseName`).

You can pass environment variables in your terminal profile, OS settings, Docker `env`, on the command-line, or within Azure.

{PANEL/}

{PANEL: Configuring Support for Certificates}

{PANEL/}

{PANEL: Configuring Azure}
At this point, the local Function app is ready to be deployed. Before you can do that, you need to set up the Function App resources in Azure first.

The template includes an ARM deployment option using the **Deploy to Azure** button. This will open the Azure Portal and walkthrough creating a default Function App with the required resources and app settings.

Follow the guide of your choice in the Microsoft docs. Once the app is created, come back here to finish configuring your database connection.

### Upload Your Client Certificate (.pfx)

Once the app is created in the portal, follow these steps to upload the client certificate and make it accessible to your Function.

![.NET upload certificate to Azure](images/dotnet-azure-upload-cert.jpg)

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

![.NET update Azure app settings](images/dotnet-azure-app-settings.jpg)

1. Go to your Azure Functions dashboard in the Portal
1. Click the Application Settings menu
1. Modify or add app setting for `WEBSITE_LOAD_CERTIFICATES` to the certificate thumbprint you copied
    - ![.NET WEBSITE_LOAD_CERTIFICATES example](images/dotnet-azure-website-load-certificates.jpg)
1. Modify or add app setting for `RavenSettings__CertThumbprint` with the certificate thumbprint you copied
    - ![.NET WEBSITE_LOAD_CERTIFICATES example](images/dotnet-azure-ravensettings__certthumbprint.jpg)
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
    - ![download Azure publish profile](images/azure-download-publish-profile.jpg)
1. Download the publish profile
1. Open it and copy the full XML
1. Go to your [GitHub repository's secrets settings][gh-secrets]
    - ![add GitHub secret for publish profile](images/github-publish-profile-secret.jpg)
1. Add a new secret: `AZURE_FUNCTIONAPP_PUBLISH_PROFILE`
1. Paste in the value of the publish profile

### Trigger a Deployment

Your repository and GitHub action is now set up. To test the deployment, you can push a commit to the repository.

If you have already committed and pushed, it is likely that the Action failed and you can re-run the job using the new secret variable.

{PANEL/}

{PANEL: Verify the Connection Works}

If the deployment succeeds, the `HttpTrigger` endpoint should now be available at your Function URL.

Once you open the URL in the browser, you should see a welcome screen like this with the connection information:

![.NET Azure Function welcome connected screen](images/dotnet-azure-func-success.jpg)

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
[az-deploy]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-deployment-technologies
[template]: https://github.com/ravendb/templates/tree/main/azure-functions/csharp-http
[gh-secrets]: https://docs.github.com/en/actions/security-guides/encrypted-secrets
[cloud-signup]: https://cloud.ravendb.net?utm_source=ravendb_docs&utm_medium=web&utm_campaign=howto_template_azurefns_dotnet&utm_content=cloud_signup
[docs-get-started]: /docs/article-page/csharp/start/getting-started
[docs-client-certs]: /docs/article-page/csharp/client-api/setting-up-authentication-and-authorization
[ravendb-dotnet]: /docs/article-page/csharp/client-api/session/what-is-a-session-and-how-does-it-work
[tool-degit]: https://npmjs.com/package/degit


[az-func-di-ip]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
[az-func-di-oop]: https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide#dependency-injection
[docs-creating-document-store]: /docs/article-page/csharp/client-api/creating-document-store
