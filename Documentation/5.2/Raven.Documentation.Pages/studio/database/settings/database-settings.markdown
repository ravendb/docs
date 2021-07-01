# Database Settings

---

{NOTE: }

* Go to **Settings > Database Settings** to view all configuration options concerning the database 
as well as the server and cluster.  

* Some configuration options can be edited from this view. Others can only be edited in the 
[`settings.json` file](../../../server/configuration/configuration-options#json).  

* After you edit a configuration option, the change does not take effect until you either disable 
and re-enable the database, or restart all the servers in the cluster.  

{DANGER: Danger}
Do not modify the database settings unless you are an expert and are sure you know what you're 
doing.  
{DANGER/}

* In this page:  
  * [Database Settings View](../../../studio/database/settings/database-settings#database-settings-view)  
  * [Edit Database Settings](../../../studio/database/settings/database-settings#edit-database-settings)  
      * [Database Settings View with Pending Values](../../../studio/database/settings/database-settings#database-settings-view-with-pending-values)  
      * [How to Reload the Database](../../../studio/database/settings/database-settings#how-to-reload-the-database)  

{NOTE/}

---

{PANEL: Database Settings View }

![Figure 1: Database Settings View](images/database-settings-1.png "Figure 1: Database Settings View")

{WARNING: }

1. Navigate to **Settings > Database Settings**.  
2. Type a keyword here to filter the configuration options that don't contain the keyword.  
3. Click on **Edit** to access the database setting editing view. [See next section](../../../studio/database/settings/database-settings#edit-database-settings).  
4. Lists the configuration options by their configuration key.  
5. The current value of the configuration option. This value doesn't immediately change when 
you edit a configuration value - you must reload the database for the changes to take effect.  
6. This indicates where and how the value of the configuration option is determined:  
  * **Default** - the configuration option currently has its default value.  
  * **Database** - the configuration option's value is determined by the database settings.  
  * **Server** - the configuration option's value is determined by the [`settings.json` file](../../../server/configuration/configuration-options#json).  

{WARNING/}

{PANEL/}

{PANEL: Edit Database Settings }

Only some configuration options can be edited from this view. Most can only be edited in the 
[`settings.json` file](../../../server/configuration/configuration-options#json).  

Changes made to the configuration options do not take effect until the database is reloaded. 
There are two ways to reload the database:  

* Disable and then enable the database from the database list view in the Studio. This will 
reload the database on all the cluster nodes immediately.  
* Restart RavenDB on each of the cluster nodes. The database settings configuration will 
take effect on each node once RavenDB is restarted.  

![Figure 2: Edit Database Settings](images/database-settings-2.png "Figure 2: Edit Database Settings")

{WARNING: }

1. Type a keyword here to filter the configuration options that don't contain the keyword.  
2. Configuration options are divided into categories according to the aspect of RavenDB that 
they affect.  
3. Lists the configuration options in the selected category by their configuration key.  
4. The value of the configuration option _as it was most recently saved._ Values entered here 
will not take effect until the database is reloaded, but they will remain the same after you 
save them.  
5. This lock icon indicates whether it is possible to edit the configuration option here in 
the database settings view. Most options cannot be edited here.  
6. Toggle whether to override the current value of the configuration option.  
7. Click on `Set default` to restore the default value of the configuration option. This too 
only takes effect after the database is reloaded.  
8. Save the changes you have made to the settings. These do not take effect until the database 
is reloaded in one of the following ways:  
  * Disable and re-enable the database.  
  * Restart RavenDB on each node in the cluster.  

{WARNING/}
<br/>
### Database Settings View with Pending Values

![Figure 3: Database Settings with Pending Values](images/database-settings-3.png "Figure 3: Database Settings with Pending Values")

After you edit the configuration options and save your changes, the main database settings 
view changes slightly: the `Origin` column is replaced by the `Pending Value` column.

{WARNING: }

1. The current value of the configuration option. This value doesn't immediately change when 
you edit a configuration value - you must reload the database for the changes to take effect.  
2. The pending value of the configuration option. This is the value that you set in the 
database setting editing view. As soon as the database is reloaded, the pending value replaces 
the effective value.  

{WARNING/}
<br/>
### How to Reload the Database

![Figure 4: How to Reload the Database](images/database-settings-4.png "Figure 3: How to Reload the Database")

The easiest way to make your changes to the database settings take effect is to enable and 
disable the database.  

{WARNING: }

1. Navigate to the **Database List View**.  
2. Click **Disable** to disable the database. This makes the database inaccessible and halts 
all processes such as indexing. Once the database is disabled, this button changes to **Enable**. 
Click on it again to enable the database.  

{WARNING/}

{PANEL/}

## Related Articles  

### Studio  

- [Database Record](../../../studio/database/settings/database-record)  
- [Database List View](../../../studio/database/databases-list-view)  

### Server  

- [Configuration Options](../../../server/configuration/configuration-options)  
