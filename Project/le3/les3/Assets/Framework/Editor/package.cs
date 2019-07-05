using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class package : MonoBehaviour
{
    [MenuItem("Tool/BuidlerAssetBundle")]
    public static void BuildAssetBundle()
    {
        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath,BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows);

        AssetDatabase.Refresh();
    }
}
