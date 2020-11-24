using System;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries;
using Raven.Documentation.Samples.Orders;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.Indexes
{
    class AdditionalAssemblies
    {
        public void Example()
        {
            using (var store = new DocumentStore())
            {
                #region complex_index
                store.Maintenance.Send(new PutIndexesOperation(new IndexDefinition
                {
                    Name = "Photographs/Tags",
                    Maps =
                    {
                        @"
                        from p in docs.Photographs
                        let photo = LoadAttachment(p, ""photo.png"")
                        where photo != null
                        let classified =  ImageClassifier.Classify(photo.GetContentAsStream())
                        select new {
                            e.Name,
                            Tag = classified.Where(x => x.Value > 0.75f).Select(x => x.Key),
                            _ = classified.Select(x => CreateField(x.Key, x.Value))
                        }"
                    },
                    AdditionalSources = new System.Collections.Generic.Dictionary<string, string>
                    {
                        {
                            "ImageClassifier", 
                            @"
                            public static class ImageClassifier
                            {
                                public static IDictionary<string, float> Classify(Stream s)
                                {
                                    // returns a list of descriptors with a
                                    // value between 0 and 1 of how well the
                                    // image matches that descriptor.
                                }
                            }"
                        }

                    },
                    AdditionalAssemblies =
                    {
                        AdditionalAssembly.FromRuntime("System.Memory"),
                        AdditionalAssembly.FromNuGet("System.Drawing.Common", "4.7.0"),
                        AdditionalAssembly.FromNuGet("Microsoft.ML", "1.5.2")
                    }
                }));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region simple_index
                    var runtimeindex = new IndexDefinition
                    {
                        Name = "Dog_Pictures",
                        Maps = { @"
                            from user in docs.Users
                            let fileName = Path.GetFileName(user.ImagePath)
                            where fileName = ""My_Dogs.jpeg""
                            select new {
                                user.Name,
                                fileName
                            }"
                        },
                        AdditionalAssemblies = {
                            AdditionalAssembly.FromRuntime("system.IO")
                        }
                    };
                    #endregion
                }
            }
        }
    }
}
