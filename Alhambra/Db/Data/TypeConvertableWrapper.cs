using System;
using System.Data.SqlTypes;
using System.Globalization;
using NUnit.Framework;

namespace Ledsun.Alhambra.Db
{
    /// <summary>
    /// 型変換機能を持つヘルパークラス
    /// DBから取得した値の型変換に使います。
    /// </summary>
    /// <example>
    /// 型名のプロパティで当該型で取得できます。
    ///     new TypeConvertableWrapper("1").Int
    /// String型へは暗黙型変換が可能です。
    ///     string hoge = new TypeConvertableWrapper("1");
    /// </example>
    public class TypeConvertableWrapper
    {
        private readonly Object _rawData;

        public TypeConvertableWrapper(object rawData)
        {
            _rawData = rawData;
        }

        //明示的な型を指定したプロパティ
        public uint UInt { get { return To.UInt(_rawData); } }
        public int Int { get { return To.Int(_rawData); } }
        public Int16 Int16 { get { return To.Int16(_rawData); } }
        public Int64 Int64 { get { return To.Int64(_rawData); } }
        public Byte Byte { get { return To.Byte(_rawData); } }
        public string String { get { return To.String(_rawData); } }
        public decimal Decimal { get { return To.Decimal(_rawData); } }
        public DateTime DateTime { get { return To.DateTime(_rawData); } }
        public double Double { get { return To.Double(_rawData); } }

        /// <summary>
        /// 0はFalseそれ以外の数字はTrue
        /// 文字列の場合はTrue、Falseは変換可能（大文字小文字を区別しない）。それ以外は例外を出す。
        /// </summary>
        public bool Bool { get { return To.Bool(_rawData); } }

        //Stringに関しては暗黙の型変換が可能
        public static implicit operator string(TypeConvertableWrapper value)
        {
            return value.String;
        }

        public override string ToString()
        {
            return String;
        }

        #region Test
        [TestFixture]
        public class Test
        {
            TypeConvertableWrapper _DateTimeData;
            TypeConvertableWrapper _DecimalData;
            TypeConvertableWrapper _DoubleData;
            TypeConvertableWrapper _IntData;
            TypeConvertableWrapper _StringData;
            TypeConvertableWrapper _UIntData;

            [SetUp]
            public void SetUp()
            {
                _DateTimeData = new TypeConvertableWrapper(new DateTime(2009, 4, 7));
                _DecimalData = new TypeConvertableWrapper(new Decimal(123.456));
                _DoubleData = new TypeConvertableWrapper(456.789d);
                _IntData = new TypeConvertableWrapper(-100);
                _StringData = new TypeConvertableWrapper("ABC");
                _UIntData = new TypeConvertableWrapper(100);
            }

            [Test]
            public void プロパティによる型変換()
            {
                Assert.That(_DateTimeData.DateTime, Is.EqualTo(new DateTime(2009, 4, 7)));
                Assert.That(_DecimalData.Decimal, Is.EqualTo(123.456));
                Assert.That(_DoubleData.Double, Is.EqualTo(456.789));
                Assert.That(_IntData.Int, Is.EqualTo(-100));
                Assert.That(_StringData.String, Is.EqualTo("ABC"));
                Assert.That(_UIntData.UInt, Is.EqualTo(100));
            }

            [Test]
            public void ToStringメソッドをオーバーライドしています()
            {
                Assert.That(_DateTimeData.ToString(), Is.EqualTo("2009/04/07 0:00:00"));
                Assert.That(_DecimalData.ToString(), Is.EqualTo("123.456"));
                Assert.That(_DoubleData.ToString(), Is.EqualTo("456.789"));
                Assert.That(_StringData.ToString(), Is.EqualTo("ABC"));
            }
        }
        #endregion

        #region 型変換をする内部クラス
        //staicクラスですがテストが複雑なため、privateメソッドにせずprivateクラスとしてまとめて分離します。
        private static class To
        {
            /// <summary>
            /// 値を日付に変換する
            /// </summary>
            /// <param name="val">変換元の値</param>
            /// <returns>nullの場合は0相当の日付値、それ以外は変換された値</returns>
            internal static DateTime DateTime(object val)
            {
                return IsNull(val) ? System.DateTime.Parse("1/1/1753 12:00:00")//sqlserver compact の制限（1/1/1753 12:00:00 AM から 12/31/9999 11:59:59 PM までの間でなければなりません。）
                    : val is SqlDateTime ? ((SqlDateTime)val).Value
                    : Convert.ToDateTime(val);
            }

            /// <summary>
            /// 値を数値に変換する
            /// </summary>
            /// <param name="val">変換元の値</param>
            /// <returns>nullの場合は0、それ以外は変換された数値</returns>
            internal static Decimal Decimal(object val)
            {
                return IsNull(val) ? 0 : Convert.ToDecimal(val, NumberFormatInfo.CurrentInfo);
            }

