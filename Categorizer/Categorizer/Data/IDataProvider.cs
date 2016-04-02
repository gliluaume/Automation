using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Categorizer.Data
{
    public interface IDataProvider<T>
    {
        void Save(T something);
        T Read();
    }
}
