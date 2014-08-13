using System;

using Raven.Client.Document;

using Xunit;

namespace Raven.Documentation.CodeSamples.ClientApi.Session
{
	public class WhatIsSession
	{
		public WhatIsSession()
		{
			using (var store = new DocumentStore())
			{
				#region session_usage_1
				string companyId;
				using (var session = store.OpenSession())
				{
					var entity = new Company { Name = "Company" };
					session.Store(entity);
					session.SaveChanges();
					companyId = entity.Id;
				}

				using (var session = store.OpenSession())
				{
					var entity = session.Load<Company>(companyId);
					Console.WriteLine(entity.Name);
				}
				#endregion

				#region session_usage_2
				using (var session = store.OpenSession())
				{
					var entity = session.Load<Company>(companyId);
					entity.Name = "Another Company";
					session.SaveChanges(); // will send the change to the database
				}
				#endregion

				using (var session = store.OpenSession())
				{
					#region session_usage_3
					Assert.Same(session.Load<Company>(companyId), session.Load<Company>(companyId));
					#endregion
				}
			}
		}
	}
}