# Guides: Azure Functions (.NET C#)
---

{NOTE: }

* Microsoft **Azure Functions** are a serverless platform that supports multiple 
  languages and frameworks that let you deploy workloads that scale without managing 
  any infrastructure.  
  Learn more about operating Microsoft Azure Functions [here][az-funcs].

* In this guide, you will learn how to deploy a C# Azure Function using the 
  [RavenDB Azure Functions C# template][template] that is connected to your 
  RavenDB database.  
  {INFO: }
  This guide assumes you are familiar with .NET development techniques and the 
  basics of Azure Function apps.
  {INFO/}

* In this page: 
    * [Before We Get Started](../../../start/guides/azure-functions/overview#before-we-get-started)  
    * [Create a Local Azure Function App](../../../start/guides/azure-functions/overview#create-a-local-azure-function-app)  
    * [Configuring Local Connection to RavenDB](../../../start/guides/azure-functions/overview#configuring-local-connection-to-ravendb)  
    * [Creating Function App Resources in Azure](../../../start/guides/azure-functions/overview#creating-function-app-resources-in-azure)  
    * [Deploying to Azure](../../../start/guides/azure-functions/overview#deploying-to-azure)  
    * [Verify the Connection Works](../../../start/guides/azure-functions/overview#verify-the-connection-works)  
    * [Using RavenDB in the Azure Functions App](../../../start/guides/azure-functions/overview#using-ravendb-in-the-azure-functions-app)  

{NOTE/}

{PANEL: Before We Get Started}

You will need the following before continuing:  

- A [RavenDB Cloud][cloud-signup] account or self-hosted client certificate  
- [Azure Function Core Tools][az-core-tools] 4.x+  
- [Git](https://git-scm.org)  
- [.NET 6.x][download-dotnet]  

If you are new to Azure Function local development, see the [Getting started guide][az-funcs] 
for how to get up and running with your toolchain of choice.  

{PANEL/}

{PANEL: Create a Local Azure Function App}

The [RavenDB Azure Function template][template] is a template repository on GitHub which means 
you can either create a new repository derived from the template or clone and push it to a new repository.  

This will set up a local Azure Function app that we will deploy to your Azure account at the end of the guide.  

### Creating a New Repository from the Template

Depending on your environment, there are several ways to clone the template and initialize a new Git repository.  
The template repository lists each clone method you can copy & paste directly.  

**Using `npx` and the [degit][tool-degit] tool if you have Node.js installed:**  

{CODE-BLOCK:bash}
npx degit ravendb/templates/azure-functions/csharp-http my-project
cd my-project
git init
{CODE-BLOCK/}

**Using Bash or PowerShell:**

{CODE-BLOCK:bash}
git clone https://github.com/ravendb/templates my-project
cd my-project
git filter-branch --subdirectory-filter azure-functions/csharp-http
rm -rf .git       # Bash
rm -r -force .git # PowerShell
git init
{CODE-BLOCK/}

### Install Dependencies

After cloning the repository locally, restore .NET dependencies with `dotnet`:  

{CODE-BLOCK:bash}
dotnet restore
{CODE-BLOCK/}

By default, the template is configured to connect to the Live Test instance of RavenDB.  
Since this is only for testing purposes, next you will configure the app to connect to your existing RavenDB database.  

### Starting the Function

You can start the Azure Function locally using:  

{CODE-BLOCK:bash}
func start
{CODE-BLOCK/}

If you are using Visual Studio Code, you can also debug the function with F5 debugging.  

You will see the welcome screen if the template is set up correctly:  

![.NET template welcome screen](images/dotnet-func-start.jpg ".NET template welcome screen")

{PANEL/}


{PANEL: Configuring Local Connection to RavenDB}

To configure the local version of your Azure Functions app to connect to RavenDB, you will need to update 
the `appsettings.json` file with the `RavenSettings:Urls` value and `RavenSettings:DatabaseName` value.  
The default is:  

{CODE-BLOCK:json}
{
  "RavenSettings": {
    "Urls": ["http://live-test.ravendb.net"],
    "DatabaseName": "Northwind"
  }
}
{CODE-BLOCK/}

If using an authenticated RavenDB URL, you will need a local client certificate installed.  
Learn more about configuring client authentication for RavenDB [here][docs-client-certs].  

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

You can also specify a certificate to use from the `CurrentUser\My` Windows certificate store by setting 
`RavenSettings:CertThumbprint`.  

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

At this point, the local Function app is ready to be deployed. Before you can do that, 
you need to set up the Function App resources in Azure.  

The template includes an ARM deployment option using the **Deploy to Azure** button. 
This will open the Azure Portal and walkthrough creating a default Function App with 
the required resources and app settings.  

Follow the guide of your choice in the Microsoft docs. Once the app is created, come 
back here to finish configuring your database connection.  

### Upload Your Client Certificate (.pfx)

Once the app is created in the portal, follow these steps to upload the client certificate and make 
it accessible to your Function.  

![.NET upload certificate to Azure](images/dotnet-azure-upload-cert.jpg ".NET upload certificate to Azure")

1. Go to your Azure Functions dashboard in the Portal
1. Click "Certificates"
1. Click the "Bring Your Own Certificate" tab
1. Click "+ Add Certificate" button
1. Upload the RavenDB client certificate (PFX) file
1. Enter the certificate password
1. Once uploaded, click the certificate to view details
1. Copy the "Thumbprint" for the next step

{WARNING: Do not store certificate password}
The Azure portal will only use the certificate password once on upload. You will not need the password 
in your Functions App, only the public thumbprint.  
You can safely delete the password from your device once the certificate is uploaded in the Portal so 
as not to risk it being discovered.  
{WARNING/}

### Configure Application Settings

![.NET update Azure app settings](images/dotnet-azure-app-settings.jpg ".NET update Azure app settings")

1. Go to your Azure Functions dashboard in the Portal  
1. Click the Application Settings menu  
1. Modify or add app setting for `WEBSITE_LOAD_CERTIFICATES` to the certificate thumbprint you copied
    
      ![.NET WEBSITE_LOAD_CERTIFICATES example](images/dotnet-azure-website-load-certificates.jpg ".NET WEBSITE_LOAD_CERTIFICATES example")

1. Modify or add app setting for `RavenSettings__CertThumbprint` with the certificate thumbprint you copied
    
      ![.NET WEBSITE_LOAD_CERTIFICATES example](images/dotnet-azure-ravensettings__certthumbprint.jpg ".NET WEBSITE_LOAD_CERTIFICATES example")

1. Modify or add app setting for `RavenSettings__Urls` with the comma-separated list of RavenDB node URLs to connect to  
1. Modify or add an app setting for `RavenSettings__DatabaseName` with the database name to connect to  

These values will override `appsettings.json` once deployed on Azure.  

{NOTE: Loading multiple certificates}
`WEBSITE_LOAD_CERTIFICATES` makes any specified certificates available in the Windows 
Certificate Store under the `CurrentUser\My` location. You can use the wildcard value 
`*` for `WEBSITE_LOAD_CERTIFICATES` to load ALL uploaded certificates for your Function App.  
However, it's recommended to be specific and use comma-separated thumbprints so that only 
allowed certificates are made available. This avoids accidentally exposing a certificate 
to the application that isn't explicitly used.
{NOTE/}

{PANEL/}

{PANEL: Deploying to Azure}

Once the Azure app is set up in the portal, you are ready to deploy your app. 
There are 3 main ways to deploy your new Azure Function app: GitHub actions, command-line, and an extension.  

The template has already been set up to use continuous deployment using GitHub Actions.  
For the other methods, see [Deploying Azure Function apps][az-deploy].

### Configure GitHub Secrets

The GitHub actions rely on having a secret environment variable `AZURE_FUNCTIONAPP_PUBLISH_PROFILE` 
in your repository secrets.  

1. Go to your Azure Functions dashboard in the Azure Portal  
1. Click "Get Publish Profile"
    
      ![download Azure publish profile](images/azure-download-publish-profile.jpg "download Azure publish profile")

1. Download the publish profile  
1. Open it and copy the full XML  
1. Go to your [GitHub repository's secrets settings][gh-secrets]

      ![add GitHub secret for publish profile](images/github-publish-profile-secret.jpg "add GitHub secret for publish profile")

1. Add a new secret: `AZURE_FUNCTIONAPP_PUBLISH_PROFILE`  
1. Paste in the value of the publish profile  

### Trigger a Deployment

Your repository and GitHub action is now set up. To test the deployment, you can push a commit to the repository.  

If you have already committed and pushed, it is likely that the Action failed and you can re-run the job using 
the new secret variable.  

{PANEL/}

{PANEL: Verify the Connection Works}

If the deployment succeeds, the `HttpTrigger` endpoint should now be available at your Function URL.  

Once you open the URL in the browser, you should see a welcome screen like this with the connection information:  

![.NET Azure Function welcome connected screen](images/dotnet-azure-func-success.jpg ".NET Azure Function welcome connected screen")

This means your Azure Functions app is correctly configured and ready to work with RavenDB.  

{PANEL/}

{PANEL: Using RavenDB in the Azure Functions App}

The template sets up a singleton `DocumentStore` and dependency injection for the `IAsyncDocumentStore` per function 
invocation which you can inject into Function classes.  

### Example: Injecting `IAsyncDocumentSession`

Pass the `IAsyncDocumentSession` in a function class constructor to make it available to trigger functions:

{CODE HttpTrigger1@Start\Guides\azureFunctions.cs /}  

You can also inject an `IDocumentStore` to get a reference to the current store instance.

### Example: Loading a user

{CODE HttpTrigger2@Start\Guides\azureFunctions.cs /}  

Learn more about using the RavenDB .NET client SDK [here][ravendb-dotnet].

{PANEL/}

[download-dotnet]: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
[az-funcs]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-get-started
[az-core-tools]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local
[az-deploy]: https://learn.microsoft.com/en-us/azure/azure-functions/functions-deployment-technologies
[template]: https://github.com/ravendb/templates/tree/main/azure-functions/csharp-http
[gh-secrets]: https://docs.github.com/en/actions/security-guides/encrypted-secrets
[cloud-signup]: https://cloud.ravendb.net?utm_source=ravendb_docs&utm_medium=web&utm_campaign=howto_template_azurefns_dotnet&utm_content=cloud_signup
[docs-get-started]: ../../../start/getting-started
[docs-client-certs]: ../../../client-api/setting-up-authentication-and-authorization
[ravendb-dotnet]: ../../../client-api/session/what-is-a-session-and-how-does-it-work
[tool-degit]: https://npmjs.com/package/degit


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
