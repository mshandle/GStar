using UnityEngine;
using UnityEngine.UI;
using Framework;
class MainScene: Framework.BaseScene
{
    static public string Name = "main";
    private GameObject MainUI = null;

    private Button BackBtn = null;
    public override string SceneName()
    {
        return Name;
    }

    public override void OnLoadSceneCompleta()
    {
        base.OnLoadSceneCompleta();
        Framework.DebugLog.Log("Main LoginScen complete");

        GameObject SpawnPoints =  GameObject.Find("SpawnPoints");
        Transform[] TransformPoints =  SpawnPoints.GetComponentsInChildren<Transform>();

        GameObject PlayerGo = GameObject.Find("Player");

        GameObject ShooterGame = new GameObject("Game");
        var monstermanager =  ShooterGame.AddComponent<MonsterManager>();
        monstermanager.Init(TransformPoints,PlayerGo.transform);


        GameObject goPrefab = ResoruceMgr.Instance.Load<GameObject>("UI/prefab/main/MainView.prefab");

        MainUI = GameObject.Instantiate<GameObject>(goPrefab);

        MainUI.transform.parent = UIMgr.Instance.UIRootGo.transform;

        BackBtn = MainUI.transform.Find("BackBtn").gameObject.GetComponent<Button>();
        BackBtn.onClick.AddListener(() =>
        {
            Framework.SceneMgr.Instance.LoadScene(LoginScene.Name);
            GameObject.Destroy(MainUI);
        });

        var text = MainUI.transform.Find("lable").gameObject.GetComponent<Text>();
        text.text = "123456";

        //MonsterFacotry.Instance.CreateMonster(1);
        //MonsterFacotry.Instance.CreateMonster(2);
    }

}

