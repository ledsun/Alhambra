using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Ledsun.Alhambra.Db
{
    //Transaction有りの複数SQL実行用
    public class DBHelperWithTransaction : IDisposable
    {
        private DBBridgeForSqlServer _bridg;

        public DBHelperWithTransaction()
        {
            _bridg = new DBBridgeForSqlServer();
            _bridg.BeginTransaction();
        }

        public void Commit()
        {
            _bridg.Commit();
        }

        public void Rollback()
        {
            _bridg.Rollback();
        }

        public int Execute(string sql)
        {
            return _bridg.Execute(sql);
        }

        public List<DataRowAccessor> Select(string sql)
        {
            return _bridg.Select(sql);
        }

        public TypeConvertableWrapper SelectOne(string sql)
        {
            return _bridg.SelectOne(sql);
        }

        public System.Data.DataSet SelectDataSet(string sql)
        {
            return _bridg.SelectDataSet(sql);
        }

        public void Dispose()
        {
            _bridg.Dispose();
        }

        #region test
        [TestFixture]
        public class Test
        {
            [TestFixtureSetUp]
            public void SetUpFixture()
            {
                DBHelper.Execute(@"
                    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ClassLibraryTest')
                    CREATE DATABASE [ClassLibraryTest] ON  PRIMARY 
                        ( NAME = N'ClassLibraryTest', FILENAME = N'D:\SQLServer\ClassLibraryTest.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
                         LOG ON 
                        ( NAME = N'ClassLibraryTest_log', FILENAME = N'D:\SQLServer\ClassLibraryTest_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
                     COLLATE Japanese_CI_AS
                ");

                using (DBHelperWithTransaction db = new DBHelperWithTransaction())
                {
                    db.Execute("USE [ClassLibraryTest]");
                    db.Execute(@"
                        IF NOT EXISTS(SELECT NAME FROM sysobjects WHERE xtype = 'U' AND NAME = 'DBHelperWithTransaction')
                        CREATE TABLE [dbo].[DBHelperWithTransaction](
                            [ID] [int] NOT NULL,
                            [VALUE] [varchar](50) COLLATE Japanese_CI_AS NULL,
                         CONSTRAINT [PK_DBHelperWithTransaction] PRIMARY KEY CLUSTERED 
                        (
                            [ID] ASC
                        )WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
                        ) ON [PRIMARY]
                    ");
                    db.Commit();
                }
            }

            [SetUp]
            public void SetUp()
            {
                //テーブルを空にします。
                using (DBHelperWithTransaction db = new DBHelperWithTransaction())
                {
                    db.Execute(new DBSqlStatement("TRUNCATE TABLE @DB@ DBHelperWithTransaction"));
                    db.Commit();
                }
            }

            [Test]
            public void コミット()
            {
                using (DBHelperWithTransaction db = new DBHelperWithTransaction())
                {
                    db.Execute(InsertSql);
                    db.Commit();
                }
                Assert.That(Select()[0]["VALUE"].String, Is.EqualTo("ABC"));
            }

            // 20091105 テスト項目追加
            // System.InvalidOperationException : ExecuteReader は、コマンドに割り当てられた接続が保留状態であるローカルのトランザクションにあるとき、トランザクション オブジェクトを持つコマンドが必要です。コマンドの Transaction プロパティがまだ初期化されていません。
            [Test]
            public void トランザクション内でのSelct()
            {
                using (DBHelperWithTransaction db = new DBHelperWithTransaction())
                    Assert.That(db.Select("SELECT 1 AS ONE")[0]["ONE"].Int, Is.EqualTo(1));
            }

            [Test]
            public void トランザクション内でのSelectOne()
            {
                using (DBHelperWithTransaction db = new DBHelperWithTransaction())
                    Assert.That(db.SelectOne("SELECT 1").Int, Is.EqualTo(1));
            }

            [Test]
            public void ロールバック()
            {
                using (DBHelperWithTransaction db = new DBHelperWithTransaction())
                {
                    db.Execute(InsertSql);
                    db.Rollback();
                }
                Assert.That(Select().Count, Is.EqualTo(0));
            }

            [Test]
            public void 自動ロールバック()
            {
                using (DBHelperWithTransaction db = new DBHelperWithTransaction())
                {
                    db.Execute(InsertSql);
                    //何もしなければロールバックします。
                }
                Assert.That(Select().Count, Is.EqualTo(0));
            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException))]
            public void 多重コミット()
            {
                using (DBHelperWithTransaction db = new DBHelperWithTransaction())
                {
                    db.Commit();
                    db.Commit();
                }
            }

            [Test]
            [ExpectedException(typeof(InvalidOperationException))]
            public void 多重ロールバック()
            {
                using (DBHelperWithTransaction db = new DBHelperWithTransaction())
                {
                    db.Rollback();
                    db.Rollback();
                }
            }

            private static string InsertSql
            {
                get
                {
                    return new DBSqlStatement("INSERT INTO @DB@ DBHelperWithTransaction VALUES (1, 'ABC')");
                }
            }

            private static List<DataRowAccessor> Select()
            {
                return DBHelper.Select(new DBSqlStatement("SELECT VALUE FROM @DB@ DBHelperWithTransaction WHERE ID = 1"));
            }
        }
        #endregion
    }
}
