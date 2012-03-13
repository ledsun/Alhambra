using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ledsun.Alhambra.Db;
using Ledsun.Alhambra.Db.Data;

namespace AlhambraTest
{
    [TestClass]
    public class DateTimeNullTest
    {
        [TestMethod]
        public void Null許容DateTimeのテスト()
        {
            DateTime? a = null;
            var target = new SqlStatement("aaa").Replace("aa", a);
            Assert.AreEqual<string>("aaa", target);

            var b = new TypeConvertableWrapper(a);
            Assert.AreEqual<DateTime>(TypeConvertableWrapper.DateTimeDefault, b.DateTime);
            Assert.AreEqual<DateTime?>(null, b.DateTimeNull);

            var c = DateTime.Now;
            var d = new TypeConvertableWrapper(c);
            Assert.AreEqual<DateTime>(c, d.DateTime);
            Assert.AreEqual<DateTime?>(new DateTime?(c), d.DateTimeNull);
        }
    }
}
