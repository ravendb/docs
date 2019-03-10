using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Smuggler;

namespace Raven.Documentation.Samples.ClientApi.Smuggler
{
    public class WhatIsSmuggler
    {
        private interface IFoo
        {
            #region export_syntax
            Task<Operation> ExportAsync(
                DatabaseSmugglerExportOptions options,
                DatabaseSmuggler toDatabase,
                CancellationToken token = default(CancellationToken));

            Task<Operation> ExportAsync(
                DatabaseSmugglerExportOptions options,
                string toFile,
                CancellationToken token = default(CancellationToken));
            #endregion

            #region import_syntax
            Task<Operation> ImportAsync(
                DatabaseSmugglerImportOptions options,
                Stream stream,
                CancellationToken token = default(CancellationToken));

            Task<Operation> ImportAsync(
                DatabaseSmugglerImportOptions options,
                string fromFile,
                CancellationToken token = default(CancellationToken));
            #endregion

            Task ImportIncrementalAsync(
                DatabaseSmugglerImportOptions options,
                string fromDirectory,
                CancellationToken token = default(CancellationToken));

        }

        public async Task Sample()
        {
            var token = new CancellationToken();

            using (var store = new DocumentStore())
            {
                #region for_database
                var northwindSmuggler = store
                    .Smuggler
                    .ForDatabase("Northwind");
                #endregion

                #region export_example
                // export only Indexes and Documents to a given file
                var exportOperation = await store
                    .Smuggler
                    .ExportAsync(
                        new DatabaseSmugglerExportOptions
                        {
                            OperateOnTypes = DatabaseItemType.Indexes
                                             | DatabaseItemType.Documents
                        },
                        @"C:\ravendb-exports\Northwind.ravendbdump",
                        token);

                await exportOperation.WaitForCompletionAsync();
                #endregion

                #region import_example
                // import only Documents from a given file
                var importOperation = await store
                    .Smuggler
                    .ImportAsync(
                        new DatabaseSmugglerImportOptions
                        {
                            OperateOnTypes = DatabaseItemType.Documents
                        },
                        @"C:\ravendb-exports\Northwind.ravendbdump",
                        token);

                await importOperation.WaitForCompletionAsync();
                #endregion
            }
        }
    }
}
