using System;
using Alhambra.Db.Helper;
using Alhambra.Db.Plugin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlhambraTest
{
     /// <summary>
    ///DBTranのテストクラスです。
    ///</summary>
    [TestClass()]
    public class OleDBTest
    {
        [TestMethod]
        public void OleDB用プラグイン読込テスト()
        {
            using (var tr = new DBTran())
            {
                DBHelper.Select("SELECT 1", tr);
                tr.Commit();
            }
        }
    }
}
