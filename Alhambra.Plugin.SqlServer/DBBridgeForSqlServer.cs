using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using Alhambra.ConfigUtil;
using Alhambra.Db.Plugin;

namespace Alhambra.Plugin.SqlServer
{
    [Export(typeof(AbstractDBBridge))]
    public class DBBridgeForSqlServer : AbstractDBBridge
    {
        public DBBridgeForSqlServer() : base() { }

        protected override IDbConnection CreateConnection()
        {
            return new SqlConnection();
        }

        protected override IDbDataAdapter CreateAdapter(string sql, IDbConnection con)
        {
            return new SqlDataAdapter(sql, con as SqlConnection);
        }

        public override string PluginName
        {
            get { return "SqlServer"; }
        }
        
        protected override string ConnectionString
        {
            get
            {
                return Config.Value.GetConnectionString("SqlServer");
            }
        }
    }
}
