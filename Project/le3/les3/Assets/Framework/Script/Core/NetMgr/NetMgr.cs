using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework {
    public class NetMgr : BaseComponentTemplate<NetMgr>
    {

        public override bool Init(AppConfig config)
        {
            DebugLog.Log("NetMgr");
            return base.Init(config);
        }
    }
}


