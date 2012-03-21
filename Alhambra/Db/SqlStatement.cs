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
        const string DATABASE = "@DB@";
        const string DATETIME_FORMAT = "\\'yyyy/MM/dd HH:mm:ss\\'";
        private readonly string _baseSql;

        public SqlStatement(string baseSql)
        {
            _baseSql = (string)baseSql.Clone();
        }

        public SqlStatement(string baseSql, string dbName)
            : this(baseSql)
        {
            _baseSql = _baseSql.Replace(DATABASE, dbName);
        }

        public SqlStatement Replace(string oldString, bool newValue)
        {
            return ReplaceByAtmark(oldString, newValue ? "1" : "0");
        }

        public SqlStatement Replace(string oldString, int newValue)
        {
            return ReplaceByAtmark(oldString, newValue.ToString());
        }

        public SqlStatement Replace(string oldString, Decimal newValue)
        {
            return ReplaceByAtmark(oldString, newValue.ToString());
        }

        public SqlStatement Replace(string oldString, Decimal? newValue)
        {
            var newString = !newValue.HasValue ? "NULL" : newValue.Value.ToString();
            return ReplaceByAtmark(oldString, newString);
        }

        public SqlStatement Replace(string oldString, DateTime newDate)
        {
            //自動初期化された値が指定された場合はNULLにします。
            //DBのdatetime型は指定無しを示す値を取れないためNULLを許可します。
            var newString = newDate == new DateTime(0) ? "NULL" : newDate.ToString(DATETIME_FORMAT);
            return ReplaceByAtmark(oldString, newString);
        }

        public SqlStatement Replace(string oldString, DateTime? newDate)
        {
            var newString = !newDate.HasValue ? "NULL" : newDate.Value.ToString(DATETIME_FORMAT);
            return ReplaceByAtmark(oldString, newString);
        }

        public SqlStatement Replace(string oldString, IEnumerable<string> newStrings)
        {
            if (string.IsNullOrEmpty(oldString))
            {
                throw new ArgumentException("oldString");
            }

            if (newStrings == null)
            {
                throw new ArgumentNullException("newStrings");
            }

            if (newStrings.Any())
            {
                var newString = "";
                foreach (string str in newStrings)
                {
                    newString += "'" + Sanitize(str) + "',";
                }

                return ReplaceByAtmark(oldString, CutLastChar(newString));
            }

            throw new ArgumentException("newStrings");
        }

        public SqlStatement Replace(string oldString, IEnumerable<long> newStrings)
        {
            if (string.IsNullOrEmpty(oldString))
            {
                throw new ArgumentException("oldString");
            }

            if (newStrings == null)
            {
                throw new ArgumentNullException("newStrings");
            }

            if (newStrings.Any())
            {
                var newString = "";
                foreach (long l in newStrings)
                {
                    newString += l.ToString() + ",";
                }

                return ReplaceByAtmark(oldString, CutLastChar(newString));
            }

            throw new ArgumentException("newStrings");
        }

        private static string CutLastChar(string val)
        {
            return val.Length > 0 ? "(" + val.Remove(val.Length - 1) + ")" : val;
        }

        //シングルクォートで囲む版
        public SqlStatement Replace(string oldString, string newString)
        {
            return ReplaceByAtmark(oldString, "N'" + Sanitize(newString) + "'");
        }

        public SqlStatement ReplaceForPartialMatchRetrieval(string oldString, string newString)
        {
            return ReplaceByAtmark(oldString, "'%" + Sanitize(newString) + "%'");
        }

        public SqlStatement ReplaceStripString(string oldString, string newString)
        {
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
            //ここでnullを空文字に置き換えるのはおかしいです。
            //呼び出し元のpublicメソッドで引数がnullかチェックしArgumentNullExceptionを返すべきです。
            //しかし、どのプロジェクトで使われているかわからないのでもう修正しません。
            if (value == null) return "";
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