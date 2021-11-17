using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.ServiceAgents.Interfaces.Exceptions
{
    public class SAExceptionBase : ApplicationException
    {
        public string Agent { get; }

        public SAExceptionBase(string agent)
        {
            this.Agent = agent;
        }

        public SAExceptionBase(string agent, string message, Exception innerException) : base(message, innerException)
        {
            this.Agent = agent;
        }

        public SAExceptionBase(string agent, string message) : base(message)
        {
            this.Agent = agent;
        }
    }
}
