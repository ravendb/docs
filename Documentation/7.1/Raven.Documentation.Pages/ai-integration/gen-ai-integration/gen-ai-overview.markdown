# GenAI Integration: Overview
---

{NOTE: }

* **GenAI Integration** allows RavenDB to connect and interact with Generative AI models, 
  introducing numerous new ways for intelligent, autonomous data processing in production.  
  
     A task can be built in minutes, e.g. to Classify customer inquiries based on sentiment, 
     Generate automated responses to frequently asked questions, Escalate support tickets, 
     Summarize lengthy documents, Provide relevant recommendations, Tag and prioritize content 
     for efficient retrieval, Enhance data security by detecting anomalies, Optimize inventory 
     predictions... or endless other options, bound only by our creativity.  

* **You can use a wide variety of AI models**, e.g. a model installed locally like `Ollama llama3.2` 
  during a development phase that requires quick service and no additional costs, or a remote model 
  like `OpenAI gpt-4o-mini` with all the benefits of the latest features and broadest access.  

* **Ongoing GenAI tasks** can be easily defined, tested and deployed using API or Studio.  

     While creating a GenAI task via Studio, a smart interactive environment is provided, 
     allowing each phase of the task to be tested in a secluded playground, freely and without 
     harming your data, but also produce result sets that can be tried out by the next phase.  

* In this article:
    * [RavenDB GenAI tasks](../../ai-integration/gen-ai-integration/gen-ai-overview#ravendb-genai-tasks)
    * [Run time](../../ai-integration/gen-ai-integration/gen-ai-overview#run-time)
    * [Licensing](../../ai-integration/gen-ai-integration/gen-ai-overview#licensing)

{NOTE/}

---

{PANEL: RavenDB GenAI tasks}

RavenDB offers an integration of generative AI capabilities through user defined **GenAI tasks**.  
A GenAI task is an ongoing process that continuously monitors a document collection associated with 
it, and reacts when a document is added or modified by Retrieving the document, Structuring it into 
"context objects", Sending these objects to a generative AI model along with instructions regarding 
what to do with the data and how to shape the reply, and potentially Acting upon the model's response.  

{CONTENT-FRAME: <a id="the-flow" />The flow}
Let's put the above stages in order.
<br>
<br>

1. The task continuously monitors the collection it is associated with.  
2. When a document is added or modified, the task retrieves it.  
3. The task structure the data contained in the document into **Context objects**.  
   The structuring is done by a "context creation script" (JavaScript) you provide, 
   that uses our `ai.genContext` method for the creation of each context object.  
4. The task sends each context object to a GenAI model for processing.  
    * The task is associated with a **Connection string** that defines how to connect to the AI model.
    * Each context object is sent over its own separate connection to the AI model.  
    * Each object is sent along with a **Prompt** and a **JSON schema**.  
      The prompt is written in regular english, with your instructions to the AI model.  
      The schema defines how the model is to structure its replies.  
5. The task can then apply an **Update script** (JavaScript) to handle the results.  
{CONTENT-FRAME/}

{CONTENT-FRAME: <a id="the-elements" />The elements}
These are the elements that need to be defined for a GenAI task.
<br>
<br>

* A **connection string** to the GenAI model.  
* A **Context generation script** that uses `ai.genContext` to create each context object.  
* A **Prompt**, written in regular English, instructing the AI model what to do with the data passed to it.  
* A **JSON schema**, written in JavaScript, instructing the AI model how to structure its replies.  
* An **Update JavaScript**, written in JavaScript, that is executed over replies returned from the AI model.  
{CONTENT-FRAME/}

{CONTENT-FRAME: <a id="how-to-create-and-run-a-gen-ai-task" />How to create and run a GenAI task}

* You can use [Studio's intuitive wizard](../../ai-integration/gen-ai-integration/gen-ai-studio#add-a-genai-task) 
  to create GenAI tasks. The wizard will guide you through the task creation phases, 
  exemplify where needed, and provide you with convenient, interactive, secluded "playgrounds" 
  for free interactive experimenting.  
* You can also create GenAI tasks using the [Client API](../../ai-integration/gen-ai-integration/gen-ai-api) 
{CONTENT-FRAME/}
 
{PANEL/}

{PANEL: Run time}

Once you complete the configuration and save the task, it will start running (if enabled).  
The task will monitor the collection associated with it, and process documents as they are 
added or modified.  

---

#### Tracking of processed document parts

* After creating a [context object](../../ai-integration/gen-ai-integration/gen-ai-studio#generate-context-objects) 
  of a document part and processing it, RavenDB will log this context object in 
  the document's metadata, under a property whose name is the unique task identifier 
  defined [here](../../ai-integration/gen-ai-integration/gen-ai-studio#unique-identifier).  
* The task will create a hash code for the context object, and log it in the document metadata.  
  {CONTENT-FRAME: Hash computation}
  When computing the hash code, the task will include these elements in the computation:  
  <br>

  * The context object  
  * The GenAI provider and model (e.g. Ollama llama3.2)  
  * The prompt  
  * The JSON schema  
  * The update script
  {CONTENT-FRAME/}
  Whenever the task is requested to process this document part again, it will recreate 
  a hash code for the same 5 elements.  
  If the new hash code is identical to one of the codes logged in the document's metadata, 
  the task will conclude that the context object was already processed with the exact same 
  model, prompt, schema and update script, and skip reprocessing it.  
  If the new code is not found in the metadata list, the context object will be processed again.  

       ![Tracking processed document parts](images/gen-ai_hash-flow.png "Tracking processed document parts")

![Metadata Identifier and Hash codes](images/gen-ai_metadata-identifier-and-hash-codes.png "Metadata Identifier and Hash codes")

1. **Identifier**  
   This is the unique identifier used by the GenAI task.  
2. **Hash codes**  
   These two hash codes were added after processing `comment/1` and `comment/3`.  

{PANEL/}

{PANEL: Licensing}

For RavenDB to support the GenAI Integration feature, you need a `RavenDB AI` license type.  
A `Developer` license will also enable the feature for experimentation and development.  

![Licensing: RavenDB AI license](images/gen-ai_licensing.png "Licensing: RavenDB AI license")

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
