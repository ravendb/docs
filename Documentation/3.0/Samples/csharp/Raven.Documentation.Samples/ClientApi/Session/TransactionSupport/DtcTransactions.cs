namespace Raven.Documentation.Samples.ClientApi.Session.TransactionSupport
{
	using System.Transactions;
	using Client.Document;
	using Client.Document.DTC;
	using CodeSamples.Orders;

	public class DtcTransactions
	{
		public DtcTransactions()
		{
			var store = new DocumentStore();

			using (var session = store.OpenSession())
			{
				#region transaction_scope_usage
				using (var transaction = new TransactionScope())
				{
					Employee employee = session.Load<Employee>("employees/1");

					employee.FirstName = "James";

					session.SaveChanges(); // will create HTTP request

					session.Delete(employee);

					session.SaveChanges(); // will create HTTP request

					transaction.Complete(); // will commit the transaction
				}

				#endregion
			}

			#region default_transaction_recovery_storage
			store.TransactionRecoveryStorage = new VolatileOnlyTransactionRecoveryStorage();
			#endregion

			#region local_directory_transaction_recovery_storage
			store.TransactionRecoveryStorage = new LocalDirectoryTransactionRecoveryStorage(@"C:\tx_recovery_data");
			#endregion
		}
	}
}