using System;
using System.Collections.Concurrent;
using System.Globalization;

namespace Hasseware.ComponentModel.Mapping
{
    public class MappingService
    {
        private readonly ConcurrentDictionary<string, object> converterCache;
        private readonly IConverterFactory converterFactory;

        public MappingService(IConverterFactory converterFactory)
        {
            if (converterFactory == null)
                throw new ArgumentNullException("converterFactory");

            this.converterCache = new ConcurrentDictionary<string, object>();
            this.converterFactory = converterFactory;
        }

        public IMappingConfiguration Configuration 
        {
            get { return this.converterFactory.Configuration; }
        }

        public object Convert(object fromObject, Type fromType, Type toType)
        {
            return this.GetConverter(fromType, toType, CultureInfo.CurrentCulture)(fromObject);
        }

        public object Convert(object fromObject, Type fromType, Type toType, IFormatProvider provider)
        {
            return this.GetConverter(fromType, toType, provider)(fromObject);
        }

        public TTo Convert<TFrom, TTo>(TFrom fromObject)
        {
            return this.GetConverter<TFrom, TTo>(CultureInfo.CurrentCulture)(fromObject);
        }

        public TTo Convert<TFrom, TTo>(TFrom fromObject, IFormatProvider provider)
        {
            return this.GetConverter<TFrom, TTo>(provider)(fromObject);
        }

        public Func<object, object> GetConverter(Type fromType, Type toType)
        {
            return this.GetConverter(fromType, toType, CultureInfo.CurrentCulture);
        }

        public Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider provider)
        {
            string key = string.Concat(toType.FullName, fromType.FullName, "NonGeneric");
            return (Func<object, object>)this.converterCache.GetOrAdd(key,
                k => this.converterFactory.CreateDelegate(fromType, toType, provider));
        }

        public Converter<TFrom, TTo> GetConverter<TFrom, TTo>()
        {
            return this.GetConverter<TFrom, TTo>(CultureInfo.CurrentCulture);
        }

        public Converter<TFrom, TTo> GetConverter<TFrom, TTo>(IFormatProvider provider)
        {
            string key = string.Concat(typeof(TTo).FullName, typeof(TFrom).FullName);
            return (Converter<TFrom, TTo>)this.converterCache.GetOrAdd(key,
                k => this.converterFactory.CreateDelegate<TFrom, TTo>(provider));
        }

        public void CreateMap<TFrom, TTo>(Action<ITypeMap<TFrom, TTo>> config)
        {
            var typeMap = new TypeMap<TFrom, TTo>();
            config(typeMap);
            this.Configuration.SetTypeMapping(typeMap.GetTypeMapping());
        }
    }
}
