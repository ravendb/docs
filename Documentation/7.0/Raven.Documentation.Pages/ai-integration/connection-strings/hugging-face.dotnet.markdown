# Connection String to Hugging Face
---

{NOTE: }

* This article explains how to define a connection string to the [Hugging Face's text embedding services](https://huggingface.co/docs/text-embeddings-inference/en/index),  
  enabling RavenDB to seamlessly integrate its [embeddings generation tasks](../../ai-integration/generating-embeddings/overview) within your environment.

* In this article:
  * [Define the connection string - from the Studio](../../ai-integration/connection-strings/hugging-face#define-the-connection-string---from-the-studio)
  * [Define the connection string - from the Client API](../../ai-integration/connection-strings/hugging-face#define-the-connection-string---from-the-client-api)
  * [Syntax](../../ai-integration/connection-strings/hugging-face#syntax) 
    
{NOTE/}

---

{PANEL: Define the connection string - from the Studio}

![connection string to hugging face](images/hugging-face.png "Define a connection string to Hugging Face")

1. **Name**  
   Enter a name for this connection string.

2. **Identifier** (optional)  
   Learn more about the identifier in the [connection string identifier](../../ai-integration/connection-strings/connection-strings-overview#the-connection-string-identifier) section.

3. **Connector**  
   Select **Hugging Face** from the dropdown menu.

4. **API Key**  
   Enter the API key used to authenticate requests to Hugging Face's text embedding services.

5. **Endpoint** (optional)  
   Enter the Hugging Face endpoint for generating embeddings from text.  
   If not specified, the default endpoint is used.  
   (`https://api-inference.huggingface.co/`)

6. **Model**  
   Specify the Hugging Face text embedding model to use.

7. Click **Test Connection** to confirm the connection string is set up correctly.

8. Click **Save** to store the connection string or **Cancel** to discard changes.

{PANEL/}

{PANEL: Define the connection string - from the Client API}

{CODE:csharp create_connection_string_hugging_face@AiIntegration\ConnectionStrings\connectionStrings.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE:csharp hugging_face_settings@AiIntegration\ConnectionStrings\connectionStrings.cs /}

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
