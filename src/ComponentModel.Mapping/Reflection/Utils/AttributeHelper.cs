﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Hasseware.Reflection
{
    internal class AttributeHelper
    {
        public static string GetEnumDisplayValue(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = GetAttribute<DisplayAttribute>(fieldInfo);

            return (attribute != null) ? attribute.GetName() : string.Empty;
        }

        public static string GetEnumDescriptionValue(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = GetAttribute<DescriptionAttribute>(fieldInfo);

            return (attribute != null) ? attribute.Description : string.Empty;
        }

        public static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            var attributes = GetAttributes<T>(member);
            return (attributes.Length > 0) ? attributes[0] : default(T);
        }

        public static T[] GetAttributes<T>(MemberInfo member) where T : Attribute
        {
            return Array.ConvertAll(member.GetCustomAttributes(typeof(T), false), input => (T)input);
        }
    }
}
