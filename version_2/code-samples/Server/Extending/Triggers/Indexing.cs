namespace RavenCodeSamples.Server.Extending.Triggers
{
	//using Lucene.Net.Documents;

	//using Raven.Database.Plugins;
	//using Raven.Imports.Newtonsoft.Json.Linq;

	//public class SnapshotShoppingCart : AbstractIndexUpdateTrigger
	//{
	//	public override void OnIndexEntryCreated(string indexName, string entryKey, Lucene.Net.Documents.Document document)
	//	{
	//		if (indexName != "Aggregates/ShoppingCart")
	//			return;
	//		var shoppingCart = JObject.Parse(document.GetField("Aggregate").StringValue());
	//		var shoppingCartId = document.GetField("Id").StringValue();

	//		var result = Database.Put("shoppingcarts/" + shoppingCartId + "/snapshots/", null, shoppingCart, new JObject(), null);
	//		document.Add(new Field("Snapshot", result.Key, Field.Store.YES, Field.Index.NOT_ANALYZED));
	//	}
	//}

	public class Indexing : CodeSampleBase
	{
	}
}