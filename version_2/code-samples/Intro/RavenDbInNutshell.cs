namespace RavenCodeSamples.Intro
{
	using System.Linq;

	using Raven.Client.Linq;

	public class RavenDbInNutshell : CodeSampleBase
	{
		public void NutshellSample()
		{
			using (var documentStore = NewDocumentStore())
			{
				#region nutshell1
				// Create a simple object of existing class Company
				var myCompany = new Company
									{
										Name = "Hibernating Rhinos",
										Employees =
				                			{
				                				new Employee
				                					{
				                						Name = "Ayende Rahien"
				                					}
				                			},
										Country = "Israel"
									};

				// Store the company in our RavenDB server
				using (var session = documentStore.OpenSession())
				{
					session.Store(myCompany);
					session.SaveChanges();
				}

				// Create a new session, retrieve an entity, and change it a bit
				using (var session = documentStore.OpenSession())
				{
					Company entity = session.Query<Company>()
						.Where(x => x.Country == "Israel")
						.FirstOrDefault();

					// We can also load by ID: session.Load<Company>(companyId);

					entity.Name = "Another Company";
					session.SaveChanges(); // will send the change to the database
				}
				#endregion
			}
		}
	}
}
