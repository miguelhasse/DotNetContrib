namespace System.Linq.Dynamic
{
    public class DynamicProperty
    {
        public DynamicProperty(string name, Type type)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (type == null) throw new ArgumentNullException("type");

            this.Name = name;
            this.Type = type;
        }

        public string Name { get; private set; }

        public Type Type { get; private set; }
    }
}
