using System.Collections.Generic;

using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Sparrow.Json;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents
{
    public class GetInterfaces
    {
        public class GetDocumentsCommand
        {
            #region get_interface_single
            public GetDocumentsCommand(string id, string[] includes, bool metadataOnly)
            #endregion
            {

            }

            #region get_interface_multiple
            public GetDocumentsCommand(string[] ids, string[] includes, bool metadataOnly)
            #endregion
            {

            }

            #region get_interface_paged
            public GetDocumentsCommand(int start, int pageSize)
            #endregion
            {

            }

            #region get_interface_startswith
            public GetDocumentsCommand(string startWith, string startAfter, string matches, string exclude, int start, int pageSize, bool metadataOnly)
            #endregion
            {

            }
        }
    }

    public class GetSamples
    {
        public void Single()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region get_sample_single
                var command = new GetDocumentsCommand("orders/1-A", null, false);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                var order = (BlittableJsonReaderObject)command.Result.Results[0];
                #endregion
            }
        }

        public void Multiple()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region get_sample_multiple
                var command = new GetDocumentsCommand(new[] { "orders/1-A", "employees/3-A" }, null, false);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                var order = (BlittableJsonReaderObject)command.Result.Results[0];
                var employee = (BlittableJsonReaderObject)command.Result.Results[1];
                #endregion
            }
        }

        public void Includes()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region get_sample_includes
                // Fetch employees/5-A and his boss.
                var command = new GetDocumentsCommand("employees/5-A", includes: new[] { "ReportsTo" }, metadataOnly: false);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                var employee = (BlittableJsonReaderObject)command.Result.Results[0];
                if (employee.TryGet<string>("ReportsTo", out var bossId))
                {
                    var boss = command.Result.Includes[bossId];
                }
                #endregion
            }
        }

        public void Missing()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region get_sample_missing
                // Assuming that products/9999-A doesn't exist.
                var command = new GetDocumentsCommand(new[] { "products/1-A", "products/9999-A", "products/3-A" }, null, false);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                var products = command.Result.Results; // products/1-A, null, products/3-A
                #endregion
            }
        }

        public void Paged()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region get_sample_paged
                var command = new GetDocumentsCommand(start: 0, pageSize: 128);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                var firstDocs = command.Result.Results;
                #endregion
            }
        }

        public void StartsWith()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region get_sample_startswith
                // return up to 128 documents with key that starts with 'products'
                var command = new GetDocumentsCommand(
                    startWith: "products",
                    startAfter: null,
                    matches: null,
                    exclude: null,
                    start: 0,
                    pageSize: 128,
                    metadataOnly: false);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                var products = command.Result.Results;
                #endregion
            }
        }

        public void StartsWithMatches()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region get_sample_startswith_matches
                // return up to 128 documents with key that starts with 'products/' 
                // and rest of the key begins with "1" or "2" e.g. products/10, products/25
                var command = new GetDocumentsCommand(
                    startWith: "products",
                    startAfter: null,
                    matches: "1*|2*",
                    exclude: null,
                    start: 0,
                    pageSize: 128,
                    metadataOnly: false);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                var products = command.Result.Results;
                #endregion
            }
        }

        public void StartsWithMatchesEnd()
        {
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region get_sample_startswith_matches_end
                // return up to 128 documents with key that starts with 'products/' 
                // and rest of the key have length of 3, begins and ends with "1" 
                // and contains any character at 2nd position e.g. products/101, products/1B1
                var command = new GetDocumentsCommand(
                    startWith: "products",
                    startAfter: null,
                    matches: "1?1",
                    exclude: null,
                    start: 0,
                    pageSize: 128,
                    metadataOnly: false);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                var products = command.Result.Results;
                #endregion
            }
        }
    }
}
