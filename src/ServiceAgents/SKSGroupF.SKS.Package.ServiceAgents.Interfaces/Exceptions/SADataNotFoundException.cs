using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.ServiceAgents.Interfaces.Exceptions
{
    public class SADataNotFoundException : SAExceptionBase
    {
        public SADataNotFoundException(string agent) : base(agent)
        {
        }

        public SADataNotFoundException(string agent, string message, Exception innerException) : base(agent, message, innerException)
        {
        }

        public SADataNotFoundException(string agent, string message) : base(agent, message)
        {
        }
    }
}
