using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alhambra.Db.Plugin
{
    public interface ISqlDefinition
    {
        string MultiBytePrefix { get; }
    }
}
