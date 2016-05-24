using System;

namespace Hasseware.ComponentModel.Mapping
{
    public interface IConverterFactory
    {
        IMappingConfiguration Configuration { get; }

        Func<object, object> CreateDelegate(Type fromType, Type toType);

        Func<object, object> CreateDelegate(Type fromType, Type toType, IFormatProvider provider);

        Converter<TFrom, TTo> CreateDelegate<TFrom, TTo>();

        Converter<TFrom, TTo> CreateDelegate<TFrom, TTo>(IFormatProvider provider);
    }
}