using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
class LoginScene : Framework.BaseScene
{
    static  public string Name = "Login";

    static public string ResPath = "UI/prefab/Login/Login.prefab";

    private GameObject LoginUIGame = null;

    private Button LoginBtn = null;
    public override string SceneName()
    {
        return Name;
    }

    public override void PreLoadScene()
    {
        base.PreLoadScene();
        Framework.DebugLog.Log("Load LoginScen");
        //TODO:
    }

    public override void OnLoadSceneCompleta()
    {
        base.OnLoadSceneCompleta();
        Framework.DebugLog.Log("Load LoginScen complete");

        LoginView View = UIMgr.Instance.OpenUI<LoginView>(WindowsID.Login);
        View.LoginBtn.onClick.RemoveAllListeners();
        View.LoginBtn.onClick.AddListener(() =>
        {
            Framework.SceneMgr.Instance.LoadScene(MainScene.Name);
            View.Close();
        });

        View.Text.text = "Hello World";
        //TODO:
    }
}