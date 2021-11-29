using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.Webhooks.Interfaces.Exceptions
{
    public class WebhookExceptionBase : ApplicationException
    {
        public string Controller { get; }

        public WebhookExceptionBase(string controller, string message, Exception innerException) : base(message, innerException)
        {
            this.Controller = controller;
        }
    }
}
