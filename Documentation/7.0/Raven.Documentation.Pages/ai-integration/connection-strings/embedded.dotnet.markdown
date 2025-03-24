# Connection String to bge-micro-v2 (Embedded)
---

{NOTE: }

* This article explains how to define a connection string to the [bge-micro-v2](https://huggingface.co/TaylorAI/bge-micro-v2) model.  
  This model is embedded within RavenDB, enabling RavenDB to seamlessly handle its  
  [embeddings generation tasks](../../ai-integration/generating-embeddings/overview) without requiring an external AI service.

* In this article:
  * [Define the connection string - from the Studio](../../ai-integration/connection-strings/embedded#define-the-connection-string---from-the-studio)
  * [Define the connection string - from the Client API](../../ai-integration/connection-strings/embedded#define-the-connection-string---from-the-client-api)
  * [Syntax](../../ai-integration/connection-strings/embedded#syntax)

{NOTE/}

---

{PANEL: Define the connection string - from the Studio}

![connection string to the embedded model](images/embedded.png "Define a connection string to the embedded model")

1. **Name**  
   Enter a name for this connection string.

2. **Identifier** (optional)  
   Learn more about the identifier in the [connection string identifier](../../ai-integration/connection-strings/connection-strings-overview#the-connection-string-identifier) section.

3. **Connector**  
   Select **Embedded (bge-micro-v2)** from the dropdown menu.

4. Click **Save** to store the connection string or **Cancel** to discard changes.

{PANEL/}

{PANEL: Define the connection string - from the Client API}

{CODE:csharp create_connection_string_embedded@AiIntegration\ConnectionStrings\connectionStrings.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE:csharp embedded_settings@AiIntegration\ConnectionStrings\connectionStrings.cs /}

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
