using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.Webhooks.Interfaces.Exceptions
{
    public class AlertException : WebhookExceptionBase
    {
        public AlertException(string controller, string message, Exception innerException) : base(controller, message, innerException)
        {
        }
    }
}
