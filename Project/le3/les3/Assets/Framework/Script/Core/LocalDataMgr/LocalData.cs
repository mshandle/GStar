using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class LocalData : BaseComponentTemplate<LocalData>
    {
        public override bool Init()
        {
            DebugLog.Log("LocalData");
            return base.Init();
        }
    }

}

