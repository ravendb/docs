using System;
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
	}
}
