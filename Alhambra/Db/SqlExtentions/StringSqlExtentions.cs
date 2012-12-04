using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alhambra.Db.SqlExtentions
{
    internal static class StringSqlExtentions
    {
        /// <summary>
        /// マルチバイト文字列として扱うために頭にNを付けます。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToMultiByteString(this string val)
        {
            return "N'" + val.SanitizeSigleQuate() + "'";
        }

        /// <summary>
        /// 部分一致文字列として扱うために%でくくります。
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ToPartialMatchString(this string newValue)
        {
            return "'%" + newValue.SanitizeSigleQuate().SanitizePercent() + "%'";
        }

        /// <summary>
        /// 文字列置換時にSQLインジェクション対策に危険な文字列（シングルクォート）をエスケープします。
        /// varchar型等にNullを指定したい場合は、nullではなく文字列"NULL"を指定してください。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SanitizeSigleQuate(this string value)
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
                builder.Append(c);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 文字列置換時にSQLインジェクション対策に危険な文字列（パーセント）をエスケープします。
        /// varchar型等にNullを指定したい場合は、nullではなく文字列"NULL"を指定してください。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SanitizePercent(this string value)
        {
            //IEnumerableの要素にnullが入っていた場合はチェックできないので、ここでnullを空文字に置き換えます
            if (String.IsNullOrEmpty(value))
            {
                return "";
            }

            var builder = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                if (c == '%')
                {
                    builder.Append('\\');
                }
                builder.Append(c);
            }
            return builder.ToString();
        }
    }
}
