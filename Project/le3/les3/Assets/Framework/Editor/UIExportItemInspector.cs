using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UIExportItem))]
public class UIExportItemInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
            return;
        base.OnInspectorGUI();

        foreach (var obj in targets)
        {
            var script = (UIExportItem)obj;

            var coms = script.GetComponents<Component>();
            if (script.typeArr == null || script.typeArr.Length != coms.Length)
            {
                script.InitComponents();
            }

            var strArr = new string[script.typeArr.Length];
            for (int i = 0; i < strArr.Length; i++)
            {
                strArr[i] = script.typeArr[i].ToString();
            }
            script.index = EditorGUILayout.Popup("Current Component", script.index, strArr);
            EditorUtility.SetDirty(obj);
        }
    }
}
