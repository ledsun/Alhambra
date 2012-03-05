using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Ledsun.Alhambra.ConfigUtil
{
    /// <summary>
    /// configファイルの内容をキャストして取得する。
    /// </summary>
    public class ConfigValueBase
    {
        private static AppSettingsReader _reader = new AppSettingsReader();

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
    }
}
