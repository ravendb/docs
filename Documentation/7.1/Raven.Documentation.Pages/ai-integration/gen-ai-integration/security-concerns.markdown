# GenAI Integration: Studio
---

{NOTE: }

* In this article:
    * [](../../ai-integration/vector-search/ravendb-as-vector-database#what-is-a-vector-database)
    * [](../../ai-integration/vector-search/ravendb-as-vector-database#why-choose-ravendb-as-your-vector-database)
    
{NOTE/}

---

{PANEL: }

GenAI Security Concerns 
Data Privacy and Confidentiality 
Concern: Generative AI models may access and process sensitive or personally identifiable information (PII). 
Mitigations: 
Using HTTPS for data encryption in transient.
The database admin has the full control of every step of the gen ai process:
Define and shape the request that is sent to the model and the action to take upon getting the response.
Controls the script to create inputs for the LLM
Controls the prompt for the LLM
Controls the action script to execute upon get a response
The user has the ability to anonymize his data that he provides to the model by editing the extraction script.
Access Control and Authentication: 
Concern: Unauthorized access to databases can lead to data breaches. 
Mitigations: 
The AI model doesn't have any access to the database, only the database calls the model.
The database admin has full control and the responsibility for choosing the model/service/account that will be used in the gen ai task.
GenAI task are scoped for a specific DB, which meant DB Isolation implementation
DB read/write access is controlled by certificate.
Audit logging for GenAI task configuration, enables monitoring all access events.
Data Leakage: 
Concern: Sensitive data might inadvertently be memorized and reproduced by the generative model. 
Mitigations: 
RavenDB doesn't force any providers or model, it is users responsibility to define the proper service
The abillity to use the Ollama model locally will avoid possible data breaches for a third party misusage of the data (optional).

{PANEL/}

{PANEL: }


#### 

* 
* 


{PANEL/}

## Related Articles

### Client API

- [RQL](../../client-api/session/querying/what-is-rql) 
- [Query overview](../../client-api/session/querying/how-to-query)

### Vector Search

- [Vector search using a dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query.markdown)
- [Vector search using a static index](../../ai-integration/vector-search/vector-search-using-static-index.markdown)
- [Data types for vector search](../../ai-integration/vector-search/data-types-for-vector-search)

### Server

- [indexing configuration](../../server/configuration/indexing-configuration)
