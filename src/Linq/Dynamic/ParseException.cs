namespace System.Linq.Dynamic
{
    public sealed class ParseException : Exception
    {
        public ParseException(string message, int position)
            : base(message)
        {
            this.Position = position;
        }

        public int Position { get; private set; }

        public override string ToString()
        {
            return string.Format(Hasseware.Properties.Resources.ParseExceptionFormat, base.Message, this.Position);
        }
    }
}
