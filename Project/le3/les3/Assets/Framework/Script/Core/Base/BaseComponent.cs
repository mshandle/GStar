using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
namespace Framework
{
    public class BaseComponent : MonoBehaviour
    {
        //TODO:
        public virtual bool Init()
        {
            return true;
        }
    }
}
