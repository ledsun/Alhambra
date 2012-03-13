using System;
using System.Data.SqlTypes;
using Ledsun.Alhambra.Db.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlhambraTest
{
    /// <summary>
    ///ToTest のテスト クラスです。
    ///</summary>
    [TestClass()]
    public class ToTest
    {
        #region DateTime
        [TestMethod]
        public void DateTime()
        {
            Assert.AreEqual<DateTime>(System.DateTime.Parse("1/1/1753 12:00:00"), To.DateTime(null));
            Assert.AreEqual<DateTime>(new DateTime(100, 1, 1), To.DateTime("100.1"));
            Assert.AreEqual<DateTime>(new DateTime(2009, 4, 7), To.DateTime(new DateTime(2009, 4, 7)));

            Assert.AreEqual<DateTime>(new DateTime(2009, 4, 7), To.DateTime("2009/04/07 0:00:00"));
            Assert.AreEqual<DateTime>(new DateTime(2009, 4, 7), To.DateTime(new SqlDateTime(2009, 4, 7)));
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void 整数はDateTime変換しない()
        {
            To.DateTime(100);
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void 少数はDateTime変換しない()
        {
            To.DateTime(100.1);
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 整数文字列はDateTime変換しない()
        {
            To.DateTime("100");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 空文字列はDateTime変換しない()
        {
            To.DateTime("");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void アルファベット文字列はDateTime変換しない()
        {
            To.DateTime("ABC");
        }
        #endregion

        #region Decimal
        [TestMethod]
        public void Decimal()
        {
            Assert.AreEqual<decimal>(0, To.Decimal(null));
            Assert.AreEqual<decimal>(100, To.Decimal(100));
            Assert.AreEqual<decimal>(100.1m, To.Decimal(100.1));
            Assert.AreEqual<decimal>(100, To.Decimal("100"));
            Assert.AreEqual<decimal>(100.1m, To.Decimal("100.1"));
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 空文字はDecimal変換しない()
        {
            To.Decimal("");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void アルファベットはDecimalに変換しない()
        {
            To.Decimal("ABC");
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void DateTimeはDecimalに変換しない()
        {
            To.Decimal(new DateTime());
        }
        #endregion Decimal

        #region Double
        [TestMethod]
        public void Double()
        {
            Assert.AreEqual<double>(0, To.Double(null));
            Assert.AreEqual<double>(100, To.Double(100));
            Assert.AreEqual<double>(100.1, To.Double(100.1));
            Assert.AreEqual<double>(100, To.Double("100"));
            Assert.AreEqual<double>(100.1, To.Double("100.1"));
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 空文字はDoubleに変換しない()
        {
            To.Double("");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void アルファベットはDoubleに変換しない()
        {
            To.Double("ABC");
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void DateTimeはDoubleに変換しない()
        {
            To.Double(new DateTime());
        }
        #endregion

        #region UInt
        [TestMethod]
        public void UInt()
        {
            Assert.AreEqual<uint>(0, To.UInt(null));
            Assert.AreEqual<uint>(100, To.UInt(100));
            Assert.AreEqual<uint>(100, To.UInt(100.1));
            Assert.AreEqual<uint>(100, To.UInt("100"));
        }

        [TestMethod, ExpectedException(typeof(OverflowException))]
        public void 負数はUIntに変換しない()
        {
            To.UInt(-100);
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void アルファベットはUIntに変換しない()
        {
            To.UInt("ABC");
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void DateTimeはUIntに変換しない()
        {
            To.UInt(new DateTime());
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 空文字はUIntに変換しない()
        {
            To.UInt("");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 小数点付き文字列はUIntに変換しない()
        {
            To.UInt("100.1");
        }
        #endregion

        #region Int
        [TestMethod]
        public void Int()
        {
            Assert.AreEqual<int>(0, To.Int(null));
            Assert.AreEqual<int>(100, To.Int(100));
            Assert.AreEqual<int>(100, To.Int(100.1));
            Assert.AreEqual<int>(100, To.Int("100"));
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void アルファベットはIntに変換しない()
        {
            To.Int("ABC");
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void DateTimeはIntに変換しない()
        {
            To.Int(new DateTime());
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 空文字はIntに変換しない()
        {
            To.Int("");
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 小数点付き文字列はIntに変換しない()
        {
            To.Int("100.1");
        }
        #endregion

        [TestMethod]
        public void String()
        {
            Assert.AreEqual<string>("", To.String(null));
            Assert.AreEqual<string>("100", To.String(100));
            Assert.AreEqual<string>("100.1",To.String(100.1));
            Assert.AreEqual<string>("100", To.String("100"));
            Assert.AreEqual<string>("100.1",To.String("100.1"));
            Assert.AreEqual<string>("", To.String(""));
            Assert.AreEqual<string>("ABC", To.String("ABC"));
            Assert.AreEqual<string>("2009/04/07 0:00:00",To.String(new DateTime(2009, 4, 7)));
        }

        [TestMethod]
        public void Bool()
        {
            Assert.AreEqual<bool>(false, To.Bool(null));
            Assert.AreEqual<bool>(true, To.Bool(-1));
            Assert.AreEqual<bool>(false ,To.Bool(0));
            Assert.AreEqual<bool>(true, To.Bool(1));
            Assert.AreEqual<bool>(true, To.Bool(100));
            Assert.AreEqual<bool>(true, To.Bool("TRue"));
            Assert.AreEqual<bool>(false, To.Bool("falSe"));
            Assert.AreEqual<bool>(true, To.Bool(true));
            Assert.AreEqual<bool>(false, To.Bool(false));
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void 真理値変換できない文字列()
        {
            To.Bool("x");
        }
    }
}
