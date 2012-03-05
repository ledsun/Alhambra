using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Ledsun.Alhambra.Db.Plugin
{
    /// <summary>
    /// DB接続用のインスタンスを返すファクトリーです。
    /// </summary>
    internal static class DBFactory
    {
        /// <summary>
        /// プラグインのカタログ
        /// </summary>
        private static AggregateCatalog _catalog = new AggregateCatalog();

        /// <summary>
        /// プラグインのカタログを初期化します。
        /// </summary>
        static DBFactory()
        {
            //自dllからプラグインの場所を取得
            string executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pluginsPath = Path.Combine(executionPath, "Plugins");
            if (!Directory.Exists(pluginsPath))
            {
                Directory.CreateDirectory(pluginsPath);
            }

            _catalog.Catalogs.Add(new DirectoryCatalog(pluginsPath));

            //ASP.NETの場合dllのあるディレクトリをHttpRuntime.BinDirectoryから取得
            try
            {
                _catalog.Catalogs.Add(new DirectoryCatalog(HttpRuntime.BinDirectory));
            }
            catch (ArgumentNullException)
            {
                //ASP.NETでない（テスト実行やクライアントアプリの）場合は例外が挙がってここに来ます。
            }

            if (_catalog.Parts.Count() == 0)
            {
                throw new DBHelperException("DBHelperプラグインdllの読み込みに失敗しました。");
            }
        }

        /// <summary>
        /// 新規のインスタンスを返します。
        /// </summary>
        internal static AbstractDBBridge NewDB
        {
            get
            {
                var i = new DBIncubator();
                try
                {
                    new CompositionContainer(_catalog).ComposeParts(i);
                }
                catch (ChangeRejectedException cre)
                {
                    throw new DBHelperException("DBHelperプラグインにAbstractDBBridgeの定義がありません。", cre);
                }

                return i.Incubatee;
            }
        }

        /// <summary>
        /// Managed Extensibility FrameworkはDIフレームワークなので生成したインスタンスを直接返却できません。<br />
        /// そこで、セッターインジェクションを使い、生成したAbstractDBBridge型のインスタンスを<br />
        /// 一回このクラスのプロパティにセットしてからDBFactioryのNewDBプロパティで返却します。<br />
        /// </summary>
        public class DBIncubator
        {
            [Import(typeof(AbstractDBBridge))]
            internal AbstractDBBridge Incubatee { get; set; }
        }
    }
}
