using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Hasseware.ComponentModel.Mapping.Converters;
using Hasseware.ComponentModel.Mapping.Emit;
using Hasseware.Reflection;

namespace Hasseware.ComponentModel.Mapping
{
    public class MappingConfiguration : IMappingConfiguration
    {
        private readonly ConcurrentDictionary<string, ITypeMapping> typeMappings = new ConcurrentDictionary<string, ITypeMapping>();

        private readonly ConcurrentDictionary<string, MethodWrapper> convertMethods = new ConcurrentDictionary<string, MethodWrapper>();

        private readonly ConcurrentDictionary<string, ILInstruction[]> convertInstructions = new ConcurrentDictionary<string, ILInstruction[]>();

        public MappingConfiguration()
        {
            AddConvertMethods<SafeConvert>();
            AddConvertMethods<SafeNullableConvert>();
            AddConvertMethods<SafeSqlConvert>();
        }

        private ILInstruction[] GetTryParseInstructions<TTo>() where TTo : struct
        {
            var toType = typeof(TTo);
            MethodInfo tryParse = toType.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => m.Name == "TryParse" && m.GetParameters().Length == 2).FirstOrDefault();

            var ilGenerator = new ILGeneratorAdapter(this);

            if (tryParse != null)
            {
                var toLocal = ilGenerator.DeclareLocal(toType);
                ilGenerator.EmitLocal(OpCodes.Ldloca, toLocal);
                ilGenerator.EmitCall(OpCodes.Call, tryParse);
                ilGenerator.Emit(OpCodes.Pop);
                ilGenerator.EmitLocal(OpCodes.Ldloc, toLocal);
            }
            return ilGenerator.Instructions;
        }

        public ITypeMapping GetTypeMapping(Type fromType, Type toType)
        {
            string typeName = String.Concat(fromType.FullName, toType.FullName);
            return this.typeMappings.GetOrAdd(typeName, k => new TypeMapping(fromType, toType));
        }

        public void SetTypeMapping(ITypeMapping typeMapping)
        {
            string typeName = String.Concat(typeMapping.FromType.FullName, typeMapping.ToType.FullName);
            this.typeMappings.AddOrUpdate(typeName, typeMapping, (key, oldValue) => typeMapping);
        }

        public ILInstruction[] GetConvertInstructions(Type fromType, Type toType)
        {
            ILInstruction[] instructions;
            string typeName = String.Concat(fromType.FullName, toType.FullName);
            return (this.convertInstructions.TryGetValue(typeName, out instructions)) ? instructions : null;
        }

        public void SetConvertInstructions<TFrom, TTo>(ILInstruction[] instructions)
        {
            this.SetConvertInstructions(typeof(TFrom), typeof(TTo), instructions);
        }

        public void SetConvertInstructions(Type fromType, Type toType, ILInstruction[] instructions)
        {
            string typeName = String.Concat(fromType.FullName, toType.FullName);
            this.convertInstructions.AddOrUpdate(typeName, instructions, (key, oldValue) => instructions);
        }

        public MethodWrapper GetConvertMethod(Type fromType, Type toType)
        {
            MethodWrapper mv;
            string typeName = String.Concat(fromType.FullName, toType.FullName);
            return (this.convertMethods.TryGetValue(typeName, out mv)) ? mv : null;
        }

        public void SetConvertMethod<TFrom, TTo>(Func<TFrom, TTo> converter)
        {
            this.SetConvertMethod(typeof(TFrom), typeof(TTo), converter.Method, converter.Target);
        }

        public void AddConvertMethods<TConvertClass>()
        {
            this.AddConvertMethods(typeof(TConvertClass));
        }

        public void AddConvertMethods(Type convertClass)
        {
            foreach (var method in convertClass.GetMethods())
            {
                var pars = method.GetParameters();
                if ((pars.Length == 2 && pars[1].ParameterType == typeof(IFormatProvider)) || pars.Length == 1)
                    SetConvertMethod(pars[0].ParameterType, method.ReturnType, method, null);
            }
        }

        private void SetConvertMethod(Type fromType, Type toType, MethodInfo method, object target)
        {
            var convertMethod = method.IsStatic ? null : ReflectionUtils.GetStaticMemberInfo(method.DeclaringType);
            this.SetConvertMethod(fromType, toType, new MethodWrapper(method, target, convertMethod));
        }

        private void SetConvertMethod(Type fromType, Type toType, MethodWrapper convertMethod)
        {
            string typeName = String.Concat(fromType.FullName, toType.FullName);
            this.convertMethods.AddOrUpdate(typeName, convertMethod, (key, oldValue) => convertMethod);
        }
    }
}
