## Security Configuration

The following configuration values allow you to control the desired level of security in a RavenDB server. For a more adetailed explanation, please visit the [Security Section](../security/authentication/certificate-configuration).

<strong>Security.Certificate.Path</strong>  
The path to .pfx certificate file. If specified, RavenDB will use HTTPS/SSL for all network activities.  
DefaultValue : null  

<strong>Security.Certificate.Exec</strong>  
A command or executable providing a .pfx certificate file. If specified, RavenDB will use HTTPS/SSL for all network activities.  
DefaultValue : null  
{NOTE The 'Security.Certificate.Path' setting has a higher priority than 'Security.Certificate.Exec' if both are defined. /}

<strong>Security.Certificate.Exec.Arguments</strong>  
The command line arguments for the 'Security.Certificate.Exec' command or executable.  
DefaultValue : null  

<strong>Security.Certificate.Exec.TimeoutInSec</strong>  
The number of seconds to wait for the certificate executable to exit.  
DefaultValue : 30 seconds  

<strong>Security.MasterKey.Path</strong>  
The path of the (512-bit) Master Key. If specified, RavenDB will use this key to protect secrets.  
DefaultValue : null  

<strong>Security.MasterKey.Exec</strong>  
A command or executable to run which will provide a (512-bit) Master Key, If specified, RavenDB will use this key to protect secrets.  
DefaultValue : null  
{NOTE The 'Security.MasterKey.Path' setting has a higher priority than 'Security.MasterKey.Exec' if both are defined. /}

<strong>Security.MasterKey.Exec.Arguments</strong>  
The command line arguments for the 'Security.MasterKey.Exec' command or executable.  
DefaultValue : null  

<strong>Security.MasterKey.Exec.TimeoutInSec</strong>  
The number of seconds to wait for the Master Key executable to exit.  
DefaultValue : 30 seconds  

<strong>Security.UnsecuredAccessAllowed</strong>  
If authentication is disabled, set address range type for which server access is unsecured (None | Local | PrivateNetwork | PublicNetwork).  
DefaultValue : Local  
