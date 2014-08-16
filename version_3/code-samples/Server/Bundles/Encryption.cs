using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.Server.Bundles
{
	public class Encryption
	{
		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region encryption_1
				store
					.DatabaseCommands
					.GlobalAdmin
					.CreateDatabase(
						new DatabaseDocument
							{
								Id = "EncryptedDB",
								// Other configuration options omitted for simplicity
								Settings =
									{
										// ...
										{ "Raven/ActiveBundles", "Encryption" },
										{
											"Raven/Encryption/Algorithm",
											"System.Security.Cryptography.DESCryptoServiceProvider, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
										}
									},
								SecuredSettings =
									{
										// ...
										{ "Raven/Encryption/Key", "<key_here>" }
									}
							});
				#endregion
			}
		}
	}
}