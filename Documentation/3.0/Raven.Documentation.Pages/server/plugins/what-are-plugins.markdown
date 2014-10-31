# Plugins : What are plugins?

Under `Raven.Database.Plugins` namespace various interfaces and classes can be found that might be used to extend the database behavior.

{NOTE All DLL's containing custom plugins must be placed in `Plugins` directory that by default are found in `~\Plugins`. To change the default location of this directory, please refer to [this](../../server/configuration/configuration-options#bundles) page. /}

* [**Triggers**](../../server/plugins/triggers) - plugins that grant the ability to manipulate certain actions that are taking place e.g. when document is deleted or index updated.
    * **PUT** triggers
    * **DELETE** triggers
    * **Read** triggers
    * **Index Query** triggers
    * **Index Update** triggers 
* [**Codecs**](../../server/plugins/codecs) - various entry points for custom compression methods.
* [**Tasks**](../../server/plugins/tasks) - server or database startup tasks
* [**Compilation Extensions**](../../server/plugins/compilation-extensions) - entry point for more complex logic used to calculate value of index entry fields.
* [**Analyzer Generators**](../../server/plugins/analyzer-generators) - entry point for creating custom analyzers
* [**Database configuration**](../../server/plugins/database-configuration) - entry point for altering database configuration

## Related articles

TODO
