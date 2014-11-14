using System.Collections.Generic;

using Raven.Bundles.Authorization.Model;
using Raven.Client.Authorization;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.Server.Bundles
{
	public class Authorization
	{
		internal class Patient
		{
			public void AuthorizeHospitalization()
			{

			}
		}

		public void Sample()
		{
			using (var documentStore = new DocumentStore())
			{

				using (var session = documentStore.OpenSession())
				{
					#region authorization1
					// Allow nurses to schedule appointment for patients
					session.Store(
						new AuthorizationRole
							{
								Id = "Authorization/Roles/Nurses",
								Permissions =
									{
										new OperationPermission
											{
												Allow = true, 
												Operation = "Appointment/Schedule", 
												Tags = new List<string> { "Patient" }
											}
									}
							});

					// Allow doctors to authorize hospitalizations
					session.Store(
						new AuthorizationRole
							{
								Id = "Authorization/Roles/Doctors",
								Permissions =
									{
										new OperationPermission
											{
												Allow = true, 
												Operation = "Hospitalization/Authorize", 
												Tags = new List<string> { "Patient" }
											}
									}
							});

					#endregion

					#region authorization2
					// Associate Patient with clinic
					session.SetAuthorizationFor(session.Load<Patient>("Patients/MaryMallon"), new DocumentAuthorization { Tags = { "Clinics/Kirya", "Patient" } });

					// Associate Doctor with clinic
					session.Store(
						new AuthorizationUser
							{
								Id = "Authorization/Users/DrHowser",
								Name = "Doogie Howser",
								Roles = { "Authorization/Roles/Doctors" },
								Permissions =
									{
										new OperationPermission
											{
												Allow = true, 
												Operation = "Patient/View", 
												Tags = new List<string> { "Clinics/Kirya" }
											},
									}
							});

					#endregion

					#region authorization3
					session.SecureFor("Authorization/Users/DrHowser", "Hospitalization/Authorize");
					Patient mary = session.Load<Patient>("Patients/MaryMallon");
					mary.AuthorizeHospitalization();
					session.SaveChanges();

					#endregion
				}
			}
		}
	}
}