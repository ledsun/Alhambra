using System;
using Alhambra.ConfigUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlhambraTest
{
    /// <summary>
    ///Configのテストクラスです。
    ///</summary>
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void テスト()
        {
            Assert.AreEqual<string>(@"Integrated Security=SSPI;server=localhost\sqlexpress;", Config.Value.GetConnectionString("SQLServer"));
            Assert.AreEqual<int>(1000, Config.Value.SqlCommandTimeout);
        }
    }
}
