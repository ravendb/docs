using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json.Parsing;
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
                using (var session = store.OpenSession())
                {
                    // these are session events, related only to this session.
                    // the equivalent store event for session.Advanced.OnAfterSaveChanges,
                    // for example, would be store.OnAfterSaveChanges.
                    session.Advanced.OnAfterSaveChanges += OnAfterSaveChangesEvent;
                    session.Advanced.OnBeforeDelete += OnBeforeDeleteEvent;
                    session.Advanced.OnBeforeQuery += OnBeforeQueryEvent;
                    session.Advanced.OnBeforeConversionToDocument += OnBeforeConversionToDocument;
                    session.Advanced.OnAfterConversionToDocument += OnAfterConversionToDocument;
                    session.Advanced.OnBeforeConversionToEntity += OnBeforeConversionToEntity;
                    session.Advanced.OnAfterConversionToEntity += OnAfterConversionToEntity;
                }

                // Open a session and store some entities
                using (var session = store.OpenSession())
                {
                    #region store_session
                    // Subscribe to the event
                    session.Advanced.OnBeforeStore += OnBeforeStoreEvent;

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
                    #endregion
                }

                // Open a session and delete entity
                using (var session = store.OpenSession())
                {
                    #region delete_session
                    // Subscribe to the event
                    session.Advanced.OnBeforeDelete += OnBeforeDeleteEvent;

                    var product = session.Load<Product>("products/1-A");
                    var product2 = session.Load<Product>("products/2-A");

                    // OnBeforeDelete is triggered whether you
                    // call Delete() on an entity or on its ID
                    session.Delete(product);
                    session.SaveChanges(); // NotSupportedException will be thrown

                    session.Delete("products/2-A");
                    session.SaveChanges(); // NotSupportedException will be thrown
                    #endregion
                }
            }
        }

        #region on_before_conversion_to_document
        private void OnBeforeConversionToDocument(object sender, BeforeConversionToDocumentEventArgs args)
        {
            if (args.Entity is Item item)
                item.Before = true;
        }
        #endregion

        #region on_after_conversion_to_document
        private void OnAfterConversionToDocument(object sender, AfterConversionToDocumentEventArgs args)
        {
            if (args.Entity is Item item)
            {
                if (args.Document.Modifications == null)
                    args.Document.Modifications = new DynamicJsonValue();

                args.Document.Modifications["After"] = true;
                args.Document = args.Session.Context.ReadObject(args.Document, args.Id);

                item.After = true;
            }
        }
        #endregion

        #region on_after_conversion_to_entity
        private void OnAfterConversionToEntity(object sender, AfterConversionToEntityEventArgs args)
        {
            if (args.Entity is Item item)
                item.After = true;
        }
        #endregion

        #region on_before_conversion_to_entity
        private void OnBeforeConversionToEntity(object sender, BeforeConversionToEntityEventArgs args)
        {
            var document = args.Document;
            if (document.Modifications == null)
                document.Modifications = new DynamicJsonValue();

            document.Modifications["Before"] = true;
            args.Document = args.Session.Context.ReadObject(document, args.Id);
        }
        #endregion
    }

    internal class Item
    {
        public bool Before { get; set; }
        public bool After { get; set; }
    }
}
