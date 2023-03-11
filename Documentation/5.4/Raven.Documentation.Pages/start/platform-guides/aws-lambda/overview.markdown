# AWS Lambda (.NET C#)
---

{NOTE: What is AWS Lambda?}
AWS Lambda is a serverless platform that supports multiple languages and frameworks that let you deploy workloads that scale without managing any infrastructure.

Learn more about [how AWS Lambda works][aws-lambda].

### New to AWS Lambda

This guide assumes you are familiar with C# development techniques and the basics of AWS Lambda functions. For a walkthrough and demo of getting started with AWS Lambda, see [TBD](#).
{NOTE/}

In this guide, you will learn how to deploy a .NET C# Lambda Handler Function using the [RavenDB AWS Lambda C# template][template] that is connected to your RavenDB database.

On this page:

* [Before We Get Started](#before-we-get-started)
* [Create a Local Lambda Function](#create-a-local-lambda-function)
* [Configuring Local Connection to RavenDB](#configuring-local-connection-to-ravendb)
* [Deploying to AWS](#deploying-to-aws)
* [Verify the Connection Works](#verify-the-connection-works)
* [Using RavenDB in the Lambda Function](#using-ravendb-in-the-lambda-function)

{PANEL: Before We Get Started}

You will need the following before continuing:

- A [RavenDB Cloud][cloud-signup] account or self-hosted client certificate
- A local [AWS .NET development environment][aws-dotnet] set up
  - _Recommended_: [AWS Toolkit for VS Code][aws-vs-code]
  - _Recommended_: [AWS Toolkit for Visual Studio][aws-vs]
- [Amazon Lambda Tools package for .NET CLI][aws-dotnet-lambda]
- [Git](https://git-scm.org)
- [.NET 6.x][download-dotnet]

If you are new to AWS Lambda local development, see the [AWS Lambda Developer Guide][aws-lambda] for how to get up and running with your toolchain of choice.

{PANEL/}

{PANEL: Create a Local Lambda Function}

The [RavenDB AWS Lambda C# template][template] is a template repository on GitHub which means you can either create a new repository derived from the template or clone and push it to a new repository.

This will set up a local Lambda C# function that we will deploy to your AWS account at the end of the guide.

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

### Install AWS .NET tools

You will need the .NET Global Tools for Lambda installed to perform the deployment steps later.

Install the `Amazon.Lambda.Tools` package:

`dotnet tool install -g Amazon.Lambda.Tools`

Or make sure it's updated if you already have it:

`dotnet tool update -g Amazon.Lambda.Tools`

### Set Up Your Environment

AWS libraries, SDKs and this template rely on several environmental artifacts to work. One is your AWS credentials, stored in  `~/.aws/credentials` and the other is the default AWS region to use.

**Using the defaults file:** You can use the template's `aws-lambda-tools-defaults.json` to set your Functions region:

{CODE-BLOCK:json}
{
  ...
  "region": "us-east-1",
  ...
}
{CODE-BLOCK/}

**Using an environment variable:** Set the `AWS_REGION` environment variable in your terminal session or profile.

Learn more about setting up AWS credentials or the default AWS region.

{PANEL/}


{PANEL: Configuring Local Connection to RavenDB}

To configure the local version of your Azure Functions app to connect to RavenDB, you will need to update the `appsettings.json` file with the `RavenSettings.Urls` value and `RavenSettings.DatabaseName` value. The default is:

{CODE-BLOCK:json}
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "RavenSettings": {
    "Urls": ["http://live-test.ravendb.net"],
    "DatabaseName": "Northwind",
    "CertFilePath": "",
    "CertPassword": ""
  }
}

{CODE-BLOCK/}

If using an authenticated RavenDB URL, you will need a local client certificate. Learn more about [configuring client authentication for RavenDB][docs-client-certs].

### Using a PFX Certificate File

To configure the local Lambda function to load a certificate from the project directory, specify the `RavenSettings.CertFilePath` and `RavenSettings.CertPassword` settings:

{CODE-BLOCK:json}
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "RavenSettings": {
    "Urls": ["https://a.MYCOMPANY.ravendb.cloud"],
    "DatabaseName": "MyDB",
    "CertFilePath": "db.pfx",
    "CertPassword": "<CERT_PASSWORD>"
  }
}
{CODE-BLOCK/}

This will connect to the `a.MYCOMPANY.ravendb.cloud` RavenDB Cloud cluster using the local certificate file.

### Loading Configuration from AWS Secrets Manager

The template uses [Kralizek.Extensions.Configuration.AWSSecretsManager][kralizek] to automatically load .NET configuration from AWS Secrets Manager to support securely loading certificates instead of relying on production environment variables.

The configuration will be loaded from AWS Secrets Manager if it exists, otherwise `appsettings.json` will be used.

{PANEL/}

{PANEL: Deploying to AWS}

At this point, the local Lambda app is ready to be deployed. There are 4 main ways to deploy your new AWS Lambda function: GitHub actions, .NET CLI, AWS SDK CLI, and or the AWS Toolkit extensions.

The template has already been set up to use continuous deployment using GitHub Actions. For the other methods, see [Deploying AWS Lamda Functions][aws-lambda-deploy].

However, we need to do a deployment manually for the first-time setup, such as setting the Function role and policy. Once it is setup, GitHub Actions will automatically deploy on new commits.

Start by deploying your function manually using the .NET CLI:

`dotnet lambda deploy-function <FUNCTION_NAME>`

{NOTE: }
The function name should match the name of the `.csproj` file.
{NOTE/}

The tool will walk you through the first-time deployment:

- **Function IAM Role:** This can be the name of your function name plus "Role", e.g. `RavenDBTemplateRole`
- **Function Policy:** Choose `AWSLambdaBasicExecutionRole` to allow for basic AWS Lambda execution permissions

### Create a deployment AWS Access Key

If you do not have code deployment user, create a new IAM user to be used by your GitHub automation.

You will need the following security policies (assigned to group or user):

- `AWSLambda_FullAccess`
- `IAMReadOnlyAccess`

Once you have the user created, create and obtain an AWS access key specific for your GitHub action deployment workflow.

{WARNING: Do not use Root Keys}
It is recommended to use a dedicated deployment IAM user with specific access policies for automated deployment through GitHub Actions.

Ensure you don't store your AWS keys in plain-text on your machine or elsewhere. They are password-equivalents. GitHub Secrets are encrypted and cannot be retrieved after being stored.
{WARNING/}

### Configure GitHub Secrets and Variables

The GitHub deployment workflow relies on having some specific secrets and variables set.

#### Setting up Secrets

1. Obtain or create an Access Key for the user
1. Go to your [GitHub repository's secrets settings][gh-secrets]
1. Add a new secret
    - Name: `AWS_ACCESS_KEY_ID`
    - Value: The access key ID
1. Add a new secret
    - Name: `AWS_SECRET_ACCESS_KEY`
    - Value: The secret access key

![Example of GitHub repository secrets set](images/overview-gh-secrets.png)

#### Setting up Variables

1. Go to your [GitHub repository's variables settings][gh-variables]
1. Add or modify a variable
    - Name: `AWS_LAMBDA_FUNCTION_NAME`
    - Value: The name of your function used in the deploy command
1. Add or modify a variable
    - Name: `AWS_LAMBDA_FUNCTION_ROLE`
    - Value: The IAM role name of the Function set when you first deployed
1. If you are **not** using the `aws-lambda-tools-defaults.json` file to set the region, add or modify a variable:
    - Name: `AWS_REGION`
    - Value: The default region your Function will deploy to

![Example of GitHub repository variables set](images/overview-gh-variables.png)

### Trigger a Deployment

Your repository and GitHub action is now set up. To test the deployment, you can push a commit to the repository.

If you have already committed and pushed, it is likely that the Action failed and you can re-run the job using the new secret variable.
Once deployed, using the default settings, the Function will connect to the Live Test database.

### Changing Application Configuration for Production

By default, configuration will be loaded from `appsettings.json` but it is likely you may have different configuration needed once the Lambda function is deployed.

{PANEL/}

{PANEL: Verify the Connection Works}

If the deployment succeeds, the Lambda function should now be available to invoke.

You can test it using the .NET CLI:

`dotnet lambda invoke-function <FUNCTION_NAME>`

You should see a message that looks like this:

`Successfully connected to RavenDB - Node A`

{PANEL/}

{PANEL: Using RavenDB in the Lambda Function}

The template sets up a singleton `DocumentStore` and dependency injection for the `IAsyncDocumentStore` per handler invocation which you can inject into Function classes.

### Example: Injecting `IAsyncDocumentSession`

Pass the `IAsyncDocumentSession` in the handler function using `[FromServices]` which is available from `Amazon.Lambda.Annotations` package:

{CODE-BLOCK:csharp}
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using Raven.Client.Documents.Session;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RavenDBLambda;

public class Functions
{
  [LambdaFunction]
  [HttpApi(LambdaHttpMethod.Get, "/")]
  public async Task<string> FunctionHandler([FromServices] IAsyncDocumentSession session, ILambdaContext context)
  {
    var node = await session.Advanced.GetCurrentSessionNode();

    return $"Successfully connected to RavenDB - Node {node.ClusterTag}";
  }
}

{CODE-BLOCK/}

### Example: Injecting `IDocumentStore`

You can also inject an `IDocumentStore` to get a reference to the current store instance. For singleton references, inject using a public class constructor:

{CODE-BLOCK:csharp}
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using Raven.Client.Documents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RavenDBLambda;

public class Functions
{

  private readonly IDocumentStore _store;

  public Functions(IDocumentStore store) {
    _store = store;
  }

  [LambdaFunction]
  [HttpApi(LambdaHttpMethod.Get, "/")]
  public async string FunctionHandler(ILambdaContext context)
  {
    // Access _store DocumentStore methods
  }
}

{CODE-BLOCK/}

### Example: Loading a user

{CODE-BLOCK:javascript}
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using Raven.Client.Documents.Session;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RavenDBLambda;

public class Functions
{
  [LambdaFunction]
  [HttpApi(LambdaHttpMethod.Get, "/users/{id}")]
  public async Task<User> FunctionHandler([FromServices] IAsyncDocumentSession session, string id, ILambdaContext context)
  {
    var user = await session.Load<User>("users/" + id);

    return user;
  }
}

{CODE-BLOCK/}

{PANEL/}

{PANEL: Next Steps}

* [Configure AWS Secrets Manager support][docs-lambda-secrets]
* [Use the RavenDB .NET client SDK][ravendb-dotnet]

{PANEL/}

[download-dotnet]: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
[aws-lambda]: https://docs.aws.amazon.com/lambda/latest/dg/welcome.html
[aws-dotnet]: https://aws.amazon.com/sdk-for-net/
[aws-dotnet-lambda]: https://docs.aws.amazon.com/lambda/latest/dg/csharp-package-cli.html
[aws-vs-code]: https://aws.amazon.com/visualstudiocode/
[aws-vs]: https://aws.amazon.com/visualstudio/
[aws-lambda-deploy]: https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/deploying-lambda.html
[template]: https://github.com/ravendb/template-aws-lambda-csharp
[gh-secrets]: https://docs.github.com/en/actions/security-guides/encrypted-secrets
[gh-variables]: https://docs.github.com/en/actions/learn-github-actions/variables
[cloud-signup]: https://cloud.ravendb.net?utm_source=ravendb_docs&utm_medium=web&utm_campaign=howto_template_lambda_csharp&utm_content=cloud_signup
[docs-lambda-secrets]: ../secrets-manager
[docs-get-started]: /docs/article-page/csharp/start/getting-started
[docs-client-certs]: /docs/article-page/csharp/client-api/setting-up-authentication-and-authorization
[ravendb-dotnet]: /docs/article-page/csharp/client-api/session/what-is-a-session-and-how-does-it-work
[kralizek]: https://github.com/Kralizek/AWSSecretsManagerConfigurationExtensions
