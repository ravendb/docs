using System.Transactions;
using RavenDBSamples.BaseForSamples;

namespace RavenDBSamples.Transaction
{
	public class Transaction : SampleBase
	{
		public void SetTransaction()
		{
			using(var session = DocumentStore.OpenSession())
			using (var transaction = new TransactionScope())
			{
				var entity = session.Load<BlogPost>("blogs/1");

				entity.Title = "Some new title";

				session.SaveChanges();

				session.Delete(entity);
				session.SaveChanges();

				transaction.Complete();
			}
		}
	}
}