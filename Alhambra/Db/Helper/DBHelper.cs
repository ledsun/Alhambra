using System.Collections.Generic;
using System.Data;
using Alhambra.Db.Data;
using Alhambra.Db.Plugin;

namespace Alhambra.Db.Helper
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
            if (tran != null)
            {
                return tran.DB.Execute(sql);
            }

            using (var d = DBFactory.NewDB)
            {
                return d.Execute(sql);
            }
        }

        public static IEnumerable<DataRowAccessor> Select(string sql, DBTran tran = null)
        {
            if (tran != null)
            {
                return tran.DB.Select(sql);
            }

            using (var d = DBFactory.NewDB)
            {
                return d.Select(sql);
            }
        }

        public static TypeConvertableWrapper SelectOne(string sql, DBTran tran = null)
        {
            if (tran != null)
            {
                return tran.DB.SelectOne(sql);
            }

            using (var d = DBFactory.NewDB)
            {
                return d.SelectOne(sql);
            }
        }

        public static DataSet SelectDataSet(string sql, DBTran tran = null)
        {
            if (tran != null)
            {
                return tran.DB.SelectDataSet(sql);
            }

            using (var d = DBFactory.NewDB)
            {
                return d.SelectDataSet(sql);
            }
        }

        public static DataTable SelectTableSchema(string tableName)
        {
            using (var d = DBFactory.NewDB)
            {
                return d.SelectTableSchema(tableName);
            }
        }
    }
}