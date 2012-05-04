using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alhambra.Db.SqlExtentions
{
    /// <summary>
    /// SQL用の文字列に置き換えます。
    /// </summary>
    internal static class DateTimeSqlExtentions
    {
        private const string SQL_DATETIME_FORMAT = "\\'yyyy/MM/dd HH:mm:ss\\'";

        public static string ToSqlString(this DateTime val)
        {
            return val.ToString(SQL_DATETIME_FORMAT);
        }
    }
}
