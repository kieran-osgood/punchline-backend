using System;
using System.ComponentModel;

namespace GraphQL.Extensions
{
    public static class EnumToString
    {
        public static string? GetValueAsString(this Enum num)
        {
            // get the field
            var field = num.GetType().GetField(num.ToString());

            var customAttributes = field?.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes != null && customAttributes?.Length > 0)
            {
                return (customAttributes[0] as DescriptionAttribute)?.Description;
            }

            return num.ToString();
        }
    }
}