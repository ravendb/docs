
# Time Series Settings

---

{NOTE: }

* The **Time Series Settings** view allows you to define time series 
  [Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention) 
  policies, and [Name](../../../document-extensions/timeseries/client-api/named-time-series-values) 
  time series values.  

* Time series Policies and Named Values are defined **per collection**.  
  
* [Rollup](../../../document-extensions/timeseries/rollup-and-retention#what-are-rollups) 
  is the aggregation of time series entries into **new time series** comprised only of the 
  aggregated data.  

* **Retention** is the ongoing removal of time series entries that exceeded a given age.  

* Time series values can be **Named** to clarify their role.  
  API methods can [access](../../../document-extensions/timeseries/client-api/named-time-series-values#usage-samples) 
  time series values by their names.  

* In this page:  
  * [Time Series Settings View](../../../)  
  * [Enabling Policies and Setting Frequency](../../../)  
  * [](../../../)  
  * [](../../../)  
  * [](../../../)  
  * [](../../../)  
{NOTE/}

---

{PANEL: Time Series Settings View}

!["Time Series Settings View"](images/time-series-settings-01_open-view.png "Time Series Settings View")

1. **Time Series Settings View**  
   Select **Settings** > **Time Series** to open the Time Series Settings View.  
2. **Enable/Disable Rollup & Retention Policies**  
    * Use the **Selection Box** to select/deselect all Rollup and Retention policies  
    * Use the **Set status** dropdown list to enable/disable selected  policies  
      !["Set Status"](images/time-series-settings-02_enable-disable.png "Set Status")  
3. **Policy Check Frequency**  
   Set the frequency by which the server checks Rollup and Retention policies.  
    {NOTE: }
     Make sure policies are checked frequently enough to implement your policies.  
     E.g. if you define a Rollup policy for one of your collections, that creates 
     a rollup time series every 2 minutes, make sure your Policy Check Frequency 
     is not longer than 2 minutes.  
    {NOTE/}
4. **View and Manage Defined Time Series Configurations**  
    * Configurations are defined per-collection.  
5. **Save**  
   Click to save configuration changes and additions.  
6. **Add or Edit time series configurations.  
    * Click the **Add a collection specific configuration** button to create a new configuration.  
    * Click a defined configuration **Edit** button to edit an existing configuration.  


---


{PANEL/}

## Related Articles

### Integrations

- [PostgreSQL Protocol - Overview](../../../integrations/postgresql-protocol/overview)  
- [PostgreSQL Protocol - Power BI](../../../integrations/postgresql-protocol/power-bi)  
