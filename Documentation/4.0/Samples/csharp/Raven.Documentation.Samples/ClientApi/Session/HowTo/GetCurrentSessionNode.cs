using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Http;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class GetCurrentSessionNode
    {
        private interface IFoo
        {
            #region current_session_node_1
            Task<ServerNode> GetCurrentSessionNode();
            #endregion
        }

        public class Foo
        {
            #region current_session_node_2
            public class ServerNode
            {
                public string Url;
                public string Database;
                public string ClusterTag;
                public Role ServerRole;

                [Flags]
                public enum Role
                {
                    None = 0,
                    Promotable = 1,
                    Member = 2,
                    Rehab = 4
                }
            }
            #endregion
        }

        async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region current_session_node_3
                    ServerNode serverNode = await session.Advanced.GetCurrentSessionNode();
                    Console.WriteLine(serverNode.Url);
                    #endregion
                }
            }
        }
    }
}
