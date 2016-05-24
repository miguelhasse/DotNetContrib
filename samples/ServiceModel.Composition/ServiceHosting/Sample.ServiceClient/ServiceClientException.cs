using System;

namespace Sample
{
    [Serializable]
    public sealed class ServiceClientException : Exception
    {
        #region Constructors

        internal ServiceClientException(Exception exception) : base(null, exception)
        {
        }

        #endregion
    }
}
