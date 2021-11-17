using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions
{
    class DALConnectionException : DALExceptionBase
    {
        public DALConnectionException(string repository, string operation) : base(repository, operation)
        {
        }

        public DALConnectionException(string repository, string operation, string message, Exception innerException) : base(repository, operation, message, innerException)
        {
        }

        public DALConnectionException(string repository, string operation, string message) : base(repository, operation, message)
        {
        }
    }
}
