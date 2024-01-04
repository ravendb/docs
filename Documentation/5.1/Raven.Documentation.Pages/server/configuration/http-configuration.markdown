# Configuration: HTTP
---

{NOTE: }

* RavenDB uses [Kestrel](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.server.kestrel?view=aspnetcore-8.0), which is an HTTP web server built on ASP.NET Core.

* You can set Kestrel's properties via the following RavenDB configuration keys.

* In this page:
  * [Http.MinDataRateBytesPerSec](../../server/configuration/http-configuration#http.mindataratebytespersec)
  * [Http.MinDataRateGracePeriodInSec](../../server/configuration/http-configuration#http.mindatarategraceperiodinsec)
  * [Http.MaxRequestBufferSizeInKb](../../server/configuration/http-configuration#http.maxrequestbuffersizeinkb)
  * [Http.MaxRequestLineSizeInKb](../../server/configuration/http-configuration#http.maxrequestlinesizeinkb)
  * [Http.UseResponseCompression](../../server/configuration/http-configuration#http.useresponsecompression)
  * [Http.AllowResponseCompressionOverHttps](../../server/configuration/http-configuration#http.allowresponsecompressionoverhttps)
  * [Http.GzipResponseCompressionLevel](../../server/configuration/http-configuration#http.gzipresponsecompressionlevel)
  * [Http.DeflateResponseCompressionLevel](../../server/configuration/http-configuration#http.deflateresponsecompressionlevel)
  * [Http.StaticFilesResponseCompressionLevel](../../server/configuration/http-configuration#http.staticfilesresponsecompressionlevel)
  * [Http.Protocols](../../server/configuration/http-configuration#http.protocols)
  * [Http.AllowSynchronousIO](../../server/configuration/http-configuration#http.allowsynchronousio)

{NOTE/}

---

{PANEL: Http.MinDataRateBytesPerSec}

* Set Kestrel's minimum required data rate in bytes per second.  

* This option must be configured together with [Http.MinDataRateGracePeriod](../../server/configuration/http-configuration#http.mindatarategraceperiodinsec).  

---

- **Type**: `int`
- **Default**: `null`
- **Scope**: Server-wide only
- **Used for setting Kestrel's properties**:
    - [MinResponseDataRate](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.server.kestrel.core.kestrelserverlimits.minresponsedatarate?view=aspnetcore-8.0#microsoft-aspnetcore-server-kestrel-core-kestrelserverlimits-minresponsedatarate)
    - [MinRequestBodyDataRate](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.server.kestrel.core.kestrelserverlimits.minrequestbodydatarate?view=aspnetcore-8.0)

{PANEL/}

{PANEL: Http.MinDataRateGracePeriodInSec}

* Set Kestrel's allowed request and response grace period in seconds.  
  This option must be configured together with [Http.MinDataRateBytesPerSec](../../server/configuration/http-configuration#http.mindataratebytespersec)

* Kestrel checks every second if data is coming in at the specified rate in bytes/second.  
  If the rate drops below the minimum set by _MinResponseDataRate_, the connection is timed out.

* The grace period _Http.MinDataRateGracePeriodInSec_ is the amount of time that Kestrel gives the client to increase its send rate up to the minimum. The rate is not checked during that time. 
  The grace period helps avoid dropping connections that are initially sending data at a slow rate due to TCP slow-start.

* When set to `null` then rates are unlimited, no minimum data rate will be enforced.

---

- **Type**: `int`
- **Default**: `null`
- **Scope**: Server-wide only
- **Used for setting Kestrel's properties**:
    - [MinResponseDataRate](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.server.kestrel.core.kestrelserverlimits.minresponsedatarate?view=aspnetcore-8.0#microsoft-aspnetcore-server-kestrel-core-kestrelserverlimits-minresponsedatarate)
    - [MinRequestBodyDataRate](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.server.kestrel.core.kestrelserverlimits.minrequestbodydatarate?view=aspnetcore-8.0)

{WARNING: }
If __either__ one of _Http.MinDataRateBytesPerSec_ or _Http.MinDataRateGracePeriodInSec_ is Not set or set to `null`,  
then __both__ Kestrel's properties ( _MinResponseDataRate_ & _MinRequestBodyDataRate_ ) will be set to `null`.
{WARNING/}

{PANEL/}

{PANEL: Http.MaxRequestBufferSizeInKb}

* Set the maximum size of the response buffer before write calls begin to block or return tasks that don't complete until the buffer size drops below the configured limit.
  
* If not set, or set to `null`, then the size of the request buffer is unlimited.

---

- **Type**: `int`
- **Default**: `null`
- **Scope**: Server-wide only
- **Used for setting Kestrel property**: [MaxRequestBufferSize](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.server.kestrel.core.kestrelserverlimits.maxrequestbuffersize?view=aspnetcore-8.0#microsoft-aspnetcore-server-kestrel-core-kestrelserverlimits-maxrequestbuffersize)

{PANEL/}

{PANEL: Http.MaxRequestLineSizeInKb}

Set the maximum allowed size for the HTTP request line.

- **Type**: `int`
- **Default**: `16`
- **Scope**: Server-wide only
- **Used for setting Kestrel property**: [MaxRequestLineSize](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.server.kestrel.core.kestrelserverlimits.maxrequestlinesize?view=aspnetcore-8.0#microsoft-aspnetcore-server-kestrel-core-kestrelserverlimits-maxrequestlinesize)

{PANEL/}

{PANEL: Http.UseResponseCompression}

* Set whether Raven's HTTP server should compress its responses.

* Using compression lowers the network bandwidth usage.   
  However, setting to `false` is needed in order to debug or view the response via sniffer tools.

---

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Http.AllowResponseCompressionOverHttps}

* Set whether Raven's HTTP server should allow response compression to happen when HTTPS is enabled.

* Please see http://breachattack.com/ before enabling this.

---

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only
- **Used for setting Kestrel property**: [EnableForHttps](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.responsecompression.responsecompressionoptions.enableforhttps?view=aspnetcore-8.0#microsoft-aspnetcore-responsecompression-responsecompressionoptions-enableforhttps)

{PANEL/}

{PANEL: Http.GzipResponseCompressionLevel}

Set the compression level to be used when compressing HTTP responses with GZip.

- **Type**: `enum CompressionLevel` (`Optimal`, `Fastest`, `NoCompression`, `SmallestSize`)
- **Default**: `Fastest`
- **Scope**: Server-wide only
- **Used for setting Kestrel property**: [Level](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.responsecompression.gzipcompressionprovideroptions.level?view=aspnetcore-8.0#microsoft-aspnetcore-responsecompression-gzipcompressionprovideroptions-level)

{PANEL/}

{PANEL: Http.DeflateResponseCompressionLevel}

Set the compression level to be used when compressing HTTP responses with Deflate.

- **Type**: `enum CompressionLevel` (`Optimal`, `Fastest`, `NoCompression`, `SmallestSize`)
- **Default**: `Fastest`
- **Scope**: Server-wide only
- **Used for setting Kestrel property**: [Level](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.responsecompression.gzipcompressionprovideroptions.level?view=aspnetcore-8.0#microsoft-aspnetcore-responsecompression-gzipcompressionprovideroptions-level)

{PANEL/}

{PANEL: Http.StaticFilesResponseCompressionLevel}

Set the compression level to be used when compressing static files.

- **Type**: `enum CompressionLevel` (`Optimal`, `Fastest`, `NoCompression`, `SmallestSize`)
- **Default**: `Optimal`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Http.Protocols}

* Set HTTP protocols that should be supported by the server.

* By default, the HTTP protocol is set by the constructor of class `HttpConfiguration`  
  (that is what is meant by the value `"DefaultValueSetInConstructor"`).

* If the platform running RavenDB is either Windows 10 or higher, Windows Server 2016 or newer, or POSIX,  
  the constructor sets Http.Protocols to `Http1AndHttp2`. Otherwise, it is set to `Http1`.

---

- **Type**: `enum HttpProtocols` ( `None`, `Http1`, `Http2`, `Http1AndHttp2`, `Http3`, `Http1AndHttp2AndHttp3`)
- **Default**: `DefaultValueSetInConstructor`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Http.AllowSynchronousIO}

Set a value that controls whether synchronous IO is allowed for the Request and Response.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only
- **Used for setting Kestrel property**: [AllowSynchronousIO](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.server.kestrel.core.kestrelserveroptions.allowsynchronousio?view=aspnetcore-8.0#microsoft-aspnetcore-server-kestrel-core-kestrelserveroptions-allowsynchronousio)

{PANEL/}
