# Time Series Design
---

{NOTE: }

* ...  

* In this page:  
  * [Design]()  
  * []()  
  * []()  
{NOTE/}

---

{PANEL: Design}

* A time series is an array of `Double` values.  
  Its keys are Time Points, ordered by time.  

- | Key| Value | Tag |
|:-------------:|:-------------:|:-------------:|
| Time Point |  An array of Doubles | Optional, indicates the measurement origin; <br> A reference to the document |

{INFO: Time Points}

- **Units**  
  Time points are measured in milliseconds. Measurement ticks are rounded **down**.  
- **Offset**  
  Each time-point in a time series' [segment]() is indicated by its **offset** from the previous point.  
- **Distance**  
  Each offset's **distance** from the previous offset is also indicated.  
  This is helpful in series of a fixed change, like **1, 2, 3** - where the offset would normally be 
  1 and the distance from the previous offset would be 0.  
  RavenDB uses these repeated 0's while compressing the series for storage, to minimize the amount 
  of storage needed for it.  
- **Encoding**  
  We encode Time Series usinf Facebook Gorilla encoding.  
{INFO/}

---

#### References

* The document refers to its time series extensions in its metadata.  
   - **The `HasTimeSeries` Flag**  
     When a time series is added to a document, RavenDB automatically sets a `HasTimeSeries` Flag in the document's metadata.  
     When all time series have been removed from a document, the server automatically removes this flag.  
* A time series contains a reference to the document it extends, by the document's name.  

---

####Segments

* A short series is contained in a single segment.  
* A longer (over 32k) series is contained in multiple segments.  
* A new segment is also opened when a long period of time [25 days?] has passed 
  since the last measurement.  

---

####Number of Time Series Per Document
...

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
