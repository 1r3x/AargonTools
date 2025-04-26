using AargonTools.Manager.GenericManager;
using AargonTools.Manager.WebHook;
using AargonTools.Models.WebHook;
using System.Threading.Tasks;

namespace AargonTools.Interfaces.WebHook
{
    public interface IComlinkv2
    {
        Task<ProcessSmsResult> ProcessInboundSms(TextRequestModel request, string clientIp, string requestUrl);
    }
}
