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
* [Initializing the Document Store](#initializing-the-document-store)
* [Adding Support for App Settings](#adding-support-for-app-settings)
* [Configuring Support for Certificates](#configuring-support-for-certificates)
* [Configuring Azure](#configuring-azure)

{PANEL: Before We Get Started}

You will need the following before continuing:

- A [RavenDB Cloud][cloud-signup] account or self-hosted client certificate
- [Azure Function Core Tools][az-core-tools] 4.x+
- [.NET 6.x][ms-download-dotnet]

{NOTE: Starting from scratch?}
For a brand new Azure Functions app, we recommend using the [RavenDB Azure Functions .NET template](overview) which is set up with dependency injection and X.509 certificate support. You can also reference the template to see how the integration is set up.
{NOTE/}

{PANEL/}

{PANEL: Installing the RavenDB Client SDK}

Get started by installing the [RavenDB.Client][nuget-ravendb-client] Nuget package in your solution or project which provides the .NET client SDK.

Using the .NET CLI:

{CODE-BLOCK:bash}
dotnet add package RavenDB.Client
{CODE-BLOCK/}

{PANEL/}

{PANEL: Initializing the Document Store}

Import the `DocumentStore` from `Raven.Client.Documents` namespace to create a new instance with the required configuration and initialize your connection to RavenDB by calling the `Initialize` method.

{CODE-BLOCK:csharp}
using Raven.Client.Documents;

var documentStore = new DocumentStore() {
  Urls = new [] { "https://a.free.mycompany.ravendb.cloud" },
  DatabaseName = "demo",
  // Other options
};
documentStore.Initialize();
{CODE-BLOCK/}

For more on what options are available, see [Creating a Document Store][docs-creating-document-store].

### Set up dependency injection

For Azure Function methods, it's recommended to configure the document store and document sessions with .NET dependency injection. The easiest way is to use the community Nuget package [RavenDB.DependencyInjection][nuget-ravendb-di]:

{CODE-BLOCK:bash}
dotnet add package RavenDB.DependencyInjection
{CODE-BLOCK/}

The pattern to set up dependency injection to inject an `IAsyncDocumentSession` with Azure Functions differs depending on whether your C# functions are running:

- Follow the [in-process DI guide][az-func-di-ip] for C# class library functions
- Follow the [out-of-process DI guide][az-func-di-oop] for .NET isolated functions

Once set up with the appropriate configuration, add a using statement for `Raven.DependencyInjection` which exposes two extension methods:

- `IServiceCollection.AddRavenDbDocStore`
- `IServiceCollection.AddRavenDbAsyncSession`

The resulting service configuration will look like this:

{CODE-BLOCK:csharp}
// Requires a using statement
using Raven.DependencyInjection;

// Configure injection for IDocumentStore
services.AddRavenDbDocStore();

// Configure injection for IAsyncDocumentSession
services.AddRavenDbAsyncSession();
{CODE-BLOCK/}

You can customize the options before they get passed down to the underlying `DocumentStore` with an overload:

{CODE-BLOCK:csharp}
services.AddRavenDbDocStore(options => {
    // ...
    // Customize `options`
    // ...

    options.Conventions.UseOptimisticConcurrency = true;
});
{CODE-BLOCK/}

{NOTE: Warm vs. Cold Starts}
In Azure Functions, the instance will be shared across function invocations if the Function is warmed up, otherwise it will be constructed each time the function warms up. For more, see [Deployment Considerations](deployment-considerations)
{NOTE/}

You can set options manually but it's more likely you'll want to configure support for app settings.

{PANEL/}

{PANEL: Adding Support for App Settings}

You will need a way to pass options to the `DocumentStore` on your local machine and when deployed to Azure.

The RavenDB.DependencyInjection package supports reading settings from `appsettings.json` for ASP.NET applications but Azure Function apps require some manually setup. To support Azure app settings, you will also need to add support to override those settings through environment variables by using `Microsoft.Extensions.Configuration`.

Within your `FunctionsStartup` class, override the `ConfigureAppConfiguration` method to customize how configuration is read.

Here's an example startup file for an in-process C# Azure Function app:

{CODE-BLOCK:csharp}
using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Raven.DependencyInjection;

[assembly: FunctionsStartup(typeof(Company.FunctionApp.Startup))]

namespace Company.FunctionApp;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddRavenDbDocStore();
        builder.Services.AddRavenDbAsyncSession();
    }

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
}
{CODE-BLOCK/}

For more on the different configuration providers supported, see [Configuration in ASP.NET Core][ms-docs-aspnet-configuration].

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

RavenDB uses client certificate authentication (mutual TLS) to secure your database connection. The .NET Client SDK supports `X509Certificate2` which is passed to the `DocumentStore.Certificate` option. There are multiple ways to load a certificate:

- Load from .pfx files
- Load from Certificate Store by thumbprint
- Load from Azure Key Vault

### Load from .pfx Files

You can load PFX files with or without a password by providing the certificate path using `RavenSettings:CertFilePath`:

{CODE-BLOCK:json}
{
    "RavenSettings": {
        "Urls": ["https://a.free.company.ravendb.cloud"],
        "DatabaseName": "demo",
        "CertFilePath": "..\\shared\\certs\\company.client.certificate.pfx"
    }
}
{CODE-BLOCK/}

The dependency injection logic will automatically load the certificate from this path without extra code.

If the `.pfx` file requires a password, provide it using the .NET secrets tool by setting `RavenSettings:CertPassword`:

{CODE-BLOCK:bash}
dotnet secrets init
dotnet secrets add "RavenSettings:CertPassword" "<CertPassword>"
{CODE-BLOCK/}

However, keep in mind that using an absolute physical file path or a user secret requires manual steps for every developer working on a project to configure.

{WARNING: Avoid uploading or deploying .pfx files}
PFX files can be compromised, especially if they are not password-protected. Using a physical file also makes it hard to manage and rotate when they expire. They are only recommended for ease-of-use on your local machine. For production, it is better to use the Certificate Store method or Azure Key Vault.
{WARNING/}

### Load from Certificate Store by Thumbprint

For .NET-based Azure Functions, it's recommended to use the Windows Certificate Store since you can upload a password-protected .pfx file to the Azure Portal and load it programmatically without deploying any files.

On your local machine, you can import a certificate on Windows by right-clicking the `.pfx` file and adding to your Current User store (`CurrentUser\My`):

![Windows certificate import wizard](images/dotnet-certificate-install.jpg)

The certificate thumbprint is displayed in the details when viewing the certificate information:

![Windows certificate thumbprint](images/dotnet-certificate-thumbprint.jpg)

You can also install and view certificates using PowerShell through the [Import-PfxCertificate][ms-powershell-import-pfxcert] and [Get-Certificate][ms-powershell-get-cert] cmdlets.

To specify the thumbprint you can add a new `RavenSettings:CertThumbprint` setting:

{CODE-BLOCK:json}
{
  "RavenSettings": {
    "Urls": ["https://a.free.mycompany.ravendb.cloud"],
    "DatabaseName": "company_db",
    "CertThumbprint": "<CERT_THUMBPRINT>"
  }
}
{CODE-BLOCK/}

Update your `DocumentStore` initialization to load the certificate by its thumbprint using the `IConfiguration.GetSection` method to retrieve it when building options. The [X509Store][ms-docs-x509store] can be used to find certificates by thumbprint. In Azure, certificates will be stored in the `CurrentUser\My` cert store.

Here is how the starter template adds support for loading certificates by thumbprint:

{CODE-BLOCK:csharp}
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Raven.DependencyInjection;

[assembly: FunctionsStartup(typeof(Company.FunctionApp.Startup))]

namespace Company.FunctionApp;

public class Startup : FunctionsStartup
{
  public override void Configure(IFunctionsHostBuilder builder)
  {
    var context = builder.GetContext();

    builder.Services.AddRavenDbDocStore(options =>
  {
    var certThumbprint = context.Configuration.GetSection("RavenSettings:CertThumbprint").Value;

    if (!string.IsNullOrWhiteSpace(certThumbprint))
    {
      var cert = GetRavendbCertificate(certThumbprint);

      options.Certificate = cert;
    }
  });

    builder.Services.AddRavenDbAsyncSession();
  }

  private static X509Certificate2 GetRavendbCertificate(string certThumb)
  {
    X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
    certStore.Open(OpenFlags.ReadOnly);

    X509Certificate2Collection certCollection = certStore.Certificates
        .Find(X509FindType.FindByThumbprint, certThumb, false);

    // Get the first cert with the thumbprint
    if (certCollection.Count > 0)
    {
      X509Certificate2 cert = certCollection[0];
      return cert;
    }

    certStore.Close();

    throw new Exception($"Certificate {certThumb} not found.");
  }
}
{CODE-BLOCK/}

This will only load by thumbprint if it is specified, meaning that you can still load by a physical `.pfx` path locally if you choose. On Azure, follow the steps below to upload a certificate.

### Load from Azure Key Vault

[Azure Key Vault][ms-az-key-vault] is a paid service that allows you to store, retrieve, and rotate encrypted secrets including X.509 Certificates. This is recommended for more robust certificate handling. 

Using the [Azure Key Vault configuration provider][ms-az-key-vault-configuration], you can load `RavenSettings` from Key Vault. However, you will need to use the [CertificateClient][ms-az-key-vault-cert-client] to retrieve a certificate from the vault.

For more, see the [sample code for using CertificateClient][ms-az-key-vault-sample-get].

{PANEL/}

{PANEL: Configuring Azure}

You will need to configure certificate authentication in Azure. Depending on the method you choose above, the steps vary.

### Specifying Path to Certificate

If you are deploying a physical `.pfx` file, you can specify the `RavenSettings__CertFilePath` and `RavenSettings__CertPassword` app settings.

### Upload Your Client Certificate (.pfx)

If you are loading a certificate by its thumbprint from the Certificate Store, follow the steps below to make your uploaded `.pfx` certificate available to your Azure Functions:

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

{WARNING: Not Supported on Linux-Based Consumption Plans}
The `WEBSITE_LOAD_CERTIFICATES` setting is not supported yet for Linux-based consumption plans. To use this method, you will need to use a Windows-based plan.
{WARNING/}

{PANEL/}

{PANEL: Next Steps}

- Learn more about [how to use the RavenDB .NET client SDK][docs-dotnet]
- Reference the [.NET Azure Function starter template][gh-ravendb-template] to see the code
- [Troubleshoot](troubleshooting) issues with RavenDB and Azure Functions
- [Deployment Considerations](deployment-considerations) for RavenDB and Azure Functions

{PANEL/}

[cloud-signup]: https://cloud.ravendb.net?utm_source=ravendb_docs&utm_medium=web&utm_campaign=howto_template_azurefns_dotnet_existing&utm_content=cloud_signup
[docs-dotnet]: /docs/article-page/csharp/client-api/session/what-is-a-session-and-how-does-it-work
[docs-creating-document-store]: /docs/article-page/csharp/client-api/creating-document-store
[gh-ravendb-template]: https://github.com/ravendb/templates/tree/main/azure-functions/csharp-http
[az-funcs]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-get-started
[az-core-tools]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local
[az-func-di-ip]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
[az-func-di-oop]: https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide#dependency-injection
[ms-download-dotnet]: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
[ms-az-key-vault]: https://learn.microsoft.com/en-us/azure/key-vault/
[ms-az-key-vault-configuration]: https://learn.microsoft.com/en-us/aspnet/core/security/key-vault-configuration
[ms-az-key-vault-cert-client]: https://learn.microsoft.com/en-us/dotnet/api/overview/azure/security.keyvault.certificates-readme
[ms-az-key-vault-sample-get]: https://github.com/Azure/azure-sdk-for-net/blob/0e2399f030aa365c13fcd06f891a57ee9154fc60/sdk/keyvault/Azure.Security.KeyVault.Certificates/samples/Sample1_HelloWorld.md
[ms-powershell-get-cert]: https://learn.microsoft.com/en-us/powershell/module/pki/get-certificate
[ms-powershell-import-pfxcert]: https://learn.microsoft.com/en-us/powershell/module/pki/import-certificate
[ms-docs-aspnet-configuration]: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/#configuration-providers
[ms-docs-x509store]: https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509store
[nuget-ravendb-client]: https://www.nuget.org/packages/RavenDB.Client
[nuget-ravendb-di]: https://www.nuget.org/packages/RavenDB.DependencyInjection