            /// <summary>
            /// 値を浮動少数値に変換する
            /// </summary>
            /// <param name="val">変換元の値</param>
            /// <returns>nullの場合は0、それ以外は変換された値</returns>
            internal static double Double(object val)
            {
                return IsNull(val) ? 0 : Convert.ToDouble(val);
            }

            /// <summary>
            /// 値を正数に変換する
            /// </summary>
            /// <param name="val">変換元の値</param>
            /// <returns>nullの場合は0、それ以外は変換された数値</returns>
            internal static uint UInt(object val)
            {
                return IsNull(val) ? 0 : Convert.ToUInt32(val, NumberFormatInfo.CurrentInfo);
            }

            /// <summary>
            /// 値を数値に変換する
            /// </summary>
            /// <param name="val">変換元の値</param>
            /// <returns>nullの場合は0、それ以外は変換された数値</returns>
            internal static int Int(object val)
            {
                return IsNull(val) ? 0 : Convert.ToInt32(val, NumberFormatInfo.CurrentInfo);
            }

            /// <summary>
            /// 値を数値に変換する
            /// </summary>
            /// <param name="val">変換元の値</param>
            /// <returns>nullの場合は0、それ以外は変換された数値</returns>
            internal static System.Int16 Int16(object val)
            {
                return IsNull(val) ? (Int16)0 : Convert.ToInt16(val, NumberFormatInfo.CurrentInfo);
            }

            /// <summary>
            /// 値を数値に変換する
            /// </summary>
            /// <param name="val">変換元の値</param>
            /// <returns>nullの場合は0、それ以外は変換された数値</returns>
            internal static System.Int64 Int64(object val)
            {
                return IsNull(val) ? (Int64)0 : Convert.ToInt64(val, NumberFormatInfo.CurrentInfo);
            }

            /// <summary>
            /// 値を数値に変換する
            /// </summary>
            /// <param name="val">変換元の値</param>
            /// <returns>nullの場合は0、それ以外は変換された数値</returns>
            internal static System.Byte Byte(object val)
            {
                return IsNull(val) ? (Byte)0 : Convert.ToByte(val, NumberFormatInfo.CurrentInfo);
            }

            /// <summary>
            /// 値を文字列に変換する
            /// </summary>
            /// <param name="val">変換元の値</param>
            /// <returns>nullの場合は空文字、それ以外は変換された文字</returns>
            internal static string String(object val)
            {
                return IsNull(val) ? "" : val.ToString();
            }

            internal static bool Bool(object val)
            {
                return IsNull(val) ? false : Convert.ToBoolean(val);
            }

            /// <summary>
            /// オブジェクトがNULLを表すものかどうかを返す
            /// </summary>
            /// <param name="val">判定する値</param>
            /// <returns>true:null false:null以外</returns>
            private static bool IsNull(object val)
            {
                return (null == val || val is DBNull);
            }

            #region test
            [TestFixture]
            public class Test
            {
                #region DateTime
                [Test]
                public void DateTime()
                {
                    Assert.That(To.DateTime(null), Is.EqualTo(System.DateTime.Parse("1/1/1753 12:00:00")));
                    Assert.That(To.DateTime("100.1"), Is.EqualTo(new DateTime(100, 1, 1)));
                    Assert.That(To.DateTime(new DateTime(2009, 4, 7)), Is.EqualTo(new DateTime(2009, 4, 7)));

                    Assert.That(To.DateTime("2009/04/07 0:00:00"), Is.EqualTo(new DateTime(2009, 4, 7)));
                    Assert.That(To.DateTime(new SqlDateTime(2009, 4, 7)), Is.EqualTo(new DateTime(2009, 4, 7)));
                }

                [Test]
                [ExpectedException(typeof(InvalidCastException))]
                public void 整数はDateTime変換しない()
                {
                    To.DateTime(100);
                }

