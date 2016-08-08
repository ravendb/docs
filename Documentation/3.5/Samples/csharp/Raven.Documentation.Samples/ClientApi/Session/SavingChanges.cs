using System;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session
{
	public class SavingChanges
	{
		private interface IInterface
		{
			#region saving_changes_1
			void SaveChanges();
			#endregion
		}

		public SavingChanges()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region saving_changes_2
					// storing new entity
					session.Store(new Employee
						              {
							              FirstName = "John", 
										  LastName = "Doe"
						              });

					session.SaveChanges();
					#endregion
				}
			}

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    // storing new entity
                    #region saving_changes_3
                    session.Advanced.WaitForIndexesAfterSaveChanges(timeout: TimeSpan.FromSeconds(30));
                    session.Store(new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    });

                    session.SaveChanges();
                    #endregion
                }
            }
        }


	}
}