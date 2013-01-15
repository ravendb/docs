using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Raven.Abstractions.Data;
using Raven.Database;
using Raven.Database.Data;
using Raven.Database.Plugins;
using Raven.Imports.Newtonsoft.Json.Linq;
using Raven.Json.Linq;

namespace RavenCodeSamples.Server.Extending.Plugins
{
	namespace Foo
	{
		#region plugins_1
		public abstract class AbstractPutTrigger
		{
			public virtual VetoResult AllowPut(string key, RavenJObject document, RavenJObject metadata, TransactionInformation transactionInformation)
			{
				return VetoResult.Allowed;
			}

			public virtual void OnPut(string key, RavenJObject document, RavenJObject metadata, TransactionInformation transactionInformation)
			{
			}

			public virtual void AfterPut(string key, RavenJObject document, RavenJObject metadata, Guid etag, TransactionInformation transactionInformation)
			{
			}

			public virtual void AfterCommit(string key, RavenJObject document, RavenJObject metadata, Guid etag)
			{
			}

			public virtual void Initialize()
			{
			}

			public virtual void SecondStageInit()
			{
			}

			public DocumentDatabase Database { get; set; }
		}

		#endregion

		#region plugins_2
		public class VetoResult
		{
			public static VetoResult Allowed
			{
				get { return new VetoResult(true, "allowed"); }
			}

			public static VetoResult Deny(string reason)
			{
				return new VetoResult(false, reason);
			}

			private VetoResult(bool allowed, string reason)
			{
				IsAllowed = allowed;
				Reason = reason;
			}

			public bool IsAllowed { get; private set; }

			public string Reason { get; private set; }
		}

		#endregion

		#region plugins_3
		public abstract class AbstractAttachmentPutTrigger
		{
			public virtual VetoResult AllowPut(string key, Stream data, RavenJObject metadata)
			{
				return VetoResult.Allowed;
			}

			public virtual void OnPut(string key, Stream data, RavenJObject metadata)
			{
			}

			public virtual void AfterPut(string key, Stream data, RavenJObject metadata, Guid etag)
			{
			}

			public virtual void AfterCommit(string key, Stream data, RavenJObject metadata, Guid etag)
			{
			}

			public virtual void SecondStageInit()
			{
			}


			public virtual void Initialize()
			{
			}

			public DocumentDatabase Database { get; set; }
		}

		#endregion

		#region plugins_4
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

		#region plugins_5
		public abstract class AbstractDeleteTrigger
		{
			public virtual VetoResult AllowDelete(string key, TransactionInformation transactionInformation)
			{
				return VetoResult.Allowed;
			}

			public virtual void OnDelete(string key, TransactionInformation transactionInformation)
			{
			}

			public virtual void AfterDelete(string key, TransactionInformation transactionInformation)
			{
			}

			public virtual void AfterCommit(string key)
			{
			}

			public virtual void SecondStageInit()
			{
			}


			public virtual void Initialize()
			{
			}

			public DocumentDatabase Database { get; set; }
		}

		#endregion

		#region plugins_6
		public abstract class AbstractAttachmentDeleteTrigger
		{
			public virtual VetoResult AllowDelete(string key)
			{
				return VetoResult.Allowed;
			}

			public virtual void OnDelete(string key)
			{
			}

			public virtual void AfterDelete(string key)
			{
			}

			public virtual void AfterCommit(string key)
			{

			}

			public virtual void SecondStageInit()
			{
			}

			public virtual void Initialize()
			{
			}

			public DocumentDatabase Database { get; set; }
		}

		#endregion

		#region plugins_7
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

		#region plugins_8
		public abstract class AbstractReadTrigger
		{
			public virtual ReadVetoResult AllowRead(string key, RavenJObject metadata, ReadOperation operation, TransactionInformation transactionInformation)
			{
				return ReadVetoResult.Allowed;
			}

			public virtual void OnRead(string key, RavenJObject document, RavenJObject metadata, ReadOperation operation, TransactionInformation transactionInformation)
			{
			}

			public virtual void Initialize()
			{
			}

			public virtual void SecondStageInit()
			{
			}

			public DocumentDatabase Database { get; set; }
		}

		#endregion

