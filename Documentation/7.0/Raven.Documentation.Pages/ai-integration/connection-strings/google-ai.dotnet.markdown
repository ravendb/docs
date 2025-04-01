# Connection String to Google AI
---

{NOTE: }

* This article explains how to define a connection string to [Google AI](https://ai.google.dev/gemini-api/docs/embeddings),  
  enabling RavenDB to seamlessly integrate its [embeddings generation tasks](../../ai-integration/generating-embeddings/overview) with Google's AI services.

* This configuration supports **Google AI provider** only and is not compatible with Vertex AI.

* In this article:
  * [Define the connection string - from the Studio](../../ai-integration/connection-strings/google-ai#define-the-connection-string---from-the-studio)
  * [Define the connection string - from the Client API](../../ai-integration/connection-strings/google-ai#define-the-connection-string---from-the-client-api)
  * [Syntax](../../ai-integration/connection-strings/google-ai#syntax) 
    
{NOTE/}

---

{PANEL: Define the connection string - from the Studio}

![connection string to google ai](images/google-ai.png "Define a connection string to Google AI")

1. **Name**  
   Enter a name for this connection string.

2. **Identifier** (optional)  
   Enter an identifier for this connection string.  
   Learn more about the identifier in the [connection string identifier](../../ai-integration/connection-strings/connection-strings-overview#the-connection-string-identifier) section.

3. **Connector**  
   Select **Google AI** from the dropdown menu.

4. **AI Version** (optional)  
   * Select the Google AI API version to use.
   * If not specified, `V1_Beta` is used. Learn more in [API versions explained](https://ai.google.dev/gemini-api/docs/api-versions).

5. **API key**  
   Enter the API key used to authenticate requests to Google's AI services.

6. **Model**  
   Select or enter the Google AI text embedding model to use.

7. **Dimensions** (optional)  
   * Specify the number of dimensions for the output embeddings.  
   * If not specified, the model's default dimensionality is used.

8. **Max concurrent query batches**: (optional)
   * When making vector search queries, the content of the search terms must also be converted to embeddings to compare them against the stored vectors.
     Requests to generate such query embeddings via the AI provider are sent in batches.
   * This parameter defines the maximum number of these batches that can be processed concurrently.  
     You can set a default value using the [Ai.Embeddings.MaxConcurrentBatches](../../server/configuration/ai-integration-configuration#ai.embeddings.maxconcurrentbatches) configuration key.

9. Click **Test Connection** to confirm the connection string is set up correctly.

10. Click **Save** to store the connection string or **Cancel** to discard changes.

{PANEL/}

{PANEL: Define the connection string - from the Client API}

{CODE:csharp create_connection_string_google_ai@AiIntegration\ConnectionStrings\connectionStrings.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE:csharp google_ai_settings@AiIntegration\ConnectionStrings\connectionStrings.cs /}

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

- [Connection strings - overview](../../ai-integration/connection-strings/connection-strings-overview)
