using System;

namespace Ledsun.Alhambra.Db.Data
{
    /// <summary>
    /// 型変換機能を持つヘルパークラス
    /// 引数で取得した値をプロパティで型を指定して任意の型に変換して取得できます。
    /// 主にDBから取得した値をDataTransferObjectの型に合わせて変換するために使います。
    /// </summary>
    /// <example>
    /// 型名のプロパティで値を当該型に変換して取得できます。
    ///     new TypeConvertableWrapper("1").Int
    /// String型へは暗黙型変換が可能です。
    ///     string hoge = new TypeConvertableWrapper("1");
    /// </example>
    public class TypeConvertableWrapper
    {
        private readonly Object _value;

        //sqlserver compact の制限（1/1/1753 12:00:00 AM から 12/31/9999 11:59:59 PM までの間でなければなりません。）
        public static DateTime DateTimeDefault = System.DateTime.Parse("1/1/1753 12:00:00");

        public TypeConvertableWrapper(object rawData)
        {
            _value = rawData;
        }

        //明示的な型を指定したプロパティ
        public uint UInt { get { return To.UInt(_value); } }
        public int Int { get { return To.Int(_value); } }
        public Int16 Int16 { get { return To.Int16(_value); } }
        public Int64 Int64 { get { return To.Int64(_value); } }
        public Byte Byte { get { return To.Byte(_value); } }
        public string String { get { return To.String(_value); } }
        public decimal Decimal { get { return To.Decimal(_value); } }
        public DateTime DateTime { get { return To.DateTime(_value); } }
        public DateTime? DateTimeNull { get { return To.DateTimeNull(_value); } }
        public double Double { get { return To.Double(_value); } }

        /// <summary>
        /// 0はFalseそれ以外の数字はTrue
        /// 文字列の場合はTrue、Falseは変換可能（大文字小文字を区別しない）。それ以外は例外を出す。
        /// </summary>
        public bool Bool { get { return To.Bool(_value); } }

        //Stringに関しては暗黙の型変換が可能
        public static implicit operator string(TypeConvertableWrapper value)
        {
            return value.String;
        }

        public override string ToString()
        {
            return String;
        }

       
    }
}
