#RavenDB Administration 101

This article contains fundamentals of RavenDB Server Administration and relevant links with details.

##File locations

The main configuration option which determines root path for RavenDB data is [`Raven/WorkingDir`](../configuration/configuration-options#data-settings).
By default, all path configurations are relative to this setting. Its default value depends on hosting option (it is especially important when IIS is used).

You can either specify relative paths (start with `~\`) or absolute ones. All exposed configuration settings you can find in [Data settings](../configuration/configuration-options#data-settings)
and [Index settings](../configuration/configuration-options#data-settings) sections.

- [Where to put RavenDB data?](../installation/deployment-considerations#where-to-put-ravendb-data)

## Backup and restore

To keep your data save you have to perform database backups. The backup is the binary state of a database and allows to quickly recover from a failure.
Incremental backups are supported, so you can set it up to do full backup every 3 days, and incremental backup every 4 hours for instance.

To run backup you can either use `Raven.Backup` tool or the Studio.

- [Backup and restore](../administration/backup-and-restore)
- [Studio: Manage Your Server : Backup & Restore](../../studio/management/backup-restore)
- [Differences between backup and export](../administration/differences-between-backup-and-export)

## Export and import

Export is a database dump of documents and definitions of indexes and transformers. In order to export / import database use `Raven.Smuggler` tool. It also supports
smuggling data between databases. Another way to export / import `.ravendump` file is to take advantage of the Studio.

Because export / import works based on JSON documents, there is a possibility to transform / filter documents on the fly (not available for backups).
The same like for backups, incremental export is available as well.

- [Exporting and importing data](../administration/exporting-and-importing-data)
- [Studio : Tasks : Import & Export Database](../../studio/overview/tasks/import-export-database)
- [Bundle: Periodic Export](../bundles/periodic-export)
- [Differences between backup and export](../administration/differences-between-backup-and-export)

##Security

RavenDB comes with built-in [authentication and authorization](../configuration/authentication-and-authorization) functionalities.

Also to protect your data, RavenDB offers [data encryption bundle](../bundles/encryption).

- [Bundle: Authorization](../bundles/authorization)
- [KB : Bundles : Authorization Bundle Design](../kb/authorization-bundle-design)
- [Studio : Manage Your Server : Windows Authentication](../../studio/management/windows-authentication)
- [Studio : Walkthroughs : Setting up encryption](../../studio/walkthroughs/how-to-setup-encryption)

## Monitoring

RavenDB 3.0 can be integrated with Pandora FMS to provide monitoring support. Click [here](../administration/monitoring/pandora-fms) for details.

## Logging

If you encounter any problem with RavenDB, probably the first thing you want to know is what is going on inside the server. RavenDB
has extensive support for logging. The first option is to get logs is to enable logs by creating an appropriate config file ([Enabling logging](../troubleshooting/enabling-logging)).
The downside of this is that it requires the server restart, which might be not an option on production system.

The second way is to enable logging in runtime, what can be achieved by Studio feature called [Admin logs](../../studio/management/admin-logs).
You can configure logging there on the fly without the need to restart your server.