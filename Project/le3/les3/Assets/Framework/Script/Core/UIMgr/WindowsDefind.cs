using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public enum WindowsID
    {
        Login,
        Main,
    }

    

    public class UIInfo
    {
        public UIInfo(string path,bool cache = false)
        {
            this.path = path;
            this.cache = cache;
        }
        public bool cache = false;
        public string path;
        //TODO:
    }

    public class WindowsDefinds
    {
        private Dictionary<WindowsID, UIInfo> infos = new Dictionary<WindowsID, UIInfo>();
        

        public void registerInfo(WindowsID id, UIInfo data)
        {
            infos.Add(id, data);
        }

        public UIInfo GetWindowsInfo(WindowsID id)
        {
            UIInfo info;
            if(infos.TryGetValue(id,out info))
            {
                return info;
            }
            return null;
        }


    }
}
