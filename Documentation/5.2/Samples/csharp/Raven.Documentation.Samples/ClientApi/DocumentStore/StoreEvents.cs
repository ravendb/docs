using System;
using System.Text.RegularExpressions;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Http;
using Raven.Documentation.Samples.Orders;
using Sparrow.Json.Parsing;
using Sparrow.Logging;

namespace Raven.Documentation.Samples.ClientApi.Session
{
    public class StoreEvents
    {
        private static readonly Logger Log;

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
                // action to be taken after a successful request
            }
        }
        #endregion

        #region OnSessionCreatedEvent
        private void OnSessionCreatedEvent(object sender, SessionCreatedEventArgs args)
        {
                args.Session.MaxNumberOfRequestsPerSession = 100;
        }
        #endregion

        #region OnFailedRequestEvent
        private void OnFailedRequestEvent(object sender, FailedRequestEventArgs args)
        {
            Logger($"Failed request for database '{args.Database}' ('{args.Url}')", args.Exception);
        }
        #endregion

        private void Logger(string txt, System.Exception Exception)
        {
        }

        #region OnTopologyUpdatedEvent
        void OnTopologyUpdatedEvent(object sender, TopologyUpdatedEventArgs args)
        {
            var topology = args.Topology;
            if (topology == null)
                return;
            for (var i = 0; i < topology.Nodes.Count; i++)
            {
                // perform relevant operations on the nodes after the topology was updated
            }
        }
        #endregion

        void Events()
        {

            using (var store = new DocumentStore())
            {
                #region SubscribeToOnBeforeRequest
                // Subscribe to the event
                store.OnBeforeRequest += OnBeforeRequestEvent;
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region SubscribeToOnSucceedRequest
                // Subscribe to the event
                store.OnSucceedRequest += OnSucceedRequestEvent;
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region SubscribeToOnSessionCreated
                // Subscribe to the event
                store.OnSessionCreated += OnSessionCreatedEvent;
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region SubscribeToOnFailedRequest
                // Subscribe to the event
                store.OnFailedRequest += OnFailedRequestEvent;
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region SubscribeToOnTopologyUpdated
                // Subscribe to the event
                store.OnTopologyUpdated += OnTopologyUpdatedEvent;
                #endregion
            }

        }

    }

}
