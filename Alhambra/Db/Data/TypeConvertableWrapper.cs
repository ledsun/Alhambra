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
        public string String { get { return _value.String(); } }

        public uint UInt { get { return _value.UInt(); } }
        public int Int { get { return _value.Int(); } }
        public Int16 Int16 { get { return _value.Int16(); } }
        public Int64 Int64 { get { return _value.Int64(); } }
        public Byte Byte { get { return _value.Byte(); } }
        public decimal Decimal { get { return _value.Decimal(); } }
        public DateTime DateTime { get { return _value.DateTime(); } }
        public double Double { get { return _value.Double(); } }
        public bool Bool { get { return _value.Bool(); } }

        public uint? UIntNull { get { return _value.UIntNull(); } }
        public int? IntNull { get { return _value.IntNull(); } }
        public Int16? Int16Null { get { return _value.Int16Null(); } }
        public Int64? Int64Null { get { return _value.Int64Null(); } }
        public Byte? ByteNull { get { return _value.ByteNull(); } }
        public decimal? DecimalNull { get { return _value.DecimalNull(); } }
        public DateTime? DateTimeNull { get { return _value.DateTimeNull(); } }
        public double? DoubleNull { get { return _value.DoubleNull(); } }
        public bool? BoolNull { get { return _value.BoolNull(); } }

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
