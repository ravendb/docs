using Raven.Abstractions.Util;

namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session.Querying
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Abstractions.FileSystem;
    using Client.FileSystem;

    public class Streaming
    {
        public interface IFoo
        {
            #region streaming_1
            Task<IAsyncEnumerator<FileHeader>> StreamQueryAsync(IAsyncFilesQuery<FileHeader> query);
            #endregion
        }

        public async Task Foo()
        {
            using (var store = new FilesStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    #region streaming_2
                    var allFilesMatchingCriteria = new List<FileHeader>();
                    var query = session.Query();

                    using (var reader = await session.Advanced.StreamQueryAsync(query))
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