# Authentication bundle

With the Authentication bundle we can use OAuth for authentication.

Currently this is supported only for the synchronous api, and it is not
available for the async api or in silverlight. 

## Installation
In the appSettings of the Raven.Server.exe.config add the line:  

    <add key="Raven/AuthenticationMode" value="OAuth"/>

After that put the Raven.Bundles.Authentication.dll file in the server's Plugins directory and then run the server.  

**Implications**  

- On the server side, support for OAuth as an authentication method.
 - This includes both 2 legged and 3 legged authentication
 - Authenticate user the bearer token
 - Extensible way to configure users
 - Provide a way to grant permissions only to certain databases, instead of all or nothing.
- New bundle, Authentication bundle:
 - Provide a simple way to define users as documents inside ravendb 

## Adding users
In order to add a user we can use the following code:

    using(var session = embeddedStore.OpenSession())
    {
    	session.Store(new AuthenticationUser
    	{
    		Name = "Ayende",
    		Id = "Raven/Users/Ayende",
    		AllowedDatabases = new[] {"*"}
     	}.SetPassword("abc"));
    	session.SaveChanges();
    }

If no users are found on the database a user "admin" will be created with an auto generated password
This data can be viewed in the "authentication.config" file.

## How to authenticate
All you need to do to authentication is set the Raven/OAuthTokenCertificatePath and the Raven/OAuthTokenCertificatePassword to the user you want to log on as.

## Customizations
**New server configuration options:**  

- Raven/AuthenticationMode - can be 'windows' (default) or 'oauth'
- Raven/OAuthTokenServer - if the oauth mode is selected, will instruct
connecting clients about the OAuthTokenServer, default is the local endpoint
inside ravendb
- Raven/OAuthTokenCertificatePath - the certificate to use when verifying
the token signature, allows you to collaborate with external oauth servers.
Default to creating a new certificate every time the server restarts
- Raven/OAuthTokenCertificatePassword - password for the certificate

**New client options:**  

- DocumentConventions.HandleUnauthorizedResponse - allows you to inject
your own custom behavior for generating a token for ravendb.
-  jsonRequestFactory.EnableBasicAuthenticationOverUnsecureHttpEvenThoughPasswordsWouldBeSentOverTheWireInClearTextToBeStolenByHackers
- pretty much what it says, by default, we don't allow you to send plain
text password over HTTP. 

**New authorization bundle options:**  

- You can provide your own user authentication in a way that simply plugs
just this part.

This is done by creating a bundle that implements:

    public interface IAuthenticateClient
    {
    	bool Authenticate(IResourceStore currentStore, string username, string password, out string[] allowedDatabases);
    } 

## 3rd party OAuth server
In order to connect a 3rd party OAuth server you need to set the Raven/OAuthTokenServer in the appSettings.