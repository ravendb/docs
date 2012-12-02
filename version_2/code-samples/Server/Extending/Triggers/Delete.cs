namespace RavenCodeSamples.Server.Extending.Triggers
{
	using Raven.Abstractions.Data;
	using Raven.Database.Plugins;

	#region delete_1
	public class CascadeDeleteTrigger : AbstractDeleteTrigger
	{
		public override void OnDelete(string key, TransactionInformation txInfo)
		{
			var document = Database.Get(key, txInfo);
			if (document == null)
				return;
			Database.Delete(document.Metadata.Value<string>("Cascade-Delete"), null, txInfo);
		}
	}

	#endregion

	public class Delete : CodeSampleBase
	{
	}
}