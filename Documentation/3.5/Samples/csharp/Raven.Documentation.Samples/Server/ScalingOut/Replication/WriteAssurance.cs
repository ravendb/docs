// -----------------------------------------------------------------------
//  <copyright file="WriteAssurance.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.Server.ScalingOut.Replication
{
	public class WriteAssurance
	{
		public async Task Sample()
		{
			using (var store = new DocumentStore())
			{
				var user = new RavenJObject
					{
						{"Name", "John"}
					};

				using (var session = store.OpenAsyncSession())
				{
					#region write_assurance_1
					await session.StoreAsync(user);
					await session.SaveChangesAsync();

					Etag userEtag = session
						.Advanced
						.GetEtagFor(user);

					await store
						.Replication
						.WaitAsync(etag: userEtag, timeout: TimeSpan.FromMinutes(1), replicas: 1);
					#endregion

					#region write_assurance_2
					await store
						.Replication
						.WaitAsync();
                    #endregion

                    #region write_assurance_3
                    session.Advanced.WaitForReplicationAfterSaveChanges(replicas: 2, timeout: TimeSpan.FromSeconds(30));
                    #endregion
                }

                #region write_assurance_4

                using (var session = store.OpenSession())
                {
                    session.Store(user);
                    session.Advanced.WaitForReplicationAfterSaveChanges(replicas: 2, timeout: TimeSpan.FromSeconds(30));
                    session.SaveChanges();
                }
                #endregion
            }
        }
	}
}