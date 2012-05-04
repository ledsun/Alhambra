using System;
using System.Collections.Generic;
using System.Data;
using Alhambra.ConfigUtil;
using Alhambra.Db.Data;
using Alhambra.Db.Helper;

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
        private readonly IDbConnection _connection;
        private IDbTransaction _trans = null;
        protected readonly IDbCommand _cmd;
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
        abstract protected IDbDataAdapter CreateAdapter(string sql, IDbConnection con);

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

        #region SQL実行
        /// <summary>
        /// 結果を返さないSQLを実行します
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareCommand(sql).Execute();
        }

        /// <summary>
        /// 値を一つ返すSQLを実行します
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public TypeConvertableWrapper SelectOne(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareCommand(sql).SelectOne();
        }

        /// <summary>
        /// SELECTを実行します。
        /// 結果はDataRowAccessorのリストで返します。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<DataRowAccessor> Select(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareDataAdapter(sql).SelectFromDataAdapter();
        }

        /// <summary>
        /// SELECTを実行します。
        /// 結果はDataSetで返します。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet SelectDataSet(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            return PrepareDataAdapter(sql).SelectDataSetFromDataAdapter();
        }
        #endregion

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

        #region プライベートメソッド
        private IDbCommand PrepareCommand(string sql)
        {
            _cmd.CommandText = sql;
            _cmd.CommandTimeout = _timeout;
            return _cmd;
        }

        private IDbDataAdapter PrepareDataAdapter(string sql)
        {
            var dataAdapter = CreateAdapter(sql, _connection);
            dataAdapter.SelectCommand.CommandTimeout = _timeout;
            dataAdapter.SelectCommand.Transaction = _trans;
            return dataAdapter;
        }
        #endregion


    }
}
