# Audit Log

We previously discussed [Authorization](../authorization/security-clearance-and-permissions), which is controlling who can get into RavenDB and what they can access.

In addition, RavenDB also provides an optional **Audit Log**, which is the ability to keep track of who has connected to the system and when. 

RavenDB supports the process of access audits at the level of database connections.

## Enabling the Audit Log

Access the [settings.json](../../configuration/configuration-options#json) file and set the following configuration values: 

* Security.AuditLog.FolderPath 
* Security.AuditLog.RetentionTimeInHours (optional, default is one year)

## What is being logged

Once the configuration values are set, RavenDB will use a dedicated audit log to record:

* Every time a connection is made to RavenDB
* Every time a connection to RavenDB is closed
* What certificate was used and what privileges it was granted
* When a connection is rejected by RavenDB as invalid 
* When a database is created or removed (from all nodes or a single node)
* When an index is created or removed. 

## Things to consider

The audit log folder will contain the audit entries and can be loaded into centralized audit and analysis by dedicated tools. 

RavenDB does nothing with the audit logs except write them. 

It is important to understand that the audit logs are local. That is, if we have a database residing on node C which is removed by a command originating on node B, 
the audit entry will be in the audit log of node B, not node C. 

Also note that RavenDB writes connections to the audit log, not requests. This is done for performance and manageability reasons. Otherwise, you'd have extremely large and unwieldy audit logs. 

With HTTP 1.1, a single TCP connection is used for many different requests. The only items you'll find in the audit log are the time of the TCP connection, the certificate being used 
and the level of access granted to this certificate at the time of the connection. If you require more detailed logs (at the level of individual HTTP requests), 
you can use a proxy in front of RavenDB that will log the appropriate requests as they are being made.

## Related articles

### Security

- [Overview](../../../server/security/authorization/security-clearance-and-permissions)
- [Authorization](../authorization/security-clearance-and-permissions)
- [Security Configuration](../../configuration/security-configuration)
- [Common Errors and FAQ](../../../server/security/common-errors-and-faq)
