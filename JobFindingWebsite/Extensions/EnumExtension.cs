using System.Text.RegularExpressions;

namespace JobFindingWebsite.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var memberInfo = type.GetMember(enumValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayNameAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((DisplayNameAttribute)attributes[0]).DisplayName;
                }
            }
            return enumValue.ToString();
        }
    }
}