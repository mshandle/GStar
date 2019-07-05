using UnityEditor;
using UnityEngine;

using Framework;
using System.IO;
using System;
using System.Collections.Generic;

public class GenUIExpInfo : MonoBehaviour {

    private class ItemInfo
    {
        public ItemInfo(string typeName,string varName)
        {
            this.TypeName = typeName;
            this.varName = varName;
        }

        public  string TypeName;
        public  string varName;
    }
    [MenuItem("Tool/UI/Bind", false, 2)]
    private static void BindViewInfo()
    {
        GameObject rootGO = Selection.activeObject as GameObject;

        var assembly =  System.Reflection.Assembly.Load("Assembly-CSharp");
        var typevalue = assembly.GetType(rootGO.name);
        var view=  rootGO.GetComponent(typevalue);  
        if(view == null)
        {
            view = rootGO.AddComponent(typevalue);
        }

        Transform[] allChildren = rootGO.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            UIExportItem expItem = child.transform.gameObject.GetComponent<UIExportItem>();
            if (expItem != null || IsRecursiveExportInclude(child.gameObject, rootGO))
            {
                string keyName = null;
                if (expItem.typeArr == null)
                {
                    expItem.InitComponents();
                }
                System.Type compType = expItem.typeArr[expItem.index];
                UnityEngine.Object comp;
                if (compType == typeof(GameObject))
                {
                    comp = expItem.gameObject;

                }
                else
                {
                    comp = expItem.GetComponent(compType);
                }

                if (expItem == null)
                {
                    keyName = child.gameObject.name;
                }
                else
                {
                    keyName = string.IsNullOrEmpty(expItem.overrideName) ? child.gameObject.name : expItem.overrideName;
                }
                view.GetType().GetField(keyName).SetValue(view, comp);
                //dst.expItems.Add(new RefData(keyName, comp));
            }
        }

        if (PrefabUtility.GetPrefabParent(rootGO.gameObject) != null)
        {
            PrefabUtility.ReplacePrefab(rootGO.gameObject, PrefabUtility.GetPrefabParent(rootGO), ReplacePrefabOptions.ConnectToPrefab);
        }
    }
    static string GenPath = Application.dataPath + "/App/Script/UI/";
    [MenuItem("Tool/UI/GenUIExpInfo", false, 2)]
    private static void GenerateUIRefs()
    {
        GameObject rootGO = Selection.activeObject as GameObject;

        //UIPanelBase dst = rootGO == null ? null : rootGO.GetComponent<UIPanelBase>();
        //UIBase dst = rootGO == null ? null : rootGO.GetComponent<UIBase>();

        //if (dst == null)
        //{
            
        //}


        //rootGO.AddComponent<>


        //dst.expItems.Add(new RefData(rootGO.name, rootGO));
        // dst.expObjects
        List<ItemInfo> vars = new List<ItemInfo>();
       
        Transform[] allChildren = rootGO.GetComponentsInChildren<Transform>(true);

        foreach ( Transform child in allChildren)
        {
            UIExportItem expItem = child.transform.gameObject.GetComponent<UIExportItem>();
            if (expItem != null || IsRecursiveExportInclude(child.gameObject, rootGO))
            {
                string keyName = null;
                if (expItem.typeArr == null)
                {
                    expItem.InitComponents();
                }
                System.Type compType = expItem.typeArr[expItem.index];
                UnityEngine.Object comp;
                if (compType == typeof(GameObject))
                {
                    comp = expItem.gameObject;

                }
                else
                {
                    comp = expItem.GetComponent(compType);
                }

                if (expItem == null)
                {
                    keyName = child.gameObject.name;
                }
                else
                {
                    keyName = string.IsNullOrEmpty(expItem.overrideName) ? child.gameObject.name : expItem.overrideName;
                }
                vars.Add(new ItemInfo(compType.Name, keyName));
                //dst.expItems.Add(new RefData(keyName, comp));
            }
        }

        GenViewFile(rootGO.name, vars);
    }

    private static bool IsRecursiveExportInclude( GameObject child, GameObject root)
    {
       
        Transform curTrans = child.transform;
        
        while (curTrans && curTrans.IsChildOf(root.transform)  &&  curTrans != root.transform)
        {
            UIExportItem curExportItem = curTrans.GetComponent<UIExportItem>();
            if (curExportItem && curExportItem.bExportRecursively)
            {
                return true;
            }

            curTrans = curTrans.parent;
        }


        return false;
    }
    private static bool GenViewFile(string name,List<ItemInfo> vars)
    { 
        string filePath = GenPath + name + ".cs";
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
        AssetDatabase.Refresh();
        FileStream ViewFile = new FileStream(filePath, FileMode.CreateNew);
        ViewFile.Seek(0, SeekOrigin.Begin);
        StreamWriter ViewFileWriter = new StreamWriter(ViewFile);
        ViewFileWriter.WriteLine("using UnityEngine;");
        ViewFileWriter.WriteLine("using UnityEngine.UI;");
        ViewFileWriter.WriteLine("using Framework;");
        ViewFileWriter.WriteLine("\n");
        string  classname = "public class "+ name+":UIBase {\n";
        ViewFileWriter.WriteLine(classname);
        foreach(var item in vars)
        {
            ViewFileWriter.WriteLine("      public "+item.TypeName+" "+ item.varName+";");
        }
        ViewFileWriter.Write("}\n");

        ViewFile.Flush();
        ViewFileWriter.Close();
        ViewFile.Close();
        AssetDatabase.Refresh();
        return true;
    }




}
