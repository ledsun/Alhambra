# アルハンブラとは #
Alhambra is .Net Database Helper

オブジェクトとデータベースのインピーダンスミスマッチがあります。
アルハンブラはO/Rマッパーではありません。
型のミスマッチは埋めます。

Not O/R Mapper, Mapping only Type mismatch.

C#/VB.NETからSQLを実行するためのラッパークラスです。
コネクションのオープンクローズは自動で行うため明示的に指定する必要はありません。

# 使い方 #

## NuGetでインストール

VisualStudioの`NuGetのパッケージ管理`を使い`Alhambra DBHelper`を検索してインストールしてください。

`plugins`ディレクトリに以下のプラグインが配置されます。

- Alhumbra.Plugin.SqlServer.dll
- Alhumbra.Plugin.OleDb.dll

## 設定 ##

### プラグインファイルのプロパティ

プラグインファイルのプロパティで`ビルドアクション`を`なし`に設定してください。
また、使用するDB用のプラグインの	`出力ディレクトリにコピー`を`新しい場合はコピーする`に設定してください。

※ClickOnceで配布する際は`ビルドアクション`を`コンテンツ`にしてください。ただしVisualStudioでは警告が表示されます。

### app.config

app.configファイルやweb.configファイルに記述が必要です

```xml
<configuration>
  <connectionStrings>
    <add name="OleDb" connectionString="Provider=Microsoft.Jet.OLEDB.4.0; Data Source=mydb.mdb;" />
    <add name="SQLServer" connectionString="Integrated Security=SSPI;server=localhost\sqlexpress;"/>
  </connectionStrings>  
  <appSettings>
    <add key="SqlCommandTimeout" value="600"/>
  </appSettings>
</configuration>
```

※connectionStringsは使用するDBのものだけがあれば大丈夫です。

複数のプラグインを配置する場合は`appSettings`の`PluginName`に`OleDb`または`SQLServer`を指定します。


## サンプルコード ##


### Select

```csharp
int value =DBHelper.Select(
  new SqlStatement(@"
     SELECT
         VALE
     FROM EXAMPLE_TABLE
     WHERE ID = @ID
     ")
  .Replace("ID", 100)
  )
.Single()["VALUE"].Int;
```

#### SQLの作成
SqlStatementクラスを使って作成します。

1. コンストラクタでベースなるSQL文字列を設定
1. `@keyword`で置き換える文字を指定
1. Replaceメソッドで置き換える

メソッドチェーンでRepalceを連続して書くことができます。
SqlStatementは暗黙的、明示的な文字列への変換をサポート

#### 取得した値
取得した値は`IEnumerable<DataRowAccessor>`で帰ってきます。
DataRowAccessorはカラムのインデックスまたはカラム名でカラムを指定して値が取得できます。
取った値はInt、DateTime、Doubleなどのプロパティを指定して明示的に型を変換できます。


### DataSetの取得方法 ###

GridViewやDropDownListのDataSourceに指定する場合はDataSetで取れた方がうれしいのでSelectDataSetを使います。

```csharp
DataSet ds = DBHelper.SelectDataSet("SELECT 3");
```

### ひとつの値を取得する場合 ###

レコードの行数を取得する場合など、確実に一つの値を返すSQLを実行する際はSelectOneを使います。

```csharp
int count = DBHelper.SelectOne("SELECT COUNT(*) FROM T_XXX").Int;
```

### Execute
結果を返さないSQLを実行する場合はExceuteを使います。
影響を与えた行数を返します。

#### UPDATE/DELETE

```csharp
DBHelper.Execute("UPDATE T_XXX SET LEVEL_VAL = 30 WHERE ID = 'abc'");
```

#### ストアドプロシージャ

```csharp
DBHelper.Execute("exec sp_XXX");
```

# 開発者向け情報

## 開発環境

- Microsoft Visual Studio Community 2015
- Microsoft SQLServer Express 2014
