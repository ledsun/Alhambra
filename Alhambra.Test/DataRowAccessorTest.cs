using System;
using System.Data;
using Alhambra.Db;
using Alhambra.Db.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlhambraTest
{
    /// <summary>
    /// DataRowAccessorのテスト
    /// </summary>
    [TestClass]
    public class DataRowAccessorTest
    {
        private DataRowAccessor _dra;

        #region 追加のテスト属性       
        [TestInitialize()]
        public void MyTestInitialize()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("VALUE", typeof(string));

            DataRow dataRow = table.NewRow();
            dataRow["ID"] = 100;
            dataRow["VALUE"] = "ABC";

            _dra = new DataRowAccessor(dataRow);
        }
  
        #endregion

        [TestMethod]
        public void DataRowオブジェクトを引数にして生成します()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("VALUE", typeof(string));

            DataRow dataRow = table.NewRow();
            dataRow["ID"] = 100;
            dataRow["VALUE"] = "ABC";

            var target = new DataRowAccessor(dataRow);

            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(DataRowAccessor));
        }

        [TestMethod]
        public void Columnsプロパティからカラム名の一覧を取得できます()
        {
            DataColumnCollection actual = _dra.Columns;
            Assert.AreEqual<string>("ID", _dra.Columns[0].ColumnName);
            Assert.AreEqual<string>("VALUE", _dra.Columns[1].ColumnName);
        }

        [TestMethod]
        public void カラムの名前を指定して要素を取得できます()
        {
            var target = _dra["ID"];

            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(TypeConvertableWrapper));
            Assert.AreEqual<int>(100, target.Int);
        }

        [TestMethod]
        public void カラムのインデックスを指定して要素を取得できます()
        {
            var target = _dra[0];

            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(TypeConvertableWrapper));
            Assert.AreEqual<int>(100, target.Int);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void カラム名が間違っていると例外を上げます()
        {
            var target = _dra["NO_COLUMN"];
        }

        [TestMethod, ExpectedException(typeof(IndexOutOfRangeException))]
        public void インデックスが間違っていると例外を上げます()
        {
            var target = _dra[2];
        }
    }
}
