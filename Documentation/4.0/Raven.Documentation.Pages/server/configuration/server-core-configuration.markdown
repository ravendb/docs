Testing string {NOTE this is a note /} and {WARNING warning /} and {INFO information string /} and {DANGER danger stuff /} and {SAFE what is this string /}

## Server Configuration : Core Options

<br><br>

#### ServerUrl
###### The URLs which the server should listen to
###### DefaultValue : "http://localhost:8080"

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
###### DefaultValue : null
Indicates the IP addresses or host addresses with ports and protocols that the server should listen on for incoming TCP connections, used for inter-node communication. If not specified, will use the server url host and random port. If it just a number specify, will use that port. Otherwise, will bind to the host & port specified

Example:

* Server will listen all incoming TCP connections: {WARNING This will expose the server to the outer network /}
```
TcpServerUrl=0.0.0.0:38888
```

<br><br>




