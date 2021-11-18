using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.Services.Interfaces.Exceptions
{
    public class SVCConversionException : SVCExceptionBase
    {
        //public SVCConversionException(string controller) : base(controller)
        //{
        //}

        public SVCConversionException(string controller, string message, Exception innerException) : base(controller, message, innerException)
        {
        }

        //public SVCConversionException(string controller, string message) : base(controller, message)
        //{
        //}
    }
}
