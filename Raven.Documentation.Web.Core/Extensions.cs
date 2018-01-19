using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Raven.Documentation.Web.Core
{
    	public static class Extensions
    {
		public static string GetDescription(this Enum @enum)
		{
			var enumInfo = @enum.GetType().GetField(@enum.ToString());
			var attributes = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

			return attributes.Length > 0 ? attributes[0].Description : @enum.ToString();
		}
		
        public static string FullContentPageId(string slug)
        {
            return "contentpages/" + slug;
        }
    }
}
