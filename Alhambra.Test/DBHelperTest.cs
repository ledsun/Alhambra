using System;
using System.Collections.Generic;
using System.Linq;
using Ledsun.Alhambra.Db.Data;
using Ledsun.Alhambra.Db.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlhambraTest
{
    /// <summary>
    ///DBHelperTest のテスト クラスです。すべての
    ///DBHelperTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class DBHelperTest
    {
        [TestMethod()]
        public void Selectのテスト()
        {
            var target = DBHelper.Select("SELECT 1");
            Assert.AreEqual<String>("1", target.First()[0]);
            Assert.AreEqual<String>("1", target.ToList()[0][0]);
        }

        [TestMethod()]
        public void 取得行数が0()
        {
            var target = DBHelper.Select("SELECT 1 WHERE 1<>1");
            Assert.AreEqual<int>(0, target.Count());
        }
    }
}
