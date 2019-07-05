using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
namespace Framework
{
    public class UIBase : MonoBehaviour
    {
        public UIInfo uiinfo = null;

        public WindowsID windosID{get;set;}

        public void Close()
        {
            UIMgr.Instance.Close(windosID);
        }
    }
}
