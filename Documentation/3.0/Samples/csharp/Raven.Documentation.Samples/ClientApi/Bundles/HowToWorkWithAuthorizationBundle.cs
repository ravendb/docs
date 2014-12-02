#region using
using Raven.Client.Authorization;
#endregion

namespace Raven.Documentation.Samples.ClientApi.Bundles
{

	using Client.Document;
	using Server.Bundles;

	public class HowToWorkWithAuthorizationBundle
	{
		public void Sample()
		{
			using (var documentStore = new DocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region secure_for
					session.SecureFor("Authorization/Users/DrHowser", "Hospitalization/Authorize");
					Authorization.Patient mary = session.Load<Authorization.Patient>("Patients/MaryMallon");
					mary.AuthorizeHospitalization();
					session.SaveChanges();

					#endregion
				}
			}
		}
	}
}