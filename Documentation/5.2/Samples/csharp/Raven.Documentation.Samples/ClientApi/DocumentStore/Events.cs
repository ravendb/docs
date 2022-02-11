using System;
using System.Text.RegularExpressions;
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

        #region OnBeforeRequestEvent
        private void OnBeforeRequestEvent(object sender, BeforeRequestEventArgs args)
        {
            var forbiddenURL = new Regex("/databases/[^/]+/docs");

            if (forbiddenURL.IsMatch(args.Url) == true)
            {
                // action to be taken if the URL is forbidden
            }
        }
        #endregion

        #region OnSucceedRequestEvent
        private void OnSucceedRequestEvent(object sender, SucceedRequestEventArgs args)
        {
            if (args.Response.IsSuccessStatusCode == true)
            {
                // action to be taken if the request was successful
            }
        }
        #endregion

        public Events()
        {
            #region SubscribeToOnBeforeRequest
            using (var store = new DocumentStore())
            {
                // Subscribe to the event
                store.OnBeforeRequest += OnBeforeRequestEvent;
                #endregion
            }

            #region SubscribeToOnSucceedRequest
            using (var store = new DocumentStore())
            {
                // Subscribe to the event
                store.OnSucceedRequest += OnSucceedRequestEvent;
                #endregion
            }

        }

    }

}
