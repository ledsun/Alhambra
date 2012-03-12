using System;
using System.Collections.Generic;
using System.Data;
using Ledsun.Alhambra.Db.Data;

namespace Ledsun.Alhambra.Db.Helper
{
   static class DBAdapterExtentions
    {
        internal static List<DataRowAccessor> SelectFromDataAdapter(this IDbDataAdapter adapter)
        {
            return SelectToDataRowList(adapter).ConvertAll<DataRowAccessor>(delegate(DataRow row) { return new DataRowAccessor(row); });
        }

        internal static DataSet SelectFromDataAdapterDataSet(this IDbDataAdapter adapter)
        {
            DataSet ds = new DataSet();
            FillDataSet(adapter, ds);
            return ds;
        }

        //Select結果をDataRowのリストで返却します。
        //複数テーブルは未対応です。
        private static List<DataRow> SelectToDataRowList(IDbDataAdapter adapter)
        {
            using (DataSet ds = new DataSet())
            {
                List<DataRow> ret = new List<DataRow>();

                if (0 >= FillDataSet(adapter, ds))
                {
                    return ret;
                }

                //DataRowCollectionをListに詰め替える。
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ret.Add(row);
                }
                return ret;
            }
        }

        private static int FillDataSet(IDbDataAdapter adapter, DataSet ds)
        {
            try
            {
                return adapter.Fill(ds);
            }
            catch (SystemException e)
            {
                throw new DBHelperException(e, adapter.SelectCommand);
            }
        }
    }
}
