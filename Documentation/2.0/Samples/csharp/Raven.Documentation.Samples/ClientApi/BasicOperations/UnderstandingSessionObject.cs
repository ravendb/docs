namespace RavenCodeSamples.ClientApi.BasicOperations
{
	using System;

	public class UnderstandingSessionObject : CodeSampleBase
	{
		public void Understanding()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region session_usage_1
				string companyId;
				using (var session = documentStore.OpenSession())
				{
					var entity = new Company { Name = "Company" };
					session.Store(entity);
					session.SaveChanges();
					companyId = entity.Id;
				}

				using (var session = documentStore.OpenSession())
				{
					var entity = session.Load<Company>(companyId);
					Console.WriteLine(entity.Name);
				}
				#endregion

				#region session_usage_2
				using (var session = documentStore.OpenSession())
				{
					var entity = session.Load<Company>(companyId);
					entity.Name = "Another Company";
					session.SaveChanges(); // will send the change to the database
				}
				#endregion
			}
		}
	}
}