using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ledsun.Alhambra.Db.SqlExtentions
{
    internal static class IEnumerableStringSqlExtentions
    {
        /// <summary>
        /// 文字列をANDで結合します。
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string AndJoin(this IEnumerable<string> parameters)
        {
            return string.Join(" AND ", parameters);
        }
    }
}
