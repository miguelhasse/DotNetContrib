using Hasseware.Reflection;
using System;
using System.Collections.Generic;

namespace Hasseware.ComponentModel.Mapping
{
    public interface ITypeMapping
    {
        Type FromType { get; }

        Type ToType { get; }

        List<MemberMap> MemberMaps { get; }
    }
}