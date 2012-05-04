using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Alhambra;
using Alhambra.Db.Data;
using Alhambra.Db.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlhambraTest
{
    /// <summary>
    ///DBHelperのテストクラスです。
    ///</summary>
    [TestClass()]
    public class DBHelperTest
    {
        [TestMethod]
        public void Selectのテスト()
        {
            var target = DBHelper.Select("SELECT 1");
            Assert.AreEqual<String>("1", target.First()[0]);
            Assert.AreEqual<String>("1", target.ToList()[0][0]);
        }

        [TestMethod]
        public void 取得行数が0()
        {
            var target = DBHelper.Select("SELECT 1 WHERE 1<>1");
            Assert.AreEqual<int>(0, target.Count());
        }

        [TestMethod]
        public void SelectDataSet()
        {
            var ds = DBHelper.SelectDataSet("SELECT 3");
            Assert.AreEqual<int>(3, (int)ds.Tables[0].Rows[0][0]);
        }

        [TestMethod]
        public void SelectOneでDBNullの時は0が返る()
        {
            var t = DBHelper.SelectOne("SELECT ID FROM ( SELECT 1 ID ) A WHERE ID = 0");
            Assert.AreEqual<int>(0, t.Int);
        }

        [TestMethod, ExpectedException(typeof(DBHelperException))]
        public void SelectOneで例外がおきたときはDBHelperExceptionが返る()
        {
            DBHelper.SelectOne("x");
        }

        #region 空文字またはnullの入力は禁止
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void EmptySelect()
        {
            DBHelper.Select("");
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void NullSelect()
        {
            DBHelper.Select(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void EmptySelectOne()
        {
            DBHelper.SelectOne("");
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void NullSelectOne()
        {
            DBHelper.SelectOne(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void EmptySelectDataSet()
        {
            DBHelper.SelectDataSet("");
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void NullSelectDataSet()
        {
            DBHelper.SelectDataSet(null);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void EmptyExecute()
        {
            DBHelper.Execute("");
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void NullExecute()
        {
            DBHelper.Execute(null);
        }
        #endregion
    }
}
