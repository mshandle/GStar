using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface ITable
    {
        int Count();

        IRecord Query(int id);

        T Query<T>(int id) where T: IRecord;

        T QueryIndex<T>(int i) where T : IRecord;
    }
}
