using Raven.Abstractions.Commands;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.HowTo
{
	public class Defer
	{
		private interface IFoo
		{
			#region defer_1
			void Defer(params ICommandData[] commands);
			#endregion
		}

		public Defer()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region defer_2
					session
						.Advanced
						.Defer(
							new PutCommandData
								{
									Key = "people/1",
									Document = RavenJObject.FromObject(new Person
																		   {
																			   FirstName = "John", 
																			   LastName = "Doe", 
																			   AddressId = "addresses/1"
																		   }),
									Metadata = new RavenJObject()
								},
							new PutCommandData
								{
									Key = "addresses/1",
									Document = RavenJObject.FromObject(new Address
																		   {
																			   Street = "Crystal Oak Street",
																		   }),
									Metadata = new RavenJObject()
								},
							new DeleteCommandData
								{
									Key = "people/2"
								});
					#endregion
				}
			}
		}
	}
}