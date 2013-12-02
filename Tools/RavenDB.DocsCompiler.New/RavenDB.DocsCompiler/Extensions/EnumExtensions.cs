namespace RavenDB.DocsCompiler.Extensions
{
    using System;
    using System.ComponentModel;

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            var enumInfo = @enum.GetType().GetField(@enum.ToString());
            var attributes = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : @enum.ToString();
        }
    }
}