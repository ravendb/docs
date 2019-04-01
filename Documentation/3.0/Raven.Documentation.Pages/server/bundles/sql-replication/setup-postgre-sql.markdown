# Bundle: SQL Replication: PostgreSQL

To setup a replication to PostgreSQL following steps need to be done:

- Download Npgsql from [here](https://github.com/npgsql/npgsql/releases).
- Copy `Npgsql.dll` to _Plugins_ directory.
- Add following configuration to your **server** configuration file (`Raven.Server.exe.config` or `Web.config`)

{CODE-BLOCK:xml}
<system.data>
	<DbProviderFactories>
		<add name="Npgsql Data Provider" invariant="Npgsql" support="FF" description=".Net Framework Data Provider for Postgresql Server" type="Npgsql.NpgsqlFactory, Npgsql, Version=3.0.3.0, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7" />
	</DbProviderFactories>
</system.data>
{CODE-BLOCK/}

{NOTE Remember that the version in provided factory must match the DLL version that you downloaded. Please change the `3.0.3.0` to the appropriate version if needed. /}

- Restart the server and setup replication according to your needs. Basic setup can be found [here](../../../server/bundles/sql-replication/basics#example).

## Related articles

- [SQL Replication : Basics](../../../server/bundles/sql-replication/basics)
