using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class SceneMgr : BaseComponentTemplate<SceneMgr>
    {

        public override bool Init(AppConfig config)
        {
            DebugLog.Log("SceneMgr");
            return base.Init(config);
        }
    }
}


