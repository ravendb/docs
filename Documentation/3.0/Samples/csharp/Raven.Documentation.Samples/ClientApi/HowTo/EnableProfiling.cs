using System;

using Raven.Client.Connection.Profiling;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.HowTo
{
	public class EnableProfiling
	{
		public EnableProfiling()
		{
			using (var documentStore = new DocumentStore())
			{
				#region initialize_profiling
				documentStore.InitializeProfiling();
				#endregion

				Guid id = Guid.Empty;

				#region get_profiling_info
				ProfilingInformation profilingInformation = documentStore.GetProfilingInformationFor(id);
				#endregion

				#region example
				Guid sesionId;
				using (var session = documentStore.OpenSession())
				{
					sesionId = ((DocumentSession)session).Id;

					session.Load<Employee>("employees/1");
				}

				var sessionProfilingInfo = documentStore.GetProfilingInformationFor(sesionId);
				#endregion
			}
		}
	}
}