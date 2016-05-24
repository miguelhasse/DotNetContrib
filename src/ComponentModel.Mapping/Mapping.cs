using System;

namespace Hasseware.ComponentModel.Mapping
{
    public static class Mapping
    {
        private static MappingService safeMapService;

        static Mapping()
        {
            var converterFactory = new ConverterFactory();
            safeMapService = new MappingService(converterFactory);
        }

        public static IMappingConfiguration Configuration 
        {
            get
            {
                return safeMapService.Configuration;
            }
            set
            {
                var converterFactory = new ConverterFactory(value);
                safeMapService = new MappingService(converterFactory);
            }
        }

        public static object Convert(object fromObject, Type fromType, Type toType)
        {
            return safeMapService.Convert(fromObject, fromType, toType);
        }

        public static object Convert(object fromObject, Type fromType, Type toType, IFormatProvider provider)
        {
            return safeMapService.Convert(fromObject, fromType, toType, provider);
        }

        public static TTo Convert<TFrom, TTo>(TFrom fromObject)
        {
            return safeMapService.Convert<TFrom, TTo>(fromObject);
        }

        public static TTo Convert<TFrom, TTo>(TFrom fromObject, IFormatProvider provider)
        {
            return safeMapService.Convert<TFrom, TTo>(fromObject, provider);
        }

        public static Func<object, object> GetConverter(Type fromType, Type toType)
        {
            return safeMapService.GetConverter(fromType, toType);
        }

        public static Func<object, object> GetConverter(Type fromType, Type toType, IFormatProvider provider)
        {
            return safeMapService.GetConverter(fromType, toType, provider);
        }

        public static Converter<TFrom, TTo> GetConverter<TFrom, TTo>()
        {
            return safeMapService.GetConverter<TFrom, TTo>();
        }

        public static Converter<TFrom, TTo> GetConverter<TFrom, TTo>(IFormatProvider provider)
        {
            return safeMapService.GetConverter<TFrom, TTo>(provider);
        }

        public static void CreateMap<TFrom, TTo>(Action<ITypeMap<TFrom, TTo>> config)
        {
            safeMapService.CreateMap(config);
        }
    }
}
