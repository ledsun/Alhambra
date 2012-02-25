using System;
using System.Configuration;
using System.Reflection;

namespace Ledsun.Alhambra.ConfigUtil
{
    //XXX.Configの設定値を読み取るクラスです。
    //各プロジェクトごとにConfigクラスを丸ごとコピーし
    //プロジェクト固有の設定をConfig.ConfigValueクラス内に実装してください。
    internal class Config
    {
        private static ConfigValue value = new ConfigValue();
        public static ConfigValue Value
        {
            get { return value; }
        }

        public class ConfigValue : ConfigValueBase
        {
            //プロジェクト固有の設定値はこのクラスのプロパティとして実装します。
            //public string Sample
            //{
            //    get { return GetValueString(MethodBase.GetCurrentMethod().Name.Substring(4)); }
            //}
            public string DbConnectionString
            {
                get
                {
                    var css = ConfigurationManager.ConnectionStrings["DBHelper"];
                    if (css != null && !String.IsNullOrEmpty(css.ConnectionString))
                    {

                        return css.ConnectionString;
                    }
                    else
                    {
                        throw new ApplicationException("configファイルのConnectionStringにDBHelerの接続文字列を指定して下さい。");
                    }
                }
            }

            public string DBPrefix
            {
                get { return GetValueString(MethodBase.GetCurrentMethod().Name.Substring(4)); }
            }

            public int SqlCommandTimeout
            {
                get
                {
                    try
                    {
                        return GetValueInt(MethodBase.GetCurrentMethod().Name.Substring(4));
                    }
                    catch
                    {
                        return 30;
                    }
                }
            }
        }
    }

    //共通ライブラリ用の設定値はここに書きます。
    public class ConfigValueBase
    {
        private static AppSettingsReader _reader = new AppSettingsReader();

        #region protected
        protected string GetValueString(string arg)
        {
            return (string)_reader.GetValue(arg, typeof(System.String));
        }

        protected int GetValueInt(string arg)
        {
            return (int)_reader.GetValue(arg, typeof(System.Int32));
        }

        protected bool GetValueBool(string arg)
        {
            return (bool)_reader.GetValue(arg, typeof(System.Boolean));
        }
        #endregion
    }
}