using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions
{
    public class BLValidationException : BLExceptionBase
    {
        //public BLValidationException(string logicModule) : base(logicModule)
        //{
        //}

        //public BLValidationException(string logicModule, string message, Exception innerException) : base(logicModule, message, innerException)
        //{
        //}

        public BLValidationException(string logicModule, string message) : base(logicModule, message)
        {
        }
    }
}
