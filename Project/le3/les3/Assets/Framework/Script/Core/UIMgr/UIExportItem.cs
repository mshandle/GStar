using UnityEngine;
using System.Collections.Generic;

public class UIExportItem : MonoBehaviour
{
    public string overrideName = null;

    public bool bExportRecursively = false;



    [HideInInspector]
    public int index = 0;

    [HideInInspector] public System.Type[] typeArr;

#if UNITY_EDITOR
    public void InitComponents()
    {
        if (Application.isPlaying)
            return;
        var coms = GetComponents<Component>();


        var typeList = new List<System.Type>();
        typeList.Add(typeof(GameObject));


        for (var i = 0; i < coms.Length; i++)
        {
            if (coms[i].GetType() == typeof(UIExportItem)) continue;
            typeList.Add(coms[i].GetType());

        }

        typeArr = typeList.ToArray();
    }
#endif


}
