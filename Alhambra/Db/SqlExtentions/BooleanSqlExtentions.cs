using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ledsun.Alhambra.Db.SqlExtentions
{
    /// <summary>
    /// SQL用の文字列に置き換えます。
    /// </summary>
    internal static class BooleanSqlExtentions
    {
        public static string ToSqlString(this bool val)
        {
            return val ? "1" : "0";
        }
    }
}
