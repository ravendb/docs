namespace RavenCodeSamples.Appendixes
{
	public class ProfilingMvcApplication : MvcCodeSampleBase
	{
		#region profiling_mvc_application_1
		protected void Application_Start()
		{
			// Code omitted for simplicity

			InitializeRavenProfiler();
		}
		#endregion

		#region profiling_mvc_application_2
		private void InitializeRavenProfiler()
		{
			Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(DocumentStoreHolder.Store);
		}
		#endregion
	}

	namespace Foo
	{
		using System.Diagnostics;

		public class Global
		{
			#region profiling_mvc_application_3
			protected void Application_Start()
			{
				// Code omitted for simplicity

			#if DEBUG
				InitializeRavenProfiler();
			#endif
			}
			#endregion

			#region profiling_mvc_application_4
			[Conditional("DEBUG")]
			private void InitializeRavenProfiler()
			{
				Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(DocumentStoreHolder.Store);
			}
			#endregion
		}
	}

	namespace Foo2
	{
		using System.ComponentModel.DataAnnotations;
		using System.Linq;

		using RavenCodeSamples.Samples.Mvc.Foo;

		#region profiling_mvc_application_6
		public class User
		{
			public string Id { get; set; }

			[Required]
			public string Name { get; set; }

			[Required]
			public string Password { get; set; }
		}
		#endregion

		#region profiling_mvc_application_5
		public class HomeController : RavenController
		{
			public ActionResult Index()
			{
				var items = RavenSession
					.Query<User>()
					.Customize(x => x.WaitForNonStaleResults())
					.ToList();

				return View("Index", items);
			}

			public ActionResult Create()
			{
				return View("Create");
			}

			[HttpPost]
			public ActionResult Create(User user)
			{
				if (!ModelState.IsValid)
				{
					return View("Create", user);
				}

				RavenSession.Store(user);

				return RedirectToAction("Index");
			}
		}
		#endregion
	}

	namespace Foo3
	{
		public class Global
		{
			#region profiling_mvc_application_7
			private readonly string[] fieldsToFilter = new[] { "Password" };

			private void InitializeRavenProfiler()
			{
				Raven.Client.MvcIntegration.RavenProfiler.InitializeFor(DocumentStoreHolder.Store, fieldsToFilter);
			}

			#endregion
		}
	}
}