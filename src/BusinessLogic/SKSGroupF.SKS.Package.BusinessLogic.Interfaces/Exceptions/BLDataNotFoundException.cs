using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions
{
    public class BLDataNotFoundException : BLExceptionBase
    {
        public BLDataNotFoundException(string logicModule) : base(logicModule)
        {
        }

        public BLDataNotFoundException(string logicModule, string message, Exception innerException) : base(logicModule, message, innerException)
        {
        }

        public BLDataNotFoundException(string logicModule, string message) : base(logicModule, message)
        {
        }
    }
}
