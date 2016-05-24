﻿using System.Reflection;

namespace Hasseware.Reflection
{
    public class MemberGetter : Member
    {
        public MemberGetter(MemberInfo member, string key = null) : base(member, key)
        {
            this.NeedsStringIndex = this.CheckNeedsStringIndex(this.MemberInfo, this.MemberType);
            this.NeedsContainsCheck = ReflectionUtils.IsStringKeyDictionary(member.DeclaringType);
        }

        public bool NeedsContainsCheck { get; private set; }

        private bool CheckNeedsStringIndex(MemberInfo member, MemberType memberType)
        {
            if (memberType == MemberType.StringIndexer)
            {
                return true;
            }

            var method = member as MethodInfo;
            if (method != null)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
