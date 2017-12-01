using System;
using Raven.Client.Documents;
using Sparrow.Json;
using GetDocumentCommand = Raven.Client.Documents.Commands.GetDocumentCommand;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents.HowTo
{
    public class Foo
    {
        public class GetDocumentCommand
        {
            #region head_1
            public GetDocumentCommand(string id, string[] includes, bool metadataOnly)
            #endregion
            {

            }
        }
    }

    public class Foo2
    { 
        public Foo2()
        {
            using (var docStore = new DocumentStore())
            using (var dbSession = docStore.OpenSession())
            using (var opContext = JsonOperationContext.ShortTermSingleUse())
            {
                #region head_2
                var getMetaCommand = new GetDocumentCommand("orders/1-A", null, metadataOnly: true);
                dbSession.Advanced.RequestExecutor.Execute(getMetaCommand, opContext);
                var result = (BlittableJsonReaderObject)getMetaCommand.Result.Results[0];
                var docMeta = (BlittableJsonReaderObject)result["@metadata"];

                // Print out all the metadata properties.
                foreach (var metaPropName in docMeta.GetPropertyNames())
                {
                    docMeta.TryGet<object>(metaPropName, out var metaPropValue);
                    Console.WriteLine("{0} = {1}", metaPropName, metaPropValue);
                }
                #endregion
            }
        }
    }
}
