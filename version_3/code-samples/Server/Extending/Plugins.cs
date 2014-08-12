using System;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Abstractions.Logging;
using Raven.Client.Document;
using Raven.Database;
using Raven.Database.Config;
using Raven.Database.Data;
using Raven.Database.Extensions;
using Raven.Database.Plugins;
using Raven.Database.Server;
using Raven.Json.Linq;
using Raven.Server;

using Attachment = Raven.Abstractions.Data.Attachment;

namespace Raven.Documentation.CodeSamples.Server.Extending
{
	namespace Foo
	{
		#region plugins_1
		public abstract class AbstractPutTrigger
		{
			public virtual VetoResult AllowPut(
				string key,
				RavenJObject document,
				RavenJObject metadata,
				TransactionInformation transactionInformation)
			{
				return VetoResult.Allowed;
			}

			public virtual void OnPut(
				string key,
				RavenJObject document,
				RavenJObject metadata,
				TransactionInformation transactionInformation)
			{
			}

			public virtual void AfterPut(
				string key,
				RavenJObject document,
				RavenJObject metadata,
				Etag etag,
				TransactionInformation transactionInformation)
			{
			}

			public virtual void AfterCommit(
				string key,
				RavenJObject document,
				RavenJObject metadata,
				Etag etag)
			{
			}

			public virtual void Initialize() { }

			public virtual void SecondStageInit() { }

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
			public virtual VetoResult AllowPut(
				string key,
				Stream data,
				RavenJObject metadata)
			{
				return VetoResult.Allowed;
			}

			public virtual void OnPut(
				string key,
				Stream data,
				RavenJObject metadata) { }

			public virtual void AfterPut(
				string key,
				Stream data,
				RavenJObject metadata,
				Etag etag) { }

			public virtual void AfterCommit(
				string key,
				Stream data,
				RavenJObject metadata,
				Etag etag) { }

			public virtual void SecondStageInit() { }

			public virtual void Initialize() { }

			public DocumentDatabase Database { get; set; }
		}
		#endregion

		#region plugins_4
		public class SecurityTrigger : AbstractPutTrigger
		{
			public override VetoResult AllowPut(string key, RavenJObject document, RavenJObject metadata, TransactionInformation transactionInformation)
			{
				var doc = Database.Documents.Get(key, transactionInformation);
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
			public virtual VetoResult AllowDelete(
				string key,
				TransactionInformation transactionInformation)
			{
				return VetoResult.Allowed;
			}

			public virtual void OnDelete(
				string key,
				TransactionInformation transactionInformation) { }

			public virtual void AfterDelete(
				string key,
				TransactionInformation transactionInformation) { }

			public virtual void AfterCommit(string key) { }

			public virtual void SecondStageInit() { }

			public virtual void Initialize() { }

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

			public virtual void OnDelete(string key) { }

			public virtual void AfterDelete(string key) { }

			public virtual void AfterCommit(string key) { }

			public virtual void SecondStageInit() { }

			public virtual void Initialize() { }

			public DocumentDatabase Database { get; set; }
		}
		#endregion

		#region plugins_7
		public class CascadeDeleteTrigger : AbstractDeleteTrigger
		{
			public override void OnDelete(string key, TransactionInformation txInfo)
			{
				var document = Database.Documents.Get(key, txInfo);
				if (document == null)
					return;

				Database.Documents.Delete(document.Metadata.Value<string>("Cascade-Delete"), null, txInfo);
			}
		}
		#endregion

		#region plugins_8
		public abstract class AbstractReadTrigger
		{
			public virtual ReadVetoResult AllowRead(
				string key,
				RavenJObject metadata,
				ReadOperation operation,
				TransactionInformation transactionInformation)
			{
				return ReadVetoResult.Allowed;
			}

			public virtual void OnRead(
				string key,
				RavenJObject document,
				RavenJObject metadata,
				ReadOperation operation,
				TransactionInformation transactionInformation)
			{
			}

			public virtual void Initialize() { }

			public virtual void SecondStageInit() { }

			public DocumentDatabase Database { get; set; }
		}
		#endregion

