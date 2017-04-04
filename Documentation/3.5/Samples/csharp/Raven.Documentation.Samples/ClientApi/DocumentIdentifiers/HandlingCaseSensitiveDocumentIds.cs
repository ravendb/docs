using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Documentation.Samples.ClientApi.DocumentIdentifiers
{
    #region document_ids_1
    public class TokenEntity
    {
        public string Token { get; set; }
        public string Secret { get; set; }

        public TokenEntity()
        {
            this.Token = string.Empty;
            this.Secret = string.Empty;
        }
    }
    #endregion

    class Example
    {
        private string Token;
        private static string IdPrefix;

        #region document_ids_2
        public string ToRavenId()
        {
            return IdPrefix + Convert.ToBase64String(CultureInfo.InvariantCulture.CompareInfo
                       .GetSortKey(Token, CompareOptions.StringSort)
                       .KeyData);
        }

        public static string ToRavenId(string key)
        {
            return IdPrefix + Convert.ToBase64String(CultureInfo.InvariantCulture.CompareInfo
                       .GetSortKey(key, CompareOptions.StringSort)
                       .KeyData);
        }
        #endregion
    }
}
