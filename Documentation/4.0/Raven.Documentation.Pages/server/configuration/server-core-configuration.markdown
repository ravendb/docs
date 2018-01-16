## Server Configuration : Core Options

<br><br>

#### ServerUrl
###### The URLs which the server should listen to
###### Default Value: "http://localhost:8080"

Indicates the IP addresses or host addresses with ports and protocols that the server should listen on for requests. Use "0.0.0.0" to indicate that the server should listen for requests on any IP address or hostname using the specified port and protocol. The protocol (http:// or https://) must be included with each URL.

Valid IP address can be localhost, domains, IPv4 or IPv6 addresses. Port can be specified after the address using ':' as a separator, or if the default is being used: *port 80* for *http* protocol, and *port 443* for *https* protocol.

{SAFE Setting to a non loopback address using the ***http*** protocol will expose the server to the network and requires security measurements (using https, certificates). When set, RavenDB will prevent a startup unless UnsecuredAccessAllowed=PublicNetwork is set manually. see LINK_TO_SETUP_SECURITY /}

#Examples:

* The server will listen to incoming requests in all the network devices available on the machine on the specific port
```
ServerUrl=http://0.0.0.0:8080
```

* The server will listen to loopback device only:
```
ServerUrl=http://localhost:8080
```

* Server using IPV6 loopback only address
```
ServerUrl=http://[0:0:0:0:0:0:0:1]:8080
```

<br><br>

#### TcpServerUrls
###### The TCP URLs which the server should listen to
###### Default Value: null

Indicates the IP addresses or host addresses with ports and protocols that the server should listen on for incoming TCP connections, are used for inter-node communication.
Valid IP address can be localhost, domains, IPv4 or IPv6 addresses. Port **must be specified** after the address using ':' as separator or just as number without address. 

If no URL is set, the ServerUrl will be used along with random port
If just a number is set, the ServerUrl will be used with the specified number as port
If the address and port are set, RavenDB will listen to the address and port specified

{SAFE Same security consideration as in ***ServerUrl*** option should be applied (see above)}

#Example:

* Server will listen to TCP connections in all the network devices available on the machine
```
TcpServerUrl=tcp://0.0.0.0:38888
```

<br><br>

#### PublicServerUrl
###### The URL under which server is publicly available
###### Default Value: null (Local Server URL)
Set the URL to be accessible by clients and other nodes, regardless of which IP is used to access the server internally. This is useful when using a secured connection via https URL, or behind a proxy server. 

#Examples:

* Use LAN proxy server address 10.0.0.1

```
PublicServerUrl=http://10.0.0.1:80
```

* Use a specific https domain

```
PublicServerUrl=https://example.com:8080
ServerUrl=http://10.0.0.123:8080
```

{NOTE In the above example, `example.com` is the external domain/ip provided by the ISP, and `ServerUrl` must be specified when the server is behind a firewall, proxy, or router /}

<br><br>

#### PublicTcpServerUrl
###### The TCP URL under which server is publicly listen to
###### Default Value: null

Set the public TCP address of the server. Used for inter-node communication and access from behind a firewall, proxy, etc.


#Example:
```
PublicTcpServerUrl=tcp://example.com:38888
```

<br><br>

#### RunInMemory
###### Set whether the database should run purely in memory
###### Default Value: false

When running in memory, RavenDB does not write to the disk. If the server is restarted, all data will be lost. This is mostly useful for testing.

#Example:
```
RunInMemory=true
```

<br><br>

#### DataDirectory
###### Path to the data directory of RavenDB
###### Default Value: "Databases/{name}"
Relative paths will be based from the application base directory (where the Raven.Server executable is located).

#Example:
```
DataDirectory="/home/user/databases"
```

<br><br>

#### Setup.Mode
###### Determines what kind of security was chosen during setup, or not to use setup on startup at all (SetupMode.None)
###### Default Value: SetupMode.None

Possible values:

- None : No setup process on RavenDB server startup
- Initial : Start the wizard process to setup RavenDB on the first server startup
- LetsEncrypt : Let RavenDB know that it needs to take care of refreshing certificates on the fly via LE
- Secured : This value will be set internally by RavenDB
- Unsecured : Run the server in unsecured mode

#Example:
```
Setup.Mode="Unsecured"
```

<br><br>

#### AcmeUrl
###### The URLs which the server should contact when requesting certificates from using the ACME protocol
###### Default Value: "https://acme-v01.api.letsencrypt.org/directory"

<br><br>

#### ThrowIfAnyIndexCannotBeOpened
###### Indicates if we should throw an exception if any index could not be opened
###### Default Value: false

This is an option that is rarely used, for debugging use only, and only if you have an expert level knowledge of coding. 


#Example:
```
Setup.ThrowIfAnyIndexCannotBeOpened=true
```