		#region plugins_9
		public abstract class AbstractAttachmentReadTrigger
		{
			public virtual ReadVetoResult AllowRead(
				string key,
				Stream data,
				RavenJObject metadata,
				ReadOperation operation)
			{
				return ReadVetoResult.Allowed;
			}

			public virtual void OnRead(
				string key,
				Attachment attachment)
			{
			}

			public virtual void OnRead(AttachmentInformation information)
			{
			}

			public virtual void SecondStageInit() { }

			public virtual void Initialize() { }

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

				var linkedDocument = Database.Documents.Get(link, transactionInformation);
				document.Add(linkName, linkedDocument.ToJson());
			}
		}

		#endregion

		#region plugins_1_3
		public abstract class AbstractIndexQueryTrigger
		{
			public virtual void Initialize() { }

			public virtual void SecondStageInit() { }

			public DocumentDatabase Database { get; set; }

			public abstract Query ProcessQuery(string indexName, Query query, IndexQuery originalQuery);
		}
		#endregion

		#region plugins_1_4
		public class CustomQueryTrigger : AbstractIndexQueryTrigger
		{
			private const string SpecificIndexName = "Specific/Index";

			public override Query ProcessQuery(string indexName, Query query, IndexQuery originalQuery)
			{
				if (indexName != SpecificIndexName)
					return query;

				var customQuery = new PrefixQuery(new Term("CustomField", "CustomPrefix"));

				return new BooleanQuery
					{
						{ query, Occur.MUST },
						{ customQuery, Occur.MUST}
					};
			}
		}

		#endregion

		#region plugins_2_0
		public abstract class AbstractIndexUpdateTrigger
		{
			public virtual void Initialize() { }

			public virtual void SecondStageInit() { }

			public abstract AbstractIndexUpdateTriggerBatcher CreateBatcher(int indexId);

			public DocumentDatabase Database { get; set; }
		}
		#endregion

		#region plugins_2_1
		public abstract class AbstractIndexUpdateTriggerBatcher : IDisposable
		{
			public virtual void OnIndexEntryDeleted(string entryKey, Document document = null) { }

			public virtual bool RequiresDocumentOnIndexEntryDeleted { get { return false; } }

			public virtual void OnIndexEntryCreated(string entryKey, Document document) { }

			public virtual void Dispose() { }

			public virtual void AnErrorOccured(Exception exception) { }
		}
		#endregion

		#region plugins_2_2
		public class SnapshotShoppingCartUpdateTrigger : AbstractIndexUpdateTrigger
		{
			public override AbstractIndexUpdateTriggerBatcher CreateBatcher(int indexId)
			{
				return new SnapshotShoppingCartBatcher(indexId, Database);
			}
		}

		public class SnapshotShoppingCartBatcher : AbstractIndexUpdateTriggerBatcher
		{
			private readonly string indexName;

			private readonly DocumentDatabase database;

			public SnapshotShoppingCartBatcher(int indexId, DocumentDatabase database)
			{
				indexName = database.IndexDefinitionStorage.GetIndexDefinition(indexId).Name;
				this.database = database;
			}

			public override void OnIndexEntryCreated(string entryKey, Document document)
			{
				if (indexName != "Aggregates/ShoppingCart")
					return;

				var shoppingCart = RavenJObject.Parse(document.GetField("Aggregate").StringValue);
				var shoppingCartId = document.GetField("Id").StringValue;

				var result = database.Documents.Put("shoppingcarts/" + shoppingCartId + "/snapshots/", null, shoppingCart, new RavenJObject(), null);
				document.Add(new Field("Snapshot", result.Key, Field.Store.YES, Field.Index.NOT_ANALYZED));
			}
		}
		#endregion

		#region plugins_3_0
		public abstract class AbstractDocumentCodec
		{
			public DocumentDatabase Database { get; set; }

			public virtual void Initialize(DocumentDatabase database) { }

			public virtual void Initialize() { }

			public virtual void SecondStageInit() { }

			public abstract Stream Encode(string key, RavenJObject data, RavenJObject metadata, Stream dataStream);

			public abstract Stream Decode(string key, RavenJObject metadata, Stream dataStream);
		}
		#endregion

