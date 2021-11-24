using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions
{
    public class DALSqlContextException : DALExceptionBase
    {
        //public DALSqlContextException(string repository, string operation) : base (repository, operation)
        //{
        //}

        public DALSqlContextException(string repository, string operation, string message, Exception innerException) : base (repository, operation, message, innerException)
        {
        }

        //public DALSqlContextException(string repository, string operation, string message) : base(repository, operation, message)
        //{
        //}
    }
}
