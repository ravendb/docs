# Authentication bundle

With the Authentication bundle we can use OAuth for authentication.

* On the server side, support for OAuth as an authentication method.
* * This includes both 2 legged and 3 legged authentication
* * Authenticate user the bearer token
* * Extensible way to configure users
* * Provide a way to grant permissions only to certain databases,
      instead of all or nothing.
* New bundle, Authentication bundle:
* * Provide a simple way to define users as documents inside ravendb 

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

* Note that it is intended to also be a show case of how you can plug in
      your own user store, if you want to.
* Client side, support for OAuth means:
* * You will be automatically authenticate if your token server (which
      ravendb by default does) uses basic auth for this.
* * You can setup a configuration on the server to point to the token
      server, so nothing needs to be done on the client.
* * You have a way to provide a 3rd party token server to do that. 

This was specifically design to support for app servers and distributed
clients, while still maintaining the "It Just Works" approach that I love so
much.

Currently this is supported only for the synchronous api, and it is not
available for the async api or in silverlight. 

**New server configuration options:**  

* Raven/AuthenticationMode - can be 'windows' (default) or 'oauth'
* Raven/OAuthTokenServer - if the oauth mode is selected, will instruct
connecting clients about the OAuthTokenServer, default is the local endpoint
inside ravendb
* Raven/OAuthTokenCertificatePath - the certificate to use when verifying
the token signature, allows you to collaborate with external oauth servers.
Default to creating a new certificate every time the server restarts
* Raven/OAuthTokenCertificatePassword - password for the certificate

**New client options:**  

* DocumentConventions.HandleUnauthorizedResponse - allows you to inject
your own custom behavior for generating a token for ravendb.
* jsonRequestFactory.EnableBasicAuthenticationOverUnsecureHttpEvenThoughPasswordsWouldBeSentOverTheWireInClearTextToBeStolenByHackers
* pretty much what it says, by default, we don't allow you to send plain
text password over HTTP. 

**New authorization bundle options:**  

* I'll document it in more details later, but the basic idea is that it
looks for a document named "Raven/Users/[User Name]" and check the hashed
password there. You can see how you create it in the code above.
* You can provide your own user authentication in a way that simply plugs
just this part.

This is done by creating a bundle that implements:

    public interface IAuthenticateClient
    {
    	bool Authenticate(IResourceStore currentStore, string username, string password, out string[] allowedDatabases);
    } 

###Scenarios

**I am running a few applications using ravendb and I want each to login to
just its database.**

* Set Raven/AuthenticationMode = 'oauth'
* Put Raven.Bundles.Authorization.dll in the plugins directory.

Create your users inside RavenDB (like so):

    using(var session = embeddedStore.OpenSession())
    {
    	session.Store(new AuthenticationUser
    	{
    		Name = "AyendeBlog",
    		Id = "Raven/Users/AyendeBlog",
    		AllowedDatabases = new[] {"ayende.com"}
     	}.SetPassword("abc"));
    	session.SaveChanges();
    }

You have created a user with the name 'AyendeBlog', the password 'abc' which
is only allowed to use the 'ayende.com' database.

**I am running several ravendb servers and I want to define all of my users
on a single place, but have them grant access to all the server**

* On each server, setup
* Set Raven/AuthenticationMode = 'oauth'
* Set Raven/OAuthTokenServer - to point to one of those servers, like:
http://ravendb1.mysite.com/OAuth/AccessToken
* Point Raven/OAuthTokenCertificatePath to the path of a X509 certificate
(set the password as well if you need to).

The certificate needs to be the same among all servers, obviously.

You can now create all of your users on http://ravendb1.mysite.com, it will
work for all servers.

**I am running a single ravendb server that allows silverlight clients to
connect to it, how do I setup security so each client can only access its
own database**

* Set Raven/AuthenticationMode = 'oauth'
* Set Raven/OAuthTokenServer - to point to your own endpoint, such as:
http://myapp.mysite.com/oauth/generateToken
* Point Raven/OAuthTokenCertificatePath to the path of a X509 certificate
(set the password as well if you need to).

Inside that endpoint (assuming ASP.Net MVC) you will have code similar to
this:

    public class OAuthController : Controller
    {
    	public ActionResult GenerateToken()
    	{
    		// autheticate the user...

    		using(var cert = new X509Certificate2("path/to/cert"))
    		{
      			return Content(AccessToken.Create(cert, "the user", new[]{ "database1", "database2"})
    					.Serialize());
    		}
    	}
    } 