using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.Services.Interfaces.Exceptions
{
    class ServiceBLCallException : ApplicationException
    {
        public string Operation { get; }
        public ServiceBLCallException( string operation)
        {
            this.Operation = operation;
        }

        public ServiceBLCallException( string operation, string message, Exception innerException) : base(message, innerException)
        {
            this.Operation = operation;
        }

        public ServiceBLCallException( string operation, string message) : base(message)
        {          
            this.Operation = operation;
        }
    }
}
