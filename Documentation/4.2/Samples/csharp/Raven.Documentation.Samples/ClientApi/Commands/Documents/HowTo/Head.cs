using System;
using Raven.Client.Documents;
using Sparrow.Json;
using GetDocumentsCommand = Raven.Client.Documents.Commands.GetDocumentsCommand;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents.HowTo
{
    public class Foo
    {
        public class GetDocumentsCommand
        {
            #region head_1
            public GetDocumentsCommand(string id, string[] includes, bool metadataOnly)
            #endregion
            {

            }

            #region head_3
            public GetDocumentsCommand(string[] ids, string[] includes, bool metadataOnly)
                #endregion
            {

            }
        }
    }

    public class Foo2
    {
        public Foo2()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region head_2
                var command = new GetDocumentsCommand("orders/1-A", null, metadataOnly: true);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                var result = (BlittableJsonReaderObject)command.Result.Results[0];
                var documentMetadata = (BlittableJsonReaderObject)result["@metadata"];

                // Print out all the metadata properties.
                foreach (var propertyName in documentMetadata.GetPropertyNames())
                {
                    documentMetadata.TryGet<object>(propertyName, out var metaPropValue);
                    Console.WriteLine("{0} = {1}", propertyName, metaPropValue);
                }
                #endregion
            }
        }
    }
}
