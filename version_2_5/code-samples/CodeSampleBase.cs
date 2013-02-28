using System.IO;
using System.Reflection;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database.Config;
using Raven.Database.Extensions;
using Raven.Storage.Managed;

namespace RavenCodeSamples
{
	public abstract class CodeSampleBase
	{
		private string path;

		public EmbeddableDocumentStore NewDocumentStore()
		{
			return NewDocumentStore("munin", true, null);
		}

		public EmbeddableDocumentStore NewDocumentStore(string storageType, bool inMemory)
		{
			return NewDocumentStore(storageType, inMemory, null);
		}
		public EmbeddableDocumentStore NewDocumentStore(string storageType, bool inMemory, int? allocatedMemory)
		{
			path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(CodeSampleBase)).CodeBase);
			path = Path.Combine(path, "TestDb").Substring(6);


			var documentStore = new EmbeddableDocumentStore
			{
				Configuration =
				{
					DataDirectory = path,
					RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true,
					DefaultStorageTypeName = storageType,
					RunInMemory = inMemory,
				}
			};

			ModifyConfiguration(documentStore.Configuration);

			if (documentStore.Configuration.RunInMemory == false)
				IOExtensions.DeleteDirectory(path);
			documentStore.Initialize();

			new RavenDocumentsByEntityName().Execute(documentStore);

			if (allocatedMemory != null && inMemory)
			{
				var transactionalStorage = ((TransactionalStorage)documentStore.DocumentDatabase.TransactionalStorage);
				transactionalStorage.EnsureCapacity(allocatedMemory.Value);
			}

			return documentStore;
		}

		protected virtual void ModifyConfiguration(RavenConfiguration configuration)
		{
		}
	}
}
