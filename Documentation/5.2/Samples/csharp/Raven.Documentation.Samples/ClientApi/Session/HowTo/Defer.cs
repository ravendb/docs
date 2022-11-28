using Raven.Client.Documents;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Operations;
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
                    #region defer_1

                    // Defer is available in the session's Advanced methods 
                    session.Advanced.Defer(
                        
                        // Define commands to be executed:
                        // i.e. Put a new document
                        new PutCommandData("products/999-A", null, new DynamicJsonValue
                            {
                                ["Name"] = "My Product",
                                ["Supplier"] = "suppliers/1-A",
                                ["@metadata"] = new DynamicJsonValue
                                {
                                    ["@collection"] = "Products"
                                }
                            }),
                        
                        // Patch document
                        new PatchCommandData("products/999-A", null, new PatchRequest
                                {
                                    Script = "this.Supplier = 'suppliers/2-A';"
                                },
                                null),
                        
                        // Force a revision to be created
                        new ForceRevisionCommandData("products/999-A"),
                        
                        // Delete a document
                        new DeleteCommandData("products/1-A", null)
                    );
                    
                    // All deferred commands will be sent to the server upon calling SaveChanges
                    session.SaveChanges();

                    #endregion
                }
            }
        }

        private interface IFoo
        {
            #region syntax

            void Defer(ICommandData command, params ICommandData[] commands);
            void Defer(ICommandData[] commands);

            #endregion
        }
    }
}
