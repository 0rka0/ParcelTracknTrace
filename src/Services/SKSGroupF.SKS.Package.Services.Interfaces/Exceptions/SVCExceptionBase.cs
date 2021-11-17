using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.Services.Interfaces.Exceptions
{
    public class SVCExceptionBase : ApplicationException
    {
        public string Controller { get; }
        public SVCExceptionBase(string controller)
        {
            this.Controller = controller;
        }

        public SVCExceptionBase(string controller, string message, Exception innerException) : base(message, innerException)
        {
            this.Controller = controller;
        }

        public SVCExceptionBase(string controller, string message) : base(message)
        {          
            this.Controller = controller;
        }
    }
}
