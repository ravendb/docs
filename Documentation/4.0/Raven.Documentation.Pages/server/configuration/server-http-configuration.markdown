## Server Configuration : Http Options

*RavenDB* uses *Kestrel* Server built in dotnet core. Http configuration options give a way to set *Kestrel's* options. See [Kestrel api](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.server.kestrel?view=aspnetcore-1.1)

<br>

#### MinDataRateBytesPerSec
###### Set Kestrel's minimum required data rate in bytes per second
###### DefaultValue : null
#### MinDataRateGracePeriodInSec
###### Set Kestrel's allowed request and reponse grace in seconds
###### Default Value : null

The http server *Kestrel* checks every second if data is coming in at the specified rate in bytes/second. If the rate drops below the minimum set by *MinResponseDataRate*, the connection is timed out. The grace period *MinDataRateGracePeriod* is the amount of time that *Kestrel* gives the client to increase its send rate up to the minimum. The rate is not checked during that time. The grace period helps avoid dropping connections that are initially sending data at a slow rate due to TCP slow-start.

If not set or set to *null* - rates are set as unlimited.

{NOTE If either *MinDataRateBytesPerSec* or *Http.MinDataRateGracePeriodInSec* are not set or set to null - both of the settings if exist will be ignored and *unlimited* value will be set /}

<br><br>

#### MaxRequestBufferSizeInKb
###### Set Kestrel's MaxRequestBufferSize
###### Default Value : null

Gets or sets the maximum size of the response buffer before write calls begin to block or return tasks that don't complete until the buffer size drops below the configured limit. 

If not set or set to *null* - size is set as unlimited.

<br><br>

#### MaxRequestLineSizeInKb
###### Set Kestrel's MaxRequestLineSize
###### Default Value : 16

Gets or sets the maximum allowed size for the HTTP request line

Example:
```
Http.MaxRequestLineSizeInKb=8
```

<br><br>

#### UseResponseCompression
###### Set whether Raven's HTTP server should compress its responses
###### Default Value : true

Using compression lower the network bandwidth usage.  However in order to debug or view the response via sniffer tools, setting to false is needed. 

Example:
```
Http.UseResponseCompression=false
```

<br><br>

#### AllowResponseCompressionOverHttps
###### Set whether Raven's HTTP server should allow response compression to happen when HTTPS is enabled
###### Default Value : false

{WARNING Setting this to `true` might expose a security risk /}
**See http://breachattack.com/ before enabling this**

<br><br>

#### GzipResponseCompressionLevel
###### Set the compression level to be used when compressing HTTP responses with GZip
###### Default Value : CompressionLevel.Fastest
#### DeflateResponseCompressionLevel
###### Set the compression level to be used when compressing HTTP responses with Deflate
###### Default Value : CompressionLevel.Fastest
#### StaticFilesResponseCompressionLevel
###### Set the compression level to be used when compressing static files
###### Default Value : CompressionLevel.Optimal

Values can be either:

* CompressionLevel.**Optimal**
* CompressionLevel.**Fastest**
* CompressionLevel.**NoCompression**

