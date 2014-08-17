namespace Raven.Documentation.Web.Helpers
{
	public static class DebugHelper
	{
		public static bool IsDebug()
		{
#if DEBUG
			return true;
#else 
			return false;
#endif
		}
	}
}