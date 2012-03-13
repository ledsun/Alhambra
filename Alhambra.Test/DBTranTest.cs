﻿using System;
using Ledsun.Alhambra.Db.Helper;
using Ledsun.Alhambra.Db.Plugin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlhambraTest
{
     /// <summary>
    ///DBTranのテストクラスです。
    ///</summary>
    [TestClass()]
    public class DBTranTest
    {
        [TestMethod]
        public void DBTranのテスト()
        {
            using (var tr = new DBTran())
            {
                DBHelper.Select("SELECT 1", tr);
                tr.Commit();
            }
        }
    }
}