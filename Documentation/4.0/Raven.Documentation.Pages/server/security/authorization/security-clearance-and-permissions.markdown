# Security Clearance and Permissions (first draft)

When the server generates a client certificate (or is told to trust an existing one), it saves it as a trusted certificate in the cluster. All the nodes of the cluster will accept requests from a client with that certificate.

The certificate is used for authentication, validating that users are who they say they are. Once a request is authenticated, RavenDB determines the security clearance and permissions for that certificate and allows access according to permissions the admin assigned when setting up the client certificate. 

A client certificate's security clearance can be one of the following:

"Cluster Admin"
"Operator"
"User"

*The server certificate is called "Cluster Node" in the UI.

"Cluster Admin" is the highest security clearance. There are no restrictions. A "Cluster Admin" has admin permissions to all of the databases in the cluster. It also has the ability to modify the cluster itself.

An "Operator" can also manage all the databases, but is unable to modify the cluster itself. It cannot perform operations such as add/remove/promote/demote nodes from the cluster, using the JS admin console, and managing certificates with "Cluster Admin" clearance. 

A certificate with "Operator" clearance is typically used in a hosted solution. If you are running on your own machines, you'll typically ignore that level in favor of "Cluster Admin" or "User"

A certificate with a "User" security clearance can only access specific databases, and only with specific permissions assigned to that certificate. It cannot perform any admin operations at the cluster level. 


The type of permissions given to a "User" certificate are "Database Admin" or "read/write". Different permissions are given to a "User" for different databases. A "User" cannot access databases which are not defined in its permissions. The one who sets these permissions is the one that generates the "User" client certificate (either "Cluster Admin" or "Operator").

See the full [list of security clearances](list-of-security-clearances) and allowed operations.
