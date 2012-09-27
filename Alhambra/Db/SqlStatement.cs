using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Alhambra.Db.SqlExtentions;

namespace Alhambra.Db
{
    /// <summary>
    /// SQLステートメントを作成するためのクラスです。
    /// @で囲んだ文字列を、指定の値に置き換えてくれるReplaceメソッドを提供します。
    /// 以下のようにしてSQL文字列を作成することができます。
    /// new SqlStatement("SELECT * FROM TABLE WHERE ID = @ID@").Replace("ID", 100).ToString();
    ///[設計メモ]
    /// 値型用のメソッドを値型（ヌル許容）と同様のジェネリックではなくオーバーロードで実装した理由。
    ///  値型用のジェネリックなメソッドがあると、文字列への暗黙変換を実装したオブジェクトを引数にしたときに
    ///  文字列を引数に取るオーバーロードではなくジェネリックのメソッドが優先して呼ばれます。
    ///  そのため呼び出し時に明示的に文字列に変換する必要が出てきます。
    /// </summary>
    public class SqlStatement
    {
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
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, newValue.ToMultiByteString());
        }

        #region 値型の置換
        /// <summary>
        /// 真理値型を置き換えます
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldValue, bool newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, ToValue(newValue));
        }

        /// <summary>
        /// 整数型を置き換えます
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldValue, int newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, ToValue(newValue));
        }

        /// <summary>
        /// 小数点型を置き換えます
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldValue, decimal newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, ToValue(newValue));
        }

        /// <summary>
        /// 日付型を置き換えます
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace(string oldValue, DateTime newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, ToValue(newValue));
        }
        #endregion

        /// <summary>
        /// 値型（ヌル許容）を置き換えます
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement Replace<T>(string oldValue, T? newValue) where T : struct
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, NullToValue(newValue));
        }

        /// <summary>
        /// IN句用複数値置換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldValue"></param>
        /// <param name="newValues"></param>
        /// <returns></returns>
        public SqlStatement Replace<T>(string oldValue, IEnumerable<T> newValues)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            if (newValues == null)
            {
                throw new ArgumentNullException("newValues");
            }

            if (newValues.Any())
            {
                var strs = newValues.Select(val =>
                {
                    if (typeof(T) == typeof(string))
                    {
                        return ((string)(object)val).ToMultiByteString();
                    }

                    if (val == null)
                    {
                        return "NULL";
                    }

                    if (typeof(T) == typeof(bool))
                    {
                        return ((bool)(object)val).ToSqlString();
                    }

                    if (typeof(T) == typeof(DateTime))
                    {
                        return ((DateTime)(object)val).ToSqlString();
                    }

                    return val.ToString();
                });

                return ReplaceByAtmark(oldValue, "(" + string.Join(",", strs) + ")");
            }

            throw new ArgumentException("newValues が0件です。");
        }

        /// <summary>
        /// 部分一致置換
        /// %%で囲みます
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement ReplaceForPartialMatch(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            return ReplaceByAtmark(oldValue, newValue.ToPartialMatchString());
        }

        /// <summary>
        /// AND検索用Replace
        /// (カラム名 LIKE '%語句1%' AND カラム名 LIKE '%語句2%')形式に置き換えます。
        /// </summary>
        /// <param name="oldValue">置き換えを行う文字列</param>
        /// <param name="columnName">条件対象のカラム名</param>
        /// <param name="newValues">空白区切りの複数条件。条件が0件だった場合は(0=0)を返します。</param>
        /// <returns>()で括った文字列に置き換えるので、そのままOR句と繋ぐことができます。</returns>
        public SqlStatement ReplaceMultiLike(string oldValue, string columnName, string newValues)
        {
            var parameters = Regex.Split(newValues, "\\s")
                .Where(s => s != "")
                .Select(s => columnName + " LIKE " + s.ToPartialMatchString())
                .AndJoin();

            return ReplaceByAtmark(oldValue, "(" + (parameters.Length > 0 ? parameters : "0=0") + ")");
        }

        /// <summary>
        /// 文字列だがシングルクォートで囲まない
        /// DB名、テーブル名の置換に必要
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public SqlStatement ReplaceStripString(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentException("oldValue");
            }

            if (string.IsNullOrEmpty(newValue))
            {
                throw new ArgumentException("newValue");
            }

            return ReplaceByAtmark(oldValue, newValue.Sanitize());
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
        /// <param name="nullValue"></param>
        /// <returns></returns>
        private static string NullToValue<T>(T? nullValue) where T : struct
        {
            if (!nullValue.HasValue)
            {
                return "NULL";
            }

            return ToValue<T>(nullValue.Value);
        }

        private static string ToValue<T>(T value) where T : struct
        {
            if (typeof(T) == typeof(bool))
            {
                return ((bool)(object)value).ToSqlString();
            }

            if (typeof(T) == typeof(DateTime))
            {
                return ((DateTime)(object)value).ToSqlString();
            }

            return value.ToString();
        }

        /// <summary>
        /// 単純に文字列を置き換えます。
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        private SqlStatement ReplaceByAtmark(string oldValue, string newValue)
        {
            return new SqlStatement(_baseSql.Replace("@" + oldValue + "@", newValue));
        }
        #endregion
    }
}