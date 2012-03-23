using System.Configuration;

namespace Ledsun.Alhambra.ConfigUtil
{
    /// <summary>
    /// configファイルの内容をキャストして取得する。
    /// </summary>
    static class AppSettingsReaderExtentions
    {
        public static T GetValue<T>(this AppSettingsReader _reader, string arg)
        {
            return (T)_reader.GetValue(arg, typeof(T));
        }
    }
}
