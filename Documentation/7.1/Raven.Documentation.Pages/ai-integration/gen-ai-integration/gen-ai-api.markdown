# GenAI Integration: API
---

{NOTE: }

* A GenAI task leverages an AI model to enable intelligent processing of documents in runtime.  
   * The task is associated with a document collection and with an AI model.  
   * It is an **ongoing task** that:  
      1. Continuously monitors the collection;  
      2. Whenever needed, like when a document is added to the collection, generates 
         user-defined context objects based on the source document data;  
      3. Passes each context object to the AI model for further processing;  
      4. Receives the AI model's JSON-based results;  
      5. And finally, runs a user-defined script that potentially acts upon the results.  

* The main steps in defining a GenAI task are:  
   * Defining a [Connection string](../../ai-integration/gen-ai-integration/gen-ai-api#defining-a-connection-string) 
     to the AI model  
   * Defining a [Context generation script](../../ai-integration/gen-ai-integration/gen-ai-overview#the-elements_context-objects)  
   * Defining a [Prompt](../../ai-integration/gen-ai-integration/gen-ai-overview#the-elements_prompt)  
   * Defining a [JSON schema](../../ai-integration/gen-ai-integration/gen-ai-overview#the-elements_json-schema)  
   * Defining an [Update script](../../ai-integration/gen-ai-integration/gen-ai-overview#the-elements_update-script)  

* In this article:
    * [Defining a Connection string](../../ai-integration/gen-ai-integration/gen-ai-api#defining-a-connection-string)
    * [Defining the GenAI task](../../ai-integration/gen-ai-integration/gen-ai-api#defining-the-genai-task)

{NOTE/}

---

{PANEL: Defining a Connection string}

* Choose the model to connect with, by what you need from your GenAI task.  
  E.g., If you require security and speed above all for the duration of a rapid 
  development phase, you may prefer a local AI service like [Ollama](../../ai-integration/connection-strings/ollama).  
* Make sure you define the correct service: both Ollama and OpenAI are supported 
  but you need to pick an Ollama/OpenAI service that supports generative AI, 
  like Ollama `llama3.2` or OpenAI `gpt-4o-mini`.  
* Learn more about connection strings [here](../../ai-integration/connection-strings/connection-strings-overview).  

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

{CODE-TABS}
{CODE-TAB:csharp:use-sample-object gen-ai_define-gen-ai-task_use-sample-object@AiIntegration\GenAI\GenAI.cs /}
{CODE-TAB:csharp:use-json-schema gen-ai_define-gen-ai-task_use-json-schema@AiIntegration\GenAI\GenAI.cs /}
{CODE-TABS/}

---

### `GenAiConfiguration`

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **Name** | `string` | Task name |
| **Identifier** | `string` | Unique task identifier, embedded in documents metadata to indicate they were processed along with hash codes for their processed parts |
| **ConnectionStringName** | `string` | Connection string name |
| **Disabled** | `bool` | Determines whether the task is enabled or disabled |
| **Collection** | `string` | Name of the document collection associated with the task |
| **GenAiTransformation** | `GenAiTransformation` | Context generation script - format for objects to be sent to the AI model |
| **Prompt** | `string` | AI model Prompt - the instructions sent to the AI model |
| **SampleObject** | `string` | A [sample response object](../../ai-integration/gen-ai-integration/gen-ai-overview#the-elements_json-schema) to format the AI model's replies by <br> If both a `SampleObject` and a `JsonSchema` are provided the schema takes precedence |
| **JsonSchema** | `string` | A [JSON schema](../../ai-integration/gen-ai-integration/gen-ai-overview#the-elements_json-schema) to format the AI model's replies by <br> If both a `SampleObject` and a `JsonSchema` are provided the schema takes precedence |
| **UpdateScript** | `string` | Update script - specifies what to do with AI model replies |
| **MaxConcurrency** | `int` | Max concurrent connections to the AI model (each connection serving a single context object |

{PANEL/}

## Related Articles

### GenAI Integration

- [GenAI Overview](../../ai-integration/gen-ai-integration/gen-ai-overview)
- [GenAI Studio](../../ai-integration/gen-ai-integration/gen-ai-studio)
- [GenAI Security Concerns](../../ai-integration/gen-ai-integration/security-concerns)

### Vector Search

- [Vector search using a dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query.markdown)
- [Vector search using a static index](../../ai-integration/vector-search/vector-search-using-static-index.markdown)
- [Data types for vector search](../../ai-integration/vector-search/data-types-for-vector-search)
