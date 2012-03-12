using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Ledsun.Alhambra.Db.Helper
{
    public class DBHelperException : ApplicationException
    {
        public DBHelperException(SystemException e, IDbCommand cmd) : base(e.Message + "\n" + cmd.CommandText, e) { }
    }
}
