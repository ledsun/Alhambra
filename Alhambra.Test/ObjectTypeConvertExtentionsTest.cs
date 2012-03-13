using System;
using System.Data.SqlTypes;
using Ledsun.Alhambra.Db.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlhambraTest
{
    /// <summary>
    ///ObjectTypeConvertExtentionsのテストクラスです。
    ///</summary>
    [TestClass()]
    public class ObjectTypeConvertExtentionsTest
    {
        #region DateTime
        [TestMethod]
        public void DateTime()
        {
            Assert.AreEqual<DateTime>(System.DateTime.Parse("1/1/1753 12:00:00"), ObjectTypeConvertExtentions.DateTime(null));
            Assert.AreEqual<DateTime>(new DateTime(100, 1, 1), ObjectTypeConvertExtentions.DateTime("100.1"));
            Assert.AreEqual<DateTime>(new DateTime(2009, 4, 7), ObjectTypeConvertExtentions.DateTime(new DateTime(2009, 4, 7)));

            Assert.AreEqual<DateTime>(new DateTime(2009, 4, 7), ObjectTypeConvertExtentions.DateTime("2009/04/07 0:00:00"));
            Assert.AreEqual<DateTime>(new DateTime(2009, 4, 7), ObjectTypeConvertExtentions.DateTime(new SqlDateTime(2009, 4, 7)));
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void 整数はDateTime変換しない()
        {
            ObjectTypeConvertExtentions.DateTime(100);
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void 少数はDateTime変換しない()
        {
            ObjectTypeConvertExtentions.DateTime(100.1);
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 整数文字列はDateTime変換しない()
        {
            ObjectTypeConvertExtentions.DateTime("100");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 空文字列はDateTime変換しない()
        {
            ObjectTypeConvertExtentions.DateTime("");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void アルファベット文字列はDateTime変換しない()
        {
            ObjectTypeConvertExtentions.DateTime("ABC");
        }
        #endregion

        #region Decimal
        [TestMethod]
        public void Decimal()
        {
            Assert.AreEqual<decimal>(0, ObjectTypeConvertExtentions.Decimal(null));
            Assert.AreEqual<decimal>(100, ObjectTypeConvertExtentions.Decimal(100));
            Assert.AreEqual<decimal>(100.1m, ObjectTypeConvertExtentions.Decimal(100.1));
            Assert.AreEqual<decimal>(100, ObjectTypeConvertExtentions.Decimal("100"));
            Assert.AreEqual<decimal>(100.1m, ObjectTypeConvertExtentions.Decimal("100.1"));
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 空文字はDecimal変換しない()
        {
            ObjectTypeConvertExtentions.Decimal("");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void アルファベットはDecimalに変換しない()
        {
            ObjectTypeConvertExtentions.Decimal("ABC");
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void DateTimeはDecimalに変換しない()
        {
            ObjectTypeConvertExtentions.Decimal(new DateTime());
        }
        #endregion Decimal

        #region Double
        [TestMethod]
        public void Double()
        {
            Assert.AreEqual<double>(0, ObjectTypeConvertExtentions.Double(null));
            Assert.AreEqual<double>(100, ObjectTypeConvertExtentions.Double(100));
            Assert.AreEqual<double>(100.1, ObjectTypeConvertExtentions.Double(100.1));
            Assert.AreEqual<double>(100, ObjectTypeConvertExtentions.Double("100"));
            Assert.AreEqual<double>(100.1, ObjectTypeConvertExtentions.Double("100.1"));
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 空文字はDoubleに変換しない()
        {
            ObjectTypeConvertExtentions.Double("");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void アルファベットはDoubleに変換しない()
        {
            ObjectTypeConvertExtentions.Double("ABC");
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void DateTimeはDoubleに変換しない()
        {
            ObjectTypeConvertExtentions.Double(new DateTime());
        }
        #endregion

        #region UInt
        [TestMethod]
        public void UInt()
        {
            Assert.AreEqual<uint>(0, ObjectTypeConvertExtentions.UInt(null));
            Assert.AreEqual<uint>(100, ObjectTypeConvertExtentions.UInt(100));
            Assert.AreEqual<uint>(100, ObjectTypeConvertExtentions.UInt(100.1));
            Assert.AreEqual<uint>(100, ObjectTypeConvertExtentions.UInt("100"));
        }

        [TestMethod, ExpectedException(typeof(OverflowException))]
        public void 負数はUIntに変換しない()
        {
            ObjectTypeConvertExtentions.UInt(-100);
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void アルファベットはUIntに変換しない()
        {
            ObjectTypeConvertExtentions.UInt("ABC");
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void DateTimeはUIntに変換しない()
        {
            ObjectTypeConvertExtentions.UInt(new DateTime());
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 空文字はUIntに変換しない()
        {
            ObjectTypeConvertExtentions.UInt("");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 小数点付き文字列はUIntに変換しない()
        {
            ObjectTypeConvertExtentions.UInt("100.1");
        }
        #endregion

        #region Int
        [TestMethod]
        public void Int()
        {
            Assert.AreEqual<int>(0, ObjectTypeConvertExtentions.Int(null));
            Assert.AreEqual<int>(100, ObjectTypeConvertExtentions.Int(100));
            Assert.AreEqual<int>(100, ObjectTypeConvertExtentions.Int(100.1));
            Assert.AreEqual<int>(100, ObjectTypeConvertExtentions.Int("100"));
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void アルファベットはIntに変換しない()
        {
            ObjectTypeConvertExtentions.Int("ABC");
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void DateTimeはIntに変換しない()
        {
            ObjectTypeConvertExtentions.Int(new DateTime());
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 空文字はIntに変換しない()
        {
            ObjectTypeConvertExtentions.Int("");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 小数点付き文字列はIntに変換しない()
        {
            ObjectTypeConvertExtentions.Int("100.1");
        }
        #endregion

        [TestMethod]
        public void String()
        {
            Assert.AreEqual<string>("", ObjectTypeConvertExtentions.String(null));
            Assert.AreEqual<string>("100", ObjectTypeConvertExtentions.String(100));
            Assert.AreEqual<string>("100.1",ObjectTypeConvertExtentions.String(100.1));
            Assert.AreEqual<string>("100", ObjectTypeConvertExtentions.String("100"));
            Assert.AreEqual<string>("100.1",ObjectTypeConvertExtentions.String("100.1"));
            Assert.AreEqual<string>("", ObjectTypeConvertExtentions.String(""));
            Assert.AreEqual<string>("ABC", ObjectTypeConvertExtentions.String("ABC"));
            Assert.AreEqual<string>("2009/04/07 0:00:00",ObjectTypeConvertExtentions.String(new DateTime(2009, 4, 7)));
        }

        [TestMethod]
        public void Bool()
        {
            Assert.AreEqual<bool>(false, ObjectTypeConvertExtentions.Bool(null));
            Assert.AreEqual<bool>(true, ObjectTypeConvertExtentions.Bool(-1));
            Assert.AreEqual<bool>(false ,ObjectTypeConvertExtentions.Bool(0));
            Assert.AreEqual<bool>(true, ObjectTypeConvertExtentions.Bool(1));
            Assert.AreEqual<bool>(true, ObjectTypeConvertExtentions.Bool(100));
            Assert.AreEqual<bool>(true, ObjectTypeConvertExtentions.Bool("TRue"));
            Assert.AreEqual<bool>(false, ObjectTypeConvertExtentions.Bool("falSe"));
            Assert.AreEqual<bool>(true, ObjectTypeConvertExtentions.Bool(true));
            Assert.AreEqual<bool>(false, ObjectTypeConvertExtentions.Bool(false));
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 真理値変換できない文字列()
        {
            ObjectTypeConvertExtentions.Bool("x");
        }
    }
}