                [Test]
                [ExpectedException(typeof(InvalidCastException))]
                public void 少数はDateTime変換しない()
                {
                    To.DateTime(100.1);
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void 整数文字列はDateTime変換しない()
                {
                    To.DateTime("100");
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void 空文字列はDateTime変換しない()
                {
                    To.DateTime("");
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void アルファベット文字列はDateTime変換しない()
                {
                    To.DateTime("ABC");
                }
                #endregion

                #region Decimal
                [Test]
                public void Decimal()
                {
                    Assert.That(To.Decimal(null), Is.EqualTo(0));
                    Assert.That(To.Decimal(100), Is.EqualTo(100));
                    Assert.That(To.Decimal(100.1), Is.EqualTo(100.1));
                    Assert.That(To.Decimal("100"), Is.EqualTo(100));
                    Assert.That(To.Decimal("100.1"), Is.EqualTo(100.1));
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void 空文字はDecimal変換しない()
                {
                    To.Decimal("");
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void アルファベットはDecimalに変換しない()
                {
                    To.Decimal("ABC");
                }

                [Test]
                [ExpectedException(typeof(InvalidCastException))]
                public void DateTimeはDecimalに変換しない()
                {
                    To.Decimal(new DateTime());
                }
                #endregion Decimal

                #region Double
                [Test]
                public void Double()
                {
                    Assert.That(To.Double(null), Is.EqualTo(0));
                    Assert.That(To.Double(100), Is.EqualTo(100));
                    Assert.That(To.Double(100.1), Is.EqualTo(100.1));
                    Assert.That(To.Double("100"), Is.EqualTo(100));
                    Assert.That(To.Double("100.1"), Is.EqualTo(100.1));
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void 空文字はDoubleに変換しない()
                {
                    To.Double("");
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void アルファベットはDoubleに変換しない()
                {
                    To.Double("ABC");
                }

                [Test]
                [ExpectedException(typeof(InvalidCastException))]
                public void DateTimeはDoubleに変換しない()
                {
                    To.Double(new DateTime());
                }
                #endregion

                #region UInt
                [Test]
                public void UInt()
                {
                    Assert.That(To.UInt(null), Is.EqualTo(0));
                    Assert.That(To.UInt(100), Is.EqualTo(100));
                    Assert.That(To.UInt(100.1), Is.EqualTo(100));
                    Assert.That(To.UInt("100"), Is.EqualTo(100));
                }

                [Test]
                [ExpectedException(typeof(OverflowException))]
                public void 負数はUIntに変換しない()
                {
                    To.UInt(-100);
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void アルファベットはUIntに変換しない()
                {
                    To.UInt("ABC");
                }

                [Test]
                [ExpectedException(typeof(InvalidCastException))]
                public void DateTimeはUIntに変換しない()
                {
                    To.UInt(new DateTime());
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void 空文字はUIntに変換しない()
                {
                    To.UInt("");
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void 小数点付き文字列はUIntに変換しない()
                {
                    To.UInt("100.1");
                }
                #endregion

                #region Int
                [Test]
                public void Int()
                {
                    Assert.That(To.Int(null), Is.EqualTo(0));
                    Assert.That(To.Int(100), Is.EqualTo(100));
                    Assert.That(To.Int(100.1), Is.EqualTo(100));
                    Assert.That(To.Int("100"), Is.EqualTo(100));
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void アルファベットはIntに変換しない()
                {
                    To.Int("ABC");
                }

                [Test]
                [ExpectedException(typeof(InvalidCastException))]
                public void DateTimeはIntに変換しない()
                {
                    To.Int(new DateTime());
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void 空文字はIntに変換しない()
                {
                    To.Int("");
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void 小数点付き文字列はIntに変換しない()
                {
                    To.Int("100.1");
                }
                #endregion

                [Test]
                public void String()
                {
                    Assert.That(To.String(null), Is.EqualTo(""));
                    Assert.That(To.String(100), Is.EqualTo("100"));
                    Assert.That(To.String(100.1), Is.EqualTo("100.1"));
                    Assert.That(To.String("100"), Is.EqualTo("100"));
                    Assert.That(To.String("100.1"), Is.EqualTo("100.1"));
                    Assert.That(To.String(""), Is.EqualTo(""));
                    Assert.That(To.String("ABC"), Is.EqualTo("ABC"));
                    Assert.That(To.String(new DateTime(2009, 4, 7)), Is.EqualTo("2009/04/07 0:00:00"));
                }

                [Test]
                public void Bool()
                {
                    Assert.That(To.Bool(null), Is.False);
                    Assert.That(To.Bool(-1), Is.True);
                    Assert.That(To.Bool(0), Is.False);
                    Assert.That(To.Bool(1), Is.True);
                    Assert.That(To.Bool(100), Is.True);
                    Assert.That(To.Bool("TRue"), Is.True);
                    Assert.That(To.Bool("falSe"), Is.False);
                    Assert.That(To.Bool(true), Is.True);
                    Assert.That(To.Bool(false), Is.False);
                }

                [Test]
                [ExpectedException(typeof(FormatException))]
                public void 真理値変換できない文字列()
                {
                    To.Bool("x");
                }

                [Test]
                public void IsNull()
                {
                    Assert.That(To.IsNull(null), Is.True);
                    Assert.That(To.IsNull(DBNull.Value), Is.True);
                }
            }
            #endregion
        }
        #endregion
    }
}
