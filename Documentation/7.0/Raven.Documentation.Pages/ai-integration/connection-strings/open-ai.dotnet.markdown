# Connection String to OpenAI
---

{NOTE: }

* This article explains how to define a connection string to the [OpenAI Service](https://platform.openai.com/docs/guides/embeddings),  
  enabling RavenDB to seamlessly integrate its [embeddings generation tasks](../../ai-integration/generating-embeddings/overview) with the OpenAI's API.

* In this article:
  * [Define the connection string - from the Studio](../../ai-integration/connection-strings/open-ai#define-the-connection-string---from-the-studio)
  * [Define the connection string - from the Client API](../../ai-integration/connection-strings/open-ai#define-the-connection-string---from-the-client-api)
  * [Syntax](../../ai-integration/connection-strings/open-ai#syntax) 
    
{NOTE/}

---

{PANEL: Define the connection string - from the Studio}

![connection string to open ai](images/open-ai.png "Define a connection string to OpenAI")

1. **Name**  
   Enter a name for this connection string.

2. **Identifier** (optional)  
   Learn more about the identifier in the [connection string identifier](../../ai-integration/connection-strings/connection-strings-overview#the-connection-string-identifier) section.

3. **Connector**  
   Select **OpenAI** from the dropdown menu.

4. **API key**  
   Enter the API key used to authenticate requests to OpenAI's API.

5. **Endpoint**  
   Select or enter the OpenAI endpoint for generating embeddings from text.

6. **Model**  
   Select or enter the OpenAI text embedding model to use.

7. **Organization ID** (optional)  
   * Set the organization ID to use for the `OpenAI-Organization` request header.
   * Users belonging to multiple organizations can set this value to specify which organization is used for an API request. 
     Usage from these API requests will count against the specified organization's quota.
   * If not specified, the header will be omitted, and the default organization will be billed.  
     You can change your default organization in your user settings.  
   * Learn more in [Setting up your organization](https://platform.openai.com/docs/guides/production-best-practices/setting-up-your-organization#setting-up-your-organization)

8. **Project ID** (optional)  
   * Set the project ID to use for the `OpenAI-Project` request header.  
   * Users who are accessing their projects through their legacy user API key can set this value to specify which project is used for an API request.
     Usage from these API requests will count as usage for the specified project.
   * If not specified, the header will be omitted, and the default project will be accessed.
 
9. **Dimensions** (optional)  
   * Specify the number of dimensions for the output embeddings.  
     Supported only by _text-embedding-3_ and later models.
   * If not specified, the model's default dimensionality is used.

10. **Max concurrent query batches**: (optional)
    * When making vector search queries, the content of the search terms must also be converted to embeddings to compare them against the stored vectors.
      Requests to generate such query embeddings via the AI provider are sent in batches.
    * This parameter defines the maximum number of these batches that can be processed concurrently.  
      You can set a default value using the [Ai.Embeddings.MaxConcurrentBatches](../../server/configuration/ai-integration-configuration#ai.embeddings.maxconcurrentbatches) configuration key.

11. Click **Test Connection** to confirm the connection string is set up correctly.

12. Click **Save** to store the connection string or **Cancel** to discard changes.

{PANEL/}

{PANEL: Define the connection string - from the Client API}

{CODE:csharp create_connection_string_open_ai@AiIntegration\ConnectionStrings\connectionStrings.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE:csharp open_ai_settings@AiIntegration\ConnectionStrings\connectionStrings.cs /}

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
