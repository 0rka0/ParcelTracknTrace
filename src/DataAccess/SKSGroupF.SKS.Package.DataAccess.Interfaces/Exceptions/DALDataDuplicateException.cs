using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.BusinessLogic.Exceptions
{
    public class DALDataDuplicateException : ApplicationException
    {
        public string Repository { get; }
        public string Operation { get; }
        public DALDataDuplicateException(string repository, string operation)
        {
            this.Repository = repository;
            this.Operation = operation;
        }

        public DALDataDuplicateException(string repository, string operation, string message, Exception innerException) : base (message, innerException)
        {
            this.Repository = repository;
            this.Operation = operation;
        }

        public DALDataDuplicateException(string repository, string operation, string message) : base(message)
        {
            this.Repository = repository;
            this.Operation = operation;
        }
    }
}
