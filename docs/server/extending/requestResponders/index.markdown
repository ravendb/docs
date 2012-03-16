# RequestResponders

Raven allow users to extend the type of requests it can handle. That is done by subclassing the RequestResponder class and dropping the resulting dll in the Plugins directory.
For example, let us assume that you have a burning desire to know what is the size of a document without retrieving it. You can do that by implementing the following responder:

    public class DocSize : RequestResponder
    {
        public override string UrlPattern
        {
            get { return "/docsize/(.+)"; }
        }
    
        public override string[] SupportedVerbs
        {
            get { return new[] { "GET" }; }
        }
    
        public override void Respond(IHttpContext context)
        {
            var match = urlMatcher.Match(context.Request.Url.LocalPath);
            var docId = match.Groups[1].Value;
            var jsonDocument = Database.Get(docId, null);
            if (jsonDocument == null)
            {
                context.SetStatusToNotFound();
                return;
            }
            context.WriteJson(new { Key = docId, Size = jsonDocument.Data.Length });
        }
    }
 
The next step is compiling this and dropping the dll into the Plugins directory. With that, we can issue:

    > curl -X GET http://localhost:8080/docsize/bobs_address

Assuming there is a document with an id of "bobs_address", RavenDB will respond with the contents of that document and an HTTP 200 OK response code:

    HTTP/1.1 200 OK
    
    {
      "Key": "bobs_address",
      "Size": "396"
    }

##Expected usages
This feature is provided mostly for Raven's own need, more than to provide an extension point for users.
An expected usage scenario is to perform expensive server side operations, for example, evaluating shortest path between two linked documents.