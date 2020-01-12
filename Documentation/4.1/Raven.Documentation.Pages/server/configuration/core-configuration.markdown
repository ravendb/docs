# Configuration: Core

[settings.json](../../server/configuration/configuration-options#json) variables change your server's behavior in various ways.  

{NOTE: }
RavenDB reads `settings.json` only during startup.  
When you edit the file, restart the server to apply changes.  
{NOTE/}

{PANEL:ServerUrl}

The URLs which the server should listen to.

- **Type**: `string`
- **Default**: `http://localhost:8080`
- **Scope**: Server-wide only

Indicates the IP addresses or host addresses with ports and protocols that the server should listen on for requests. Use `0.0.0.0` to indicate that the server should listen for requests on any IP address or hostname using the specified port and protocol. The protocol (`http://` or `https://`) must be included with each URL.

Valid IP addresses can be localhost, domains, IPv4 or IPv6. Ports can be specified after the address using ':' as a separator, or if the default is being used: *port 80* for *http* protocol, and *port 443* for *HTTPS* protocol.

{SAFE Setting to a non loopback address using the ***HTTP*** protocol will expose the server to the network and requires security measurements (using HTTPS, certificates). When set, RavenDB will prevent a startup unless **UnsecuredAccessAllowed** is set to **PublicNetwork** manually. /}

### Examples

* The server will listen to incoming requests in all the network devices available on the machine on the specific port

{CODE-BLOCK:plain}
http://0.0.0.0:8080
{CODE-BLOCK/}

* The server will listen to loopback device only:

{CODE-BLOCK:plain}
http://localhost:8080
{CODE-BLOCK/}

* Server using IPV6 loopback only address

{CODE-BLOCK:plain}
http://[0:0:0:0:0:0:0:1]:8080
{CODE-BLOCK/}

{PANEL/}

{PANEL:ServerUrl.Tcp}

The TCP URLs which the server should listen to.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

Indicates the IP addresses or host addresses with ports and protocols that the server should listen on for incoming TCP connections, are used for inter-node communication.
Valid IP addresses can be localhost, domains, IPv4 or IPv6 addresses. Ports **must be specified** after the address using ':' as separator or just as number without address. 

If no URL is set, the ServerUrl will be used along with random port
If just a number is set, the ServerUrl will be used with the specified number as port
If the address and port are set, RavenDB will listen to the address and port specified

{SAFE Same security consideration as in ***ServerUrl*** option should be applied (see above) /}

### Examples

* The server will listen to TCP connections in all the network devices available on the machine

{CODE-BLOCK:plain}
tcp://0.0.0.0:38888
{CODE-BLOCK/}

{PANEL/}

{PANEL:PublicServerUrl}

The URL under which server is publicly available.

- **Type**: `string`
- **Default**: `null` (local Server URL)
- **Scope**: Server-wide only

Set the URL to be accessible by clients and other nodes, regardless of which IP is used to access the server internally. This is useful when using a secured connection via https URL, or behind a proxy server. 

### Examples

* Use LAN proxy server address 10.0.0.1

{CODE-BLOCK:plain}
http://10.0.0.1:80
{CODE-BLOCK/}

* Use a specific https domain

{CODE-BLOCK:plain}
https://example.com:8080
{CODE-BLOCK/}

{NOTE In the above example, `example.com` is the external domain/ip provided by the ISP, and `ServerUrl` must be specified when the server is behind a firewall, proxy, or router /}

{PANEL/}

{PANEL:PublicServerUrl.Tcp}

The TCP URL under which server is publicly listened to.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

Set the public TCP address of the server. Used for inter-node communication and access from behind a firewall, proxy, etc.

### Examples

{CODE-BLOCK:plain}
tcp://example.com:38888
{CODE-BLOCK/}

{PANEL/}

{PANEL:ExternalIp}

External IP address.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:RunInMemory}

Set whether the database should run purely in memory.

- **Type**: `bool`
- **Default**: `null`
- **Scope**: Server-wide or per database

When running in memory, RavenDB does not write to the disk. If the server is restarted, all data will be lost. This is mostly useful for testing.

{PANEL/}

{PANEL:DataDir}

Path to the data directory of RavenDB

- **Type**: `string`
- **Default**: `Databases/{name}`
- **Scope**: Server-wide or per database

Relative paths will be based from the application base directory (where the Raven.Server executable is located).

### Examples

{CODE-BLOCK:plain}
/home/user/databases
{CODE-BLOCK/}

{PANEL/}

{PANEL:Setup.Mode}

Determines what kind of security was chosen during setup, or not to use setup on startup at all (`None`).

- **Type**: `enum`
- **Default**: `None`
- **Scope**: Server-wide only

Possible values:

- `None`: No setup process on RavenDB server startup
- `Initial`: Start the wizard process to setup RavenDB on the first server startup
- `LetsEncrypt`: Let RavenDB know that it needs to take care of refreshing certificates on the fly via LE
- `Secured`: This value will be set internally by RavenDB
- `Unsecured`: Run the server in unsecured mode

{PANEL/}

{PANEL:AcmeUrl}

The URLs which the server should contact when requesting certificates from using the ACME protocol.

- **Type**: `string`
- **Default**: `https://acme-v01.api.letsencrypt.org/directory`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:ThrowIfAnyIndexCannotBeOpened}

 Indicates if we should throw an exception if any index could not be opened.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Features.Availability}

This [settings.json](../../server/configuration/configuration-options#json) variable determines whether to run RavenDB with its standard 
features set, orr add to a set of experimental features.  
Some features, like ones recently released, are considered **experimental**. They are disabled by default, you can enable 
them by setting `Features.Availability` to `Experimental`.  

- **Type**: `enum`
- **Default**: `Stable`
- **Scope**: Server-wide only

Possible values:

- `Stable`: Standard set of features  
- `Experimental`: Enables experimental features  

{NOTE: }
We'd be grateful for any feedback you send us regarding experimental features.  
{NOTE/}

{PANEL/}
