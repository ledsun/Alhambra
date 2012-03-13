using Ledsun.Alhambra.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AlhambraTest
{
    /// <summary>
    ///SqlStatementのテストクラス
    ///</summary>
    [TestClass()]
    public class SqlStatementTest
    {
        [TestMethod]
        public void 配列の置換()
        {
            string[] moto = { "aaa", "bbb", "ccc" };
            Assert.AreEqual<string>("WHERE HAGE IN ('aaa','bbb','ccc')", new SqlStatement("WHERE HAGE IN (@HAGES@)").Replace("HAGES", moto));

            long[] moto1 = { 1, 2, 3 };
            Assert.AreEqual<string>("WHERE HAGE IN (1,2,3)", new SqlStatement("WHERE HAGE IN (@HAGES@)").Replace("HAGES", moto1));
        }

        [TestMethod]
        public void ToStringのオーバーライド()
        {
            Assert.AreEqual<string>("SELECT 1", new SqlStatement("SELECT 1").ToString());
            //暗黙的な文字列変換と同じ結果を返します。
            Assert.AreEqual<string>(new SqlStatement("SELECT 1"),new SqlStatement("SELECT 1").ToString());
        }
    }
}
