# Connection String to Azure OpenAI
---

{NOTE: }

* This article explains how to define a connection string to the [Azure OpenAI Service](https://azure.microsoft.com/en-us/products/ai-services/openai-service),  
  enabling RavenDB to seamlessly integrate its [embeddings generation tasks](../../ai-integration/generating-embeddings/overview) with your Azure environment.

* In this article:
  * [Define the connection string - from the Studio](../../ai-integration/connection-strings/azure-open-ai#define-the-connection-string---from-the-studio)
  * [Define the connection string - from the Client API](../../ai-integration/connection-strings/azure-open-ai#define-the-connection-string---from-the-client-api)
  * [Syntax](../../ai-integration/connection-strings/azure-open-ai#syntax) 
    
{NOTE/}

---

{PANEL: Define the connection string - from the Studio}

![connection string to azure open ai](images/azure-open-ai.png "Define a connection string to Azure OpenAI")

1. **Name**  
   Enter a name for this connection string.

2. **Identifier** (optional)  
   Enter an identifier for this connection string.  
   Learn more about the identifier in the [connection string identifier](../../ai-integration/connection-strings/connection-strings-overview#the-connection-string-identifier) section.

3. **Connector**  
   Select **Azure OpenAI** from the dropdown menu.

4. **API Key**  
   Enter the API key used to authenticate requests to the Azure OpenAI service.

5. **Endpoint**  
   Enter the Azure OpenAI endpoint URL for generating embeddings from text.

6. **Model**  
   Specify the Azure OpenAI text embedding model to use.

7. **Deployment Name**  
   Specify the unique identifier assigned to your model deployment in your Azure environment.

8. **Dimensions** (optional)  
   * Specify the number of dimensions for the output embeddings.  
     Supported only by _text-embedding-3_ and later models.  
   * If not specified, the model's default dimensionality is used.

9. Click **Test Connection** to confirm the connection string is set up correctly.

10. Click **Save** to store the connection string or **Cancel** to discard changes.

{PANEL/}

{PANEL: Define the connection string - from the Client API}

{CODE:csharp create_connection_string_azure_open_ai@AiIntegration\ConnectionStrings\connectionStrings.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE:csharp azure_open_ai_settings@AiIntegration\ConnectionStrings\connectionStrings.cs /}

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
