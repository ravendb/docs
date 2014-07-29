using Raven.Client.Changes;

namespace Raven.Documentation.CodeSamples.ClientApi.Changes
{
	public class WhatIsChangesApi
	{
		 private interface IFoo
		 {
			 #region changes_1
			 IDatabaseChanges Changes(string database = null);
			 #endregion
		 }
	}
}