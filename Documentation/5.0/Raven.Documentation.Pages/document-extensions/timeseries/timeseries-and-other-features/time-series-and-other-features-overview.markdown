# Time-Series and Other Features - Overview
---

{NOTE: }

* This section explains the relations of time series and other RavenDB features 
  in general.  

* In this page:  
  * []()  
  * []()  
{NOTE/}

---

{PANEL: }

###Time Series and Other Features: Summary

Use this table to find if and how various RavenDB features are triggered by time series, 
and how the various features handle time series values.  

* In the **Triggered By** column:  
    * _Document Change_ - Feature is triggered by a time series Name update.  
    * _TS Value Modification_ - Feature is triggered by a time series Value modification.  
    * _Time Schedule_ - Feature is invoked by a pre-set time routine.  
    * _No Trigger_ - Feature is executed manually, through the Studio or by a Client.  

| **Feature** | **Triggered by** |
|-------------|:-------------|
| [Indexing]() | _Document Change_ | 
| [LINQ Queries]() | _No trigger_ |
| [Raw Queries]() | _No trigger_ |
| [Smuggler]() | _No trigger_ |
| [Backup Task]() | _Time Schedule_ |
| [External Replication task]() | _Document Change_, <br> _TS Value Modification_ |
| [Changes API]() | _TS Value Modification_ |
| [Revision creation]() | _Document Change_ |

{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Time Series Management]()  

**Client-API - Session Articles**:  
[Time Series Overview]()  
[Creating and Modifying Time Series]()  
[Deleting Time Series]()  
[Retrieving Time Series Values]()  
[Time Series and Other Features]()  

**Client-API - Operations Articles**:  
[Time Series Operations]()  
