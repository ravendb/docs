﻿
# Time Series Settings

---

{NOTE: }

* The **Time Series Settings view** allows you to define and manage time series 
  configurations per collection. Configurations may include:  

    * [Named Values](../../../document-extensions/timeseries/client-api/named-time-series-values)  
      A customized name can be set per time series value.  
      API methods can access time series values by their names.  
    
    * [Rollup Policies](../../../document-extensions/timeseries/rollup-and-retention#what-are-rollups)  
      Rollup policies define the time frame by which data from an origin time series 
      will be aggregated into a new **rollup time series**. 
      They summarize the current data and zoom out 
      to a bigger picture with a lower resolution that uses less storage space.
      You can set different levels of resolution by adding new rollup policies that further aggregate 
      the previously aggregated data.

    * [Retention Periods](../../../document-extensions/timeseries/rollup-and-retention)  
      A retention period defines how long time series entries are kept.  
      An entry that exceeds this period will be removed.  
      You can set a different retention period for each rollup so that you can delete the previous data after it has been summarized/aggregated.

* In this page:  
  * [Rationale for Configuring Time Series Policies](../../../studio/database/settings/time-series-settings#rationale-for-configuring-time-series-policies)  
  * [Time Series Settings View](../../../studio/database/settings/time-series-settings#time-series-settings-view)  
  * [Defined Time Series Configurations](../../../studio/database/settings/time-series-settings#defined-time-series-configurations)  
  * [Add or Edit Time Series Configuration](../../../studio/database/settings/time-series-settings#add-or-edit-time-series-configuration)  
  * [Rollup Policies for Rollup Time Series](../../../studio/database/settings/time-series-settings#rollup-policies-for-rollup-time-series)  
{NOTE/}

---

{PANEL: Rationale for Configuring Time Series Policies}

Many applications continuously store massive amounts of time series data.  

Rollup policies help you manage the amount of data in storage by 
setting how close you want to zoom in and out in different time frames.  

Retention time periods then allow you to decide for how long to keep the higher resolution data in storage before it is deleted, 
or if to delete it at all.  
It must be kept at least until it is aggregated/summarized in the rollup policy, 
but can be set to be available for a period after the rollup, as seen in the example below.  

{NOTE: }
#### Heart Clinic Example  
Imagine a heart clinic whose raw data shows their patients' heart rates every minute.  
Here are the different levels of resolution that they want to see:  

!["Heart Clinic Example"](images/time-series-settings-10_HeartDoctorExample.png "Heart Clinic Example")

 * Raw Data  
   Tracks heart rate every minute.  
   Set to delete after 20 minutes when the data has already aggregated.  

 * DailySummary  
   They may want to know a person's average heart rate 
   every 15 minutes in a day to see a summary graph of daily activity levels. 
   They want to keep these daily activity summaries for a week.  

 * Month  
   Zooming out, they need to know the average heart rate every 3 hours, 
   and they want to keep the 3-hour summaries info for a month.  
   They can still zoom back in to see the daily summaries because each day is retained for a week. 

 * Long-term  
   After this, they only need to know their daily averages to see long-term trends.  
   They can always zoom into the latest Week, Day, and 20 Minute graphs.  

{NOTE/}

{PANEL/}

{PANEL: Time Series Settings View}

!["Time Series Settings View"](images/time-series-settings-01_view.png "Time Series Settings View")

1. **Time Series Settings View**  
   Select **Settings** > **Time Series** to open the Time Series Settings View.  
2. **Enable/Disable Rollup & Retention Policies**  
    * Use the **checkbox** to select or deselect all the Rollup and Retention 
      policies defined in your configurations.  
    * Use the **Set status** dropdown list to enable or disable the selected  policies.  
      !["Set Status"](images/time-series-settings-02_enable-disable.png "Set Status")  
       * Disabled policies will **not be executed**.  
       * Enabled policies' **execution frequency** is determined -  
         By the server Policy Check Frequency (see below),  
         **-and-**  
         By the Rollup Time Frame and Retention Period set by each policy.  
         {NOTE: When a policy is executed:}
         
         * Aggregated entries will be created according to the policy's rollup time frame.  
         * Entries will be removed according to the policy's retention period.  
         {NOTE/}
3. **Policy Check Frequency**  
   Set the frequency by which the server checks and executes the Rollup and Retention policies.  
   {NOTE: }
   Note that any policy defined with a time frame shorter than the server's 
   **Policy Check Frequency** will still be executed at the server checkup time.  
   E.g., If Policy Check Frequency is set to 8 seconds, a retention policy set to 
   2 seconds will still be checked and executed every 8 seconds.  
   {NOTE/}
4. **Defined Time Series Configurations**  
   View and manage the time series configurations that were already defined (read more [below](../../../studio/database/settings/time-series-settings#defined-time-series-configurations)).  
5. **Add or Edit time series configurations**  
    * Click the **Add a collection-specific configuration** button to create a new configuration.  
    * Click a defined configuration **Edit** button to edit an existing configuration.  
6. **Save**  
   Click to save the time series configurations.  

{PANEL/}

{PANEL: Defined Time Series Configurations}

!["Defined Time Series Configurations - Actions"](images/time-series-settings-03_per-collection-configuration-actions.png "Defined Time Series Configurations - Actions")

1. **Select Configuration**  
   Check to select/deselect this configuration.  
   Rollup and Retention policies for selected configurations 
   can be enabled or disabled using the 
   [time series settings view](../../../studio/database/settings/time-series-settings#time-series-settings-view) **Set Status** button.  
2. **Enable/Disable Configuration**  
   Click to enable or disable Rollup and Retention policies for this configuration.  
3. **Edit**  
   Click to [edit this configuration](../../../studio/database/settings/time-series-settings#add-or-edit-time-series-configuration).  
4. **Remove**  
  Click to remove this configuration.  

---

!["Defined Time Series Configurations - Info"](images/time-series-settings-04_per-collection-configuration-info.png "Defined Time Series Configurations - Info")

1. **Collection Name**  
   Time series configurations are defined **per collection** (`Companies` in the above image).  

2. **Rollup and Retention Policies Status**  
   The left sidebar indicates whether Rollup and Retention policies 
   in this configuration are enabled or disabled.  

3. **Rollup and Retention Policies**  
   Rollup and Retention policies defined for time series in this collection (`Companies`).  
   !["Defined Configuration - Rollup and Retention"](images/time-series-settings-04.1_per-collection-rollup-and-retention.png "Defined Configuration - Rollup and Retention")
    * **(a) Raw Data**  
      The retention policy for the raw time series entries.
      In this example, entries older than 12 hours will be removed 
      from every raw-data time series in the `Companies` collection.
    * **(b) Rollup Policy for Raw Data**  
       * **Rollup Policy Name**  
         In this example, the policy name is: `ByMinute`.  
         Raw data of time series in the Companies collection will be aggregated 
         into a new rollup time series named: `<raw-timeseries-name>@ByMinute`  
       * **Aggregation**  
         The aggregation period defined by the rollup policy.  
         In this example, every 1 minute of raw data will be aggregated into 
         a single entry of the `ByMinute` rollup time series.
         {NOTE: }
         An entry is added to the rollup time series only when the 
         defined aggregation time frame has ended.  
         {NOTE/}
       * **Retention**  
         The retention period defined for the rollup time series entries.  
         In this example, entries older than 1 hour are removed 
         from the `ByMinute` rollup time series.
    * **(c) Rollup Policy for Rollup Time Series**  
       * **Rollup Policy Name**  
         In this example, the policy name is: `30Mins`.  
         Data from the rollup time series `<raw-timeseries-name>@ByMinute>` 
         will be aggregated into a new rollup time series named: `<raw-timeseries-name>@30Mins`  
       * **Aggregation**  
         The aggregation period defined by the rollup policy.  
         In this example, every 30 minutes of data from the `ByMinute` 
         rollup time series will be aggregated into a single entry of the 
         `30Mins` rollup time series.  
         {NOTE: }
         An entry is added to the rollup time series only when the defined 
         aggregation time frame has ended.  
         {NOTE/}
       * **Retention**  
         The retention period defined for the rollup time series entries.  
         In this example, entries older than 7 days are removed 
         from the `30Mins` rollup time series.

4. **Named Values**  
   Named values defined for time series in this collection (`Companies`).  
   !["Defined Configuration - Named Values"](images/time-series-settings-04.2_per-collection-named-values.png "Defined Configuration - Named Values")
    * **(a) Time Series Name**  
      The time series for which the names are defined.  
    * **(b) Values and their Names**  
      Time series entries values, in their consecutive order within 
      the entry (#0, #1..), with the name defined for each value.  

{PANEL/}

{PANEL: Add or Edit Time Series Configuration}



!["Add or Edit Time Series Configuration"](images/time-series-settings-05_add-or-edit-configuration.png "Add or Edit Time Series Configuration")

1. **Edit Configuration**  
   Click to edit an existing configuration.  
2. **Add a Collection Specific Configuration**  
   Click to define a new time series configuration for one of your collections.  
3. **Select Collection**  
   Select the collection that this time series configuration is for.  
4. **Enable Retention - Raw Data**  
   Toggle to set a retention policy for raw time series entries.  
   !["Set Retention Policy"](images/time-series-settings-06_retention.png "Set Retention Policy")
    * **(a) Enable Retention**  
      Toggle to set a retention time period for **raw data** of all time series in the selected collection.  
    * **(b) Retention Time**  
      Set the retention time.  
      In the example above, raw time series entries older than 1 hour will be removed.  
5. **Add Rollup Policy**  
   Click to add a rollup policy.  
   {NOTE: }
   Rollup policies can be defined both for **raw time series data** 
   and for **rollup time series**.  
   Read more [here](../../../studio/database/settings/time-series-settings#rollup-policies-for-rollup-time-series).  
   {NOTE/}
   In the above image, the rollup policy aggregates the **raw data** 
   of the selected collection's time series, into **new** rollup time series.  
   !["Set Rollup Policy"](images/time-series-settings-07_rollup.png "Set Rollup Policy")
    * **(a) Policy Name**  
      Enter rollup policy Name  
    * **(b) Aggregation Time**  
      Set the aggregation period  
      In the above example, every 10 minutes of raw time series data will be aggregated  
      into a single entry of the new rollup time series.  
    * **(c) Enable Retention**  
      Toggle to set a retention time period for the new rollup time series entries.
      {NOTE: }
      The **aggregation time** cannot exceed the **retention period** 
      defined for the origin time series.  
      {NOTE/}

6. **Add Named Values**  
   !["Named Values"](images/time-series-settings-08_named-values.png "Named Values")
    * **(a) Time Series Name**  
      Enter the name of the time series whose values you want to name.  
    * **(b) Value Name**  
      Enter the name you want to give this value.  
    * **(c) Add Name**  
      Click to define an additional value name.  
7. **Add/Update**  
   Click **Add** or **Update** to add this configuration to the configuration list.  
   {WARNING: }
   Note: Any modifications made in this view will **not** be saved until 
   you click the **Save** button (9).  
   {WARNING/}
8. **Cancel**  
   Click to cancel.  
9. **Save**  
   Remember to also **click Save** when you're finished.

{PANEL/}


{PANEL: Rollup Policies for Rollup Time Series}

* Rollup policies can be created both for **Raw time series data**, 
  and for **data aggregated by other rollup time series**.  
* In a time series configuration, a rollup policy's source data 
  is determined by the policy's position.  
   * The **first** rollup policy aggregates **raw data** from 
     the time series of the selected collection.  
   * The **second** rollup policy aggregates data **from 
     the first rollup time series**.  
   * And so on: each additional rollup policy aggregates data from the 
     time series that precedes it.

{NOTE: }

* The Aggregation Time set by each policy must be **greater** than the 
  aggregation time set for the previous policy.  
* The Aggregation Time **cannot exceed** the Retention Period defined for the 
  previous time series.
{NOTE/}

!["Rollup Policies for Rollup Time Series"](images/time-series-settings-09_rollup-time-series.png "Rollup Policies for Rollup Time Series")  


1. **Time Series Raw Data**  
   The raw data collected in time series of the selected collection.  

2. **Rollup Time Series for Raw Data**  
   The `ByMinute` rollup policy aggregates raw data from time series 
   in the `Companies` collection.  
   Every minute of raw time series data is aggregated into 
   a single entry of the new rollup time series.  

3. **Rollup Time Series for Rollup Time Series**  
   The `30Mins` rollup policy aggregates data from the rollup time series 
   that was created by the `ByMinute` policy.  
   Every 30 minutes of data from the `ByMinute` rollup time series 
   are aggregated into a single entry of the new `30Mins` rollup time series.  

{PANEL/}

---

## Related Articles

### Document Extensions

- [Time Series Overview](../../../document-extensions/timeseries/overview)  
- [Time Series Client API Overview](../../../document-extensions/timeseries/client-api/overview)  

### Studio

- [Document Extensions: Time Series](../../../studio/database/document-extensions/time-series)
