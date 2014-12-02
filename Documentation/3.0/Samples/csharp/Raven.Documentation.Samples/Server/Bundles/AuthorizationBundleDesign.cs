using System.Collections.Generic;
using System.Linq;

using Raven.Client;
using Raven.Client.Authorization;
using Raven.Client.Document;
using Raven.Client.Linq;

namespace Raven.Documentation.Samples.Server.Bundles
{
	public class AuthorizationBundleDesign
	{
		private class Debt
		{
			public decimal Amount { get; set; }

			public string Department { get; set; }
		}

		public void ContextUser()
		{
			string department = "SampleDepartment";

			using (var store = new DocumentStore())
			{
				#region authorization_bundle_design_1
				using (IDocumentSession session = store.OpenSession())
				{
					session.SecureFor("raven/authorization/users/8458", "/Operations/Debt/Finalize");

					var debtsQuery = from debt in session.Query<Debt>("Debts/ByDepartment")
									 where debt.Department == department
									 orderby debt.Amount
									 select debt;

					List<Debt> debts = debtsQuery.Take(25).ToList();

					// do something with the debts
				}
				#endregion
			}
		}

	}
}