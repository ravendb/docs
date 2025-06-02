# GenAI Integration: API
---

{NOTE: }

* A GenAI task leverage an AI model to enable intelligent processing of documents in runtime.  
   * The task is associated with a documents collection and with an AI model.  
   * It is an ongoing task that continuously monitors the collection and whenever needed 
     (e.g. when a document is added to it) retrieves document data, structures it, sends 
     the structured data object ("context object") to the AI model for processing, receives 
     the model's replies and acts upon them as needed.  

* The main steps in the definition of a GenAI task are:  
   * Defining a **connection string** that the task can connect the AI model with.  
   * Defining a **context objects creation** JavaScript.  
     To process a document, the task first runs a JavaScript on it to create **context objects**.  
     These objects are then sent one by one to the AI model for processing.  
     Each context object is sent to the model along with the Prompt and JSON schema 
     defined for this task, instructing the model what to do with the data and how 
     to structure its reply.  
   * Defining a **Prompt**.  
     The prompt, written in regular English, instructs the AI model what to do with the data passed to it.  
   * Defining a **JSON schema** Javascript.  
     The schema script instructs the AI model how to structure its replies.  
   * Defining an **Update JavaScript**.  
     The update script is executed over replies returned from the AI model.  
     It can, for example, modify or delete documents based on the conclusions made by the AI model.  

* In this article:
    * [Defining a Connection string](../../ai-integration/gen-ai-integration/gen-ai-api#defining-a-connection-string)
    * [Defining the GenAI task](../../ai-integration/gen-ai-integration/gen-ai-api#defining-the-genai-task)
    
{NOTE/}

---

{PANEL: Defining a Connection string}

The GenAI task can connect and leverage a variety of AI models, each requiring 
its own connection string.  

* Learn how to define a connection string for various AI destinations in the 
  [article dedicated to this subject](../../ai-integration/connection-strings/connection-strings-overview).  
* Choose what model to connect by your requirements from your GenAI task.  
  If you require security and speed above all, for example, for the duration of a development 
  phase you're in, you may prefer a local AI model like [Ollama](../../ai-integration/connection-strings/ollama).  
* **Note** that each AI model may apply different engines for different purposes.  
  We need to handle and generate text, so if we use Ollama, for example, we can 
  apply its `llama3.2` model, or if we use OpenAI we can apply `gpt-4o-mini`.  

---

### Example:

{CODE-TABS}
{CODE-TAB:csharp:set-ollama gen-ai-create-connection-string_ollama@AiIntegration\GenAI\GenAI.cs /}
{CODE-TAB:csharp:set-open-ai gen-ai_create-connection-string-open-ai@AiIntegration\GenAI\GenAI.cs /}
{CODE-TABS/}

---

### Syntax:

{CODE-TABS}
{CODE-TAB:csharp:ollama-syntax ollama_settings@AiIntegration\GenAI\GenAI.cs /}
{CODE-TAB:csharp:open-ai-syntax open-ai_settings@AiIntegration\GenAI\GenAI.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Defining the GenAI task}

* Define a GenAI task using a `GenAiConfiguration` object.  
* Run the task using `AddGenAiOperation`.  

{CODE:csharp gen-ai_define-gen-ai-task@AiIntegration\GenAI\GenAI.cs /}

---

### `GenAiConfiguration`

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **Name** | `string` | Task name |
| **Identifier** | `string` | Unique task identifier, embedded in documents metadata to indicate they were processed along with hash codes for their processed parts |
| **ConnectionStringName** | `string` | Connection string name |
| **Disabled** | `bool` | Determines whether the task is enabled or disabled |
| **Collection** | `string` | Name of the documents collection associated with the task |
| **GenAiTransformation** | `GenAiTransformation` | Context generation script - format for objects to be sentto the AI model |
| **Prompt** | `string` | AI model Prompt - the instructions sent to the AI model |
| **SampleObject** | `string` | JSON schema - a sample response object to format AI model replies by |
| **UpdateScript** | `string` | Update script - specifies what to do with AI model replies |
| **MaxConcurrency** | `int` | Max concurrent connections to AI model |

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
