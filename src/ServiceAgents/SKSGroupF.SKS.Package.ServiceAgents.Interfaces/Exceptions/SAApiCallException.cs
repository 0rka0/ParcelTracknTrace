using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.ServiceAgents.Interfaces.Exceptions
{
    public class SAApiCallException : SAExceptionBase
    {
        //public SAApiCallException(string agent) : base(agent)
        //{
        //}

        public SAApiCallException(string agent, string message, Exception innerException) : base(agent, message, innerException)
        {
        }

        //public SAApiCallException(string agent, string message) : base(agent, message)
        //{
        //}
    }
}
