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
    static class DBAdapterExtentions
    {
        /// <summary>
        /// 取得したDataTable.RowsをIEnumerable{DataRowAccessor}に変換します
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        internal static IEnumerable<DataRowAccessor> SelectFromDataAdapter(this IDbDataAdapter adapter)
        {
            var ds = new DataSet();
            FillDataSet(adapter, ds);
            return new LinqList<DataRow>(ds.Tables[0].Rows)
                .Select(r => new DataRowAccessor(r));
        }

        /// <summary>
        /// DataBindに使えるようにDataSetで取得します。
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        internal static DataSet SelectDataSetFromDataAdapter(this IDbDataAdapter adapter)
        {
            var ds = new DataSet();
            FillDataSet(adapter, ds);
            return ds;
        }

        /// <summary>
        /// 例外が発生した時に、メッセージに実行したSQL文を追加します。
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="ds"></param>
        private static void FillDataSet(IDbDataAdapter adapter, DataSet ds)
        {
            try
            {
                adapter.Fill(ds);
            }
            catch (SystemException e)
            {
                throw new DBHelperException(e, adapter.SelectCommand);
            }
        }
    }
}
