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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public virtual bool Init(AppConfig config)
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void GameUpdata(float det)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool Exit()
        {
            return true;
        }
        //TODO:
    }
}
