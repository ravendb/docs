# The Embeddings Generation Task
---

{NOTE: }

* In RavenDB, you can define AI tasks to automatically generate embeddings from your document content.  
  These embeddings are then stored in [dedicated collections](../../ai-integration/generating-embeddings/embedding-collections) within the database,  
  enabling [vector search](../../ai-integration/vector-search/ravendb-as-vector-database) on your documents.

* This article explains how to configure such a task.  
  It is recommended to first refer to this [overview](../../ai-integration/generating-embeddings/overview#embeddings-generation---overview)
  to understand the embeddings generation process flow.

* In this page:
    * [Configuring an embeddings generation task - from the Studio](../../ai-integration/generating-embeddings/embeddings-generation-task#configuring-an-embeddings-generation-task---from-the-studio)
    * [Configuring an embeddings generation task - from the Client API](../../ai-integration/generating-embeddings/embeddings-generation-task#configuring-an-embeddings-generation-task---from-the-client-api)
    * [Chunking methods and tokens](../../ai-integration/generating-embeddings/embeddings-generation-task#chunking-methods-and-tokens)
    * [Syntax](../../ai-integration/generating-embeddings/embeddings-generation-task#syntax)
    
{NOTE/}

---

{PANEL: Configuring an embeddings generation task - from the Studio}

* **Define the general task settings**:

     ![Create embeddings generation task - general](images/add-ai-task-3.png)

  1. **Name**  
     Enter a name for the task.
  2. **Identifier**  
     Enter a unique identifier for the task.  
     Each AI task in the database must have a distinct identifier.  

        If not specified, or when clicking the "Regenerate" button,  
        RavenDB automatically generates the identifier based on the task name. For example:     
        * If the task name is: _"Generate embeddings from OpenAI service"_
        * The generated identifier will be: _"generate-embeddings-from-openai"_
      
        Allowed characters: only lowercase letters (a-z), numbers (0-9), and hyphens (-).  
        See how this identifier is used in the [embeddings collection](../../ai-integration/generating-embeddings/embedding-collections#the-embeddings-collection)
        documents that reference the generated embeddings.  
  
  3. **Regenerate**  
     Click "Regenerate" to automatically create an identifier based on the task name.
  4. **Task State**  
     Enable/Disable the task.
  5. **Responsible node**  
     Select a node from the [database group](../../studio/database/settings/manage-database-group) to be the responsible node for this task.
  6. **Connection string**  
     Select a previously defined [AI connection string](../../ai-integration/connection-strings/connection-strings-overview) or create a new one.
  7. **Enable document expiration**  
     This toggle appears only if the [document expiration feature](../../studio/database/settings/document-expiration) is Not enabled in the database.
     Enabling document expiration ensures that embeddings in the `@embeddings-cache` collection are automatically deleted when they expire.
  8. **Save**  
     Click _Save_ to store the task definition or _Cancel_.

---

* **Define the embeddings source - using "Paths"**:
  
    ![Create embeddings generation task - source by paths](images/add-ai-task-4.png)

  1. **Collection**  
     Enter or select the source document collection from the dropdown.
  2. **Embeddings source**  
     Select `Paths` to define the source content specifying document properties.
  3. **Source Text Path**  
     Enter the property name from the document that contains the text for embedding generation.
  4. **Chunking Method**  
     Select the method for splitting the source text into chunks.  
     Learn more in [chunking methods and tokens](../../ai-integration/generating-embeddings/embeddings-generation-task#chunking-methods-and-tokens).
  5. **Max tokens per chunk**  
     Enter the maximum number of tokens allowed per chunk (this depends on the service provider).
  6. **Add path configuration**  
     Click to add the specified to the list.

---

* **Define the embeddings source - using "Script"**:
  
    ![Create embeddings generation task - source by script](images/add-ai-task-4-script.png)

  1. **Embeddings source**  
     Select `Script` to define the source content and chunking methods using a JavaScript script.
  2. **Script**  
     Refer to the [syntax section](../../todo) for available JavaScript methods.
     
---

* **Define quantization and expiration -  
  for the generated embeddings**:

    ![Create embeddings generation task - quantization and expiration](images/add-ai-task-5.png)

  1. **Quantization**  
     Select the quantization method that RavenDB will apply to embeddings received from the service provider.  
     Available options:  
     * Single (no quantization)
     * Int8
     * Binary
  2. **Embeddings cache expiration**  
     Set the expiration period for documents stored in the `@embeddings-cache` collection.  
     These documents contain embeddings generated from the source documents, serving as a cache for these embeddings.  
     The default initial period is `90` days. This period may be extended when the source documents change.  
     Learn more in [The embeddings cache collection](../../ai-integration/generating-embeddings/embedding-collections#the-embeddings-cache-collection).
  3. **Regenerate embeddings**  
     This toggle is visible only when editing an exiting task.  
     Toggle ON to regenerate embeddings for all documents in the collection, as specified by the _Paths_ or _Script_.

---

* **Define quantization and expiration -  
  for the embedding generated from a search term in a vector search query**:

    ![Create embeddings generation task - for the query](images/add-ai-task-6.png)

  1. **Querying**  
     This label indicates that this section configures parameters only for embeddings  
     generated by the task for **search terms** in vector search queries.
  2. **Chunking Method**  
     Select the method for splitting the search term into chunks.  
     Learn more in [chunking methods and tokens](../../ai-integration/generating-embeddings/embeddings-generation-task#chunking-methods-and-tokens).
  3. **Max tokens per chunk**  
     Enter the maximum number of tokens allowed per chunk (this depends on the service provider).
  4. **Embeddings cache expiration**  
     Set the expiration period for documents stored in the `@embeddings-cache` collection.  
     These documents contain embeddings generated from the search terms, serving as a cache for these embeddings.  
     The default period is `14` days. Learn more in [The embeddings cache collection](../../ai-integration/generating-embeddings/embedding-collections#the-embeddings-cache-collection).

{PANEL/}

{PANEL: Configuring an embeddings generation task - from the Client API}

{PANEL/}

{PANEL: Chunking methods and tokens}

{CONTENT-FRAME: }

* **Text chunking methods**:  
  To manage lengthy texts, you can define the chunking strategy in the task definition and specify the number of tokens per chunk. 
  RavenDB provides several chunking methods to split input text before sending it to the provider:

    * Plain Text:
        * `Split`: Divides text based on a specified delimiter.
        * `Split Lines`: Separates text at line breaks.
        * `Split Paragraphs`: Splits text at paragraph breaks.
    * Markdown:
        * `Split Lines`: Divides markdown content at line breaks.
        * `Split Paragraphs`: Splits markdown content at paragraph breaks.
    * HTML:
        * `Strip`: Removes HTML tags, processing only the textual content.

* **Chunking and embeddings**:  
  A single embedding is generated by the service provider for each chunk.  
  Depending on the maximum tokens per chunk setting, a single piece of input text may result in multiple embeddings.

* **Understanding tokens**:   
  A token is the basic unit that Large Language Models (LLMs) use to process text.  
  RavenDB does not tokenize text but approximates the number of tokens for text chunking purposes.  
  The approximation is always the length of the text divided by 4.

     RavenDB's chunking utility, powered by Microsoft's Semantic Kernel library, splits large input text into smaller chunks,
     ensuring that each chunk contains no more than the specified maximum number of tokens.  
     The maximum number of tokens per chunk varies depending on the AI service provider and the specific model specified in the [connection string](../../ai-integration/connection-strings/connection-strings-overview).

* **Why token limits matter**:  
  If your text exceeds the token limit, the service provider may truncate or reject it.

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Syntax}

{PANEL/}

## Related Articles

### Vector Search

- [RavenDB as a vector database](../../ai-integration/vector-search/ravendb-as-vector-database)
- [Vector search using a static index](../../ai-integration/vector-search/vector-search-using-static-index)
- [Vector search using a dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query)

### Embeddings Generation

- [Generating Embeddings - overview](../../ai-integration/generating-embeddings/overview)
- [The Embedding Collections](../../ai-integration/generating-embeddings/embedding-collections)

### AI Connection Strings

- [Connection strings - overview](../../ai-integration/connection-strings/connection-strings-overview)
- [Azure Open AI](../../ai-integration/connection-strings/azure-open-ai)
- [Google AI](../../ai-integration/connection-strings/google-ai)
- [Hugging Face](../../ai-integration/connection-strings/hugging-face)
- [Ollama](../../ai-integration/connection-strings/ollama)
- [OpenAI](../../ai-integration/connection-strings/open-ai)
- [Mistral AI](../../ai-integration/connection-strings/mistral-ai)
- [Embedded model](../../ai-integration/connection-strings/embedded)
