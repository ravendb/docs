using Raven.Abstractions.Data;
using Raven.Abstractions.Util;

namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session.Querying
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Abstractions.FileSystem;
    using Client.FileSystem;

    public class StreamingFiles
    {
        public interface IFoo
        {
            #region streaming_files_1
            Task<IAsyncEnumerator<FileHeader>> StreamFileHeadersAsync(Etag fromEtag, int pageSize = int.MaxValue);
            #endregion
        }

        public async Task Foo()
        {
            using (var store = new FilesStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    Etag fromEtag = Etag.Empty;

                    #region streaming_files_2
                    var allFilesMatchingCriteria = new List<FileHeader>();

                    using (var reader = await session.Advanced.StreamFileHeadersAsync(fromEtag: fromEtag, pageSize: 10))
                    {
                        while (await reader.MoveNextAsync())
                        {
                            allFilesMatchingCriteria.Add(reader.Current);
                        }
                    }

                    #endregion
                }
            }
        }
    }
}