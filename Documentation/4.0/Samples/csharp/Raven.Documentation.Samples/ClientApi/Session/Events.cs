using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Sparrow.Logging;

namespace Raven.Documentation.Samples.ClientApi.Session
{
    public class Events
    {
        private static readonly Logger Log;

        #region on_before_store_event
        private void OnBeforeStoreEvent(object sender, BeforeStoreEventArgs args)
        {
            var product = args.Entity as Product;
            if (product?.UnitsInStock == 0)
            {
                product.Discontinued = true;
            }
        }
        #endregion

        #region on_before_delete_event
        private void OnBeforeDeleteEvent(object sender, BeforeDeleteEventArgs args)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region on_before_query_execute_event
        private void OnBeforeQueryEvent(object sender, BeforeQueryEventArgs args)
        {
            args.QueryCustomization.NoCaching();
        }
        #endregion

        private class Foo
        {
            #region on_before_query_execute_event_2
            private void OnBeforeQueryEvent(object sender, BeforeQueryEventArgs args)
            {
                args.QueryCustomization.WaitForNonStaleResults(TimeSpan.FromSeconds(30));
            }
            #endregion
        }

        #region on_after_save_changes_event
        private void OnAfterSaveChangesEvent(object sender, AfterSaveChangesEventArgs args)
        {
            if (Log.IsInfoEnabled)
                Log.Info($"Document '{args.DocumentId}' was saved.");
        }
        #endregion

        public Events()
        {
            using (var store = new DocumentStore())
            {
                store.OnAfterSaveChanges += OnAfterSaveChangesEvent;
                store.OnBeforeDelete += OnBeforeDeleteEvent;
                store.OnBeforeQuery += OnBeforeQueryEvent;

                #region store_session
                // Subscribe to the event
                store.OnBeforeStore += OnBeforeStoreEvent;

                // Open a session and store some entities
                using (var session = store.OpenSession())
                {
                    session.Store(new Product
                    {
                        Name = "RavenDB v3.5",
                        UnitsInStock = 0
                    });
                    session.Store(new Product
                    {
                        Name = "RavenDB v4.0",
                        UnitsInStock = 1000
                    });

                    session.SaveChanges(); // Here the method is invoked
                }
                #endregion

                #region delete_session
                // Subscribe to the event
                store.OnBeforeDelete += OnBeforeDeleteEvent;

                // Open a session and delete entity
                using (var session = store.OpenSession())
                {
                    var product = session.Load<Product>("products/1-A");
                    session.Delete(product);


                    session.SaveChanges(); // NotSupportedException will be thrown here
                }
                #endregion

            }
        }
    }
}
