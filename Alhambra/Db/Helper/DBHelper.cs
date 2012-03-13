using System.Collections.Generic;
using System.Data;
using Ledsun.Alhambra.Db.Data;
using Ledsun.Alhambra.Db.Plugin;

namespace Ledsun.Alhambra.Db.Helper
{
    /// <summary>
    /// DBアクセス用のユーティリティクラスです。
    /// 実行にはconfigファイルにconnectionStringにDBHelperという名前で接続文字列を設定する必要があります。
    /// SQLの生成にはSqlStatementの使用を想定していますが、ただの文字列でも実行可能です。
    /// </summary>
    /// <example>
    /// int value =DBHelper.Select(new SqlStatement(@"
    ///              SELECT
    ///                  VALE
    ///              FROM EXAMPLE_TABLE
    ///              WHERE ID = @ID@
    ///              ")
    ///             .Replace("ID", 100)
    ///             )[0]["VALUE"].Int;
    /// </example>
    public class DBHelper
    {
        /// <summary>
        /// UPDATE,DELETEなどの結果を返さないSQLを実行します。
        /// </summary>
        /// <param name="sql">実行するSQL文字列</param>
        public static int Execute(string sql, DBTran tran = null)
        {
            return tran == null
                ? DBFactory.NewDB.Execute(sql)
                : tran.DB.Execute(sql);
        }

        public static IEnumerable<DataRowAccessor> Select(string sql, DBTran tran = null)
        {
            return tran == null
                ? DBFactory.NewDB.Select(sql)
                : tran.DB.Select(sql);
        }

        public static TypeConvertableWrapper SelectOne(string sql, DBTran tran = null)
        {
            return tran == null
                ? DBFactory.NewDB.SelectOne(sql)
                : tran.DB.SelectOne(sql);
        }

        public static DataSet SelectDataSet(string sql, DBTran tran = null)
        {
            return tran == null
                ? DBFactory.NewDB.SelectDataSet(sql)
                : tran.DB.SelectDataSet(sql);
        }
    }
}