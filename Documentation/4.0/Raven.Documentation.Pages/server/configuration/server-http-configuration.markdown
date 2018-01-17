## Server Configuration : Http Options

*RavenDB* uses *Kestrel* Server built in dotnet core. HTTP configuration options give a way to set *Kestrel's* options. See [Kestrel API](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.server.kestrel?view=aspnetcore-1.1)

<br>

{PANEL:Http.MinDataRateBytesPerSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Http.MinDataRateBytesPerSec | Set Kestrel's minimum required data rate in bytes per second | `null` | Server-wide only |

{PANEL/}

{PANEL:Http.MinDataRateGracePeriodInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Http.MinDataRateGracePeriodInSec | Set Kestrel's allowed request and response grace in seconds | `null` | Server-wide only |

The http server *Kestrel* checks every second if data is coming in at the specified rate in bytes/second. If the rate drops below the minimum set by *MinResponseDataRate*, the connection is timed out. The grace period *MinDataRateGracePeriod* is the amount of time that *Kestrel* gives the client to increase its send rate up to the minimum. The rate is not checked during that time. The grace period helps avoid dropping connections that are initially sending data at a slow rate due to TCP slow-start.

If not set or set to *null* - rates are set as unlimited.

{NOTE If either *MinDataRateBytesPerSec* or *Http.MinDataRateGracePeriodInSec* are not set or set to null - both of the settings if exist will be ignored and *unlimited* value will be set /}

{PANEL/}

{PANEL:Http.MaxRequestBufferSizeInKb}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Http.MaxRequestBufferSizeInKb | Set Kestrel's MaxRequestBufferSize | `null` | Server-wide only |

Gets or sets the maximum size of the response buffer before write calls begin to block or return tasks that don't complete until the buffer size drops below the configured limit. 

If not set or set to *null* - size is set as unlimited.

{PANEL/}

{PANEL:Http.MaxRequestLineSizeInKb}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Http.MaxRequestLineSizeInKb | Set Kestrel's MaxRequestLineSize | `16` | Server-wide only |

Gets or sets the maximum allowed size for the HTTP request line

{PANEL/}

{PANEL:Http.UseResponseCompression}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Http.UseResponseCompression | Set whether Raven's HTTP server should compress its responses | `true` | Server-wide only |

Using compression lower the network bandwidth usage.  However in order to debug or view the response via sniffer tools, setting to false is needed. 

{PANEL/}

{PANEL:Http.AllowResponseCompressionOverHttps}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Http.AllowResponseCompressionOverHttps | Set whether Raven's HTTP server should allow response compression to happen when HTTPS is enabled | `false` | Server-wide only |

{WARNING Setting this to `true` might expose a security risk. See **http://breachattack.com/** before enabling this. /}

{PANEL/}

{PANEL:Http.GzipResponseCompressionLevel}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Http.GzipResponseCompressionLevel | Set the compression level to be used when compressing HTTP responses with GZip | `Fastest` | Server-wide only |

{PANEL/}

{PANEL:Http.DeflateResponseCompressionLevel}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Http.DeflateResponseCompressionLevel | Set the compression level to be used when compressing HTTP responses with Deflate | `Fastest` | Server-wide only |

{PANEL/}

{PANEL:Http.DeflateResponseCompressionLevel}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Http.DeflateResponseCompressionLevel | Set the compression level to be used when compressing HTTP responses with Deflate | `Fastest` | Server-wide only |

{PANEL/}

{PANEL:Http.StaticFilesResponseCompressionLevel}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Http.StaticFilesResponseCompressionLevel | Set the compression level to be used when compressing static files | `Optimal` | Server-wide only |

Values can be either:

* `Optimal`
* `Fastest`
* `NoCompression`

{PANEL/}
