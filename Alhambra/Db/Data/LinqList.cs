using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ledsun.Alhambra.Db.Data
{
    /// <summary>
    /// DataTable.RowsをLINQで扱えるようにするためのラッパ
    /// http://cs.rthand.com/blogs/blog_with_righthand/archive/2006/01/15/284.aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class LinqList<T> : IEnumerable<T>, IEnumerable
    {
        IEnumerable items;

        internal LinqList(IEnumerable items)
        {
            this.items = items;
        }

        #region IEnumerable<DataRow> Members
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (T item in items)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerable<T> ie = this;
            return ie.GetEnumerator();
        }
        #endregion
    }
}
