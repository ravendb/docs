# Create or Modify Counters
---

{NOTE: }

* Use the `counters_for .increment` method to **create** a new Counter or **modify** an existing Counter's value.  

*  If the Counter exists, `increment` will add the specified number to the Counter's current value.  
   If the Counter doesn't exist, `increment` will create it and set its initial value.  

* For all other `counters_for ` methods see this [Overview](../../document-extensions/counters/overview#counter-methods-and-the--object).

* In this page:
  - [`increment` usage](../../document-extensions/counters/create-or-modify#increment-usage)
  - [Example](../../document-extensions/counters/create-or-modify#example)
  - [Syntax](../../document-extensions/counters/create-or-modify#syntax)

{NOTE/}

---

{PANEL: `increment` usage}

 **Flow**:  

* Open a session.  
* Create an instance of `counters_for `.  
    * Either pass `counters_for ` an explicit document ID, -or-  
    * Pass it an [entity tracked by the session](../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.Query](../../client-api/session/querying/how-to-query) or from [session.Load](../../client-api/session/loading-entities#load).  
* Call `counters_for .increment`.
* Call `session.save_changes` for the changes to take effect.  

**Note**:

* Modifying a Counter using `increment` only takes effect when `session.aave_changes()` is executed.  
* To **decrease** a Counter's value, pass the method a negative number to the `increment` method.  

{PANEL/}

{PANEL: Example}

{CODE:python counters_region_Increment@DocumentExtensions\Counters\Counters.py /}

{PANEL/}

{PANEL: Syntax}

{CODE:python Increment-definition@DocumentExtensions\Counters\Counters.py /}

| Parameter     | Type    | Description      |
|---------------|---------|------------------|
| `counter_name` | string  | Counter's name  |
| `delta`       | long    | Increase Counter by this value.<br>Default value is 1.<br>For a new Counter, this number will be its initial value. |

{PANEL/}

## Related articles

**Studio Articles**:  
[Studio Counters Management](../../studio/database/document-extensions/counters#counters)  

**Client-API - Session Articles**:  
[Counters Overview](../../document-extensions/counters/overview)  
[Deleting a Counter](../../document-extensions/counters/delete)  
[Retrieving Counter Values](../../document-extensions/counters/retrieve-counter-values)  
[Counters and other features](../../document-extensions/counters/counters-and-other-features)  
[Counters In Clusters](../../document-extensions/counters/counters-in-clusters)  

**Client-API - Operations Articles**:  
[Counters Operations](../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)  
