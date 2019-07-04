using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class ResoruceMgr : BaseComponentTemplate<ResoruceMgr>
    {
        public override bool Init(AppConfig config)
        {
            DebugLog.Log("ResoruceMgr");
            return base.Init(config);
        }
    }

}

