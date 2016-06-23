using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Alhambra.Db.Data;

namespace Alhambra.Db.Helper
{
    /// <summary>
    /// IDbDataAdapterの拡張
    /// </summary>
    static class IDbAdapterExtensions
    {
        /// <summary>
        /// 取得したDataTable.RowsをIEnumerable{DataRowAccessor}に変換します
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        internal static IEnumerable<DataRowAccessor> SelectFromDataAdapter(this IDbDataAdapter adapter)
        {
            return new LinqList<DataRow>(FillDataSet(adapter).Tables[0].Rows)
                .Select(r => new DataRowAccessor(r));
        }

        /// <summary>
        /// DataBindに使えるようにDataSetで取得します。
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        internal static DataSet SelectDataSetFromDataAdapter(this IDbDataAdapter adapter)
        {
            return FillDataSet(adapter);
        }

        /// <summary>
        /// テーブルスキーマを取得します。
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        internal static DataTable SelectTableSchema(this IDbDataAdapter adapter)
        {
            try
            {
                var ds = new DataSet();
                adapter.FillSchema(ds, SchemaType.Source);
                return ds.Tables[0];
            }
            catch (SystemException e)
            {
                throw new DBHelperException(e, adapter.SelectCommand);
            }
        }

        /// <summary>
        /// 例外が発生した時に、メッセージに実行したSQL文を追加します。
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="ds"></param>
        private static DataSet FillDataSet(IDbDataAdapter adapter)
        {
            try
            {
                var ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            catch (SystemException e)
            {
                throw new DBHelperException(e, adapter.SelectCommand);
            }
        }
    }
}
