namespace Hasseware.Reflection
{
    public sealed class MemberMap
    {
        public MemberMap(MemberGetter fromMember, MemberSetter toMember)
        {
            this.FromMember = fromMember;
            this.ToMember = toMember;
        }

        public MemberGetter FromMember { get; private set; }

        public MemberSetter ToMember { get; private set; }
    }
}
