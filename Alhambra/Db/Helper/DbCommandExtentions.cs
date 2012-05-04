using System;
using System.Collections.Generic;
using System.Data;
using Alhambra.Db.Data;

namespace Alhambra.Db.Helper
{
    /// <summary>
    /// IDbCommand拡張
    /// 例外が発生した場合はメッセージに実行したSQLを追加します。
    /// </summary>
    static class DBCommandExtentions
    {
        /// <summary>
        /// SQLを実行します。
        /// 影響のあった値を返します。
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
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

        /// <summary>
        /// SQLを実行して一つの値を返します。
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        internal static TypeConvertableWrapper SelectOne(this IDbCommand cmd)
        {
            try
            {
                return new TypeConvertableWrapper(cmd.ExecuteScalar());
            }
            catch (SystemException e)
            {
                throw new DBHelperException(e, cmd);
            }
        }
    }
}
