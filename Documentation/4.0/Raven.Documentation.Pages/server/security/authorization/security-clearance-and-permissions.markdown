# Security Clearance and Permissions

X.509 certificates are used for authentication - validating that users are who they say they are. Once a request is authenticated, RavenDB uses the certificate for authorization as well. 

Each certificate is associated with a security clearance and access permissions per database.

It is the administrator's responsibility to generate client certificates and assign permissions.

A client certificate's security clearance can be one of the following:

#### Cluster Admin

`Cluster Admin` is the highest security clearance. There are no restrictions. A `Cluster Admin` certificate has admin permissions to all of the databases in the cluster. It also has the ability to modify the cluster itself.

{NOTE The server certificate security cleatrance is called `Cluster Node` and is equivalent to `Cluster Admin`. /}

#### Operator

A client certificate with an `Operator` security clearance has admin access to <strong>all</strong> databases, but is unable to modify the cluster. It cannot perform operations such as add/remove/promote/demote nodes from the cluster, using the JS admin console, and managing certificates with `Cluster Admin` security clearance. This is useful in a hosted solution. If you are running on your own machines, you'll typically ignore that level in favor of `Cluster Admin` or `User`.

#### User

A `User` client certificate has a list of databases it is allowed to access. In addition, the access level to each database can be either `Database Admin` or `read/write`. A `User` certificate cannot perform any admin operations at the cluster level.