		#region plugins_9
		public abstract class AbstractAttachmentReadTrigger
		{
			public virtual ReadVetoResult AllowRead(string key, Stream data, RavenJObject metadata, ReadOperation operation)
			{
				return ReadVetoResult.Allowed;
			}

			public virtual void OnRead(string key, Attachment attachment)
			{
			}

			public virtual void OnRead(AttachmentInformation information)
			{
			}

			public virtual void SecondStageInit()
			{
			}

			public virtual void Initialize()
			{
			}

			public DocumentDatabase Database { get; set; }

		}

		#endregion

		#region plugins_1_0
		public class ReadVetoResult
		{
			public static ReadVetoResult Allowed
			{
				get { return new ReadVetoResult(ReadAllow.Allow, "allowed"); }
			}

			public static ReadVetoResult Ignore
			{
				get { return new ReadVetoResult(ReadAllow.Ignore, "ignore"); }
			}

			public static ReadVetoResult Deny(string reason)
			{
				return new ReadVetoResult(ReadAllow.Deny, reason);
			}

			private ReadVetoResult(ReadAllow allowed, string reason)
			{
				Veto = allowed;
				Reason = reason;
			}

			public ReadAllow Veto { get; private set; }

			public enum ReadAllow
			{
				Allow,
				Deny,
				Ignore
			}

			public string Reason { get; private set; }
		}

		#endregion

		#region plugins_1_1
		public class SecurityReadTrigger : AbstractReadTrigger
		{
			public override ReadVetoResult AllowRead(string key, RavenJObject metadata, ReadOperation operation, TransactionInformation transactionInformation)
			{
				if (metadata.Value<string>("Document-Owner") == Thread.CurrentPrincipal.Identity.Name)
					return ReadVetoResult.Allowed;

				if (operation == ReadOperation.Load)
					return ReadVetoResult.Deny("You don't have permission to read this document");

				return ReadVetoResult.Ignore;
			}
		}

		#endregion

		#region plugins_1_2
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

		#region plugins_1_3
		public abstract class AbstractIndexQueryTrigger
		{
			public virtual void Initialize()
			{
			}

			public virtual void SecondStageInit()
			{
			}


			public DocumentDatabase Database { get; set; }

			public abstract Query ProcessQuery(string indexName, Query query, IndexQuery originalQuery);
		}

		#endregion

		#region plugins_2_0
		public abstract class AbstractIndexUpdateTrigger
		{
			public virtual void Initialize()
			{
			}

			public virtual void SecondStageInit()
			{
			}

			public abstract AbstractIndexUpdateTriggerBatcher CreateBatcher(string indexName);

			public DocumentDatabase Database { get; set; }
		}

		#endregion

		#region plugins_2_1
		public abstract class AbstractIndexUpdateTriggerBatcher
		{
			public virtual void OnIndexEntryDeleted(string entryKey)
			{
			}

			public virtual void OnIndexEntryCreated(string entryKey, Document document)
			{
			}

			public virtual void AnErrorOccured(Exception exception)
			{
			}
		}

		#endregion

		#region plugins_2_2
		public class SnapshotShoppingCartUpdateTrigger : AbstractIndexUpdateTrigger
		{
			public override AbstractIndexUpdateTriggerBatcher CreateBatcher(string indexName)
			{
				return new SnapshotShoppingCartBatcher(indexName, Database);
			}
		}

		public class SnapshotShoppingCartBatcher : AbstractIndexUpdateTriggerBatcher
		{
			private readonly string indexName;

			private readonly DocumentDatabase database;

			public SnapshotShoppingCartBatcher(string indexName, DocumentDatabase database)
			{
				this.indexName = indexName;
				this.database = database;
			}

			public override void OnIndexEntryCreated(string entryKey, Document document)
			{
				if (indexName != "Aggregates/ShoppingCart")
					return;

				var shoppingCart = RavenJObject.Parse(document.GetField("Aggregate").StringValue);
				var shoppingCartId = document.GetField("Id").StringValue;

				var result = database.Put("shoppingcarts/" + shoppingCartId + "/snapshots/", null, shoppingCart, new RavenJObject(), null);
				document.Add(new Field("Snapshot", result.Key, Field.Store.YES, Field.Index.NOT_ANALYZED));
			}
		}

		#endregion
	}

	public class Index : CodeSampleBase
	{

	}
}