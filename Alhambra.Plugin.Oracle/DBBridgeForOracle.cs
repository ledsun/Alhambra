using Alhambra.Db.Plugin;
using Alhambra.Plugin.ConfigUtil;
using Oracle.DataAccess.Client;
using System.ComponentModel.Composition;
using System.Data;

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

        public override IDbDataAdapter CreateAdapter(string sql, IDbConnection con)
        {
            return new OracleDataAdapter(sql, con as OracleConnection);
        }

        public override string PluginName
        {
            get { return "Oracle"; }
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
