using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class UIMgr : BaseComponentTemplate<UIMgr>
    {

        public override bool Init(AppConfig config)
        {
            DebugLog.Log("UIMgr");
            return base.Init(config);
        }
    }
}
