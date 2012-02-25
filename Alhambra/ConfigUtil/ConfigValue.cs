using System.Configuration;

namespace Ledsun.Alhambra.ConfigUtil
{
    public static class ConfigValue
    {
        private static AppSettingsReader _reader = new AppSettingsReader();

        #region private
        private static string GetValueString(string arg)
        {
            return (string)_reader.GetValue(arg, typeof(System.String));
        }
        #endregion
    }
}
