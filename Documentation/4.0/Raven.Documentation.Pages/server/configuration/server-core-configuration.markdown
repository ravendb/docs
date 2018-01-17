## Server Configuration : Core

{PANEL:ServerUrl}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| ServerUrl | The URLs which the server should listen to | `http://localhost:8080` | Server-wide only |

Indicates the IP addresses or host addresses with ports and protocols that the server should listen on for requests. Use `0.0.0.0` to indicate that the server should listen for requests on any IP address or hostname using the specified port and protocol. The protocol (`http://` or `https://`) must be included with each URL.

Valid IP address can be localhost, domains, IPv4 or IPv6 addresses. Port can be specified after the address using ':' as a separator, or if the default is being used: *port 80* for *http* protocol, and *port 443* for *https* protocol.

{SAFE Setting to a non loopback address using the ***http*** protocol will expose the server to the network and requires security measurements (using https, certificates). When set, RavenDB will prevent a startup unless UnsecuredAccessAllowed=PublicNetwork is set manually. see LINK_TO_SETUP_SECURITY /}

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

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| ServerUrl.Tcp | The TCP URLs which the server should listen to | `null` | Server-wide only |

Indicates the IP addresses or host addresses with ports and protocols that the server should listen on for incoming TCP connections, are used for inter-node communication.
Valid IP address can be localhost, domains, IPv4 or IPv6 addresses. Port **must be specified** after the address using ':' as separator or just as number without address. 

If no URL is set, the ServerUrl will be used along with random port
If just a number is set, the ServerUrl will be used with the specified number as port
If the address and port are set, RavenDB will listen to the address and port specified

{SAFE Same security consideration as in ***ServerUrl*** option should be applied (see above) /}

### Examples

* Server will listen to TCP connections in all the network devices available on the machine

{CODE-BLOCK:plain}
tcp://0.0.0.0:38888
{CODE-BLOCK/}

{PANEL/}

{PANEL:PublicServerUrl}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| PublicServerUrl | The URL under which server is publicly available | `null` (Local Server URL) | Server-wide only |

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

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| PublicServerUrl.Tcp | The TCP URL under which server is publicly listen to | `null` | Server-wide only |

Set the public TCP address of the server. Used for inter-node communication and access from behind a firewall, proxy, etc.

### Examples

{CODE-BLOCK:plain}
tcp://example.com:38888
{CODE-BLOCK/}

{PANEL/}

{PANEL:RunInMemory}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| RunInMemory | Set whether the database should run purely in memory | `false` | Server-wide or per database |

When running in memory, RavenDB does not write to the disk. If the server is restarted, all data will be lost. This is mostly useful for testing.

{PANEL/}

{PANEL:DataDir}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| DataDir | Path to the data directory of RavenDB | `Databases/{name}` | Server-wide or per database |

Relative paths will be based from the application base directory (where the Raven.Server executable is located).

### Examples

{CODE-BLOCK:plain}
/home/user/databases
{CODE-BLOCK/}

{PANEL/}

{PANEL:Setup.Mode}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Setup.Mode | Determines what kind of security was chosen during setup, or not to use setup on startup at all (SetupMode.None) | `None` | Server-wide only |

Possible values:

- `None` : No setup process on RavenDB server startup
- `Initial` : Start the wizard process to setup RavenDB on the first server startup
- `LetsEncrypt` : Let RavenDB know that it needs to take care of refreshing certificates on the fly via LE
- `Secured` : This value will be set internally by RavenDB
- `Unsecured` : Run the server in unsecured mode

{PANEL/}

{PANEL:AcmeUrl}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| AcmeUrl | The URLs which the server should contact when requesting certificates from using the ACME protocol | `https://acme-v01.api.letsencrypt.org/directory` | Server-wide only |

{PANEL/}

{PANEL:ThrowIfAnyIndexCannotBeOpened}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| ThrowIfAnyIndexCannotBeOpened | Indicates if we should throw an exception if any index could not be opened | `false` | Server-wide or per database |

{PANEL/}
