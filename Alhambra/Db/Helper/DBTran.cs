using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Ledsun.Alhambra.Db.Plugin;

namespace Ledsun.Alhambra.Db.Helper
{
    public class DBTran : IDisposable
    {
        public AbstractDBBridge DB{get; private set;}

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
