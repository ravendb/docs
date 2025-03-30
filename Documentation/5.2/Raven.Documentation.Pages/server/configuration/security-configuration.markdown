# Configuration: Security
---

{NOTE: }

* The following configuration keys allow you to control the desired level of security in a RavenDB server.  
  To learn more about RavenDB's security features, see this [security overview](../../server/security/overview).

* In this page:
  * Security.AuditLog:  
    [Security.AuditLog.FolderPath](../../server/configuration/security-configuration#security.auditlog.folderpath)  
    [Security.AuditLog.RetentionTimeInHours](../../server/configuration/security-configuration#security.auditlog.retentiontimeinhours)
  * Security.Certificate:  
    [Security.Certificate.Path](../../server/configuration/security-configuration#security.certificate.path)  
    [Security.Certificate.Password](../../server/configuration/security-configuration#security.certificate.password)  
    [Security.Certificate.Exec](../../server/configuration/security-configuration#security.certificate.exec)  
    [Security.Certificate.Exec.Arguments](../../server/configuration/security-configuration#security.certificate.exec.arguments)  
    [Security.Certificate.Load.Exec](../../server/configuration/security-configuration#security.certificate.load.exec)  
    [Security.Certificate.Renew.Exec](../../server/configuration/security-configuration#security.certificate.renew.exec)  
    [Security.Certificate.Change.Exec](../../server/configuration/security-configuration#security.certificate.change.exec)  
    [Security.Certificate.Load.Exec.Arguments](../../server/configuration/security-configuration#security.certificate.load.exec.arguments)  
    [Security.Certificate.Renew.Exec.Arguments](../../server/configuration/security-configuration#security.certificate.renew.exec.arguments)  
    [Security.Certificate.Change.Exec.Arguments](../../server/configuration/security-configuration#security.certificate.change.exec.arguments)  
    [Security.Certificate.Exec.TimeoutInSec](../../server/configuration/security-configuration#security.certificate.exec.timeoutinsec)  
    [Security.Certificate.LetsEncrypt.Email](../../server/configuration/security-configuration#security.certificate.letsencrypt.email)  
  * Security.MasterKey:  
    [Security.MasterKey.Path](../../server/configuration/security-configuration#security.masterkey.path)  
    [Security.MasterKey.Exec](../../server/configuration/security-configuration#security.masterkey.exec)  
    [Security.MasterKey.Exec.Arguments](../../server/configuration/security-configuration#security.masterkey.exec.arguments)  
    [Security.MasterKey.Exec.TimeoutInSec](../../server/configuration/security-configuration#security.masterkey.exec.timeoutinsec)  
  * Other:  
    [Security.UnsecuredAccessAllowed](../../server/configuration/security-configuration#security.unsecuredaccessallowed)
    [Security.DoNotConsiderMemoryLockFailureAsCatastrophicError](../../server/configuration/security-configuration#security.donotconsidermemorylockfailureascatastrophicerror)  
    [Security.DisableHttpsRedirection](../../server/configuration/security-configuration#security.disablehttpsredirection)  
    [Security.WellKnownCertificates.Admin](../../server/configuration/security-configuration#security.wellknowncertificates.admin)  
    [Security.WellKnownIssuers.Admin](../../server/configuration/security-configuration#security.wellknownissuers.admin)  
    [Security.TlsCipherSuites](../../server/configuration/security-configuration#security.tlsciphersuites)  
  
{NOTE/}

---

{PANEL: Security.AuditLog.FolderPath}

The path to a folder where RavenDB will store the access audit logs.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.AuditLog.RetentionTimeInHours}

How far back we should retain audit log entries.

- **Type**: `int`
- **Default**: `365 * 24`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.Certificate.Path}

The path to .pfx certificate file. If specified, RavenDB will use HTTPS/SSL for all network activities.   
Certificate setting priority order:   
1. Path   
2. Executable   

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.Certificate.Password}

The (optional) password of the .pfx certificate file.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.Certificate.Exec}

Deprecated in 4.2 and replaced with [Security.Certificate.Load.Exec](../../server/configuration/security-configuration#security.certificate.load.exec), see below.

{PANEL/}

{PANEL: Security.Certificate.Exec.Arguments}

Deprecated in 4.2 and replaced with [Security.Certificate.Load.Exec.Arguments](../../server/configuration/security-configuration#security.certificate.load.exec.arguments), see below.

{PANEL/}

{PANEL: Security.Certificate.Load.Exec}

A command or executable that provides the .pfx cluster certificate when invoked by RavenDB. If specified, RavenDB will use HTTPS/SSL for all network activities. 
The `Security.Certificate.Path` setting takes precedence over this executable.  

- **Type**: `string`  
- **Default**: `null`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL: Security.Certificate.Renew.Exec}

A command or executable that handles automatic cluster certificate renewals and passes the renewed .pfx certificate to the rest of the cluster. The 
[leader node](../../server/clustering/rachis/cluster-topology#leader) will invoke this executable once every hour and if a new certificate is received, it 
will be sent to the other nodes. The executable specified in `Security.Certificate.Change.Exec` will then be used to persist the certificate across the cluster.  

- **Type**: `string`  
- **Default**: `null`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL: Security.Certificate.Change.Exec}

A command or executable that replaces the old cluster certificate with the renewed cluster certificate. After `Security.Certificate.Change.Exec` distributes the 
new certificate, this executable persists it locally at each follower node.

- **Type**: `string`  
- **Default**: `null`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL: Security.Certificate.Load.Exec.Arguments}

The command line arguments for the 'Security.Certificate.Load.Exec' command or executable.  

- **Type**: `string`  
- **Default**: `null`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL: Security.Certificate.Renew.Exec.Arguments}

The command line arguments for the 'Security.Certificate.Renew.Exec' command or executable.  

- **Type**: `string`  
- **Default**: `null`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL: Security.Certificate.Change.Exec.Arguments}

The command line arguments for the 'Security.Certificate.Change.Exec' command or executable.  

- **Type**: `string`  
- **Default**: `null`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL: Security.Certificate.Exec.TimeoutInSec}

The number of seconds to wait for the certificate executables to exit.  
Applies to: `Security.Certificate.Load.Exec`, `Security.Certificate.Renew.Exec`, and `Security.Certificate.Change.Exec`.  

- **Type**: `int`  
- **Default**: `30`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL: Security.Certificate.LetsEncrypt.Email}

The E-mail address associated with the Let's Encrypt certificate. Used for renewal requests.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.MasterKey.Path}

The path of the (256-bit) Master Key. If specified, RavenDB will use this key to protect secrets.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.MasterKey.Exec}

A command or executable to run which will provide a (256-bit) Master Key, If specified, RavenDB will use this key to protect secrets.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.MasterKey.Exec.Arguments}

The command line arguments for the 'Security.MasterKey.Exec' command or executable. 

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.MasterKey.Exec.TimeoutInSec}

The number of seconds to wait for the Master Key executable to exit.

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.UnsecuredAccessAllowed}

If authentication is disabled, set the address range type for which server access is unsecured (`None | Local | PrivateNetwork | PublicNetwork`).

- **Type**: `flags`
- **Default**: `Local`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.DoNotConsiderMemoryLockFailureAsCatastrophicError}

Determines whether RavenDB will consider memory lock error to be catastrophic. This is used with encrypted databases to ensure that temporary buffers are never written to disk and are locked to memory. Setting this to true is not recommended and should be done only after a proper security analysis has been performed.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide or per database

{WARNING Use with caution. /}

{PANEL/}

{PANEL: Security.DisableHttpsRedirection}

Disable automatic redirection when listening to HTTPS. By default, when using port 443, RavenDB redirects all incoming HTTP traffic on port 80 to HTTPS on port 443.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.WellKnownCertificates.Admin}

Allow to specify well known certificate thumbprints that will be trusted by the server as cluster admins.

- **Type**: `strings separated by ;`
- **Example**: `297430d6d2ce259772e4eccf97863a4dfe6b048c;e6a3b45b062d509b3382282d196efe97d5956ccb`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.WellKnownIssuers.Admin}

Specify well-known issuer certificates in Base64 format or provide file paths to the certificate files.  
This will be used to validate a new client certificate when the issuer's certificate changes.

- **Type**: `string[]` or `string with values separated by ;`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Security.TlsCipherSuites}

{DANGER: For experts, use with caution}
Defines a list of supported TLS Cipher Suites. Values must be semicolon separated.
{DANGER/}

- **Type**: `TlsCipherSuite[]`
- **Example**: `TLS_RSA_WITH_RC4_128_MD5;TLS_RSA_WITH_RC4_128_SHA`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}
