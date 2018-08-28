using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands.Batches;
using Sparrow.Json.Parsing;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class Defer
    {
        public Defer()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region defer_2

                    session
                        .Advanced
                        .Defer(
                            new PutCommandData("products/999-A", null, new DynamicJsonValue
                            {
                                ["Name"] = "My Product",
                                ["Supplier"] = "suppliers/999-A",
                                ["@metadata"] = new DynamicJsonValue
                                {
                                    ["@collection"] = "Users"
                                }
                            }),
                            new PutCommandData("suppliers/999-A", null, new DynamicJsonValue
                            {
                                ["Name"] = "My Product",
                                ["Supplier"] = "suppliers/999-A",
                                ["@metadata"] = new DynamicJsonValue
                                {
                                    ["@collection"] = "Suppliers"
                                }
                            }),
                            new DeleteCommandData("products/1-A", null)
                        );

                    #endregion
                }
            }
        }

        private interface IFoo
        {
            #region defer_1

            void Defer(ICommandData command, params ICommandData[] commands);
            void Defer(ICommandData[] commands);

            #endregion
        }
    }
}
