using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions
{
    public class BLExceptionBase : ApplicationException
    {
        public string logicModule { get; }

        public BLExceptionBase(string logicModule) : base()
        {
            this.logicModule = logicModule;
        }

        public BLExceptionBase(string logicModule, string message, Exception innerException) : base(message, innerException)
        {
            this.logicModule = logicModule;
        }

        public BLExceptionBase(string logicModule, string message) : base(message)
        {
            this.logicModule = logicModule;
        }
    }
}
