using System;
using System.Data;

using NUnit.Framework;

namespace Ledsun.Alhambra.Db
{
    /// <summary>
    /// DataRowオブジェクトの各列の値をTypeConvertableWrapperに変換して取得するラッパー。
    /// </summary>
    public class DataRowAccessor
    {
        private readonly DataRow _row;

        public DataRowAccessor(DataRow row)
        {
            _row = row;
        }

        public DataColumnCollection Columns
        {
            get
            {
                return _row.Table.Columns;
            }
        }

        public TypeConvertableWrapper this[string key]
        {
            get
            {
                return new TypeConvertableWrapper(_row[key]);
            }
        }

        public TypeConvertableWrapper this[int key]
        {
            get
            {
                return new TypeConvertableWrapper(_row[key]);
            }
        }

        #region テスト
        [TestFixture]
        public class Test
        {
            private DataRowAccessor _dra;

            [Test]
            public void カラム名を使ったインデックスアクセス()
            {
                //TypeConvertableWrapperが帰ってくるのでプロパティによる型変換が可能です。
                Assert.That(_dra["Int"].Int, Is.EqualTo(100));
                Assert.That(_dra["String"].String, Is.EqualTo("ABC"));
                Assert.That(_dra["Decimal"].Decimal, Is.EqualTo(123.456));
                Assert.That(_dra["Datetime"].DateTime, Is.EqualTo(new DateTime(2009, 4, 7)));
                Assert.That(_dra["Double"].Double, Is.EqualTo(100.000000009));
            }

            [SetUp]
            public void SetUp()
            {
                DataTable table = new DataTable();
                table.Columns.Add("Int", typeof(int));
                table.Columns.Add("String", typeof(string));
                table.Columns.Add("Decimal", typeof(decimal));
                table.Columns.Add("Datetime", typeof(DateTime));
                table.Columns.Add("Double", typeof(Double));

                DataRow dataRow = table.NewRow();
                dataRow["Int"] = 100;
                dataRow["String"] = "ABC";
                dataRow["Decimal"] = 123.456;
                dataRow["Datetime"] = new DateTime(2009, 4, 7);
                dataRow["Double"] = 100.000000009;

                _dra = new DataRowAccessor(dataRow);
            }
        }
        #endregion
    }
}