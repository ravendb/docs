namespace RavenCodeSamples.Server.Extending.Triggers
{
	using System.Threading;

	using Raven.Abstractions.Data;
	using Raven.Database.Plugins;
	using Raven.Json.Linq;

	#region put_1
	public class SecurityTrigger : AbstractPutTrigger
	{
		public override VetoResult AllowPut(string key, RavenJObject document, RavenJObject metadata, TransactionInformation transactionInformation)
		{
			var doc = Database.Get(key, transactionInformation);
			if (doc == null) // new document
				return VetoResult.Allowed;
			if (doc.Metadata["Document-Owner"] == null)// no security
				return VetoResult.Allowed;
			if (doc.Metadata["Document-Owner"].Value<string>() == Thread.CurrentPrincipal.Identity.Name)
				return VetoResult.Allowed;
			return VetoResult.Deny("You are not the document owner, cannot modify document");
		}

		public override void OnPut(string key, RavenJObject document, RavenJObject metadata, TransactionInformation transactionInformation)
		{
			if (metadata["Document-Owner"] == null) // user didn't explicitly set it
			{
				// modify the metadata to the current user
				metadata["Document-Owner"] = RavenJObject.FromObject(Thread.CurrentPrincipal.Identity.Name);
			}
		}
	}

	#endregion

	public class Put : CodeSampleBase
	{

	}
}