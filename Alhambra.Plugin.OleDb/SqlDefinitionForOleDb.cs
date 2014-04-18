using Alhambra.Db.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alhambra.Plugin.OleDb
{
    [Export(typeof(ISqlDefinition))]
    public class SqlDefinitionForOleDb : ISqlDefinition
    {
        public string MultiBytePrefix
        {
            get { return ""; }
        }
    }
}
