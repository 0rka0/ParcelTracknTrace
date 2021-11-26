using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions
{
    public class BLDataDuplicateException : BLExceptionBase
    {
        //public BLDataDuplicateException(string logicModule) : base(logicModule)
        //{
        //}

        public BLDataDuplicateException(string logicModule, string message, Exception innerException) : base (logicModule, message, innerException)
        {
        }

        /*public BLDataDuplicateException(string logicModule, string message) : base(logicModule, message)
        {
        }*/
    }
}
