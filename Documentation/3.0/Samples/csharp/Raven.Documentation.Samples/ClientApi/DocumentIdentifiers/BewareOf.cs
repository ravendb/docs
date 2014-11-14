namespace Raven.Documentation.Samples.ClientApi.DocumentIdentifiers
{
	using Client.Document;
	using CodeSamples.Orders;

	public class BewareOf
	{
		public BewareOf()
		{
			var store = new DocumentStore();

			using (var session = store.OpenSession())
			{
				#region session_value_types
				Employee employee = session.Load<Employee>(9); // get "employees/9"

				session.Delete<Employee>(12); // delete "employees/12"
				#endregion
			}
		} 
	}
}