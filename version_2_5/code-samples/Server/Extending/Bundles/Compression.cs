using Raven.Abstractions.Data;
using Raven.Client.Extensions;

namespace RavenCodeSamples.Server.Bundles
{
    public class Compression : CodeSampleBase
    {
        public void Sample()
        {
            using (var store = NewDocumentStore())
            {
                #region compression_1
                store.DatabaseCommands.CreateDatabase(new DatabaseDocument
                {
                    Id = "CompressedDB",
                    // Other configuration options omitted for simplicity
                    Settings =
                            {
                                // ...
                                {"Raven/ActiveBundles", "Compression"}
                            }
                });

                #endregion
            }
        }
    }
}