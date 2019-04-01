# Plugins: What are plugins?

Under the `Raven.Database.Plugins` namespace, various interfaces and classes that might be used to extend the database behavior can be found.

{NOTE All DLL's containing custom plugins must be placed in `Plugins` directory that by default is found in `~\Plugins`. To change the default location of this directory, please refer to [this](../../server/configuration/configuration-options#bundles) page. /}

* [**Triggers**](../../server/plugins/triggers) - plugins that grant the ability to manipulate certain actions while, for example, a document is being deleted or an index is  being updated.
    * **PUT** triggers
    * **DELETE** triggers
    * **Read** triggers
    * **Index Query** triggers
    * **Index Update** triggers 
* [**Codecs**](../../server/plugins/codecs) - various entry points for custom compression methods.
* [**Tasks**](../../server/plugins/tasks) - server or database startup tasks
* [**Compilation Extensions**](../../server/plugins/compilation-extensions) - entry point for more complex logic, used to calculate value of the index entry fields.
* [**Analyzer Generators**](../../server/plugins/analyzer-generators) - entry point for creating custom analyzers
* [**Database configuration**](../../server/plugins/database-configuration) - entry point for altering database configuration
* [**Custom sorters**](../../indexes/querying/sorting#custom-sorting)
