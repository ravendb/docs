# RavenDB Cloud Instance Settings
---

{NOTE: }

* In most cases, the cloud configuration settings are similar to the on-premise settings.  
  To see general RavenDB configuration settings see: 
   * [Client API Configurations](../client-api/configuration/conventions) 
   * [Server Configurations](../server/configuration/configuration-options) 

* Whenever **cloud** configuration defaults differ from **on-premise** defaults, 
  the cloud configurations will be discussed in this article.

* In this page:  
   * [Setting Cloud Configurations via Studio](../cloud/cloud-settings#setting-cloud-configurations-via-studio)
   * [Indexing Settings](../cloud/cloud-settings#indexing-settings)

{NOTE/}

---


{PANEL: Setting Cloud Configurations via Studio}

If you want to change configurations for cloud instances, they must be set in Studio. 

* [Indexing Configurations](../studio/database/indexes/create-map-index#configuration)

To learn how to access the RavenDB Cloud Studio interface, see the [Studio section in the Cloud Overview](../cloud/cloud-overview#ravendb-studio---graphic-user-interface) article.

{PANEL/}

{PANEL: Indexing Settings}

---

### Max Batch Size

Indexing is done in batches to make it possible to adjust the number of documents that an index processes at a time.  
Adjusting the `Indexing.MapBatchSize` configuration can be done to prevent exhausting system resources.  
If an index processes a collection in a few batches, it will continue each new batch where the previous batch stopped.  

The factors to consider when adjusting the max indexing batch size:

* [Size of documents](https://ravendb.net/articles/dealing-with-large-documents-100-mb#:~:text=RavenDB%20can%20handle%20large%20documents,isn't%20a%20practical%20one.)
* [IOPS](../cloud/cloud-settings#changing-the-iops-number) - Input/Output Operations Per Second

#### Cloud Indexing Batch Size 

* RavenDB sets batch sizes with the following formula:  
   * `Indexing.MapBatchSize = max(PowerOf2(iops * 5), 1024);`
      * Explanation of the configuration value:
              {NOTE: In an instance with IOPS of 500}
               * `max()` = returns the larger of two arguments:
                  * `PowerOf2(iops * 5)` = The power of two that's larger than the argument passed  
                    500 * 5 = 2,500. The next power of two after 2,500 is 2 ^ 12, which is 4,096 documents.  
                  * `1024` = minimum number of documents  
               
                  In this example, 4,096 is larger than 1024, so the maximum batch size will be 4,096 documents.
               {NOTE/}

---

#### Overriding the Default Setting

Change [cloud configurations in Studio](../studio/database/indexes/create-map-index#configuration).

**Type**: `int`  
**MinValue**: `128`  
**Configuration Key**: `Indexing.MapBatchSize`  
**Value**: Set your preferred maximum number of documents per batch.  

---

#### Changing the cloud instance IOPS number 

(IOPS - Input/Output Operations Per Second - can be adjusted according to your needs.)

  !["Find IOPS Number"](images\configuration-see-iops.png "Find IOPS Number")

   1. **Products**  
      In the Cloud Portal, click the Products tab.
   2. **IOPS**  
      Your current IOPS number is written here.  
   3. **Change Storage**  
      It can be adjusted by clicking "Change Storage". 

{PANEL/}


##Related Articles
  
[Portal](../cloud/portal/cloud-portal)  
  
**Links**  
[Register]( https://cloud.ravendb.net/user/register)  
[Login]( https://cloud.ravendb.net/user/login)  
  
