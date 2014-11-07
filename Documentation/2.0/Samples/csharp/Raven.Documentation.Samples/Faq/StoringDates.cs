namespace RavenCodeSamples.Faq
{
	using System;
	using System.Net;

	public class PersonCreateInputModel
	{
		public string name { get; set; }

		public string surname { get; set; }
	}

	public class Person
	{
		public Person(string name, string surname, DateTime dateOfBirth)
		{
		}
	}

	public class StoringDates : MvcCodeSampleBase
	{
		#region storing_dates_1
		[ActionName("Rest Person")]
		[HttpPost]
		public JsonResult Create(PersonCreateInputModel model)
		{
			if (ModelState.IsValid)
			{
				var session = MvcApplication.RavenSession;
				var person = new Person(
					model.name,
					model.surname,
					DateTime.UtcNow
				);
				session.Store(person);
				session.SaveChanges();
				return Json(person);
			}

			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return null;
		}

		#endregion
	}
}