# Fiddler Usage With Secured Database

When you want to use fiddler with secure database you need to configure fiddler that it could response and do requests as you.

## Respond to Requests Requiring a Client Certificate

First you need to create .CER file: 

1. Open Manege User certificates

2. Right-click the certificate in Personal Certificates Store.

3. Click All Tasks > Export 

Then you need to specify the .CER file for Fiddler to return for a given session.

You have two options to do so:

1. Add a FiddlerScript to OnBeforeRequest function: 
{CODE-BLOCK:plain}
oSession["https-Client-Certificate"] = "C:\\test\\someCert.cer"; 
{CODE-BLOCK/}
2. Place your .CER file in '%USERPROFILE%\My Documents\Fiddler2\ClientCertificate.cer' ( the name must be ClientCertificate.cer)

{WARNING If you do this your client certificate is exposed through fiddler /}

## Accepting response by the client

* Option 1: Configure Windows Client to trust Fiddler Root Certificate

    1. Click Tools > Fiddler Options > HTTPS.

    2. Click the Decrypt HTTPS Traffic box.

    3. Next to `Trust the Fiddler Root certificate?`, click Yes.

    4. After `Do you want to install this certificate?`, click Yes.

{DANGER If you do this windows will automatically trust any certificate issued by this CA. This is a security risk! /}

* Option 2: Client will ignore certificate validation

    In the application you should set:

    Raven.Client.Http.RequestExecutor.RemoteCertificateValidationCallback += (sender, cert, chain, errors) => true;

{DANGER If you do this and forget to remove it from your code, your client will accept any response! /}
