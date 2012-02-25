using System;
using System.Collections.Generic;
using System.Data;

namespace Ledsun.Alhambra.Db
{
    static class DbCommandUtil
    {
        public static int Execute(IDbCommand cmd)
        {
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (SystemException e)
            {

                throw MakeException(e, cmd);
            }
        }

        public static object ExecuteScalar(IDbCommand cmd)
        {
            try
            {
                return cmd.ExecuteScalar();
            }
            catch (SystemException e)
            {
                throw MakeException(e, cmd);
            }
        }

        public static List<DataRowAccessor> SelectFromDataAdapter(IDbDataAdapter adapter)
        {
            return SelectToDataRowList(adapter).ConvertAll<DataRowAccessor>(delegate(DataRow row) { return new DataRowAccessor(row); });
        }

        public static DataSet SelectFromDataAdapterDataSet(IDbDataAdapter adapter)
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
                throw MakeException(e, adapter.SelectCommand);
            }
        }

        private static ApplicationException MakeException(SystemException e, IDbCommand cmd)
        {
            return new ApplicationException(e.Message + "\n" + cmd.CommandText, e);
        }
    }
}
