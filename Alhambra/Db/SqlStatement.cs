using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ledsun.Alhambra.Db
{
    //SQLステートメントを作成するためのクラスです。
    //@で囲んだ文字列を、指定の値に置き換えてくれるReplaceメソッドを提供します。
    //以下のようにしてSQL文字列を作成することができます。
    // new SqlStatement("SELECT * FROM TABLE WHERE ID = @ID@").Replace("ID", 100).ToString();
    public class SqlStatement
    {
        private const string SQL_DATETIME_FORMAT = "\\'yyyy/MM/dd HH:mm:ss\\'";
        private readonly string _baseSql;

        /// <summary>
        /// コンストラクタ
        /// 元になる文字列を指定します。
        /// </summary>
        /// <example>new SqlStatement("SELECT * FROM TABLE WHERE ID = @ID@")</example>
        /// <param name="baseSql"></param>
        public SqlStatement(string baseSql)
        {
            if (string.IsNullOrEmpty(baseSql))
            {
                throw new ArgumentException("baseSql");
            }

            _baseSql = (string)baseSql.Clone();
        }

        /// <summary>
        /// 文字列型の置換
        /// シングルクォートで囲みます。
        /// </summary>
        /// <param name="oldString"></param>
        /// <param name="newString"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldString, string newString)
        {
            if (string.IsNullOrEmpty(oldString))
            {
                throw new ArgumentException("oldString");
            }

            if (string.IsNullOrEmpty(newString))
            {
                throw new ArgumentException("newString");
            }

            return ReplaceByAtmark(oldString, StringToString(newString));
        }

        /// <summary>
        /// 値型を置き換えます
        /// </summary>
        /// <param name="oldString"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace<T>(string oldString, T newValue) where T : struct
        {
            if (string.IsNullOrEmpty(oldString))
            {
                throw new ArgumentException("oldString");
            }

            return ReplaceByAtmark(oldString, ToValue(newValue));
        }

        /// <summary>
        /// 値型（ヌル許容）を置き換えます
        /// </summary>
        /// <param name="oldString"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace<T>(string oldString, T? newValue) where T : struct
        {
            if (string.IsNullOrEmpty(oldString))
            {
                throw new ArgumentException("oldString");
            }

            return ReplaceByAtmark(oldString, NullToValue(newValue));
        }

        /// <summary>
        /// IN句用複数値置換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldString"></param>
        /// <param name="newValues"></param>
        /// <returns></returns>
        public SqlStatement ReplaceIn<T>(string oldString, IEnumerable<T> newValues)
        {
            if (string.IsNullOrEmpty(oldString))
            {
                throw new ArgumentException("oldString");
            }

            if (newValues == null)
            {
                throw new ArgumentNullException("newStrings");
            }

            if (newValues.Any())
            {
                var strs = newValues.Select(val =>
                {
                    if (val == null)
                    {
                        return "NULL";
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        return StringToString((string)(object)val);
                    }
                    else if (typeof(T) == typeof(bool))
                    {
                        return BoolToString((bool)(object)val);
                    }
                    else if (typeof(T) == typeof(DateTime))
                    {
                        return DateTimeToString((DateTime)(object)val);
                    }
                    else
                    {
                        return val.ToString();
                    }
                });

                return ReplaceByAtmark(oldString, "(" + string.Join(",", strs) + ")");
            }

            throw new ArgumentException("newStrings");
        }

        /// <summary>
        /// 部分一致置換
        /// %%で囲みます
        /// </summary>
        /// <param name="oldString"></param>
        /// <param name="newString"></param>
        /// <returns></returns>
        public SqlStatement ReplaceForPartialMatchRetrieval(string oldString, string newString)
        {
            if (string.IsNullOrEmpty(oldString))
            {
                throw new ArgumentException("oldString");
            }

            if (string.IsNullOrEmpty(newString))
            {
                throw new ArgumentException("newString");
            }

            return ReplaceByAtmark(oldString, "'%" + Sanitize(newString) + "%'");
        }

        /// <summary>
        /// 文字列だがシングルクォートで囲まない
        /// DB名、テーブル名の置換に必要
        /// </summary>
        /// <param name="oldString"></param>
        /// <param name="newString"></param>
        /// <returns></returns>
        public SqlStatement ReplaceStripString(string oldString, string newString)
        {
            if (string.IsNullOrEmpty(oldString))
            {
                throw new ArgumentException("oldString");
            }

            if (string.IsNullOrEmpty(newString))
            {
                throw new ArgumentException("newString");
            }

            return ReplaceByAtmark(oldString, Sanitize(newString));
        }

        #region 文字列変換
        /// <summary>
        /// String型への暗黙型変換演算子
        /// このメソッドを実装することで、String型へのキャストでSQL文字列を取得できます。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>作成したSQL文字列</returns>
        public static implicit operator string(SqlStatement value)
        {
            return value._baseSql;
        }

        /// <summary>
        /// 暗黙的な型変換をサポートしているのでキャストすればSQL文字列が取得できるため、厳密には本メソッドは必要ありません。
        /// しかし、ToStringメソッドとString型への型変換の結果が異なる場合、混乱を招くため同じ結果を返します。
        /// </summary>
        /// <returns>作成したSQL文字列</returns>
        public override string ToString()
        {
            return this;
        }
        #endregion

        #region private_method
        /// <summary>
        /// NULL許容型を値に変換します。
        /// nullが来たらNULLにします。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newValue"></param>
        /// <returns></returns>
        private static string NullToValue<T>(T? newValue) where T : struct
        {
            if (!newValue.HasValue)
            {
                return "NULL";
            }

            return ToValue<T>(newValue.Value);
        }

        private static string ToValue<T>(T newValue) where T : struct
        {
            if (typeof(T) == typeof(bool))
            {
                return BoolToString((bool)(object)newValue);
            }

            if (typeof(T) == typeof(DateTime))
            {
                return DateTimeToString((DateTime)(object)newValue);
            }

            return newValue.ToString();
        }

        private static string BoolToString(bool val)
        {
            return val ? "1" : "0";
        }

        private static string DateTimeToString(DateTime val)
        {
            return val.ToString(SQL_DATETIME_FORMAT);
        }

        private string StringToString(string val)
        {
            return "N'" + Sanitize(val) + "'";
        }

        /// <summary>
        /// 単純に文字列を置き換えます。
        /// </summary>
        /// <param name="oldString"></param>
        /// <param name="newString"></param>
        /// <returns></returns>
        private SqlStatement ReplaceByAtmark(string oldString, string newString)
        {
            return new SqlStatement(_baseSql.Replace("@" + oldString + "@", newString));
        }

        /// <summary>
        /// 文字列置換時にSQLインジェクション対策に危険な文字列（シングルクォートとパーセント）をエスケープします。
        /// varchar型等にNullを指定したい場合は、nullではなく文字列"NULL"を指定してください。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string Sanitize(string value)
        {
            //IEnumerableの要素にnullが入っていた場合はチェックできないので、ここでnullを空文字に置き換えます
            if (String.IsNullOrEmpty(value))
            {
                return "";
            }

            var builder = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                if (c == '\'')
                {
                    builder.Append('\'');
                }
                if (c == '%')
                {
                    builder.Append('\\');
                }
                builder.Append(c);
            }
            return builder.ToString();
        }
        #endregion
    }
}