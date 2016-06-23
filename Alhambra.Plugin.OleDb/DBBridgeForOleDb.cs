using Alhambra.Db.Plugin;
using Alhambra.Plugin.ConfigUtil;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.OleDb;

namespace Alhambra.Plugin.OleDb
{
    [Export(typeof(AbstractDBBridge))]
    public class DBBridgeForOleDb : AbstractDBBridge
    {
        public DBBridgeForOleDb() : base() { }

        protected override IDbConnection CreateConnection()
        {
            return new OleDbConnection();
        }

        public override IDbDataAdapter CreateAdapter(string sql, IDbConnection con)
        {
            return new OleDbDataAdapter(sql, con as OleDbConnection);
        }

        public override string PluginName
        {
            get { return "OleDb"; }
        }

        protected override string ConnectionString
        {
            get
            {
                return Config.Value.GetConnectionString("OleDb");
            }
        }
    }
}
