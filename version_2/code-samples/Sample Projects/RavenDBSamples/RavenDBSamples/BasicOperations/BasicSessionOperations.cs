using System;
using System.Linq;
using RavenDBSamples.BaseForSamples;

namespace RavenDBSamples.BasicOperations
{
	public class BasicSessionOperations : SampleBase
	{
		public string CompanyId { get; set; }

		public void StoreItem()
		{
			using (var session = DocumentStore.OpenSession())
			{
				var entity = new Company { Name = "Company" };
				session.Store(entity);
				session.SaveChanges();
				CompanyId = entity.Id;
			}
		}

		public void StoreMultipulItems()
		{
			using (var session = DocumentStore.OpenSession())
			{
				var entity1 = new Company { Name = "Company1" };
				var entity2 = new Company { Name = "Company1" };
				session.Store(entity1);
				session.Store(entity2);
				session.SaveChanges();
				CompanyId = entity1.Id;
			}
		}

		public void LoadItem()
		{
			using (var session = DocumentStore.OpenSession())
			{
				var entity = session.Load<Company>(CompanyId);
				Console.Out.WriteLine(entity.Name);
			}
		}

		public void UpdateItem()
		{
			using (var session = DocumentStore.OpenSession())
			{
				var entity = session.Load<Company>(CompanyId);
				entity.Name = "Another Company";
				session.SaveChanges();
			}
		}

		public void DeleteItemByEntity()
		{
			using (var session = DocumentStore.OpenSession())
			{
				var entity = session.Load<Company>(CompanyId);
				session.Delete(entity);
				session.SaveChanges();
			}
		}

		public void DeleteItemById()
		{
			using (var session = DocumentStore.OpenSession())
			{
				session.Advanced.DocumentStore.DatabaseCommands.Delete(CompanyId, null);
				// This opercation is immidiate and will not wait for "Save Changes"
			}
		}

		public void UseMetadata()
		{
			using (var session = DocumentStore.OpenSession())
			{
				var product = session.Load<Product>(1);
				var metadata = session.Advanced.GetMetadataFor(product);

				// Get the last modified time stamp, which is known to be of type DateTime
				var lastModified = metadata.Value<DateTime>("Last-Modified");
			}
		}

		public void QueryForTerms()
		{
			using (var session = DocumentStore.OpenSession())
			{
				var firstPage = session.Advanced.DocumentStore.DatabaseCommands.GetTerms("indexName", "MyProperty", null, 128);
				var secondPage = session.Advanced.DocumentStore.DatabaseCommands.GetTerms("indexName", "MyProperty", firstPage.Last(), 128);
			}
		}
	}
}
