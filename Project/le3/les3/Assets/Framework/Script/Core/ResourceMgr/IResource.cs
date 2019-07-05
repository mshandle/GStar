using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    interface IResource
    {
        T Load<T>(string path) where T:UnityEngine.Object;
    }
}
