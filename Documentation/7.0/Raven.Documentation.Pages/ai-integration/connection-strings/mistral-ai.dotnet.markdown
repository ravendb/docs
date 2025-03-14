# Connection String to Mistral AI
---

{NOTE: }

* This article explains how to define a connection string to [Mistral AI](https://docs.mistral.ai/capabilities/embeddings/),  
  enabling RavenDB to seamlessly integrate its [embeddings generation tasks](../../todo..) with Mistral's API.

* In this page:
  * [Define the connection string - from the Studio](../../ai-integration/connection-strings/mistral-ai#define-the-connection-string---from-the-studio)
  * [Define the connection string - from the Client API](../../ai-integration/connection-strings/mistral-ai#define-the-connection-string---from-the-client-api)
  * [Syntax](../../ai-integration/connection-strings/mistral-ai#syntax) 
    
{NOTE/}

---

{PANEL: Define the connection string - from the Studio}

![connection string to mistral ai](images/mistral-ai.png "Define a connection string to Mistral AI")

1. **Name**  
   Enter a name for this connection string.

2. **Identifier** (optional)  
   Learn more about the identifier in the [connection string identifier](../../ai-integration/connection-strings/connection-strings-overview#the-connection-string-identifier) section.

3. **Connector**  
   Select **Mistral AI** from the dropdown menu.

4. **API Key**  
   Enter the API key used to authenticate requests to Mistral AI's API.

5. **Endpoint**  
   Enter the Mistral AI endpoint for generating embeddings from text.

6. **Model**  
   Specify the Mistral AI text embedding model to use.

7. Click **Test Connection** to confirm the connection string is set up correctly.

8. Click **Save** to store the connection string or **Cancel** to discard changes.

{PANEL/}

{PANEL: Define the connection string - from the Client API}

{CODE:csharp create_connection_string_mistral_ai@AiIntegration\ConnectionStrings\connectionStrings.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE:csharp mistral_ai_settings@AiIntegration\ConnectionStrings\connectionStrings.cs /}

{PANEL/}

## Related Articles

### Vector Search

- [RavenDB as a vector database](../../ai-integration/vector-search/ravendb-as-vector-database)
- [Vector search using a static index](../../ai-integration/vector-search/vector-search-using-static-index)
- [Vector search using a dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query)

### AI Integration

- [Connection strings - overview](../../ai-integration/connection-strings/connection-strings-overview)
- [Embeddings generation task](../../todo..)
