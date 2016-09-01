using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Json.Linq;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents
{
    public class Stream
    {
        #region transformer
        private class SimpleTransformer : AbstractTransformerCreationTask
        {
            public class Result
            {
                public string Name { get; set; }
            }

            public override TransformerDefinition CreateTransformerDefinition(bool prettify = true)
            {
                return new TransformerDefinition
                {
                    Name = "SimpleTransformer",
                    TransformResults = "from r in results select new { Name = Parameter(\"Name\") }"
                };
            }
        }
        #endregion

        private interface IFoo
        {
            #region stream_1

            IEnumerator<RavenJObject> StreamDocs(
                Etag fromEtag = null,
                string startsWith = null,
                string matches = null,
                int start = 0,
                int pageSize = int.MaxValue,
                string exclude = null,
                RavenPagingInformation pagingInformation = null,
                string skipAfter = null,
                string transformer = null,
                Dictionary<string, RavenJToken> transformerParameters = null);

            #endregion
        }

        public Stream()
        {
            using (var store = new DocumentStore())
            {
                #region stream_2

                IEnumerator<RavenJObject> enumerator = store.DatabaseCommands.StreamDocs(null, "products/");
                while (enumerator.MoveNext())
                {
                    RavenJObject document = enumerator.Current;
                }

                #endregion
            }
        }

        public void StreamWithTransformer()
        {
            var store = new DocumentStore();

            #region stream_3

            var transformer = new SimpleTransformer();
            transformer.Execute(store);

            using (IEnumerator<RavenJObject> enumerator = store.DatabaseCommands.StreamDocs(startsWith: "people/", transformer: transformer.TransformerName, transformerParameters: new Dictionary<string, RavenJToken> {{"Name", "Bill"}}))
            {
                while (enumerator.MoveNext())
                {
                    RavenJObject result = enumerator.Current;
                    string name = result.Value<string>("Name");
                    Assert.Equal("Bill", name); // Should be true
                }
            }
            #endregion
        }
    }
}