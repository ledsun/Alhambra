using System.ComponentModel.Composition;
using System.Data;
using Alhambra.ConfigUtil;
using Alhambra.Db.Plugin;
using Oracle.DataAccess.Client;

namespace Alhambra.Plugin.Oracle
{
    [Export(typeof(AbstractDBBridge))]
    public class DBBridgeForOracle : AbstractDBBridge
    {
        public DBBridgeForOracle() : base() { }

        protected override IDbConnection CreateConnection()
        {
            return new OracleConnection();
        }

        protected override IDbDataAdapter CreateAdapter(string sql, IDbConnection con)
        {
            return new OracleDataAdapter(sql, con as OracleConnection);
        }

        protected override string ConnectionString
        {
            get
            {
                return Config.Value.GetConnectionString("Oracle");
            }
        }
    }
}
