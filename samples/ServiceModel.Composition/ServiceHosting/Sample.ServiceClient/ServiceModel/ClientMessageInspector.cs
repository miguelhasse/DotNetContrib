using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace Sample.ServiceModel
{
    internal sealed class ClientMessageInspector : IClientMessageInspector
    {
        private const string CultureInfoHeaderKey = "Culture";
        public const string CultureInfoNamespace = "http://schemas.hasseware.com/service/2016/01/ws-i18n";

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
			System.Diagnostics.Trace.TraceInformation("SessionID: {0}", channel.SessionId);
			AddCultureInfo(request);
			return null;
        }

        private void AddCultureInfo(Message request)
        {
			var cultureInfoHeader = MessageHeader.CreateHeader(CultureInfoHeaderKey,
				CultureInfoNamespace, Thread.CurrentThread.CurrentCulture.Name);
            request.Headers.Add(cultureInfoHeader);
        }
    }
}
