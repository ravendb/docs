using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class Exists
	{
		private interface IExists
		{
            #region exists_1
            bool Exists(string id)
		    #endregion
		    ;
		}

        public Exists()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
				    #region exists_2
				    bool exists = session.Advanced.Exists("employees/1-A");
				    if (exists)
				    {
				        //do something...
				    }
                    #endregion
                }
			}
		}
	}
}
