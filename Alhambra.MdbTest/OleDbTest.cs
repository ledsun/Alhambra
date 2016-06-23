using System;
using Alhambra.Db.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Alhambra.Db;

namespace AlhambraTest
{
    /// <summary>
    ///OleDbのテストクラスです。
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

        [TestMethod]
        public void 日付入力のテスト()
        {
            var justNow = DateTime.Now;
            DBHelper.Execute(new SqlStatement(@"INSERT INTO DateTimeTest (TEST_DATE) VALUES (@DATE)").Replace("DATE", justNow));
            Assert.AreEqual<string>(justNow.ToString(SqlStatement.SQL_DATETIME_FORMAT), DBHelper.SelectOne("SELECT TEST_DATE FROM DateTimeTest WHERE ID = 1"));
        }

        [TestMethod]
        public void マルチバイト文字列のプレフィックスにNがつかないテスト()
        {
            Assert.AreEqual<string>("\'ABC\'", new SqlStatement("@STR").Replace("STR", "ABC"));
        }
    }
}
