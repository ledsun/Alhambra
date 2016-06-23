using System;
using System.Data;
using Alhambra.ConfigUtil;

namespace Alhambra.Db.Plugin
{
    /// <summary>
    /// DBに対する操作を定義したクラスです。
    /// DBごと（SQLServerとかOracleとか）に実装クラスを作成します。
    /// </summary>
    public abstract class AbstractDBBridge : IDisposable
    {
        private const string SQL_SHOULD_NOT_NULL_OR_EMPTY = "SQLにヌルまたは空文字は指定できません。";

        private readonly int _timeout;
        public int Timeout { get { return _timeout; } }

        private readonly IDbConnection _connection;
        public IDbConnection Connection { get { return _connection; } }

        private IDbTransaction _trans = null;
        public IDbTransaction Trans { get { return _trans; } }

        protected readonly IDbCommand _cmd;
        public IDbCommand Cmd {  get { return _cmd; } }

        private bool _isNotTrasactionComplete = true;

        /// <summary>
        /// DBへのコネクションを作成します。
        /// </summary>
        /// <returns></returns>
        abstract protected IDbConnection CreateConnection();

        /// <summary>
        /// DBとのDataAdapterを作成します。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        abstract public IDbDataAdapter CreateAdapter(string sql, IDbConnection con);

        /// <summary>
        /// プラグイン名を返します。
        /// </summary>
        abstract public string PluginName { get; }

        /// <summary>
        /// DBへの接続文字列を返します。
        /// </summary>
        abstract protected string ConnectionString { get; }

        /// <summary>
        /// コンストラクタ。
        /// 実装クラスから呼び出す。
        /// </summary>
        protected AbstractDBBridge()
        {
            _timeout = Config.Value.SqlCommandTimeout;
            _connection = CreateConnection();
            _connection.ConnectionString = ConnectionString;
            _connection.Open();
            _cmd = _connection.CreateCommand();
        }

        /// <summary>
        /// トランザクションとコネクションを片付けます。
        /// トランザクションがコミットされていなければロールバックします。
        /// </summary>
        public void Dispose()
        {
            _cmd.Dispose();
            if (_trans != null)
            {
                if (_isNotTrasactionComplete) Rollback();
                _trans.Dispose();
            }
            _connection.Dispose();
        }

        #region トランザクション操作
        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void BeginTransaction()
        {
            _trans = _connection.BeginTransaction();
            _cmd.Transaction = _trans;
        }

        /// <summary>
        /// コミット
        /// </summary>
        public void Commit()
        {
            _trans.Commit();
            _isNotTrasactionComplete = false;
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        public void Rollback()
        {
            _trans.Rollback();
            _isNotTrasactionComplete = false;
        }
        #endregion
    }
}
