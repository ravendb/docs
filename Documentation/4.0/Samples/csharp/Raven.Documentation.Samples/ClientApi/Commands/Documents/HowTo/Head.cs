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
            {
                #region head_2
                // Create a command to fetch the metadata.
                var docId = "FooBars/1-A";
                var getMetaCommand = new GetDocumentCommand(docId, includes: null, metadataOnly: true);

                var executor = Raven.Client.Http.RequestExecutor.Create(docStore.Urls, docStore.Database, docStore.Certificate, docStore.Conventions);
                using (var opContext = JsonOperationContext.ShortTermSingleUse())
                {
                    // Fetch the metadata
                    executor.Execute(getMetaCommand, opContext);
                    var metaResult = getMetaCommand.Result; // null if does not exist

                    // We asked for a single document, so get the first (only) item.
                    var metaResultItem = (BlittableJsonReaderObject)metaResult.Results[0];

                    // We asked for metadata only, so grab that object.
                    var docMeta = (BlittableJsonReaderObject)metaResultItem["@metadata"];

                    // Print out all the metadata properties.
                    foreach (var metaPropName in docMeta.GetPropertyNames())
                    {
                        docMeta.TryGet<object>(metaPropName, out var metaPropValue);
                        Console.WriteLine("{0} = {1}", metaPropName, metaPropValue);
                    }
                }
                #endregion
            }
        }
    }
}
