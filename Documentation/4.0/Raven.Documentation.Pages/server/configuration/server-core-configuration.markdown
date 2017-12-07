## Server Configuration : Core Options

<br><br>

#### ServerUrl
###### The URLs which the server should listen to
###### Default Value: "http://localhost:8080"
Indicates the IP addresses or host addresses with ports and protocols that the server should listen on for requests. Use "0.0.0.0" to indicate that the server should listen for requests on any IP address or hostname using the specified port and protocol. The protocol (http:// or https://) must be included with each URL. 

Examples:

* Server will serve all incoming requests: {WARNING This will expose the server to the outer network /}
```
ServerUrl=http://0.0.0.0:8080
```

* Server will serve only local incoming requests:
```
ServerUrl=http://localhost:8080
```

<br><br>

#### TcpServerUrls
###### The TCP URLs which the server should listen to
###### Default Value: null
Indicates the IP addresses or host addresses with ports and protocols that the server should listen on for incoming TCP connections, used for inter-node communication. If not specified, will use the server url host and random port. If it just a number specify, will use that port. Otherwise, will bind to the host & port specified

Example:

* Server will listen all incoming TCP connections: {WARNING This will expose the server to the outer network /}
```
TcpServerUrl=0.0.0.0:38888
```

<br><br>

#### PublicServerUrl
###### The URL under which server is publicly available
###### Default Value: null
Set the public address of the server on which requests will be served. Used for access from behind a firewall, proxy etc.

Examples:

* Use LAN proxy server address 10.0.0.1

```
PublicServerUrl=http://10.0.0.1:80
```

* Use a specific domain

```
PublicServerUrl=http://example.com:8080
```

<br><br>

#### PublicTcpServerUrl
###### The TCP URL under which server is publicly listen to
###### Default Value: null
Set the public TCP address of the server. Used for inter-node communication and access from behind a firewall, proxy etc.


Example:
```
PublicServerUrl=example.com:38888
```

<br><br>

#### RunInMemory
###### Set whether the database should run purely in memory
###### Default Value: false
When running in memory, RavenDB does not write to the disk and if the server is restarted all data will be lost. This is mostly useful for testing.


Example:
```
RunInMemory=true
```

<br><br>

#### DataDirectory
###### The directory for the RavenDB resource
###### Default Value: "Databases/{name}"
Relative path will be located under the application base directory

Example:
```
DataDirectory="/home/user/databases"
```

<br><br>

#### Setup.Mode
###### Determines what kind of security was chosen during setup, or not to use setup on startup at all (SetupMode.None)
###### Default Value: SetupMode.None
Possible values:

- None
- Initial
- LetsEncrypt,
- Secured,
- Unsecured

Example:
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

Example:
```
Setup.ThrowIfAnyIndexCannotBeOpened=true
```




