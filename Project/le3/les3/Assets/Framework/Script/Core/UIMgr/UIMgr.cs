using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class UIMgr : BaseComponentTemplate<UIMgr>
    {

        public override bool Init()
        {
            DebugLog.Log("UIMgr");
            return base.Init();
        }
    }
}
