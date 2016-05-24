using Hasseware.ComponentModel.Mapping.Emit;
using Hasseware.Reflection;
using System;

namespace Hasseware.ComponentModel.Mapping
{
    public interface IMappingConfiguration
    {
        ITypeMapping GetTypeMapping(Type fromType, Type toType);

        void SetTypeMapping(ITypeMapping typeMapping);

        MethodWrapper GetConvertMethod(Type fromType, Type toType);

        void SetConvertMethod<TFrom, TTo>(Func<TFrom, TTo> converter);

        void AddConvertMethods<TConvertClass>();

        void AddConvertMethods(Type convertClass);

        ILInstruction[] GetConvertInstructions(Type fromType, Type toType);

        void SetConvertInstructions<TFrom, TTo>(ILInstruction[] instructions);

        void SetConvertInstructions(Type fromType, Type toType, ILInstruction[] instructions);
    }
}