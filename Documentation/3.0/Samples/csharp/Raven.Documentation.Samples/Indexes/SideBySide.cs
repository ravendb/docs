using System;
using System.Threading;
using System.Threading.Tasks;

using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Client.Connection.Async;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.Samples.Server.Bundles;
using Raven.Documentation.Samples.Server.Bundles.Foo1;

namespace Raven.Documentation.Samples.Indexes
{
	public class SideBySide
	{
		private interface IFoo
		{
			#region side_by_side_1
			void SideBySideExecute(
				IDatabaseCommands databaseCommands,
				DocumentConvention documentConvention,
				Etag minimumEtagBeforeReplace = null,
				DateTime? replaceTimeUtc = null);

			Task SideBySideExecuteAsync(
				IAsyncDatabaseCommands asyncDatabaseCommands,
				DocumentConvention documentConvention,
				Etag minimumEtagBeforeReplace = null,
				DateTime? replaceTimeUtc = null,
				CancellationToken token = default(CancellationToken));

			void SideBySideExecute(
				IDocumentStore store,
				Etag minimumEtagBeforeReplace = null,
				DateTime? replaceTimeUtc = null);

			Task SideBySideExecuteAsync(
				IDocumentStore store,
				Etag minimumEtagBeforeReplace = null,
				DateTime? replaceTimeUtc = null);
			#endregion

			#region side_by_side_2
			void SideBySideExecuteIndex(
				AbstractIndexCreationTask indexCreationTask,
				Etag minimumEtagBeforeReplace = null,
				DateTime? replaceTimeUtc = null);

			Task SideBySideExecuteIndexAsync(
				AbstractIndexCreationTask indexCreationTask,
				Etag minimumEtagBeforeReplace = null,
				DateTime? replaceTimeUtc = null);
			#endregion
		}

		public SideBySide()
		{
			#region side_by_side_3
			using (var store = new DocumentStore
				                   {
					                   Url = "http://localhost:8080/",
									   DefaultDatabase = "Northwind"
				                   })
			{
				store.Initialize();

				// This method will create 'ReplacementOf/Orders/ByCompany' index, which will replace 'Orders/ByCompany' when
				// - new index will become non-stale
				// - new index will reach at least '01000000-0000-000E-0000-000000000293' etag
				// - in 6 hours from the deployment date
				store.SideBySideExecuteIndex(new Orders_ByCompany(), Etag.Parse("01000000-0000-000E-0000-000000000293"), DateTime.UtcNow.AddHours(6));
			}
			#endregion

			using (var store = new DocumentStore())
			{
				#region side_by_side_4
				// This method will create 'ReplacementOf/Orders/ByCompany' index, which will replace 'Orders/ByCompany' when
				// - new index will become non-stale
				// - new index will reach at least '01000000-0000-000E-0000-000000000293' etag
				// - in 6 hours from the deployment date
				new Orders_ByCompany().SideBySideExecute(store.DatabaseCommands, store.Conventions, Etag.Parse("01000000-0000-000E-0000-000000000293"), DateTime.UtcNow.AddHours(6));
				#endregion
			}
		}
	}
}