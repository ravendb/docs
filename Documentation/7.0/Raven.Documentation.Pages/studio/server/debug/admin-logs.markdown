# Admin Logs
---

{NOTE: }

* Studio's **Admin Logs** view provides a convenient means to watch 
  a live stream of RavenDB's logged data.  
* Log entries can be concisely displayed in a plain raw bulk, or 
  expanded in a structured, user-friendly arrangement that can also 
  be exported to a file.  
* Powerful filters can be applied both to the live log stream and 
  to stored log files.  
  Users can apply their own logic over log entries selected by 
  content, index, exception, origin logger, or any other criteria.  
* Log entries can be orderly downloaded by date from stored log 
  files in compressed archives.  

* In this page:  
  * [Admin Logs view](../../../studio/server/debug/admin-logs#admin-logs-view)  
  * [Logs on this view](../../../studio/server/debug/admin-logs#logs-on-this-view)  
  * [Logs on disk](../../../studio/server/debug/admin-logs#logs-on-disk)  
  * [Log stream](../../../studio/server/debug/admin-logs#log-stream)  

{NOTE/}

---

{PANEL: Admin Logs view}

From Studio's main menu, click **Manage Server**.  
![CLick to open the Debug menu](images/admin-logs_manage-server-option.png "CLick to open the Debug menu")

---

From the debug menu, click **Admin Logs**.  
![Click to open the Admin Logs view](images/admin-logs_admin-logs-menu.png "Click to open the Admin Logs view")

---

![Admin Logs view](images/admin-logs_admin-logs-view.png "Admin Logs view")

1. [Logs on this view](../../../studio/server/debug/admin-logs#logs-on-this-view)  
   Use this section to view and adjust RavenDB's ongoing log stream to your display.  
   You can filter the streamed data and export it in JSON format.  
2. [Logs on disk](../../../studio/server/debug/admin-logs#logs-on-disk)  
   Use this section to customize RavenDB's logging to your local storage.  
   Filters can be applied here too, and you can download selected portions of your logs.  
3. [Log stream](../../../studio/server/debug/admin-logs#log-stream)  
   Use this section to view, search and expand log entries.  

{PANEL/}

{PANEL: Logs on this view}

**Use these controls to**:  

* Adjust [monitor controls](../../../studio/server/debug/admin-logs#monitor-controls).  
* Set the [minimal logging level](../../../studio/server/debug/admin-logs#set-displayed-logs-minimum-level) 
  from which log entries are displayed.  
* Create [additional filters](../../../studio/server/debug/admin-logs#set-displayed-logs-filters) 
  to select the precise data that you want to view.  
* [Export the displayed log data](../../../studio/server/debug/admin-logs#export-displayed-logs-data) 
  to files.  

![Logs on this view](images/admin-logs_logs-on-this-view.png "Logs on this view")

1. **🔻** **Active Filters**  
   A lit funnel symbol indicates that filters were created and are now active.  
   See **Settings** below to learn what filters do and how to create and remove them.  
2. <a id="set-displayed-logs-minimum-level" />**Min level**  
   The minimal [logging level](../../../server/troubleshooting/logging#logging-levels) 
   from which log entries are displayed on this view.  
   Selecting `Warn` (warning), for example, will display log entries from this level 
   on, including `Warn`, `Error`, and `Fatal`.  
3. <a id="monitor-controls" />**Monitor controls**  
    * **Pause** - Pause log entries streaming to the display  
    * **Clear** - Clear the displayed log  
    * **Monitor (tail -f)** - Enable to automatically scroll the log's tail when an 
      entry is added.  
      Clicking an entry and expanding its details automatically disables this option.  
4. <a id="export-displayed-logs-data" />**Export**  
   Expand all log entries and export the log to file in JSON format.  
5. **Settings**  
   Click to create and apply **filters** to select log entries of interest.  
   Be aware that your filters select log entries from the remaining data 
   after your basic **Min level** selection (see no. 2 above) was applied.  

      ![Logs on this view - Settings](images/admin-logs_logs-on-this-view_settings.png "Logs on this view - Settings")
      
      * **A.** **Default Filter Action**  
        Once at least one filter has been defined, a **default filter** can also be created.  
        The default filter determines how log entries should be handled when they either 
        **match no filter**, or all the filters they do match apply a **Neutral** action.  
      * <a id="set-displayed-logs-filters" />**B.** **Filters**  
        Use this section to define filters and to view and remove existing filters.  
         * **Minimum Level** - Log entries are selected from this level on.  
         * **Maximum level** - log entries are selected up to this level.  
         * **Action** - How to handle selected log entries.  
         
              | Action | Meaning |
              | ------ | ------- |
              | `Ignore` | Do not log the selected entries. |
              | `IgnoreFinal` | Do not log the selected entries, and ignore any subsequent filter that matches these entries. |
              | `Log` | Log the selected entries. |
              | `LogFinal` | Log the selected entries, and ignore any subsequent filter that matches these entries. |
              | `Neutral` | Take no immediate action (do not Ignore matching entries nor Log them).<br>If no subsequent filters match the entry or all matching filters are Neutral, the entries will be handled by the _default filter action_. |

         * **Condition**  
           An expression to evaluate log entries by.  
           Hover over the info `ⓘ` symbol for a few examples.  
           Find more information about NLog filter conditions [here](https://github.com/NLog/NLog/wiki/When-filter#conditions).  

         * **🗑️** **Trash bin**  
           Click to remove this filter.  

         * **Add Filter**  
           Click to Add a new filter.  
           Filters are evaluated successively, from the top down.  

      * **C.** **Close**/**Save**  
        _Close_ to leave without applying any changes,  
        or _Save_ to apply your changes and leave.  

{PANEL/}

{PANEL: Logs on disk}

**Use these controls to**:  

* Set the [minimal logging level](../../../studio/server/debug/admin-logs#define-logs-on-disk-minimum-level) 
  from which log entries are stored in log files on the server disk.  
* Create [additional filters](../../../studio/server/debug/admin-logs#define-logs-on-disk-filters) 
  to select the precise data that you want to log.  
* [Download existing log files](../../../studio/server/debug/admin-logs#download-log-files) 
  from the server disk by a date range of your choice.  

![Logs on disk](images/admin-logs_logs-on-disk.png "Logs on disk")

1. **🔻** **Active Filters** (informational)  
   A lit funnel symbol indicates that filters were created and are now active.  
   Use **Settings** (see below) to create, apply and remove filters.  

2. **Min level** (informational)  
   The minimal logging level from which log entries are stored in log files.  
   Use **Settings** (see below) to set the minimal logging level.  

3. <a id="download-log-files" />**Download**  
   Download log entries from the server in a single compressed file of plain text *.log files.  
   ![Logs on disk - Download](images/admin-logs_logs-on-disk_download.png "Logs on disk - Download")

    * **A.** **Select start date**  
      Select a date for the start of the period for which you want to download RavenDB logs.  
      Or, alternatively, toggle **Use minimum start data** ON to load logs starting from 
      the earliest log stored on your disk.  
    * **B.** **Select end date**  
      Select a date for the end of the period for which you want to download RavenDB logs.  
      Or, alternatively, toggle **Use maximum end data** to load logs ending at 
      the latest log stored on your disk.  
    * **C.** **Close**/**Download**  
      Close to leave without downloading logs, or Download logs from the selected period.  
   
5. **Settings**  
   Use this section to set the minimal logging level that you require, and to create and 
   apply logging **filters**.  
   The filters you create will select log entries from the data that remains after 
   your minimal level selection is applied.  

      ![Logs on disk - Select logging target to set](images/admin-logs_logs-on-disk_settings_main.png "Logs on disk - Select logging target to set")

      * Select the logging target you want to customize.  
        Below, we go through the **Logs** section - RavenDB logging settings.  

      ![Logs on disk - Settings](images/admin-logs_logs-on-disk_settings.png "Logs on disk - Settings")
      
      * <a id="define-logs-on-disk-minimum-level" />**Minumum Level**  
        The minimal [logging level](../../../server/troubleshooting/logging#logging-levels) 
        from which log entries will be stored in log files.  
        Selecting `Warn` (warning), for example, will store log entries from this level 
        on, including `Warn`, `Error`, and `Fatal`.  
         * **Save the minimum level in `settings.json`** 
           Mark this checkbox if you want your configuration to be saved in `settings.json`.  
           If you don't save your configuration in `settings.json` it will be overridden by 
           the current configuration on the next server restart.  
      * **B.** **Default Filter Action**  
        Once at least one filter has been defined, a **default filter** can also be applied.  
        The default filter determines how log entries should be handled when they either 
        **match no filter**, or all the filters they do match apply a **Neutral** action.  
      * <a id="define-logs-on-disk-filters" />**C.** **Filters**  
        Use this section to define filters and to view and remove existing filters.  
         * **Minimum Level** - Log entries are selected from this level on.  
         * **Maximum level** - log entries are selected up to this level.  
         * **Action** - How to handle selected log entries.  
         
              | Action | Meaning |
              | ------ | ------- |
              | `Ignore` | Do not log the selected entries. |
              | `IgnoreFinal` | Do not log the selected entries, and ignore any subsequent filter that matches these entries. |
              | `Log` | Log the selected entries. |
              | `LogFinal` | Log the selected entries, and ignore any subsequent filter that matches these entries. |
              | `Neutral` | Take no immediate action (do not Ignore matching entries nor Log them).<br>If no subsequent filters match the entry or all matching filters are Neutral, the entries will be handled by the _default filter action_. |

         * **Condition**  
           An expression to evaluate log entries by.  
           Hover over the info `ⓘ` symbol for a few examples.  
           Find more information about NLog filter conditions [here](https://github.com/NLog/NLog/wiki/When-filter#conditions).  

         * **🗑️** **Trash bin**  
           Click to remove this filter.  

         * **Add Filter**  
           Click to Add a new filter.  
           Filters are evaluated successively, from the top down.  

      * **D.** **Save**  
        Apply your changes and close the settings window.  

{PANEL/}

{PANEL: Log stream}

![Log stream](images/admin-logs_log-stream.png "Log stream")

The log stream area displays log entries as the server creates them, and allows 
you to search and expand specific entries.  

1. **Search**  
   Only log entries that contain the text you enter here will be displayed.  
   Leave empty to display all log entries.  
2. **Expand all**/**Collapse all**  
   Toggle to expand or collapse a structured, easy to read layout of all log entries content.  
3. **Display settings**  
   
      ![Display settings](images/admin-logs_display-settings.png "Display settings")
   
      Enter a value and save your settings to change the maximum number of log entries 
      that can be displayed on this view.  
4. **Log entry**  
   Click a log entry to expand or collapse structured details for this entry, 
   including the entry's date and time, its 
   [logging level](../../../server/troubleshooting/logging#logging-levels), 
   and the [logger](../../../server/troubleshooting/logging#your-own-loggers) 
   that produced it.  
   
      ![Log entry](images/admin-logs_log-entry.png "Log entry")

{PANEL/}

## Related Articles

### Server

- [Logging](../../../server/troubleshooting/logging)

### Configuration

- [Logs options](../../../server/configuration/logs-configuration)
- [Security options](../../../server/configuration/security-configuration)
- [settings.json](../../../server/configuration/configuration-options#settings.json)

### Administration

- [CLI](../../../server/administration/cli)
- [log](../../../server/administration/cli#log)
