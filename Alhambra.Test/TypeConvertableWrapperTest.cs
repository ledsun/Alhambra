using System;
using Alhambra.Db;
using Alhambra.Db.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlhambraTest
{
    /// <summary>
    ///TypeConvertableWrapperのテスト
    ///</summary>
    [TestClass()]
    public class TypeConvertableWrapperTest
    {
        TypeConvertableWrapper _BoolData;
        TypeConvertableWrapper _ByteData;
        TypeConvertableWrapper _DateTimeData;
        TypeConvertableWrapper _DateTimeNullData;
        TypeConvertableWrapper _DecimalData;
        TypeConvertableWrapper _DecimalNullData;
        TypeConvertableWrapper _DoubleData;
        TypeConvertableWrapper _IntData;
        TypeConvertableWrapper _Int16Data;
        TypeConvertableWrapper _Int64Data;
        TypeConvertableWrapper _StringData;
        TypeConvertableWrapper _UIntData;

        #region 追加のテスト属性
        [TestInitialize()]
        public void MyTestInitialize()
        {
            _BoolData = new TypeConvertableWrapper(false);
            _ByteData = new TypeConvertableWrapper(Byte.Parse("255"));
            _DateTimeData = new TypeConvertableWrapper(new DateTime(2009, 4, 7));
            _DateTimeNullData = new TypeConvertableWrapper(null);
            _DecimalData = new TypeConvertableWrapper(new Decimal(123.456));
            _DecimalNullData = new TypeConvertableWrapper(null);
            _DoubleData = new TypeConvertableWrapper(456.789d);
            _IntData = new TypeConvertableWrapper(-100);
            _Int16Data = new TypeConvertableWrapper(30000);
            _Int64Data = new TypeConvertableWrapper(4000000000);
            _StringData = new TypeConvertableWrapper("ABC");
            _UIntData = new TypeConvertableWrapper(100);
        }
        #endregion


        [TestMethod]
        public void オブジェクトを引数にして生成します()
        {
            var target = new TypeConvertableWrapper(new Object());
            Assert.IsNotNull(target);
            Assert.IsInstanceOfType(target, typeof(TypeConvertableWrapper));
        }

        [TestMethod]
        public void ToStringメソッドで値の文字列表現を取得できます()
        {
            Assert.AreEqual<string>("False", _BoolData.ToString());
        }

        [TestMethod]
        public void 値の文字列は暗黙変換でも取得できます()
        {
            Assert.AreEqual<string>("False", _BoolData);
        }

        [TestMethod]
        public void プロパティ名で値の型を指定して変換できます()
        {
            Assert.AreEqual<bool>(false, _BoolData.Bool);
            Assert.AreEqual<byte>(Byte.Parse("255"), _ByteData.Byte);
            Assert.AreEqual<DateTime>(new DateTime(2009, 4, 7), _DateTimeData.DateTime);
            Assert.AreEqual<DateTime?>(null, _DateTimeNullData.DateTimeNull);
            Assert.AreEqual<decimal>(123.456m, _DecimalData.Decimal);
            Assert.AreEqual<decimal?>(null, _DecimalNullData.DecimalNull);
            Assert.AreEqual<double>(456.789d, _DoubleData.Double);
            Assert.AreEqual<int>(-100, _IntData.Int);
            Assert.AreEqual<short>(30000, _Int16Data.Int16);
            Assert.AreEqual<long>(4000000000L, _Int64Data.Int64);
            Assert.AreEqual<string>("ABC", _StringData.String);
            Assert.AreEqual<uint>(100, _UIntData.UInt);
        }
    }
}
