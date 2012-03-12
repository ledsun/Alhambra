using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Ledsun.Alhambra.Db.Plugin;

namespace Ledsun.Alhambra.Db.Helper
{
    /// <summary>
    /// クライアントクラスで使用するトランザクションクラスです。
    /// トランザクションを開いたAbstractDBBridgeを保持しているのでクライアントは本クラスのインスタンスを持ちまわります。
    /// </summary>
    /// <example>
    /// using(var tr = new DBTran){
    ///     DBHelper.Select("INSERT INTO T_PARENT...", tr);
    ///     DBHelper.Select("INSERT INTO T_CHILD...", tr);
    ///     DB.Commit();
    /// }
    /// </example>
    public class DBTran : IDisposable
    {
        internal AbstractDBBridge DB{get; private set;}

        public DBTran()
        {
            DB = DBFactory.NewDB;
            DB.BeginTransaction();
        }

        public void Commit()
        {
            DB.Commit();
        }

        public void Rollback()
        {
            DB.Rollback();
        }

        public void Dispose()
        {
            DB.Dispose();
        }
    }
}
