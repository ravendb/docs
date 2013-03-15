namespace RavenCodeSamples.Faq
{
	using System;
	using System.Transactions;

	public class WorkingWithDtc : CodeSampleBase
	{
		private class User
		{
			public string Name { get; set; }
		}

		public void Sample()
		{
			using (var documentStore = this.NewDocumentStore())
			{
				#region working_with_dtc_1
				using (var tx = new TransactionScope())
				{
					using (var session = documentStore.OpenSession())
					{
						session.Store(new User { Name = "Ayende" });
						session.SaveChanges();
					}

					using (var session = documentStore.OpenSession())
					{
						session.Store(new User { Name = "Rahien" });
						session.SaveChanges();
					}

					tx.Complete();
				}

				#endregion

				#region working_with_dtc_2
				using (var tx = new TransactionScope())
				{
					using (var session = documentStore.OpenSession())
					{
						var user = session.Load<User>("users/1");
						user.Name = "Ayende"; // old name is "Oren"
						session.SaveChanges();
					}

					tx.Complete();
				}

				using (var session = documentStore.OpenSession())
				{
					var user = session.Load<User>("users/1");
					Console.WriteLine(user.Name);
				}

				#endregion

				#region working_with_dtc_3
				using (var tx = new TransactionScope())
				{
					using (var session = documentStore.OpenSession())
					{
						var user = session.Load<User>("users/1");
						user.Name = "Ayende"; // old name is "Oren"
						session.SaveChanges();
					}

					tx.Complete();
				}

				using (var session = documentStore.OpenSession())
				{
					session.Advanced.AllowNonAuthoritativeInformation = false;
					var user = session.Load<User>("users/1");
					Console.WriteLine(user.Name);
				}

				#endregion
			}
		}
	}
}