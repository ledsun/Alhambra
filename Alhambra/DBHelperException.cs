using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace  Ledsun.Alhambra
{
    public class DBHelperException : Exception
    {
        public DBHelperException(string message)
            : base(message) { }

        public DBHelperException(string message, Exception innerException)
            : base(message, innerException) { }

        public DBHelperException(SystemException e, IDbCommand cmd) 
            : base(e.Message + "\n" + cmd.CommandText, e) { }
    }
}
