using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class SceneMgr : BaseComponentTemplate<SceneMgr>
    {

        public override bool Init()
        {
            DebugLog.Log("SceneMgr");
            return base.Init();
        }
    }
}


