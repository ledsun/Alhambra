# アルハンブラとは #
Alhambra is .Net Database Helper

オブジェクトとデータベースのインピーダンスミスマッチがあります。
アルハンブラはO/Rマッパーではありません。
型のミスマッチは埋めます。

Not O/R Mapper, Mapping only Type mismatch.

C#/VB.NETからSQLを実行するためのラッパークラスです。
コネクションのオープンクローズは自動で行うため明示的に指定する必要はありません。

# 使い方 #

## 設定 ##
app.configファイルやweb.configファイルに記述が必要です

>     <configuration>
>       <appSettings>
>         <add key="DbConnectionString" value="Data Source=Hoge1; User ID=sa; Password=XXX;"/>
>         <add key="SqlCommandTimeout" value="600"/>
>       </appSettings>
>     </configuration>

## サンプルコード ##


### Selectの仕方 ###
取得した値はDataRowAccessorのリストで帰ってきます。
DataRowAccessorはカラムのインデックスまたはカラム名でカラムを指定して値が取得できます。
取った値はInt、DateTime、Doubleなどのプロパティを指定して明示的に型を変換できます。

>     int value =DBHelper.Select(
>         new SqlStatement(@"
>             SELECT
>                 VALE
>             FROM EXAMPLE_TABLE
>             WHERE ID = @ID@
>             ")
>         .Replace("ID", 100)
>         )
>     [0]["VALUE"].Int;

### DataSetの取得方法 ###

GridViewやDropDownListのDataSourceに指定する場合はDataSetで取れた方がうれしいのでSelectDataSetを使います。

>     DataSet ds = DBHelper.SelectDataSet("SELECT 3");

### ひとつの値を取得する場合 ###

レコードの行数を取得する場合など、確実に一つの値を返すSQLを実行する際はSelectOneを使います。

>     int count = DBHelper.SelectOne("SELECT COUNT(*) FROM T_XXX").Int;

## Execute ##
結果を返さないSQLを実行する場合はExceuteを使います。
影響を与えた行数を返します。

### UPDATE/DELETE ###

>     DBHelper.Execute("UPDATE T_XXX SET LEVEL_VAL = 30 WHERE ID = 'abc'");

### ストアドプロシージャ ###

>     DBHelper.Execute("exec sp_XXX");
