using System;
using System.Reflection.Emit;

namespace Hasseware.ComponentModel.Mapping.Emit
{
    internal sealed class LocalBuilderWrapper
    {
        public LocalBuilderWrapper(Type localType)
        {
            this.LocalType = localType;
        }

        public LocalBuilder LocalBuilder { get; set; }

        public Type LocalType { get; private set; }
    }
}
