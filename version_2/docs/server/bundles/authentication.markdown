# Authentication bundle

With the Authentication bundle we can use OAuth for authentication.

## Installation
In the appSettings of the RavenDB configuration file add the line:  

    <add key="Raven/AuthenticationMode" value="OAuth"/>

After that put the Raven.Bundles.Authentication.dll file in the server's Plugins directory and then run the server.  
Once done you can use OAuth for authentication.

## Adding users
In order to add a user we can use the following code:

{CODE authenticate_1@Server\Bundles\Authentication.cs /}

If no users are found on the database a user "admin" will be created with an auto generated password.
This data can be viewed in the "authentication.config" file.

## How to authenticate
In order to authenticate we configure our documentStore:  

{CODE authenticate_2@Server\Bundles\Authentication.cs /}

## Customizations
**Related server configuration options:**  

- Raven/AuthenticationMode - can be 'windows' (default) or 'oauth'
- Raven/OAuthTokenServer - if the oauth mode is selected, will instruct
connecting clients about the OAuthTokenServer, default is the local endpoint
inside ravendb
- Raven/OAuthTokenCertificatePath - the certificate to use when verifying
the token signature, allows you to collaborate with external oauth servers.
Default to creating a new certificate every time the server restarts
- Raven/OAuthTokenCertificatePassword - password for the certificate

## 3rd party OAuth server
In order to user a 3rd party server we need to specify that server in the Raven/OAuthTokenServer
and make sure that the server will return an Access Token created with the same Certificate as our server in string format.

Example:

{CODE authenticate_3@Server\Bundles\Authentication.cs /}
