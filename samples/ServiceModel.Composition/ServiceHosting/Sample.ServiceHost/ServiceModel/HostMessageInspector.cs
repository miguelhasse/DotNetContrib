using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace Sample.ServiceModel
{
    internal sealed class HostMessageInspector : IDispatchMessageInspector
    {
        private const string CultureInfoHeaderKey = "Culture";
        public const string CultureInfoNamespace = "http://schemas.newhotel.com/service/2015/09/ws-i18n";

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            ApplyCultureInfo(request);
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        private void ApplyCultureInfo(Message request)
        {
            int headerIndex = request.Headers.FindHeader(CultureInfoHeaderKey, CultureInfoNamespace);
            if (headerIndex != -1) Thread.CurrentThread.CurrentCulture = new CultureInfo(request.Headers.GetHeader<string>(headerIndex));
        }
    }
}
