using Raven.Client.Documents;

namespace Raven.Documentation.Samples.Migration.ClientApi
{
    public class Commands
    {
        public Commands()
        {
            using (var store = new DocumentStore())
            {
                string id = "";

                /*
                #region exists_1
                var exists = store.DatabaseCommands.Head(id) != null;
                #endregion
                */

                using (var session = store.OpenSession())
                {
                    #region exists_2
                    var exists = session.Advanced.Exists(id);
                    #endregion
                }
            }
            
        }
    }
}
