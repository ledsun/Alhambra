using System;
using System.Collections.Generic;
using System.Data;
using Ledsun.Alhambra.Db.Data;

namespace Ledsun.Alhambra.Db.Helper
{
    static class DBCommandExtentions
    {
        internal static int Execute(this IDbCommand cmd)
        {
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (SystemException e)
            {

                throw new DBHelperException(e, cmd);
            }
        }

        internal static object ExecuteScalar(this IDbCommand cmd)
        {
            try
            {
                return cmd.ExecuteScalar();
            }
            catch (SystemException e)
            {
                throw new DBHelperException(e, cmd);
            }
        }

       
    }
}
