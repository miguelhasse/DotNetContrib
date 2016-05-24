using System.Reflection;

namespace Hasseware.ComponentModel.Mapping
{
    public class MappingMethodWrapper
    {
        public MappingMethodWrapper(MethodInfo method, object target = null)
        {
            this.Method = method;
            this.Target = target;
        }

        public MethodInfo Method { get; set; }

        public object Target { get; set; }
    }
}
