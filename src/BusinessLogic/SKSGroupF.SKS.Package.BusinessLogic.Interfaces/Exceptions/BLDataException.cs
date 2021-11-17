using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions
{
    public class BLDataException : BLExceptionBase
    {
        public BLDataException(string logicModule) : base(logicModule)
        {
        }

        public BLDataException(string logicModule, string message, Exception innerException) : base(logicModule, message, innerException)
        {
        }

        public BLDataException(string logicModule, string message) : base(logicModule, message)
        {
        }
    }
}
