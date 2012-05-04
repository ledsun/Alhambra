using System;
using ObjectExtentions.TypeConvert;

namespace Alhambra.Db.Data
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

        public static DateTime DateTimeDefault = ObjectTypeConvertExtentions.DateTimeDefault;

        public TypeConvertableWrapper(object rawData)
        {
            _value = rawData;
        }

        //明示的な型を指定したプロパティ
        public uint UInt { get { return _value.UInt(); } }
        public int Int { get { return _value.Int(); } }
        public Int16 Int16 { get { return _value.Int16(); } }
        public Int64 Int64 { get { return _value.Int64(); } }
        public Byte Byte { get { return _value.Byte(); } }
        public string String { get { return _value.String(); } }
        public decimal Decimal { get { return _value.Decimal(); } }
        public DateTime DateTime { get { return _value.DateTime(); } }
        public DateTime? DateTimeNull { get { return _value.DateTimeNull(); } }
        public double Double { get { return _value.Double(); } }

        /// <summary>
        /// 0はFalseそれ以外の数字はTrue
        /// 文字列の場合はTrue、Falseは変換可能（大文字小文字を区別しない）。それ以外は例外を出す。
        /// </summary>
        public bool Bool { get { return _value.Bool(); } }

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
