using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions
{
    public class DALDataException : ApplicationException
    {
        public string Repository { get; }
        public string Operation { get; }
        public DALDataException(string repository, string operation)
        {
            this.Repository = repository;
            this.Operation = operation;
        }

        public DALDataException(string repository, string operation, string message, Exception innerException) : base (message, innerException)
        {
            this.Repository = repository;
            this.Operation = operation;
        }

        public DALDataException(string repository, string operation, string message) : base(message)
        {
            this.Repository = repository;
            this.Operation = operation;
        }
    }
}
