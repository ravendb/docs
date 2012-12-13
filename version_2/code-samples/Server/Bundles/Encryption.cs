using Raven.Abstractions.Data;

namespace RavenCodeSamples.Server.Bundles
{
    using Raven.Client.Extensions;

    public class Encryption : CodeSampleBase
    {
        public void Sample()
        {
            using (var store = NewDocumentStore())
            {
                #region encryption_1
                store.DatabaseCommands.CreateDatabase(new DatabaseDocument
                    {
                        Id = "EncryptedDB",
                        // Other configuration options ommited for simplicity
                        Settings =
                            {
                                // ...
                                {"Raven/ActiveBundles", "Encryption"},
                                {"Raven/Encryption/Algorithm", "System.Security.Cryptography.DESCryptoServiceProvider, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"},
                                {"Raven/Encryption/Key", "<key_here>"}
                            }
                    });

                #endregion
            }
        }
    }
}