namespace RavenCodeSamples.Server.Extending.Triggers
{
	using System.Threading;

	using Raven.Abstractions.Data;
	using Raven.Database.Plugins;
	using Raven.Json.Linq;

	public class SampleTrigger : AbstractReadTrigger
	{
		#region read_1
		public override ReadVetoResult AllowRead(string key, RavenJObject metadata, ReadOperation operation, TransactionInformation transactionInformation)
		{
			if (metadata.Value<string>("Document-Owner") == Thread.CurrentPrincipal.Identity.Name)
				return ReadVetoResult.Allowed;

			if (operation == ReadOperation.Load)
				return ReadVetoResult.Deny("You don't have permission to read this document");
			else
				return ReadVetoResult.Ignore;
		}

		#endregion
	}

	#region read_2
	public class EmbedLinkDocument : AbstractReadTrigger
	{
		public override void OnRead(string key, RavenJObject document, RavenJObject metadata, ReadOperation operation, TransactionInformation transactionInformation)
		{
			var linkName = metadata.Value<string>("Raven-Link-Name");
			var link = metadata.Value<string>("Raven-Link");
			if (link == null)
				return;

			var linkedDocument = Database.Get(link, transactionInformation);
			document.Add(linkName, linkedDocument.ToJson());
		}
	}

	#endregion

	public class Read : CodeSampleBase
	{
	}
}