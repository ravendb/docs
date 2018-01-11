using System;
using System.Reflection;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Session.Configuration
{
    public class FindIdentityProperty
    {
        private interface IFoo
        {
            #region identity_1
            Func<MemberInfo, bool> FindIdentityProperty { get; set; }
            #endregion
        }

        public FindIdentityProperty()
        {
            using (var store = new DocumentStore())
            {
                #region identity_2
                store.Conventions.FindIdentityProperty = memberInfo => memberInfo.Name == "Identifier";
                #endregion
            }
        }
    }
}
