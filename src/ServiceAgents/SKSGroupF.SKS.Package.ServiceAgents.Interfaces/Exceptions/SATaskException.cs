using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.ServiceAgents.Interfaces.Exceptions
{
    public class SATaskException : SAExceptionBase
    {
        public SATaskException(string agent) : base(agent)
        {
        }

        public SATaskException(string agent, string message, Exception innerException) : base(agent, message, innerException)
        {
        }

        public SATaskException(string agent, string message) : base(agent, message)
        {
        }
    }
}
