using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions
{
    public class BLLogicException : BLExceptionBase
    {
        //public BLLogicException(string logicModule) : base(logicModule)
        //{
        //}

        public BLLogicException(string logicModule, string message, Exception innerException) : base(logicModule, message, innerException)
        {
        }

        public BLLogicException(string logicModule, string message) : base(logicModule, message)
        {
        }
    }
}
