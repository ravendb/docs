# Connection String to Ollama
---

{NOTE: }

* This article explains how to define a connection string to [Ollama](https://ollama.com/blog/embedding-models),  
  enabling RavenDB to seamlessly integrate its [embeddings generation tasks](../../todo..) with your Ollama setup.

* In this page:
  * [Define the connection string - from the Studio](../../ai-integration/connection-strings/ollama#define-the-connection-string---from-the-studio)
  * [Define the connection string - from the Client API](../../ai-integration/connection-strings/ollama#define-the-connection-string---from-the-client-api)
  * [Syntax](../../ai-integration/connection-strings/ollama#syntax) 
    
{NOTE/}

---

{PANEL: Define the connection string - from the Studio}

![connection string to ollama](images/ollama.png "Define a connection string to Ollama")

1. **Name**  
   Enter a name for this connection string.

2. **Identifier** (optional)  
   Learn more about the identifier in the [connection string identifier](../../ai-integration/connection-strings/connection-strings-overview#the-connection-string-identifier) section.

3. **Connector**  
   Select **Ollama** from the dropdown menu.

4. **Model**  
   Specify the Ollama text embedding model to use.

5. **URI**  
   Enter the Ollama API URI.

6. Click **Test Connection** to confirm the connection string is set up correctly.

7. Click **Save** to store the connection string or **Cancel** to discard changes.

{PANEL/}

{PANEL: Define the connection string - from the Client API}

{CODE:csharp create_connection_string_ollama@AiIntegration\ConnectionStrings\connectionStrings.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE:csharp ollama_settings@AiIntegration\ConnectionStrings\connectionStrings.cs /}

{PANEL/}

## Related Articles

### Vector Search

- [RavenDB as a vector database](../../ai-integration/vector-search/ravendb-as-vector-database)
- [Vector search using a static index](../../ai-integration/vector-search/vector-search-using-static-index)
- [Vector search using a dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query)

### AI Integration

- [Connection strings - overview](../../client-api/session/querying/how-to-query)
- [Embedding generation tasks](../../todo..)