		#region plugins_3_1
		public abstract class AbstractIndexCodec
		{
			public virtual void Initialize(DocumentDatabase database) { }

			public virtual void SecondStageInit() { }

			public abstract Stream Encode(string key, Stream dataStream);

			public abstract Stream Decode(string key, Stream dataStream);
		}
		#endregion

		#region plugins_3_2
		public class SimpleCompressionCodec : AbstractDocumentCodec
		{
			private readonly SimpleCompressor compressor = new SimpleCompressor();

			public override Stream Encode(string key, RavenJObject data, RavenJObject metadata, Stream dataStream)
			{
				return compressor.Compress(key, data, metadata, dataStream);
			}

			public override Stream Decode(string key, RavenJObject metadata, Stream dataStream)
			{
				return compressor.Decompress(key, metadata, dataStream);
			}
		}

		#endregion

		#region plugins_3_3
		public class SimpleEncryptionCodec : AbstractDocumentCodec
		{
			private readonly SimpleEncryptor encryptor = new SimpleEncryptor();

			public override Stream Encode(string key, RavenJObject data, RavenJObject metadata, Stream dataStream)
			{
				return encryptor.Encrypt(key, data, metadata, dataStream);
			}

			public override Stream Decode(string key, RavenJObject metadata, Stream dataStream)
			{
				return encryptor.Decrypt(key, metadata, dataStream);
			}
		}

		#endregion

		public class SimpleCompressor
		{
			public Stream Compress(string key, RavenJObject data, RavenJObject metadata, Stream dataStream)
			{
				return null;
			}

			public Stream Decompress(string key, RavenJObject metadata, Stream dataStream)
			{
				return null;
			}
		}

		public class SimpleEncryptor
		{
			public Stream Encrypt(string key, RavenJObject data, RavenJObject metadata, Stream dataStream)
			{
				return null;
			}

			public Stream Decrypt(string key, RavenJObject metadata, Stream dataStream)
			{
				return null;
			}
		}

		#region plugins_4_0
		public interface IStartupTask
		{
			void Execute(DocumentDatabase database);
		}

		public interface IServerStartupTask
		{
			void Execute(RavenDbServer server);
		}
		#endregion

		#region plugins_4_1
		public abstract class AbstractBackgroundTask : IStartupTask
		{
			private static readonly ILog log = LogManager.GetCurrentClassLogger();

			public DocumentDatabase Database { get; set; }

			public void Execute(DocumentDatabase database)
			{
				Database = database;
				Initialize();
				Task.Factory.StartNew(BackgroundTask, TaskCreationOptions.LongRunning);
			}

			protected virtual void Initialize()
			{
			}

			int workCounter;
			public void BackgroundTask()
			{
				var name = GetType().Name;
				var context = Database.WorkContext;
				while (context.DoWork)
				{
					var foundWork = false;
					try
					{
						foundWork = HandleWork();
					}
					catch (Exception e)
					{
						log.ErrorException("Failed to execute background task", e);
					}
					if (foundWork == false)
					{
						context.WaitForWork(TimeoutForNextWork(), ref workCounter, name);
					}
					else
					{
						context.UpdateFoundWork();
					}
				}
			}

			protected virtual TimeSpan TimeoutForNextWork()
			{
				return TimeSpan.FromHours(1);
			}

			protected abstract bool HandleWork();
		}
		#endregion

		#region plugins_4_2
		public class SendEmailWhenServerIsStartingTask : IServerStartupTask
		{
			public void Execute(RavenDbServer server)
			{
				var message = new MailMessage("ravendb@myhost.com", "admin@myhost.com")
				{
					Subject = "RavenDB server started.",
					Body = "Start at: " + DateTime.Now.ToShortDateString()
				};

				using (var smtpClient = new SmtpClient("mail.myhost.com"))
				{
					smtpClient.Send(message);
				}
			}
		}
		#endregion

		#region plugins_4_3
		public class CleanupWhenDatabaseIsStarting : IStartupTask
		{
			private const string SpecificDatabaseName = "Northwind";

