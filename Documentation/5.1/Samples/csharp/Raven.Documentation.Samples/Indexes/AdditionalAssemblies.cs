using System;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries;
using Raven.Documentation.Samples.Orders;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;

namespace Raven.Documentation.Samples.Indexes
{
    class AdditionalAssemblies
    {
        public void Examples()
        {
            using (var store = new DocumentStore()) {
                using (var session = store.OpenSession())
                {
                    #region index_1
                    var runtimeIndex = new IndexDefinition
                    {
                        Name = "XmlIndex",
                        Maps = {
                            "from c in docs.Companies select new { Name = typeof(System.Xml.XmlNode).Name }"
                        },
                        AdditionalAssemblies = {
                            AdditionalAssembly.FromRuntime("System.Xml", "System.Xml.ReaderWriter"),
                            AdditionalAssembly.FromRuntime("System.Private.Xml")
                        }
                    };
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region index_2
                    var runtimeIndex = new IndexDefinition
                    {
                        Name = "XmlIndex",
                        Maps = {
                            "from c in docs.Companies select new { Name = typeof(System.Xml.XmlNode).Name }"
                        },
                        AdditionalAssemblies = {
                            AdditionalAssembly.FromPath("C:/Path/To/Local/Assembly");
                        }
                    };
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region index_3
                    var runtimeIndex = new IndexDefinition
                    {
                        Name = "XmlIndex",
                        Maps = {
                            "from c in docs.Companies select new { Name = typeof(System.Xml.XmlNode).Name }"
                        },
                        AdditionalAssemblies =
                        {
                            AdditionalAssembly.FromRuntime("System.Private.Xml"),
                            AdditionalAssembly.FromNuGet("System.Xml.ReaderWriter", "4.3.1")
                        }
                    };
                    #endregion
                }
            }
        }
    }
}
