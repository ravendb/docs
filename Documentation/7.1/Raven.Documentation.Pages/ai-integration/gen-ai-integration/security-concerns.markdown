# GenAI Integration: Security Concerns
---

{NOTE: }

This page addresses concerns that potential GenAI tasks' users may have, 
regarding the safety of data sent to an AI model through the task and the 
security of the database while running such tasks.

* In this article:
    * [Security measures](../../ai-integration/gen-ai-integration/security-concerns#security-measures)
    
{NOTE/}

---

{PANEL: Security measures}

Our approach toward data safety while using RavenDB AI tasks, is that we need 
to take care of security on our end, rather than expect the AI model to protect 
our data.  

You can take these security measures:  

* **Use a local model when possible**  
  Use a local AI model like Ollama whenever you don't have to transit your data 
  to an external model, to keep the data, as much as possible, within the safe 
  boundaries of your own network.  

* **Pick the right model**  
  RavenDB does not dictate what model to use, giving you full freedom to pick 
  the services that you want to connect.  
  Choose wisely the AI model you connect, some seem to be in better hands than others.  

* **Send only the data you want to send**  
  You are in full control of the data that is sent from your server to the AI model.  
  Your choices while defining the task, including the collection you associate the 
  task with and the [context generation script](../../ai-integration/gen-ai-integration/gen-ai-studio#generate-context-objects) 
  you define, determine the only data that will be exposed to the AI model.  
  Take your time, when preparing this script, to make sure you send only the 
  data you actually want to send.  

* **Use the playgrounds**  
  While defining your AI task, take the time and use Studio's 
  [playgrounds](../../ai-integration/gen-ai-integration/gen-ai-studio#generate-context-objects-playground) 
  to double-check what is actually sent.  
  There are separate playgrounds for the different stages, using them is 
  really enjoyable, and you can test your configuration on various documents 
  and see exactly what you send and what you receive.  

* **Use a secure server**  
  The AI model is **not** given entry to your database. The data that you send it 
  voluntarily is all it gets. However, as always, if you care about your privacy 
  and safety, you'd want to use a [secure server](../../start/installation/setup-wizard#select-setup-mode).  
  This will assure that you have full control over visitors to your database and 
  their permissions.

* **Use your update script wisely**  
  When considering threats to our data we often focus on external risks, 
  but many times it is us that endanger it the most.  
  The [update script](../../ai-integration/gen-ai-integration/gen-ai-studio#provide-update-script) 
  is the JavaScript that the GenAI task runs after receiving a reply from 
  the AI model. Here too, take your time to check this powerful script 
  using the built in Studio [playground](../../ai-integration/gen-ai-integration/gen-ai-studio#provide-update-script-playground).  

{PANEL/}

## Related Articles

### GenAI Integration

- [GenAI Overview](../../ai-integration/gen-ai-integration/gen-ai-overview)
- [GenAI API](../../ai-integration/gen-ai-integration/gen-ai-api)  
- [GenAI Studio](../../ai-integration/gen-ai-integration/gen-ai-studio)

### Vector Search

- [Vector search using a dynamic query](../../ai-integration/vector-search/vector-search-using-dynamic-query.markdown)
- [Vector search using a static index](../../ai-integration/vector-search/vector-search-using-static-index.markdown)
- [Data types for vector search](../../ai-integration/vector-search/data-types-for-vector-search)
