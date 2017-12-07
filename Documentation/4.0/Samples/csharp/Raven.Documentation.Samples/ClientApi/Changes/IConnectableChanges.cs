using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents.Changes;

namespace Raven.Documentation.Samples.ClientApi.Changes
{
    #region connectable_changes
    public interface IConnectableChanges<TChanges> : IDisposable
        where TChanges : IDatabaseChanges
    {
        // returns state of the connection
        bool Connected { get; }

        // A task that ensures that the connection to the server was established.
        Task<TChanges> EnsureConnectedNow();

        //An event handler to detect changed to the connection status
        event EventHandler ConnectionStatusChanged;

        //An action to take if an error occured in the connection to the server
        event Action<Exception> OnError;
    }
    #endregion
}
