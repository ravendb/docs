using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
    public class Url
    {
        private interface IFoo
        {
            #region url_1

            string Url { get; }
            #endregion
        }

        public Url()
        {
            using (var store = new DocumentStore { Url = "http://localhost:8080" })
            {
                store.Initialize();
                #region url_2
                // http://localhost:8080/
                string url = store.DatabaseCommands.ForSystemDatabase().Url;
                #endregion
            }
        }
    }
}