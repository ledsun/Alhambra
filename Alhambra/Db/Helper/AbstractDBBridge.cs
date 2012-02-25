using System;
using System.Collections.Generic;
using System.Data;

namespace Ledsun.Alhambra.Db
{
    public abstract class AbstractDBBridge : IDisposable
    {
        private const string SQL_SHOULD_NOT_NULL_OR_EMPTY = "SQLÇ…ÉkÉãÇ‹ÇΩÇÕãÛï∂éöÇÕéwíËÇ≈Ç´Ç‹ÇπÇÒÅB";

        private readonly int _timeout;
        private readonly IDbConnection _connection;
        private IDbTransaction _trans = null;
        protected readonly IDbCommand _cmd;
        private bool _isNotTrasactionComplete = true;

        abstract protected IDbConnection CreateConnection();
        abstract protected IDbDataAdapter CreateAdapter(string sql, IDbConnection con);

        public AbstractDBBridge(string connectionString, int timeout)
        {
            _timeout = timeout;
            _connection = CreateConnection();
            _connection.ConnectionString = connectionString;
            _connection.Open();
            _cmd = _connection.CreateCommand();
        }

        public int Execute(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            _cmd.CommandText = sql;
            _cmd.CommandTimeout = _timeout;
            return DbCommandUtil.Execute(_cmd);
        }

        public TypeConvertableWrapper SelectOne(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            _cmd.CommandText = sql;
            _cmd.CommandTimeout = _timeout;
            return new TypeConvertableWrapper(DbCommandUtil.ExecuteScalar(_cmd));
        }

        public List<DataRowAccessor> Select(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            var dataAdapter = CreateAdapter(sql, _connection);
            SetCommadParameter(dataAdapter);
            return DbCommandUtil.SelectFromDataAdapter(dataAdapter);
        }

        public DataSet SelectDataSet(string sql)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentException(SQL_SHOULD_NOT_NULL_OR_EMPTY);

            var dataAdapter = CreateAdapter(sql, _connection);
            SetCommadParameter(dataAdapter);
            return DbCommandUtil.SelectFromDataAdapterDataSet(dataAdapter);
        }

        private void SetCommadParameter(IDbDataAdapter dataAdapter)
        {
            dataAdapter.SelectCommand.CommandTimeout = _timeout;
            dataAdapter.SelectCommand.Transaction = _trans;
        }

        public void BeginTransaction()
        {
            _trans = _connection.BeginTransaction();
            _cmd.Transaction = _trans;
        }

        public void Commit()
        {
            _trans.Commit();
            _isNotTrasactionComplete = false;
        }

        public void Rollback()
        {
            _trans.Rollback();
            _isNotTrasactionComplete = false;
        }

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
    }
}
