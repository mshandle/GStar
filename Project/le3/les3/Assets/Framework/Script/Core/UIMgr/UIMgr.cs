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
            UIRootGo = new GameObject("UIRoot");
            GameObject.DontDestroyOnLoad(UIRootGo);
            return base.Init(config);
        }
        /// <summary>
        /// 
        /// </summary>
        private List<UIBase> openWindows = new List<UIBase>();

        private List<UIBase> HideWindows = new List<UIBase>();
        /// <summary>
        /// 
        /// </summary>
        private WindowsDefinds uiinfos = new WindowsDefinds();
        /// <summary>
        /// 
        /// </summary>
        public GameObject UIRootGo = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void registerInfo(WindowsID id, UIInfo data)
        {
            uiinfos.registerInfo(id, data);
        }

        public UIBase GetHideWindow(WindowsID id)
        {
            foreach(var item in HideWindows)
            {
                item.windosID = id;
                HideWindows.Remove(item);
                return item;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T OpenUI<T>(WindowsID id)where T:UIBase
        {
            var info = uiinfos.GetWindowsInfo(id);

            var oldWindow =  GetHideWindow(id);
            if(oldWindow != null)
            {
                oldWindow.gameObject.SetActive(true);

                return (T)oldWindow;
            }

            if (info != null)
            {
                GameObject UIPrefab = ResoruceMgr.instance.Load<GameObject>(info.path);
                var UIObject =  GameObject.Instantiate<GameObject>(UIPrefab);
                UIObject.transform.parent = UIRootGo.transform;
                UIBase uiInstance =  UIObject.GetComponent<UIBase>();
                uiInstance.windosID = id;
                uiInstance.uiinfo = info;
                openWindows.Add(uiInstance);
                return (T)uiInstance;
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void Close(WindowsID id)
        {
            foreach(var item in openWindows)
            {
                if(item.windosID == id)
                {
                    if (item.uiinfo.cache)
                    {
                        item.gameObject.SetActive(false);
                        HideWindows.Add(item);
                        openWindows.Remove(item);
                    }
                    else
                    {
                        openWindows.Remove(item);
                        GameObject.Destroy(item.gameObject);
                    }
                   
                    return;
                }
            }
        }

        public void PopTop()
        {
            if (openWindows.Count <= 0) return;

            Close(openWindows[openWindows.Count].windosID);
        }
    }
}
