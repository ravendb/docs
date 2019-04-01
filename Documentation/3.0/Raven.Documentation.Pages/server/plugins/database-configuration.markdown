# Plugins: Database configuration

To alter database configuration you can edit the configuration document (more about how it can be done and what configuration options are available can be found [here](../../server/configuration/configuration-options)) but sometimes it might be better to change configuration programmatically e.g. imagine a situation, where you have 100 databases and you want to change one setting in each of them. This is why the `IAlterConfiguration` interface was created.

{CODE plugins_5_0@Server\Plugins.cs /}

## Example - Disable compression

{CODE plugins_5_1@Server\Plugins.cs /}

## Related articles

- [Configuration : Options](../configuration/configuration-options)
