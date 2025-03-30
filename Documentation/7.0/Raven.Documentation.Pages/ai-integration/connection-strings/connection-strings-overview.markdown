# AI Connection Strings - Overview
---

{NOTE: }

* In RavenDB, you can define [Embeddings Generation Tasks](../../ai-integration/generating-embeddings/overview) that generate embeddings from the content of your documents.
  These embeddings are stored in a dedicated collection within the database and enable vector search on your document content.

* Each embeddings generation task must define a **connection string** to an embedding provider.  
  This connection string specifies where the embeddings will be generated,
  allowing RavenDB to integrate with external services such as Azure OpenAI, OpenAI, Hugging Face, Google AI, Ollama, Mistral AI, or RavenDB's embedded model (bge-micro-v2).

* While each task can have only one connection string, you can define multiple connection strings in your database to support different providers or configurations.  
  A single connection string can also be reused across multiple tasks in the database.

* These connection strings can be created from:
  * The **AI Connection Strings view** in the Studio, where you can create, edit, and delete connection strings that are not in use.
  * The **Client API** - examples are available in the dedicated articles for each provider.

---

* In this article:
  * [The AI Connection Strings view](../../ai-integration/connection-strings/connection-strings-overview#the-ai-connection-strings-view)
  * [Creating an AI connection string](../../ai-integration/connection-strings/connection-strings-overview#creating-an-ai-connection-string)
    
{NOTE/}

---

{PANEL: The AI Connection Strings view}

![connection strings view](images/connection-strings-view.png "The AI Connection Strings view")

1. Go to the **AI Hub** menu.

2. Open the **AI Connection Strings** view.

3. Click **"Add new"** to create a new connection string.

4. View the list of all AI connection strings that have been defined.

5. Edit or delete a connection string.  
   Only connection strings that are not in use by a task can be deleted.

{PANEL/}

{PANEL: Creating an AI connection string}

![create connection string](images/create-connection-string.png "Create connection string")

1. **Name**  
   Enter a unique name for the connection string.

2. **Identifier**  
   Enter a unique identifier for the connection string.  
   Each AI connection string in the database must have a distinct identifier.

     If not specified, or when clicking the "Regenerate" button,  
     RavenDB automatically generates the identifier based on the connection string name. For example:
       * If the connection string name is: _"My connection string to Google AI"_
       * The generated identifier will be: _"my-connection-string-to-google-ai"_
  
     Allowed characters: only lowercase letters (a-z), numbers (0-9), and hyphens (-).  
     See how this identifier is used in the [embeddings cache collection](../../ai-integration/generating-embeddings/embedding-collections#the-embeddings-cache-collection).

3. **Regenerate**  
   Click "Regenerate" to automatically create an identifier based on the connection string name.

4. **Connector**  
   Select an AI provider from the dropdown menu.  
   This will open a dialog where you can configure the connection details.  
   Configuration details for each provider are explained in the following articles:
   * [Azure Open AI](../../ai-integration/connection-strings/azure-open-ai)
   * [Google AI](../../ai-integration/connection-strings/google-ai)
   * [Hugging Face](../../ai-integration/connection-strings/hugging-face)
   * [Ollama](../../ai-integration/connection-strings/ollama)
   * [OpenAI](../../ai-integration/connection-strings/open-ai)
   * [Mistral AI](../../ai-integration/connection-strings/mistral-ai)
   * [Embedded model (bge-micro-v2)](../../ai-integration/connection-strings/embedded)

5. Once you complete all configurations for the selected provider in the dialog,  
   save the connection string definition.

{PANEL/}

## Related Articles

### Vector Search

- [RavenDB as a vector database](../../ai-integration/vector-search/ravendb-as-vector-database)
- [Vector search using a static index](../../ai-integration/vector-search/vector-search-using-static-index)
- [Vector search using a dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query)

### Embeddings Generation

- [Generating embeddings - overview](../../ai-integration/generating-embeddings/overview)
- [Embeddings generation task](../../ai-integration/generating-embeddings/embeddings-generation-task)

### AI Connection Strings

- [Azure Open AI](../../ai-integration/connection-strings/azure-open-ai)
- [Google AI](../../ai-integration/connection-strings/google-ai)
- [Hugging Face](../../ai-integration/connection-strings/hugging-face)
- [Ollama](../../ai-integration/connection-strings/ollama)
- [OpenAI](../../ai-integration/connection-strings/open-ai)
- [Mistral AI](../../ai-integration/connection-strings/mistral-ai)
- [Embedded model](../../ai-integration/connection-strings/embedded)