			public void Execute(DocumentDatabase database)
			{
				if (database.Name != SpecificDatabaseName)
					return;

				using (var cts = new CancellationTokenSource())
				{
					bool stale;
					var queryResults = database.Queries.QueryDocumentIds(
						"Notifications/Temp",
						new IndexQuery(),
						CancellationTokenSource.CreateLinkedTokenSource(database.WorkContext.CancellationToken, cts.Token),
						out stale);

					foreach (var documentId in queryResults)
					{
						database.Documents.Delete(documentId, null, null);
					}
				}
			}
		}

		#endregion

		#region plugins_4_4
		public class RemoveAllTemporaryNotificationsTask : AbstractBackgroundTask
		{
			protected override bool HandleWork()
			{
				var queryResults = Database.Queries.Query("Notifications/Temp", new IndexQuery(), Database.WorkContext.CancellationToken);
				foreach (var document in queryResults.Results)
				{
					var id = ((RavenJObject)document["@metadata"]).Value<string>("@id");
					Database.Documents.Delete(id, null, null);
				}

				return true;
			}

			protected override TimeSpan TimeoutForNextWork()
			{
				return TimeSpan.FromHours(6);
			}
		}
		#endregion

		#region plugins_5_0
		public interface IAlterConfiguration
		{
			void AlterConfiguration(InMemoryRavenConfiguration configuration);
		}
		#endregion

		#region plugins_5_1
		public class CommonConfiguration : IAlterConfiguration
		{
			public void AlterConfiguration(InMemoryRavenConfiguration configuration)
			{
				configuration.HttpCompression = false;
			}
		}
		#endregion

		#region plugins_6_0
		public abstract class AbstractDynamicCompilationExtension
		{
			public abstract string[] GetNamespacesToImport();

			public abstract string[] GetAssembliesToReference();
		}
		#endregion

		#region plugins_6_1
		public static class Palindrome
		{
			public static bool IsPalindrome(string word)
			{
				if (string.IsNullOrEmpty(word))
					return true;

				var min = 0;
				var max = word.Length - 1;
				while (true)
				{
					if (min > max)
						return true;

					var a = word[min];
					var b = word[max];
					if (char.ToLower(a) != char.ToLower(b))
						return false;

					min++;
					max--;
				}
			}
		}
		#endregion

		#region plugins_6_2
		public class PalindromeDynamicCompilationExtension : AbstractDynamicCompilationExtension
		{
			public override string[] GetNamespacesToImport()
			{
				return new[]
					{
						typeof (Palindrome).Namespace
					};
			}

			public override string[] GetAssembliesToReference()
			{
				return new[]
					{
						typeof (Palindrome).Assembly.Location
					};
			}
		}
		#endregion

		#region plugins_7_0
		public abstract class AbstractAnalyzerGenerator
		{
			public abstract Analyzer GenerateAnalyzerForIndexing(string indexName, Document document, Analyzer previousAnalyzer);

			public abstract Analyzer GenerateAnalyzerForQuerying(string indexName, string query, Analyzer previousAnalyzer);
		}
		#endregion

		#region plugins_7_1
		public class CustomAnalyzerGenerator : AbstractAnalyzerGenerator
		{
			private const string SpecificIndexName = "Specific/Index";

			public override Analyzer GenerateAnalyzerForIndexing(string indexName, Document document, Analyzer previousAnalyzer)
			{
				if (indexName == SpecificIndexName)
				{
					return new WhitespaceAnalyzer();
				}

				return previousAnalyzer;
			}

			public override Analyzer GenerateAnalyzerForQuerying(string indexName, string query, Analyzer previousAnalyzer)
			{
				if (indexName == SpecificIndexName)
				{
					return new WhitespaceAnalyzer();
				}

				return previousAnalyzer;
			}
		}

		#endregion
	}

	public class Index
	{
		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region plugins_6_3
				store.DatabaseCommands.PutIndex("Dictionary/Palindromes", new IndexDefinition
				{
					Map = @"from word in docs.Words 
								select new 
								{ 
											Word = word.Value, 
											IsPalindrome = Palindrome.IsPalindrome(word.Value) 
								}"
				});
				#endregion
			}
		}
	}
}