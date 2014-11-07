namespace Raven.Documentation.CodeSamples.ClientApi.HowTo
{
	using System;
	using Client.Connection.Profiling;
	using Client.Document;
	using Orders;

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