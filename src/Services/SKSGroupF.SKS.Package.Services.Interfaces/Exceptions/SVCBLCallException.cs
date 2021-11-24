using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.Services.Interfaces.Exceptions
{
    public class SVCBLCallException : SVCExceptionBase
    {
        //public SVCBLCallException(string controller) : base(controller)
        //{
        //}

        public SVCBLCallException(string controller, string message, Exception innerException) : base(controller, message, innerException)
        {
        }

        //public SVCBLCallException(string controller, string message) : base(controller, message)
        //{
        //}
    }
}
