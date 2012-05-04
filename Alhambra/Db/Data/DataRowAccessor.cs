using System.Data;

namespace Alhambra.Db.Data
{
    /// <summary>
    /// DataRowオブジェクトの各列の値をTypeConvertableWrapperに変換して取得するラッパー。
    /// </summary>
    public class DataRowAccessor
    {
        private readonly DataRow _row;

        public DataRowAccessor(DataRow row)
        {
            _row = row;
        }

        public DataColumnCollection Columns
        {
            get
            {
                return _row.Table.Columns;
            }
        }

        public TypeConvertableWrapper this[string key]
        {
            get
            {
                return new TypeConvertableWrapper(_row[key]);
            }
        }

        public TypeConvertableWrapper this[int key]
        {
            get
            {
                return new TypeConvertableWrapper(_row[key]);
            }
        }
       
    }
}