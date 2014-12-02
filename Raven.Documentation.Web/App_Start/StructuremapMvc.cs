[assembly: WebActivator.PreApplicationStartMethod(typeof(Raven.Documentation.Web.StructureMapMvc), "Start")]
namespace Raven.Documentation.Web
{
	using System.Web.Mvc;

	using Raven.Documentation.Web.DependencyResolution;

	public static class StructureMapMvc
	{
		public static void Start()
		{
			var container = IoC.Initialize();
			DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
		}
	}
}