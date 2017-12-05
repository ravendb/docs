using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session
{
    public class Events
    {
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

        private void OnBeforeDeleteEvent(object sender, BeforeDeleteEventArgs args)
        {
          
        }

        #region on_before_query_execute_event
        private readonly List<string> _recentQueries = new List<string>();
        private void OnBeforeQueryExecutedEvent(object sender, BeforeQueryExecutedEventArgs args)
        {
            _recentQueries.Add(args.QueryCustomization.ToString());
        }
        #endregion

        private void OnAfterStoreEvent(object sender, AfterStoreEventArgs args)
        {
           
        }

        public Events()
        {
            using (var store = new DocumentStore())
            {
                store.OnAfterStore += OnAfterStoreEvent;
                store.OnBeforeDelete += OnBeforeDeleteEvent;
                store.OnBeforeQueryExecuted += OnBeforeQueryExecutedEvent;

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

            }
        }
    }
}
