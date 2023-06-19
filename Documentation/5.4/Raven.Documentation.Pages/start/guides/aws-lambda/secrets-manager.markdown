# Using the AWS Secrets Manager
---

The template supports using AWS Secrets Manager to store app configuration, including the X.509 certificate contents.

{WARNING: AWS Secrets Manager Incurs a Cost}
While AWS Secrets Manager is the most secure way to load a client certificate, it does incur a cost. Learn more about [how much it will cost to store secrets][aws-secrets-pricing] for your application.

If you do not wish to use this method, you can still co-locate and deploy your `.pfx` file using the `RavenSettings.CertFilePath` option.
{WARNING/}

Before continuing, make sure you have:

- The [AWS CLI][aws-cli] installed
- A configured AWS local environment
- Your RavenDB client certificate with password (`.pfx` file) or PEM file
- Your IAM role name used by your AWS Lambda function(s)
- Your AWS account ID number

{PANEL: Storing RavenDB Secrets}

For `RavenSettings` values, you can use the **Key/Value** JSON storage using a secret named `RavenSettings` that the Lambda function will load.

Learn more about [adding secrets to Secrets Manager][aws-secrets-mgr-add].

There are two ways to specify certificates using `RavenSettings`:

### CertPem: Store PEM Certificate in Secrets Manager

The AWS template for RavenDB can load certificates through the `CertPem` secret setting, supported through [X502Certificate2.CreateFromPem][dotnet-createfrompem].

The `CertPem` JSON key should be set to a value containing the contents of the `.pem` file from the RavenDB client certificate package.

**Example RavenSettings configuration:**

{CODE-BLOCK:json}
{
  // ... other settings
  "CertPem": "-----BEGIN CERTIFICATE-----\nabc123\n-----END CERTIFICATE-----\n-----BEGIN RSA PRIVATE KEY-----\nabc123\n-----END RSA PRIVATE KEY-----"
}
{CODE-BLOCK/}

You will need to ensure both the `BEGIN CERTIFICATE` and `BEGIN RSA PRIVATE KEY` values are included, which they are by default in the exported RavenDB client certificate `.pem` file.

{NOTE: Converting line endings for JSON}
If you are copying the contents of the PEM file, you will likely need to format the value for pasting into a JSON file. You can use a custom script (e.g. `JSON.stringify`), use a tool like [JSON Stringify a String][tool-stringify], or if you have Node installed, you can use [jsesc][npm-jsesc]:

{CODE-BLOCK:bash}
# Bash
$ npx jsesc < free.mycompany.client.certificate.pem
# PowerShell
gc free.mycompany.client.certificate.pem | npx jsesc
{CODE-BLOCK/}
    
{NOTE/}

### CertBytes: Store PFX Certificate in Secrets Manager

The AWS template for RavenDB can also load certificates through the `CertBytes` secret setting. This means the client certificate needs to be stored in binary in AWS Secrets Manager. In the Secrets Manager console, you can add JSON and plaintext secrets. Binary secrets must be uploaded through the AWS CLI.



{WARNING: Risk of command history being accessed}
When you enter commands into your terminal, the command history is at risk of being accessed. Learn more about [mitigating risks of using the AWS CLI to store secrets][aws-secrets-mgr-cli]
{WARNING/}

{CODE-BLOCK:bash}
aws secretsmanager create-secret \
    --name RavenSettings.CertBytes \
    --description "RavenDB Client Certificate file" \
    --secret-binary file://free.mycompany.client.certificate.with.password.pfx
{CODE-BLOCK/}

We then need to grant access to the IAM role used by the Lambda function (created above).

### Apply a Resource Policy

First, create a file `certpolicy.json` with the following AWS policy:

{CODE-BLOCK:json}
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "AWS": "arn:aws:iam::<ACCOUNT_ID>:role/<LAMBDA_FUNCTION_ROLE>"
      },
      "Action": "secretsmanager:GetSecretValue",
      "Resource": "*"
    }
  ]
}
{CODE-BLOCK/}

Replace `<ACCOUNT_ID>` with your AWS account ID and `<LAMBDA_FUNCTION_ROLE>` with the above-created role assigned to the Lambda function.

Next, use `aws secretsmanager put-resource-policy` command to set the resource policy while also verifying the secret is not broadly accessible:

{CODE-BLOCK:bash}
aws secretsmanager put-resource-policy \
    --secret-id RavenSettings.CertBytes \
    --resource-policy file://certpolicy.json \
    --block-public-policy
{CODE-BLOCK/}

The certificate file contents is now stored and will be accessed by the Lambda function on startup.

### Verifying the Secret is Loaded

Test invoking the Lambda function again, which should access AWS Secrets Manager successfully and load the X.509 certificate to use with RavenDB.

{PANEL/}

[aws-cli]: https://aws.amazon.com/cli/
[aws-secrets-pricing]: https://aws.amazon.com/secrets-manager/pricing/
[aws-secrets-mgr-add]: https://docs.aws.amazon.com/secretsmanager/latest/userguide/hardcoded.html
[aws-secrets-mgr-cli]: https://docs.aws.amazon.com/secretsmanager/latest/userguide/security_cli-exposure-risks.html
[dotnet-createfrompem]: https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509certificate2.createfrompem?view=net-7.0
[npm-jsesc]: https://www.npmjs.com/package/jsesc
[tool-stringify]: https://onlinestringtools.com/json-stringify-string
