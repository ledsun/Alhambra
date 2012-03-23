using System;
using System.Configuration;
using System.Reflection;

namespace Ledsun.Alhambra.ConfigUtil
{
    /// <summary>
    /// XXX.Configの設定値を読み取るクラスです。
    /// Config.Value.SqlCommandTimeoutという形で値を取得できます。
    /// プロジェクト固有の設定はConfig.ConfigValueクラスに拡張メソッドを追加すれば同じように行けると思うけど、未検証です。
    /// </summary>
    public class Config
    {
        //ConfigValue継承して実装してるのでインスタンス化が必要です。
        private static ConfigValue value = new ConfigValue();

        /// <summary>
        /// Config.Value.XXXを静的に参照できるように、静的なプロパティを提供します。
        /// </summary>
        public static ConfigValue Value
        {
            get { return value; }
        }

        public class ConfigValue 
        {
            private static AppSettingsReader _reader = new AppSettingsReader();

            /// <summary>
            /// 名前を指定してデータベース接続文字列を取得する。
            /// プラグインごとにデータベース接続文字列を分けるために使用。
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public string GetConnectionString(string name)
            {
                var css = ConfigurationManager.ConnectionStrings[name];
                if (css != null && !String.IsNullOrEmpty(css.ConnectionString))
                {

                    return css.ConnectionString;
                }
                else
                {
                    throw new DBHelperException("configファイルのConnectionStringに" + name + "の接続文字列を指定して下さい。");
                }
            }

            /// <summary>
            /// SQL実行タイムアウトを設定します。
            /// 設定されてなければ30秒を使います。
            /// </summary>
            internal int SqlCommandTimeout
            {
                get
                {
                    try
                    {
                        return _reader.GetValue<int>(MethodBase.GetCurrentMethod().Name.Substring(4));
                    }
                    catch (InvalidOperationException)
                    {
                        return 30;
                    }
                }
            }
        }
    }
}