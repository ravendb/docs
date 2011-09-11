using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RavenCodeSamples.Server
{
    public class Bundles : CodeSampleBase
    {
        public void VersioningBundle()
        {
            using (var store = NewDocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region versioning1
                    session.Store(new {
                                          Exclude = false,
                                          Id = "Raven/Versioning/DefaultConfiguration",
                                          MaxRevisions = 5
                                      });
                    #endregion

                    #region versioning2

                    session.Store(new {
                                          Exclude = true,
                                          Id = "Raven/Versioning/Users",
                                      });

                    #endregion
                }
            }
        }
    }
}
