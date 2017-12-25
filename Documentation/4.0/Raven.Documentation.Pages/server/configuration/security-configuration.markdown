## Security Configuration

The following configuration values allow you to control the desired level of security in a RavenDB server. For a more detailed explanation, please visit the [Security Section](../security/overview).

<strong>Security.Certificate.Path</strong>  
The path to the .pfx certificate file. If specified, RavenDB will use HTTPS/SSL for all network activities.  
Default: null  

<strong>Security.Certificate.Exec</strong>  
A command or executable providing a .pfx certificate file. If specified, RavenDB will use HTTPS/SSL for all network activities.  
Default: null  
{NOTE The 'Security.Certificate.Path' setting has a higher priority than 'Security.Certificate.Exec' if both are defined. /}

<strong>Security.Certificate.Exec.Arguments</strong>  
The command line arguments for the 'Security.Certificate.Exec' command or executable.  
Default: null  

<strong>Security.Certificate.Exec.TimeoutInSec</strong>  
The number of seconds to wait for the certificate executable to exit.  
Default: 30 seconds  

<strong>Security.MasterKey.Path</strong>  
The path of the (512-bit) Master Key. If specified, RavenDB will use this key to protect secrets.  
Default: null  

<strong>Security.MasterKey.Exec</strong>  
A command or executable to run which will provide a (512-bit) Master Key, If specified, RavenDB will use this key to protect secrets.  
Default: null  
{NOTE The 'Security.MasterKey.Path' setting has a higher priority than 'Security.MasterKey.Exec' if both are defined. /}

<strong>Security.MasterKey.Exec.Arguments</strong>  
The command line arguments for the 'Security.MasterKey.Exec' command or executable.  
Default: null  

<strong>Security.MasterKey.Exec.TimeoutInSec</strong>  
The number of seconds to wait for the Master Key executable to exit.  
Default: 30 seconds  

<strong>Security.UnsecuredAccessAllowed</strong>  
If authentication is disabled, set the address range type for which server access is unsecured (None | Local | PrivateNetwork | PublicNetwork).  
Default: Local  
