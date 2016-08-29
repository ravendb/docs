namespace Raven.Documentation.Samples.FileSystem.ClientApi.Session.Querying
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Abstractions.FileSystem;
    using Client.FileSystem;

    public class Statistics
    {
        public async Task Foo()
        {

            using (var store = new FilesStore())
            {
                using (var session = store.OpenAsyncSession())
                {
                    #region statistics_1

                    FilesQueryStatistics stats;

                    List<FileHeader> results = await session.Query()
                                                .OrderBy(x => x.Name).Statistics(out stats)
                                                .ToListAsync();
                    #endregion
                }
            }
        }
    }
}