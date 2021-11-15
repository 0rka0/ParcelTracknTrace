using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Interfaces.Exceptions
{
    class BLLogicException : ApplicationException
    {
        public string LogicModule { get; }

        public BLLogicException (string logicModule, string message, Exception innerException) : base(message, innerException )
        {
            this.LogicModule = logicModule;
        }
        public BLLogicException(string logicModule, string message) 
        {
            this.LogicModule = logicModule;
        }
        public BLLogicException(string logicModule) : base()
        {
            this.LogicModule = logicModule;
        }
        

    }
}
