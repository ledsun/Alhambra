using Alhambra.Db.Data;
using Alhambra.Db.Plugin;
using System;
using System.Collections.Generic;
using System.Data;

namespace Alhambra.Db.Helper
{
    static class AbstractDBBridgeExtensions
    {
        private const string SQL_SHOULD_NOT_NULL_OR_EMPTY = "SQLにヌルまたは空文字は指定できません。";

        /// <summary>
        /// 結果を返さないSQLを実行します
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Execute(this AbstractDBBridge db, string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareCommand(db, sql).Execute();
        }

        /// <summary>
        /// SELECTを実行します。
        /// 結果はDataRowAccessorのリストで返します。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<DataRowAccessor> Select(this AbstractDBBridge db, string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareDataAdapter(db, sql).SelectFromDataAdapter();
        }

        /// <summary>
        /// 値を一つ返すSQLを実行します
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static TypeConvertableWrapper SelectOne(this AbstractDBBridge db, string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareCommand(db, sql).SelectOne();
        }

        /// <summary>
        /// SELECTを実行します。
        /// 結果はDataSetで返します。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet SelectDataSet(this AbstractDBBridge db, string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareDataAdapter(db, sql).SelectDataSetFromDataAdapter();
        }

        /// <summary>
        /// テーブルスキーマを取得します。
        /// 結果はDataTableで返します。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable SelectTableSchema(this AbstractDBBridge db, string tableName)
        {
            if (String.IsNullOrEmpty(tableName))
                throw new ArgumentException("テーブル名にヌルまたは空文字は指定できません。");

            return PrepareDataAdapter(db, string.Format("SELECT * FROM {0};", tableName)).SelectTableSchema();
        }

        private static IDbCommand PrepareCommand(AbstractDBBridge db, string sql)
        {
            db.Cmd.CommandText = sql;
            db.Cmd.CommandTimeout = db.Timeout;
            return db.Cmd;
        }

        private static IDbDataAdapter PrepareDataAdapter(AbstractDBBridge db, string sql)
        {
            var dataAdapter = db.CreateAdapter(sql, db.Connection);
            dataAdapter.SelectCommand.CommandTimeout = db.Timeout;
            dataAdapter.SelectCommand.Transaction = db.Trans;
            return dataAdapter;
        }
    }

}
